using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using TeamsEXILED.API;
using Exiled.API.Extensions;
using Exiled.API.Enums;
using MEC;
using System.Linq;
using UnityEngine;
using System.Globalization;

namespace TeamsEXILED
{
    public class EventHandlers
    {
        #region vars
        private readonly Plugin<Config> plugin;

        public EventHandlers(Plugin<Config> plugin)
        {
            this.plugin = plugin;
        }

        public Dictionary<Player, Teams> teamedPlayers = new Dictionary<Player, Teams>();
        public List<RoomPoint> fixedpoints = new List<RoomPoint>();
        public List<CoroutineHandle> coroutineHandle = new List<CoroutineHandle>();

        public CoroutineHandle RemoveChosenTeam;

        public Teams chosenTeam;
        public Teams latestSpawn;

        public System.Random random = new System.Random();

        public LeadingTeam leadingTeam = LeadingTeam.Draw;

        public bool HasReference = false;
        public bool ForcedTeam = false;

        public int respawns = 0;

        public string mtfTrans, chaosTrans;
        public Respawning.SpawnableTeamType spawnableTeamType = Respawning.SpawnableTeamType.None;

        #endregion
        public void OnRoundStart()
        {
            if (MainPlugin.assemblyTimer)
            {
                coroutineHandle.Add(Timing.RunCoroutine(Methods.RespawnTimerPatch()));
            }

            foreach (SpawnLocation sp in (SpawnLocation[])Enum.GetValues(typeof(SpawnLocation)))
            {
                var rn = Methods.PointThings(sp);

                if (rn != "")
                {
                    var point = rn.Split(':');
                    var post = point[1].Split(',');
                    var dirt = point[2].Split(',');

                    var roomt = Map.Rooms.First(x => x.Name.Contains(point[0])).Transform;
                    var pos = new Vector3(float.Parse(post[0], NumberStyles.Float, CultureInfo.InvariantCulture), float.Parse(post[1], NumberStyles.Float, CultureInfo.InvariantCulture), float.Parse(post[2], NumberStyles.Float, CultureInfo.InvariantCulture));
                    var dir = new Vector2(float.Parse(dirt[0], NumberStyles.Float, CultureInfo.InvariantCulture), float.Parse(dirt[1], NumberStyles.Float, CultureInfo.InvariantCulture));

                    var fpos = roomt.TransformPoint(pos);
                    var fdir = roomt.TransformDirection(dir);

                    fixedpoints.Add(
                    new RoomPoint
                    {
                        Position = fpos,
                        Direction = fdir,
                        Type = sp
                    });
                }
            }
        }

        // This is for development
        /*public void OnSendingCommand(SendingConsoleCommandEventArgs ev)
        {
            if (ev.Name == "point")
            {
                ev.IsAllowed = false;
                var rotation = ev.Player.Rotations;
                var position = ev.Player.Position + (Vector3.up * 0.1f);
                var room = ev.Player.CurrentRoom;
                var p2 = room.Transform.InverseTransformPoint(position);
                var r2 = room.Transform.InverseTransformDirection(rotation);
                var lastr = new Vector2(r2.x, r2.y);

                Log.Info($"Room: {ev.Player.CurrentRoom.Name} Pos:{p2} Rotation:{lastr}");
                ev.ReturnMessage = $"Room: {ev.Player.CurrentRoom.Name} Pos:{p2} Rotation:{lastr}";
            }
        }*/

        public void OnVerified(VerifiedEventArgs ev)
        {
            ev.Player.SetAdvancedTeam(Extensions.GetNormalTeam(Team.RIP));
        }

        public void OnLeave(LeftEventArgs ev)
        {
            if (ev.Player != null)
            {
                teamedPlayers.Remove(ev.Player);
                Methods.CheckRoundEnd();
            }
        }

        public void OnRoleChange(ChangedRoleEventArgs ev)
        {
            ev.Player.CustomInfo = string.Empty;
            ev.Player.InfoArea |= PlayerInfoArea.Role;
            ev.Player.SetAdvancedTeam(Extensions.GetNormalTeam(ev.Player.Team));
        }

        public void OnRespawning(RespawningTeamEventArgs ev)
        {
            // Need this, because ev.Players isn't working for methods
            List<Player> tempPlayers = new List<Player>();

            foreach (Player i in ev.Players)
            {
                if (i.IsOverwatchEnabled == false)
                {
                    tempPlayers.Add(i);
                }
            }

            if (ForcedTeam && HasReference)
            {
                ForcedTeam = false;

                ev.NextKnownTeam = chosenTeam.SpawnTypes.FirstOrDefault();

                if (MainPlugin.assemblyUIU)
                {
                    if (Methods.IsUIU())
                    {
                        Methods.SpawneableUIUToFalse();
                    }
                }

                if (MainPlugin.assemblySerpentHands)
                {
                    if (Methods.IsSerpentHand())
                    {
                        Methods.SpawneableSerpentToFalse();
                    }
                }
            }

            if (MainPlugin.assemblyUIU == true)
            {
                if (Methods.IsUIU())
                {
                    TeamMethods.RemoveTeamReference();
                    coroutineHandle.Add(Timing.CallDelayed(0.2f, () =>
                    {
                        Extensions.SetAdvancedTeam(tempPlayers, Methods.UiUTeam);
                    }));
                    
                    return;
                }
            }

            if (MainPlugin.assemblySerpentHands == true)
            {
                if (Methods.IsSerpentHand())
                {
                    TeamMethods.RemoveTeamReference();
                    coroutineHandle.Add(Timing.CallDelayed(0.2f, () =>
                    {
                        Extensions.SetAdvancedTeam(tempPlayers, Methods.SerpentHandsTeam);
                    }));
                    
                    return;
                }
            }

            if (!HasReference)
            {
                TeamMethods.RefNextTeamSpawn(ev.NextKnownTeam);
                Log.Debug("Possible admin spawn due to No Team Reference yet", this.plugin.Config.Debug);
            }

            latestSpawn = chosenTeam;

            spawnableTeamType = ev.NextKnownTeam;

            if (ev.NextKnownTeam == Respawning.SpawnableTeamType.NineTailedFox)
            {
                respawns++;
            }

            if (chosenTeam != null)
            {
                Log.Debug("Spawned " + chosenTeam.Name, this.plugin.Config.Debug);

                coroutineHandle.Add(Timing.CallDelayed(0.2f, () => TeamMethods.ChangePlysToTeam(tempPlayers, chosenTeam)));

                if (random.Next(0, 100) <= chosenTeam.CassieMessageChaosAnnounceChance && ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
                {
                    Cassie.DelayedGlitchyMessage(chosenTeam.CassieMessageChaosMessage, 0, 0.25f, 0.25f);
                }
            }

            TeamMethods.RemoveTeamReference();
        }

        public void OnHurt(HurtingEventArgs ev)
        {
            if (ev.DamageType.isWeapon || ev.DamageType.isScp)
            {
                try
                {
                    if (ev.Attacker.AdvancedTeam().IsTeamFriendly(ev.Attacker.AdvancedTeam()) && !this.plugin.Config.FriendlyFire)
                    {
                        ev.IsAllowed = false;
                        ev.Attacker.ShowHint(plugin.Config.TransConfigs.FriendlyFireHint, 5);
                        Log.Debug("Protected a player in " + ev.Target.AdvancedTeam().Name + " from " + ev.Attacker.AdvancedTeam().Name, this.plugin.Config.Debug);
                    }
                }
                catch (Exception)
                {
                    Log.Debug("Player possibly left so we caught this error");
                }
            }
        }

        public void MTFSpawnAnnounce(AnnouncingNtfEntranceEventArgs ev)
        {
            if (chosenTeam != null)
            {
                ev.IsAllowed = false;
                if (chosenTeam.CassieMessageMTFSpawn != null)
                {
                    Cassie.Message(chosenTeam.CassieMessageMTFSpawn.Replace("{SCP}", ev.ScpsLeft.ToString()).Replace("{unit}", ev.UnitNumber.ToString()).Replace("{nato}", "nato_" + ev.UnitName[0].ToString()), isNoisy: false);
                }

                Extensions.RenameUnit(respawns, chosenTeam.Name.ToUpper() + "-" + ev.UnitNumber.ToString());
                Map.ChangeUnitColor(respawns, chosenTeam.Color);
            }
        }

        public void OnDied(DiedEventArgs ev)
        {
            try
            {
                Log.Debug(ev.Killer.AdvancedTeam(), plugin.Config.Debug);

                if (ev.Killer.AdvancedTeam().IsTeamFriendly(ev.Target.AdvancedTeam()))
                {
                    ev.Target.Broadcast(5, plugin.Config.TransConfigs.TeamKillBroadcast);
                }
                else
                {
                    ev.Target.Broadcast(5, plugin.Config.TransConfigs.KilledByNonfriendlyPlayer);
                }

                ev.Target.SetAdvancedTeam(Extensions.GetNormalTeam(Team.RIP));

                Methods.CheckRoundEnd();
            }
            catch (Exception)
            {
                if (ev.Target != null)
                {
                    ev.Target.SetAdvancedTeam(Extensions.GetNormalTeam(Team.RIP));
                }

                Methods.CheckRoundEnd();

                Log.Debug("Caught On died error. this probably happened because someone left", this.plugin.Config.Debug);
            }
        }

        public void RoundEnding(EndingRoundEventArgs ev)
        {
            if (ev.IsAllowed && plugin.Config.Debug)
            {
                Log.Debug("List of teams:", this.plugin.Config.Debug);
                foreach (KeyValuePair<Player, Teams> t in teamedPlayers)
                {
                    Log.Debug(t.Value.Name + " : " + t.Key, this.plugin.Config.Debug);
                }
            }

            // This prevents to finish the round if any team has an active requeriment
            foreach (Teams tm in teamedPlayers.Values)
            {
                foreach (Teams team in teamedPlayers.Values)
                {
                    if (tm.Requirements.Contains(team.Name))
                    {
                        ev.IsAllowed = false;
                        return;
                    }
                }
            }

            ev.LeadingTeam = leadingTeam;
        }

        public void OnEscaping(EscapingEventArgs ev)
        {
            // Setting team due to RoleChangeEventArgs not changing the team
            ev.Player.SetAdvancedTeam(Extensions.GetNormalTeam(ev.NewRole.GetTeam()));

            if (latestSpawn != null)
            {
                if (latestSpawn.escapeChange.ToList().Contains(ev.Player.Role))
                {
                    ev.IsAllowed = false;
                    TeamMethods.ChangeTeam(ev.Player, latestSpawn, latestSpawn.Subclasses.First(), true);
                }
            }
        }

        public void OnRestartRound()
        {
            if (MainPlugin.assemblyTimer)
            {
                Methods.DefaultTimerConfig();
            }

            foreach (var coroutine in coroutineHandle)
            {
                Timing.KillCoroutines(coroutine);
            }

            HasReference = false;
            respawns = 0;
            latestSpawn = null;
            chosenTeam = null;
            ForcedTeam = false;

            coroutineHandle.Clear();
            teamedPlayers.Clear();
            fixedpoints.Clear();
        }
    }
}
