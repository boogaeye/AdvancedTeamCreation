using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsEXILED.API;
using Exiled.API.Features;
using TeamsEXILED.Enums;
using Exiled.API.Interfaces;
using Exiled.CustomItems.API.Features;
using Exiled.API.Enums;
using MEC;

namespace TeamsEXILED
{
    public class TeamMethods
    {
        public void RefNextTeamSpawn()
        {
            MainPlugin.Singleton.EventHandlers.HasReference = true;
            Log.Debug("Getting Team Referances", MainPlugin.Singleton.Config.Debug);
            MainPlugin.Singleton.EventHandlers.chosenTeam = MainPlugin.Singleton.Config.Teams[MainPlugin.Singleton.EventHandlers.random.Next(0, MainPlugin.Singleton.Config.Teams.Length)];
            var handler = new Events.EventArgs.TeamReferencedEventArgs(MainPlugin.Singleton.EventHandlers.chosenTeam);
            handler.StartInvoke();
            MainPlugin.Singleton.EventHandlers.chosenTeam = handler.Team;

            if (!handler.IsAllowed)
            {
                MainPlugin.Singleton.EventHandlers.chosenTeam = null;
                return;
            }

            if (handler.ForceTeam)
            {
                return;
            }

            if (MainPlugin.Singleton.EventHandlers.chosenTeam.SpawnTypes.Contains(Respawn.NextKnownTeam) && MainPlugin.Singleton.EventHandlers.chosenTeam.Active)
            {
                Log.Debug("Next Known Spawn is " + Respawn.NextKnownTeam, MainPlugin.Singleton.Config.Debug);

                if (MainPlugin.Singleton.EventHandlers.random.Next(0, 100) <= MainPlugin.Singleton.EventHandlers.chosenTeam.Chance)
                {
                    Log.Debug("Next Known Chosen Team is " + MainPlugin.Singleton.EventHandlers.chosenTeam.Name, MainPlugin.Singleton.Config.Debug);
                    return;
                }
                else
                {
                    MainPlugin.Singleton.EventHandlers.chosenTeam = null;
                    Log.Debug("Next Known Chosen Team is " + Respawn.NextKnownTeam.ToString().ToLower(), MainPlugin.Singleton.Config.Debug);
                }
            }
            else
            {
                MainPlugin.Singleton.EventHandlers.chosenTeam = null;
                Log.Debug("Next Known Chosen Team is " + Respawn.NextKnownTeam.ToString().ToLower(), MainPlugin.Singleton.Config.Debug);
            }
        }

        public void RefNextTeamSpawn(Respawning.SpawnableTeamType spawnableTeamType)
        {
            Log.Debug("Getting Team Referances", MainPlugin.Singleton.Config.Debug);
            MainPlugin.Singleton.EventHandlers.chosenTeam = MainPlugin.Singleton.Config.Teams[MainPlugin.Singleton.EventHandlers.random.Next(0, MainPlugin.Singleton.Config.Teams.Length)];
            var handler = new Events.EventArgs.TeamReferencedEventArgs(MainPlugin.Singleton.EventHandlers.chosenTeam);
            handler.StartInvoke();
            MainPlugin.Singleton.EventHandlers.chosenTeam = handler.Team;

            if (!handler.IsAllowed)
            {
                MainPlugin.Singleton.EventHandlers.chosenTeam = null;
                return;
            }

            if (handler.ForceTeam)
            {
                return;
            }

            if (MainPlugin.Singleton.EventHandlers.chosenTeam.SpawnTypes.Contains(spawnableTeamType) && MainPlugin.Singleton.EventHandlers.chosenTeam.Active)
            {
                Log.Debug("Next Known Spawn is " + spawnableTeamType, MainPlugin.Singleton.Config.Debug);
                Log.Debug("Next Known Chosen Team is " + MainPlugin.Singleton.EventHandlers.chosenTeam.Name, MainPlugin.Singleton.Config.Debug);

                if (MainPlugin.Singleton.EventHandlers.random.Next(0, 100) < MainPlugin.Singleton.EventHandlers.chosenTeam.Chance)
                {
                    return;
                }
                else
                {
                    MainPlugin.Singleton.EventHandlers.chosenTeam = null;
                }
            }
            else
            {
                MainPlugin.Singleton.EventHandlers.chosenTeam = null;
            }
        }

        public void RefNextTeamSpawn(string teamname)
        {
            Log.Debug("Getting Team Referances", MainPlugin.Singleton.Config.Debug);
            MainPlugin.Singleton.EventHandlers.chosenTeam = MainPlugin.Singleton.Classes.GetTeamFromString(teamname, MainPlugin.Singleton.Config);

            if (!Respawn.IsSpawning && MainPlugin.Singleton.EventHandlers.chosenTeam.Active)
            {
                Log.Debug("Next Known Spawn is " + MainPlugin.Singleton.EventHandlers.spawnableTeamType, MainPlugin.Singleton.Config.Debug);
                Log.Debug("Next Known Chosen Team is " + MainPlugin.Singleton.EventHandlers.chosenTeam.Name, MainPlugin.Singleton.Config.Debug);
                return;
            }
            else
            {
                MainPlugin.Singleton.EventHandlers.chosenTeam = null;
            }
        }

        public void ChangeTeamReferancing(List<Player> p, string t)
        {
            //finding teams
            var team = MainPlugin.Singleton.Config.Teams.First(x => x.Name == t);
            if (team.Name != null)
            {
                Log.Debug("Got team " + team.Name + " from referance method", MainPlugin.Singleton.Config.Debug);
                Dictionary<Player, string> switchList = new Dictionary<Player, string>();
                int i = 0;
                int selectedSubclass = 0;

                foreach (Player y in p)
                {
                    i++;
                    if (team.Subclasses[selectedSubclass].NumOfAllowedPlayers > i && team.Subclasses[selectedSubclass].NumOfAllowedPlayers != -1)
                    {
                        ChangeTeam(y, t, team.Subclasses[selectedSubclass].Name);
                        Log.Debug("allowed subteam " + team.Subclasses[selectedSubclass].Name + " from referance method", MainPlugin.Singleton.Config.Debug);
                    }
                    else if (team.Subclasses[selectedSubclass].NumOfAllowedPlayers == -1)
                    {
                        ChangeTeam(y, t, team.Subclasses[selectedSubclass].Name);
                        Log.Debug("allowed subteam " + team.Subclasses[selectedSubclass].Name + " from referance method with -1 players allowed(making everyone else this role)", MainPlugin.Singleton.Config.Debug);
                    }
                    else
                    {
                        ChangeTeam(y, t, team.Subclasses[selectedSubclass].Name);
                        i = 0;
                        selectedSubclass++;
                        Log.Debug("allowed subteam " + team.Subclasses[selectedSubclass].Name + " from referance method reseting method as selected subclass no longer allows more players to be this role", MainPlugin.Singleton.Config.Debug);
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
            var team = MainPlugin.Singleton.Config.Teams.First(x => x.Name == t);

            if (team != null)
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

                        if (team.spawnLocation != Enums.SpawnLocation.Normal)
                        {
                            var point = MainPlugin.Singleton.EventHandlers.fixedpoints.First(x => x.Type == team.spawnLocation);
                            switch (team.spawnLocation)
                            {
                                case Enums.SpawnLocation.Escape:
                                    {
                                        p.Position = point.Position;
                                        p.Rotations = point.Direction;
                                        break;
                                    }
                                case Enums.SpawnLocation.SCP106:
                                    {
                                        if (!Warhead.IsDetonated)
                                        {
                                            p.Position = point.Position;
                                            p.Rotations = point.Direction;
                                        }
                                        break;
                                    }
                                case Enums.SpawnLocation.SurfaceNuke:
                                    {
                                        p.Position = point.Position;
                                        p.Rotations = point.Direction;
                                        break;
                                    }
                                case Enums.SpawnLocation.SCP012:
                                    {
                                        if (!Map.IsLCZDecontaminated && !Warhead.IsDetonated)
                                        {
                                            p.Position = point.Position;
                                            p.Rotations = point.Direction;
                                        }
                                        break;
                                    }
                                case Enums.SpawnLocation.SCP079:
                                    {
                                        if (!Warhead.IsDetonated)
                                        {
                                            p.Position = point.Position;
                                            p.Rotations = point.Direction;
                                        }
                                        break;
                                    }
                                case Enums.SpawnLocation.SCP096:
                                    {
                                        if (!Warhead.IsDetonated)
                                        {
                                            p.Position = point.Position;
                                            p.Rotations = point.Direction;
                                        }
                                        break;
                                    }
                                case Enums.SpawnLocation.SCP173:
                                    {
                                        if (!Map.IsLCZDecontaminated && !Warhead.IsDetonated)
                                        {
                                            p.Position = point.Position;
                                            p.Rotations = point.Direction;
                                        }
                                        break;
                                    }
                                case Enums.SpawnLocation.Shelter:
                                    {
                                        if (!Warhead.IsDetonated)
                                        {
                                            p.Position = point.Position;
                                            p.Rotations = point.Direction;
                                        }
                                        break;
                                    }
                            }
                        }

                        var ihandler = new Events.EventArgs.AddingInventoryItemsEventArgs(p, subteams);
                        ihandler.StartInvoke();

                        if (MainPlugin.Singleton.Config.UseHints)
                        {
                            p.ShowHint(subteams.RoleMessage, 10);
                        }
                        else
                        {
                            p.Broadcast(10, subteams.RoleMessage);
                        }

                        if (MainPlugin.Singleton.EventHandlers.spawnableTeamType == Respawning.SpawnableTeamType.NineTailedFox)
                        {
                            MainPlugin.Singleton.EventHandlers.coroutineHandle.Add(Timing.CallDelayed(1f, () => p.ReferenceHub.characterClassManager.NetworkCurUnitName = Respawning.RespawnManager.Singleton.NamingManager.AllUnitNames[MainPlugin.Singleton.EventHandlers.respawns].UnitName));
                        }

                        MainPlugin.Singleton.EventHandlers.coroutineHandle.Add(Timing.CallDelayed(0.2f, () =>
                        {
                            p.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
                            p.CustomInfo = subteams.RoleName;
                            MainPlugin.Singleton.EventHandlers.teamedPlayers[p] = t;
                            Log.Debug("Changing player " + p.Nickname + " to " + t, MainPlugin.Singleton.Config.Debug);
                        }));
                    }
                }
                //delay needed so it overrides normal teams
            }
        }

        public bool TeamExists(string team)
        {
            return MainPlugin.Singleton.EventHandlers.teamedPlayers.ContainsValue(team);
        }
    }
}
