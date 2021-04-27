using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.CustomItems.API.Features;
using System.Linq;
using MEC;

namespace TeamsEXILED.Handlers
{
    public class TeamsEvents
    {
        private readonly Plugin<Config> plugin;

        public TeamsEvents(Plugin<Config> plugin)
        {
            this.plugin = plugin;
        }

        public void OnReferencingTeam(Events.General.ReferencingTeamEventArgs ev)
        {
            Log.Debug($"Forceteam: {ev.ForceTeam}\nIsAllowed: {ev.IsAllowed}\nTeamName: {ev.Team.Name}", this.plugin.Config.Debug);

            if (ev.IsAllowed == false)
            {
                return;
            }

            if (ev.Team == null)
            {
                return;
            }

            if (ev.ForceTeam)
            {
                MainPlugin.Singleton.EventHandlers.chosenTeam = ev.Team;
                MainPlugin.Singleton.EventHandlers.HasReference = true;
                MainPlugin.Singleton.EventHandlers.ForcedTeam = true;
            }
            else
            {
                Log.Debug("Next Known Spawn is " + ev.Spawning, MainPlugin.Singleton.Config.Debug);

                if (MainPlugin.Singleton.EventHandlers.random.Next(0, 100) <= ev.Team.Chance)
                {
                    MainPlugin.Singleton.EventHandlers.chosenTeam = ev.Team;
                    MainPlugin.Singleton.EventHandlers.HasReference = true;
                    Log.Debug("Next Known Chosen Team is " + MainPlugin.Singleton.EventHandlers.chosenTeam.Name, MainPlugin.Singleton.Config.Debug);
                }
                else
                {
                    MainPlugin.Singleton.EventHandlers.chosenTeam = null;
                }
            }
        }

        public void OnSettingPlayerTeam(Events.General.SettingPlayerTeamEventArgs ev)
        {
            if (!Round.IsStarted)
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint("Couldnt spawn you in before the round started");
            }

            var p = ev.Player;
            var team = ev.Team;
            var subteams = ev.Subclass;

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

            var ihandler = new Events.General.AddingInventoryItemsEventArgs(p, subteams, keepInv:ev.KeepItems);

            ihandler.StartInvoke();

            if (MainPlugin.Singleton.Config.UseHints)
            {
                p.ShowHint(subteams.RoleMessage, 10);
            }
            else
            {
                p.Broadcast(10, subteams.RoleMessage);
            }

            MainPlugin.Singleton.EventHandlers.coroutineHandle.Add(Timing.CallDelayed(0.2f, () => 
            {
                if (MainPlugin.Singleton.EventHandlers.spawnableTeamType == Respawning.SpawnableTeamType.NineTailedFox)
                {
                    p.ReferenceHub.characterClassManager.NetworkCurUnitName = Respawning.RespawnManager.Singleton.NamingManager.AllUnitNames[MainPlugin.Singleton.EventHandlers.respawns].UnitName;
                }

                p.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
                p.CustomInfo = subteams.RoleName;
                MainPlugin.Singleton.EventHandlers.teamedPlayers[p] = ev.Team.Name;
            }
            ));

            Log.Debug("Changing player " + p.Nickname + " to " + ev.Team.Name, MainPlugin.Singleton.Config.Debug);
        }

        public void OnAddingInventoryItems(Events.General.AddingInventoryItemsEventArgs ev)
        {
            Log.Debug($"Giving Inventory Items of the subclass {ev.Subteam.Name}, to {ev.Player.Nickname}", this.plugin.Config.Debug);
            if (ev.IsAllowed == false)
            {
                return;
            }

            if (ev.KeepInv)
            {
                ev.Player.DropItems();
            }

            ev.Player.ClearInventory();

            foreach (string i in ev.Subteam.Inventory)
            {
                if (int.TryParse(i, out int citem))
                {
                    CustomItem.TryGive(ev.Player, citem, plugin.Config.DisplayDescription);
                }
                else if (ItemType.TryParse<ItemType>(i, out ItemType item))
                {
                    ev.Player.AddItem(item);
                }
                else
                {
                    Log.Error($"The config item {i} of the subteam {ev.Subteam.Name} isn't valid");
                }
            }

            foreach (KeyValuePair<AmmoType, uint> a in ev.Subteam.Ammo)
            {
                ev.Player.Ammo[(int)a.Key] = a.Value;
            }
        }
    }
}
