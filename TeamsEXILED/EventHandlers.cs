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

namespace TeamsEXILED
{
    public class EventHandlers
    {
        #region vars
        private readonly Plugin<Config> plugin;

        public EventHandlers(Plugin<Config> plugin) => this.plugin = plugin;

        public Dictionary<Player, string> teamedPlayers = new Dictionary<Player, string>();

        public Teams chosenTeam;

        public Random random = new Random();

        public Classes.Classes Classes = new Classes.Classes();

        public LeadingTeam leadingTeam = LeadingTeam.Draw;

        public bool AllowNormalRoundEnd = false;

        public int respawns = 0;

        public bool HasReference = false;

        public string mtfTrans, chaosTrans;

        public Respawning.SpawnableTeamType spawnableTeamType = Respawning.SpawnableTeamType.None;
        #endregion
        #region Fake Respawn Timer
        CoroutineHandle coroutineHandle = new CoroutineHandle();
        CoroutineHandle respawnCoroutineHandle = new CoroutineHandle();

        public void OnRoundStart()
        {
            if (coroutineHandle.IsRunning)
            {
                Timing.KillCoroutines(coroutineHandle);
            }
            if (respawnCoroutineHandle.IsRunning)
            {
                Timing.KillCoroutines(respawnCoroutineHandle);
            }
            Timing.RunCoroutine(Timer());
            Timing.RunCoroutine(RespawnTimerPatch());
        }

        IEnumerator<float> Timer()
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(1f);
                if (Respawn.NextKnownTeam != Respawning.SpawnableTeamType.None)
                {
                    if (!HasReference)
                    {
                        RefNextTeamSpawn();
                    }
                }
            }
        }

        IEnumerator<float> RespawnTimerPatch()
        {
            while (Round.IsStarted)
            {
                if (MainPlugin.assemblyTimer)
                {
                    yield return Timing.WaitForSeconds(MainPlugin.rtconfig.Interval - 0.01f);
                    if (HasReference && chosenTeam != null)
                    {
                        switch (Respawn.NextKnownTeam)
                        {
                            case Respawning.SpawnableTeamType.NineTailedFox:
                                MainPlugin.rtconfig.translations.Ntf = $"<color={chosenTeam.Color}>{chosenTeam.Name}</color>";
                                break;
                            case Respawning.SpawnableTeamType.ChaosInsurgency:
                                MainPlugin.rtconfig.translations.Ci = $"<color={chosenTeam.Color}>{chosenTeam.Name}</color>";
                                break;
                        }
                    }
                }
            }
        }
        #endregion
        #region Base Plugin

        public void OnReferanceTeam(Events.EventArgs.TeamReferencedEventArgs ev)
        {
            Log.Debug($"Forceteam: {ev.ForceTeam}\nIsAllowed: {ev.IsAllowed}\nTeamName: {ev.Team.Name}", this.plugin.Config.Debug);
        }

        public void OnCreatingTeam(Events.EventArgs.CreatingTeamEventArgs ev)
        {
            Log.Debug("Creating Team Event Called", this.plugin.Config.Debug);
            if (Teams.IsDefinedInConfig(ev.Team.Name, this.plugin.Config))
            {
                Log.Debug($"{ev.Team.Name} is defined in Normal Teams Config", this.plugin.Config.Debug);
                foreach (NormalTeam t in this.plugin.Config.TeamRedefine)
                {
                    if (t.Team.ToString().ToLower() == ev.Team.Name && t.Active)
                    {
                        Log.Debug("Redefined Team!", this.plugin.Config.Debug);
                        ev.Team = new Teams
                        {
                            Name = ev.Team.Name,
                            Neutral = t.Neutral,
                            Friendlys = t.Friendlys,
                            Requirements = t.Requirements,
                            teamLeaders = ev.Team.teamLeaders
                        };
                    }
                }
            }
        }

        public void RefNextTeamSpawn()
        {
            HasReference = true;
            Log.Debug("Getting Team Referances", this.plugin.Config.Debug);
            chosenTeam = this.plugin.Config.Teams[random.Next(0, this.plugin.Config.Teams.Length)];
            var handler = new Events.EventArgs.TeamReferencedEventArgs(chosenTeam);
            handler.StartInvoke();
            chosenTeam = handler.Team;
            if (!handler.IsAllowed)
            {
                chosenTeam = null;
                return;
            }
            if (handler.ForceTeam)
            {
                return;
            }
            if (chosenTeam.SpawnTypes.Contains(Respawn.NextKnownTeam) && chosenTeam.Active)
            {
                Log.Debug("Next Known Spawn is " + Respawn.NextKnownTeam, this.plugin.Config.Debug);
                Log.Debug("Next Known Chosen Team is " + chosenTeam.Name, this.plugin.Config.Debug);
                if (random.Next(0, 100) < chosenTeam.Chance)
                {
                    return;
                }
                else
                {
                    chosenTeam = null;
                }
            }
            else
            {
                chosenTeam = null;
            }
        }

        public void RefNextTeamSpawn(Respawning.SpawnableTeamType spawnableTeamType)
        {
            Log.Debug("Getting Team Referances", this.plugin.Config.Debug);
            chosenTeam = this.plugin.Config.Teams[random.Next(0, this.plugin.Config.Teams.Length)];
            var handler = new Events.EventArgs.TeamReferencedEventArgs(chosenTeam);
            handler.StartInvoke();
            chosenTeam = handler.Team;
            if (!handler.IsAllowed)
            {
                chosenTeam = null;
                return;
            }
            if (handler.ForceTeam)
            {
                return;
            }
            if (chosenTeam.SpawnTypes.Contains(spawnableTeamType) && chosenTeam.Active)
            {
                Log.Debug("Next Known Spawn is " + spawnableTeamType, this.plugin.Config.Debug);
                Log.Debug("Next Known Chosen Team is " + chosenTeam.Name, this.plugin.Config.Debug);
                if (random.Next(0, 100) < chosenTeam.Chance)
                {
                    return;
                }
                else
                {
                    chosenTeam = null;
                }
            }
            else
            {
                chosenTeam = null;
            }
        }
        public void RefNextTeamSpawn(string teamname)
        {
            Log.Debug("Getting Team Referances", this.plugin.Config.Debug);
            chosenTeam = Classes.GetTeamFromString(teamname, this.plugin.Config);
            if (!Respawn.IsSpawning && chosenTeam.Active)
            {
                Log.Debug("Next Known Spawn is " + spawnableTeamType, this.plugin.Config.Debug);
                Log.Debug("Next Known Chosen Team is " + chosenTeam.Name, this.plugin.Config.Debug);
                return;
            }
            else
            {
                chosenTeam = null;
            }
        }

        public void OnVerified(VerifiedEventArgs ev)
        {
            teamedPlayers[ev.Player] = "RIP";
        }

        public void OnLeave(LeftEventArgs ev)
        {
            teamedPlayers.Remove(ev.Player);
        }

        public void OnRoleChange(ChangingRoleEventArgs ev)
        {
            if (ev.Player.IsOverwatchEnabled)
            {
                return;
            }
            ev.Player.CustomInfo = string.Empty;
            ev.Player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.Role;
            Timing.CallDelayed(0.01f, () =>
            {
                teamedPlayers[ev.Player] = ev.Player.Team.ToString().ToLower();
            });
        }

        public void OnTeamSpawn(Events.EventArgs.SetTeamEventArgs ev)
        {
            if (!Round.IsStarted)
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint("Couldnt spawn you in before the round started");
            }
            if (HasReference)
            {
                if (ev.Player.Role == RoleType.Spectator)
                {
                    ev.IsAllowed = false;
                    ev.Player.ShowHint("Couldnt spawn you in because you are already spawning in");
                }
            }
        }

        public void OnRespawning(RespawningTeamEventArgs ev)
        {
            if (!HasReference)
            {
                RefNextTeamSpawn(ev.NextKnownTeam);
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
                List<Exiled.API.Features.Player> tempPlayers = new List<Player>();
                foreach (Player i in ev.Players)
                {
                    tempPlayers.Add(i);
                }
                Timing.CallDelayed(1.5f, () =>
                {
                    ChangeTeamReferancing(tempPlayers, chosenTeam.Name);
                });
                if (random.Next(0, 100) < chosenTeam.CassieMessageChaosAnnounceChance && ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
                {
                    Cassie.DelayedGlitchyMessage(chosenTeam.CassieMessageChaosMessage, 0, 0.25f, 0.25f);
                }
            }
            Timing.CallDelayed(3f, () =>
            {
                chosenTeam = null;
                MainPlugin.rtconfig.translations.Ci = chaosTrans;
                MainPlugin.rtconfig.translations.Ntf = mtfTrans;
            });
            HasReference = false;
        }

        void ChangeTeamReferancing(List<Player> p, string t)
        {
            //finding teams
            foreach (Teams team in this.plugin.Config.Teams)
            {
                if (team.Name == t)
                {
                    Log.Debug("Got team " + team.Name + " from referance method", this.plugin.Config.Debug);
                    Dictionary<Player, string> switchList = new Dictionary<Player, string>();
                    int i = 0;
                    int selectedSubclass = 0;
                    foreach (Player y in p)
                    {
                        i++;
                        if (team.Subclasses[selectedSubclass].NumOfAllowedPlayers > i && team.Subclasses[selectedSubclass].NumOfAllowedPlayers != -1)
                        {
                            ChangeTeam(y, t, team.Subclasses[selectedSubclass].Name);
                            Log.Debug("allowed subteam " + team.Subclasses[selectedSubclass].Name + " from referance method", this.plugin.Config.Debug);
                        }
                        else if (team.Subclasses[selectedSubclass].NumOfAllowedPlayers == -1)
                        {
                            ChangeTeam(y, t, team.Subclasses[selectedSubclass].Name);
                            Log.Debug("allowed subteam " + team.Subclasses[selectedSubclass].Name + " from referance method with -1 players allowed(making everyone else this role)", this.plugin.Config.Debug);
                        }
                        else
                        {
                            ChangeTeam(y, t, team.Subclasses[selectedSubclass].Name);
                            i = 0;
                            selectedSubclass++;
                            Log.Debug("allowed subteam " + team.Subclasses[selectedSubclass].Name + " from referance method reseting method as selected subclass no longer allows more players to be this role", this.plugin.Config.Debug);
                        }
                    }
                }
            }
        }
        public void ChangeTeam(Player p, string t, string s)
        {
            if (p.IsOverwatchEnabled)
            {
                return;
            }
            var handler = new Events.EventArgs.SetTeamEventArgs(t, s, p);
            handler.StartInvoke();
            t = handler.Team;
            s = handler.Subclass;
            if (!handler.IsAllowed) return;
            //Finding teems and seeing if the string exists
            foreach (Teams team in this.plugin.Config.Teams)
            {
                if (team.Name == t)
                {
                    //Finding subteams and seeing if the string exists
                    foreach (Subteams subteams in team.Subclasses)
                    {
                        if (subteams.Name == s)
                        {
                            //If found it will give you everything defined here
                            p.SetRole(subteams.ModelRole, true);
                            p.Health = subteams.HP;
                            p.MaxHealth = subteams.HP;
                            if (spawnableTeamType == Respawning.SpawnableTeamType.NineTailedFox)
                            {
                                p.ReferenceHub.characterClassManager.NetworkCurUnitName = Respawning.RespawnManager.Singleton.NamingManager.AllUnitNames[respawns].UnitName;
                            }
                            if (team.spawnLocation != Enums.SpawnLocation.Normal)
                            {
                                string nameLooked = string.Empty;
                                int xoff = 0;
                                int yoff = 0;
                                int zoff = 0;
                                switch (team.spawnLocation)
                                {
                                    case Enums.SpawnLocation.Escape:
                                        nameLooked = "ESCAPE_PRIMARY";
                                        zoff = 2;
                                        break;
                                    case Enums.SpawnLocation.SCP106:
                                        if (!Warhead.IsDetonated)
                                        {
                                            nameLooked = "106_BOTTOM";
                                        }
                                        break;
                                    case Enums.SpawnLocation.SurfaceNuke:
                                        nameLooked = "SURFACE_NUKE";
                                        zoff = 2;
                                        break;
                                }
                                if (DoorNametagExtension.NamedDoors.ContainsKey(nameLooked))
                                {
                                    DoorNametagExtension.NamedDoors[nameLooked].TargetDoor.NetworkTargetState = true;
                                    p.Position = DoorNametagExtension.NamedDoors[nameLooked].gameObject.transform.position + new UnityEngine.Vector3(0 + xoff, 1 + yoff, 0 + zoff);
                                }
                            }
                            p.ClearInventory();
                            foreach (ItemType i in subteams.Inventory)
                            {
                                p.AddItem(i);
                            }
                            if (subteams.CustomItemIds.Length != 0)
                            {
                                foreach (int it in subteams.CustomItemIds)
                                {
                                    CustomItem.TryGive(p, it, true);
                                }
                            }
                            foreach (System.Collections.Generic.KeyValuePair<AmmoType, uint> a in subteams.Ammo)
                            {
                                p.Ammo[(int)a.Key] = a.Value;
                            }
                            p.ShowHint(subteams.RoleHint, 10);
                            Timing.CallDelayed(0.2f, () =>
                            {
                                p.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
                                p.CustomInfo = subteams.RoleName;
                                teamedPlayers[p] = t;
                                Log.Debug("Changing player " + p.Nickname + " to " + t, this.plugin.Config.Debug);
                            });
                        }
                    }
                    //delay needed so it overrides normal teams
                }
            }
        }

        public void OnHurt(HurtingEventArgs ev)
        {
            List<DamageTypes.DamageType> damageTypes = new List<DamageTypes.DamageType> { DamageTypes.Contain, DamageTypes.Bleeding, DamageTypes.Asphyxiation, DamageTypes.Decont, DamageTypes.Falldown, DamageTypes.Grenade, DamageTypes.Lure, DamageTypes.MicroHid, DamageTypes.Nuke, DamageTypes.Pocket, DamageTypes.Poison, DamageTypes.Recontainment, DamageTypes.Scp207, DamageTypes.Tesla, DamageTypes.None };
            if (damageTypes.Contains(ev.DamageType))
            {
                return;
            }
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

        bool TeamExists(string team)
        {
            return teamedPlayers.ContainsValue(team);
        }

        public void OnDied(DiedEventArgs ev)
        {
            try
            {
                Log.Debug(teamedPlayers[ev.Killer], this.plugin.Config.Debug);
                if (teamedPlayers[ev.Target] == teamedPlayers[ev.Killer])
                {
                    ev.Target.Broadcast(5, this.plugin.Config.TeamKillBroadcast);
                }
                else
                {
                    ev.Target.Broadcast(5, this.plugin.Config.KilledByNonfriendlyPlayer);
                }
                teamedPlayers[ev.Target] = "Dead";
                foreach (string t in Classes.GetTeamFromString(teamedPlayers[ev.Killer], this.plugin.Config).Requirements)
                {
                    Log.Debug("got " + t + " from enemies", this.plugin.Config.Debug);
                    if (TeamExists(t))
                    {
                        Log.Debug("This team is an enemy of this team stopping the round from ending", this.plugin.Config.Debug);
                        return;
                    }
                }
                if (ev.Target == ev.Killer)
                {
                    int alive = 0;
                    foreach (Player p in Player.List)
                    {
                        if (p.IsAlive)
                        {
                            alive++;
                        }
                    }
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
                    if (TeamExists(a))
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
            if (AllowNormalRoundEnd)
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

        public void WaitingForPlayers()
        {
            Timing.KillCoroutines(coroutineHandle);
            Timing.KillCoroutines(respawnCoroutineHandle);
            AllowNormalRoundEnd = false;
            respawns = 0;
            HasReference = false;
            teamedPlayers.Clear();
        }
        #endregion
    }
}
