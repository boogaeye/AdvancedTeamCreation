using System;
using System.Collections.Generic;
using Exiled.Loader;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using TeamsEXILED.API;
using Exiled.API.Enums;
using Exiled.Permissions.Extensions;
using TeamsEXILED.Classes;
using MEC;
using Exiled.CustomItems.API.Features;
using Exiled.API.Interfaces;
using Interactables.Interobjects.DoorUtils;
using TeamsEXILED.Events;
using System.Linq;
using TeamsEXILED.Enums;
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

        public Dictionary<Player, string> teamedPlayers = new Dictionary<Player, string>();
        public List<RoomPoint> fixedpoints = new List<RoomPoint>();
        public List<CoroutineHandle> coroutineHandle = new List<CoroutineHandle>();

        public Teams chosenTeam;
        public Teams latestSpawn;

        public System.Random random = new System.Random();
        public Classes.Classes Classes = new Classes.Classes();
        public TeamMethods TmMethods = new TeamMethods();

        public LeadingTeam leadingTeam = LeadingTeam.Draw;

        public bool AllowNormalRoundEnd = false;
        public bool HasReference = false;

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
            teamedPlayers.Remove(ev.Player);
        }

        public void OnRoleChange(ChangedRoleEventArgs ev)
        {
            if (ev.Player.IsOverwatchEnabled)
            {
                return;
            }

            ev.Player.CustomInfo = string.Empty;
            ev.Player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.Role;
            teamedPlayers[ev.Player] = ev.Player.Team.ToString().ToLower();
        }

        public void OnRespawning(RespawningTeamEventArgs ev)
        {
            if (MainPlugin.assemblyUIU == true)
            {
                if (Methods.IsUIU())
                {
                    coroutineHandle.Add(Timing.CallDelayed(3f, () =>
                    {
                        if (MainPlugin.assemblyTimer)
                        {
                            var cfg = (RespawnTimer.Config)Methods.GetRespawnTimerCfg();
                            cfg.translations.Ci = chaosTrans;
                            cfg.translations.Ntf = mtfTrans;
                        }

                        chosenTeam = null;
                        HasReference = false;
                    }));
                    return;
                }
            }

            if (MainPlugin.assemblySerpentHands == true)
            {
                if (Methods.IsSerpentHand())
                {
                    coroutineHandle.Add(Timing.CallDelayed(3f, () =>
                    {
                        if (MainPlugin.assemblyTimer)
                        {
                            var cfg = (RespawnTimer.Config)Methods.GetRespawnTimerCfg();
                            cfg.translations.Ci = chaosTrans;
                            cfg.translations.Ntf = mtfTrans;
                        }

                        chosenTeam = null;
                        HasReference = false;
                    }));
                    return;
                }
            }

            if (!HasReference)
            {
                TmMethods.RefNextTeamSpawn(ev.NextKnownTeam);
                Log.Debug("Possible admin spawn due to No Team Reference yet", this.plugin.Config.Debug);
            }

            spawnableTeamType = ev.NextKnownTeam;

            if (ev.NextKnownTeam == Respawning.SpawnableTeamType.NineTailedFox)
            {
                respawns++;
            }

            if (chosenTeam != null)
            {
                Log.Debug("Spawned " + chosenTeam.Name, this.plugin.Config.Debug);
                List<Player> tempPlayers = new List<Player>();

                foreach (Player i in ev.Players)
                {
                    tempPlayers.Add(i);
                }

                coroutineHandle.Add(Timing.CallDelayed(0.2f, () =>
                {
                    TmMethods.ChangeTeamReferancing(tempPlayers, chosenTeam.Name);
                }));

                if (random.Next(0, 100) < chosenTeam.CassieMessageChaosAnnounceChance && ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
                {
                    Cassie.DelayedGlitchyMessage(chosenTeam.CassieMessageChaosMessage, 0, 0.25f, 0.25f);
                }
            }
            
            coroutineHandle.Add(Timing.CallDelayed(3f, () =>
            {
                if (MainPlugin.assemblyTimer)
                {
                    var cfg = (RespawnTimer.Config)Methods.GetRespawnTimerCfg();
                    cfg.translations.Ci = chaosTrans;
                    cfg.translations.Ntf = mtfTrans;
                }

                chosenTeam = null;
                HasReference = false;
            }));
        }

        public void OnHurt(HurtingEventArgs ev)
        {
            if (ev.DamageType.isWeapon || ev.DamageType.isScp)
            {
                try
                {
                    if (Classes.IsTeamFriendly(Classes.GetTeamFromString(teamedPlayers[ev.Target], this.plugin.Config), teamedPlayers[ev.Attacker]) && !this.plugin.Config.FriendlyFire)
                    {
                        ev.IsAllowed = false;
                        ev.Attacker.ShowHint("You cant hurt teams teamed with you!");
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

                Classes.RenameUnit(respawns, chosenTeam.Name.ToUpper() + "-" + ev.UnitNumber.ToString());
                Map.ChangeUnitColor(respawns, chosenTeam.Color);
            }
        }

        public void OnDied(DiedEventArgs ev)
        {
            try
            {
                Log.Debug(teamedPlayers[ev.Killer], this.plugin.Config.Debug);

                if (teamedPlayers[ev.Target] == teamedPlayers[ev.Killer])
                {
                    ev.Target.Broadcast(5, this.plugin.InternalTranslation.d);
                }
                else
                {
                    ev.Target.Broadcast(5, this.plugin.Config.KilledByNonfriendlyPlayer);
                }

                teamedPlayers[ev.Target] = "Dead";

                foreach (string t in Classes.GetTeamFromString(teamedPlayers[ev.Killer], this.plugin.Config).Requirements)
                {
                    Log.Debug("got " + t + " from enemies", this.plugin.Config.Debug);

                    if (TmMethods.TeamExists(t))
                    {
                        Log.Debug("This team is an enemy of this team stopping the round from ending", this.plugin.Config.Debug);
                        return;
                    }
                }

                if (ev.Target == ev.Killer)
                {
                    var alive = Player.List.Where(x => x.IsAlive).ToList().Count;

                    if (alive == 1 && this.plugin.Config.Allow1Player)
                    {
                        return;
                    }

                    if (alive > 1)
                    {
                        return;
                    }
                }

                if (AllowNormalRoundEnd)
                {
                    return;
                }

                AllowNormalRoundEnd = true;
                leadingTeam = Classes.GetTeamFromString(teamedPlayers[ev.Killer], this.plugin.Config).teamLeaders;

                foreach (string a in Classes.GetTeamFromString(teamedPlayers[ev.Killer], this.plugin.Config).Neutral)
                {
                    if (TmMethods.TeamExists(a))
                    {
                        leadingTeam = LeadingTeam.Draw;
                    }
                }

                Round.ForceEnd();
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
            teamedPlayers[ev.Player] = Classes.ConvertToNormalTeamName(ev.NewRole).ToString().ToLower();
        }

        public void OnRestartRound()
        {
            if (MainPlugin.assemblyTimer)
            {
                var cfg = (RespawnTimer.Config)Methods.GetRespawnTimerCfg();
                cfg.translations.Ci = chaosTrans;
                cfg.translations.Ntf = mtfTrans;
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
           
            teamedPlayers.Clear();
            fixedpoints.Clear();
        }
    }
}
