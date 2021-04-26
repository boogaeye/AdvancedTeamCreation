using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using TeamsEXILED.API;
using TeamsEXILED.Handlers;
using Exiled.API.Enums;
using MEC;
using System.Linq;
using TeamsEXILED.Enums;
using UnityEngine;
using System.Globalization;
using static TeamsEXILED.Events.General;

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

        public Dictionary<Player, string> teamedPlayers = new Dictionary<Player, string>();
        public List<RoomPoint> fixedpoints = new List<RoomPoint>();
        public List<CoroutineHandle> coroutineHandle = new List<CoroutineHandle>();

        public CoroutineHandle RemoveChosenTeam;

        public Teams chosenTeam;
        public Teams latestSpawn;

        public System.Random random = new System.Random();

        public LeadingTeam leadingTeam = LeadingTeam.Draw;

        public bool AllowNormalRoundEnd = false;
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
                coroutineHandle.Add(Timing.RunCoroutine(RTimerMethods.RespawnTimerPatch()));
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
            teamedPlayers[ev.Player] = "RIP";
        }

        public void OnLeave(LeftEventArgs ev)
        {
            if (ev.Player != null)
            {
                Methods.CheckRoundEnd(ev.Player);
                teamedPlayers.Remove(ev.Player);
            }
        }

        public void OnRoleChange(ChangedRoleEventArgs ev)
        {
            ev.Player.CustomInfo = string.Empty;
            ev.Player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.Role;
            teamedPlayers[ev.Player] = ev.Player.Team.ToString().ToLower();
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
                    MainPlugin.Singleton.TmMethods.RemoveTeamReference();
                    coroutineHandle.Add(Timing.CallDelayed(0.2f, () =>
                    {
                        TeamConvert.SetPlayerTeamName(tempPlayers, "uiu");
                    }));
                    
                    return;
                }
            }

            if (MainPlugin.assemblySerpentHands == true)
            {
                if (Methods.IsSerpentHand())
                {
                    MainPlugin.Singleton.TmMethods.RemoveTeamReference();
                    coroutineHandle.Add(Timing.CallDelayed(0.2f, () =>
                    {
                        TeamConvert.SetPlayerTeamName(tempPlayers, "serpentshand");
                    }));
                    
                    return;
                }
            }

            if (!HasReference)
            {
                MainPlugin.Singleton.TmMethods.RefNextTeamSpawn(ev.NextKnownTeam);
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

                coroutineHandle.Add(Timing.CallDelayed(0.2f, () => MainPlugin.Singleton.TmMethods.ChangePlysToTeam(tempPlayers, chosenTeam)));

                if (random.Next(0, 100) <= chosenTeam.CassieMessageChaosAnnounceChance && ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
                {
                    Cassie.DelayedGlitchyMessage(chosenTeam.CassieMessageChaosMessage, 0, 0.25f, 0.25f);
                }
            }

            MainPlugin.Singleton.TmMethods.RemoveTeamReference();
        }

        public void OnHurt(HurtingEventArgs ev)
        {
            if (ev.DamageType.isWeapon || ev.DamageType.isScp)
            {
                try
                {
                    if (MainPlugin.Singleton.Classes.IsTeamFriendly(MainPlugin.Singleton.Classes.GetTeamFromString(teamedPlayers[ev.Target]), teamedPlayers[ev.Attacker]) && !this.plugin.Config.FriendlyFire)
                    {
                        ev.IsAllowed = false;
                        ev.Attacker.ShowHint("You can't hurt teams teamed with you!");
                        Log.Debug("Protected a player in " + teamedPlayers[ev.Target] + " from " + teamedPlayers[ev.Attacker], this.plugin.Config.Debug);
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

                MainPlugin.Singleton.Classes.RenameUnit(respawns, chosenTeam.Name.ToUpper() + "-" + ev.UnitNumber.ToString());
                Map.ChangeUnitColor(respawns, chosenTeam.Color);
            }
        }

        public void OnDied(DiedEventArgs ev)
        {
            try
            {
                Log.Debug(teamedPlayers[ev.Killer], this.plugin.Config.Debug);

                if (MainPlugin.Singleton.Classes.IsTeamFriendly(MainPlugin.Singleton.Classes.GetTeamFromString(teamedPlayers[ev.Killer]), teamedPlayers[ev.Target]))
                {
                    ev.Target.Broadcast(5, this.plugin.Config.TeamKillBroadcast);
                }
                else
                {
                    ev.Target.Broadcast(5, this.plugin.Config.KilledByNonfriendlyPlayer);
                }

                teamedPlayers[ev.Target] = "Dead";

                Methods.CheckRoundEnd(ev.Killer);
            }
            catch (Exception)
            {
                Log.Debug("Caught On died error. this probably happened because someone left", this.plugin.Config.Debug);
            }
        }

        public void RoundEnding(EndingRoundEventArgs ev)
        {
            if (AllowNormalRoundEnd && this.plugin.Config.Debug)
            {
                Log.Debug("List of teams:", this.plugin.Config.Debug);
                foreach (KeyValuePair<Player, string> t in teamedPlayers)
                {
                    Log.Debug(t.Value + " : " + t.Key, this.plugin.Config.Debug);
                }
            }

            ev.IsAllowed = AllowNormalRoundEnd;
            ev.IsRoundEnded = AllowNormalRoundEnd;
            ev.LeadingTeam = leadingTeam;
        }

        public void OnEscaping(EscapingEventArgs ev)
        {
            teamedPlayers[ev.Player] = MainPlugin.Singleton.Classes.ConvertToNormalTeamName(ev.NewRole).ToString().ToLower();
            if (latestSpawn != null)
            {
                if (latestSpawn.escapeChange.ToList().Contains((EscapeRoles)ev.Player.Role))
                {
                    ev.IsAllowed = false;
                    MainPlugin.Singleton.TmMethods.ChangeTeam(ev.Player, latestSpawn, latestSpawn.Subclasses.First(), true);
                }
            }
        }

        public void OnRestartRound()
        {
            if (MainPlugin.assemblyTimer)
            {
                MainPlugin.Singleton.TmMethods.DefaultTimerConfig();
            }

            foreach (var coroutine in coroutineHandle)
            {
                Timing.KillCoroutines(coroutine);
            }

            AllowNormalRoundEnd = false;
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
