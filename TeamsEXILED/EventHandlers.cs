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

namespace TeamsEXILED
{
    public class EventHandlers
    {
        private readonly Plugin<Config> plugin;
        public EventHandlers(Plugin<Config> plugin) => this.plugin = plugin;
        public Dictionary<Player, string> teamedPlayers = new Dictionary<Player, string>();
        public Teams chosenTeam;
        public Random random = new Random();
        public Classes.Classes Classes = new Classes.Classes();
        public LeadingTeam leadingTeam = LeadingTeam.Draw;
        public bool AllowNormalRoundEnd = false;
        public int respawns = 0;
        public void RACommand(SendingRemoteAdminCommandEventArgs ev)
        {
            if (ev.Name == "forceteam")
            {
                ev.IsAllowed = false;
                if (ev.Sender.CheckPermission("ATS.forceteam"))
                {
                    ev.ReplyMessage = "<color=red>Error Team Does Not Exist</color>";
                    ev.Success = false;
                    foreach (Teams t in this.plugin.Config.Teams)
                    {
                        if (t.Name == ev.Arguments[0].ToLower())
                        {
                            if (!t.Active && !ev.Sender.CheckPermission("ATS.bypassActivity"))
                            {
                                ev.ReplyMessage = "<color=red>Error you cant force team this team as you dont have the ATS.bypassActivity Permission</color>";
                                return;
                            }
                            ev.ReplyMessage = "<color=red>Error Could not find subclass</color>";
                            foreach (Subteams st in t.Subclasses)
                            {
                                if (ev.Arguments[1].ToLower() == st.Name)
                                {
                                    ev.ReplyMessage = "<color=red>Error Player Not Found</color>";
                                    if (Player.Get(ev.Arguments[2]).IsVerified)
                                    {
                                        ev.ReplyMessage = "<color=green>Changed players Team!!!</color>";
                                        ChangeTeam(Player.Get(ev.Arguments[2]), ev.Arguments[0].ToLower(), ev.Arguments[1].ToLower());
                                        return;
                                    }
                                    return;
                                }
                            }
                            return;
                        }
                    }
                }
                else
                {
                    ev.ReplyMessage = "<color=red>You do not have permission to use this command</color>";
                }
                return;
            }
            if (ev.Name == "pos")
            {
                ev.IsAllowed = false;
                ev.ReplyMessage = ev.Sender.Position.ToString();
                return;
            }
            if (ev.Name == "team")
            {
                ev.IsAllowed = false;
                ev.ReplyMessage = teamedPlayers[ev.Sender];
                return;
            }
            if (ev.Name == "teamsalive")
            {
                ev.IsAllowed = false;
                foreach (KeyValuePair<Player, string> t in teamedPlayers)
                {
                    ev.ReplyMessage = ev.ReplyMessage + "\n" + t.Value + " : " + t.Key.Nickname;
                }
                return;
            }
        }
        public void OnJoin(VerifiedEventArgs ev)
        {
            teamedPlayers[ev.Player] = "RIP";
        }
        public void OnLeave(DestroyingEventArgs ev)
        {
            teamedPlayers.Remove(ev.Player);
        }
        public void OnRoleChange(ChangingRoleEventArgs ev)
        {
            if (ev.Player.IsOverwatchEnabled)
            {
                return;
            }
            ev.Player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
            ev.Player.CustomInfo = ev.NewRole.ToString();
            Timing.CallDelayed(0.01f, () =>
            {
                teamedPlayers[ev.Player] = ev.Player.Team.ToString().ToLower();
            });
        }
        public void Respawn(RespawningTeamEventArgs ev)
        {
            respawns++;
            chosenTeam = this.plugin.Config.Teams[random.Next(0, this.plugin.Config.Teams.Length)];
            if (chosenTeam.SpawnTypes.Contains(ev.NextKnownTeam) && chosenTeam.Active)
            {
                Log.Debug("Next Known Spawn is " + ev.NextKnownTeam, this.plugin.Config.Debug);
                Log.Debug("Next Known Chosen Team is " + chosenTeam.Name, this.plugin.Config.Debug);
                if (random.Next(0, 100) < chosenTeam.Chance)
                {
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
        void ChangeTeam(Player p, string t, string s)
        {
            if (p.IsOverwatchEnabled)
            {
                return;
            }
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
                if (Classes.IsTeamFriendly(Classes.GetTeamFromString(teamedPlayers[ev.Target], this.plugin.Config), teamedPlayers[ev.Attacker]))
                {
                    ev.IsAllowed = false;
                    ev.Attacker.ShowHint("You cant hurt teams teamed with you!");
                    Log.Debug("Protected a player in " + teamedPlayers[ev.Target] + " from " + teamedPlayers[ev.Attacker], this.plugin.Config.Debug);
                }
            }
            catch (Exception)
            {
                if (Classes.IsTeamFriendly(Classes.GetTeamFromString(teamedPlayers[ev.Attacker], this.plugin.Config), teamedPlayers[ev.Target]))
                {
                    ev.IsAllowed = false;
                    Log.Debug("Caught Exception and using other method", this.plugin.Config.Debug);
                    Log.Debug("Protected a player in " + teamedPlayers[ev.Attacker] + " from " + teamedPlayers[ev.Target], this.plugin.Config.Debug);
                }

            }
        }
        public void MTFSpawnAnnounce(AnnouncingNtfEntranceEventArgs ev)
        {
            if (chosenTeam != null)
            {
                ev.IsAllowed = false;
                Cassie.Message(chosenTeam.CassieMessageMTFSpawn.Replace("{SCP}", ev.ScpsLeft.ToString()).Replace("{unit}", ev.UnitNumber.ToString()).Replace("{nato}", "nato_" + ev.UnitName[0].ToString()), isNoisy: false);
                Map.ChangeUnitColor(respawns, chosenTeam.Color);
            }
        }
        bool TeamExists(string team)
        {
            return teamedPlayers.ContainsValue(team);
        }
        public void OnDied(DiedEventArgs ev)
        {
            Log.Debug(teamedPlayers[ev.Killer], this.plugin.Config.Debug);
            ev.Target.Broadcast(5, "You didnt get team killed you where probably killed by someone who looks like you but isnt");
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
                return;
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
        public void RoundEnd(RoundEndedEventArgs ev)
        {
            AllowNormalRoundEnd = false;
            respawns = 0;
            teamedPlayers.Clear();
        }
    }
}
