using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.CustomItems.API.Features;
using System.Linq;
using MEC;
using TeamsEXILED.API;

namespace TeamsEXILED.Handlers
{
    public class TeamsEvents
    {
        private readonly Plugin<Config> plugin;

        public TeamsEvents(Plugin<Config> plugin)
        {
            this.plugin = plugin;
        }

        public void OnReferencingTeam(TeamEvents.ReferencingTeamEventArgs ev)
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

        public void OnSettingPlayerTeam(TeamEvents.SettingPlayerTeamEventArgs ev)
        {
            if (!Round.IsStarted)
            {
                ev.IsAllowed = false;
            }

            if (ev.IsAllowed == false)
            {
                return;
            }

            if (MainPlugin.assemblyAdvancedSubclass)
            {
                if (Methods.HasAdvancedSubclass(ev.Player))
                {
                    Methods.RemoveAdvancedSubclass(ev.Player);
                }

                if (ev.Subclass.AdvancedSubclass != string.Empty)
                {
                    Methods.GiveAdvancedSubclass(ev.Player, ev.Subclass.AdvancedSubclass);
                    return;
                }
            }

            ev.Player.SetRole(ev.Subclass.ModelRole, true);
            ev.Player.Health = ev.Subclass.HP;
            ev.Player.MaxHealth = ev.Subclass.HP;

            if (ev.Team.spawnLocation != SpawnLocation.Normal)
            {
                var point = MainPlugin.Singleton.EventHandlers.fixedpoints.First(x => x.Type == ev.Team.spawnLocation);
                switch (ev.Team.spawnLocation)
                {
                    case SpawnLocation.Escape:
                        {
                            ev.Player.Position = point.Position;
                            ev.Player.Rotations = point.Direction;
                            break;
                        }
                    case SpawnLocation.SCP106:
                        {
                            if (!Warhead.IsDetonated)
                            {
                                ev.Player.Position = point.Position;
                                ev.Player.Rotations = point.Direction;
                            }
                            break;
                        }
                    case SpawnLocation.SurfaceNuke:
                        {
                            ev.Player.Position = point.Position;
                            ev.Player.Rotations = point.Direction;
                            break;
                        }
                    case SpawnLocation.SCP012:
                        {
                            if (!Map.IsLCZDecontaminated && !Warhead.IsDetonated)
                            {
                                ev.Player.Position = point.Position;
                                ev.Player.Rotations = point.Direction;
                            }
                            break;
                        }
                    case SpawnLocation.SCP079:
                        {
                            if (!Warhead.IsDetonated)
                            {
                                ev.Player.Position = point.Position;
                                ev.Player.Rotations = point.Direction;
                            }
                            break;
                        }
                    case SpawnLocation.SCP096:
                        {
                            if (!Warhead.IsDetonated)
                            {
                                ev.Player.Position = point.Position;
                                ev.Player.Rotations = point.Direction;
                            }
                            break;
                        }
                    case SpawnLocation.SCP173:
                        {
                            if (!Map.IsLCZDecontaminated && !Warhead.IsDetonated)
                            {
                                ev.Player.Position = point.Position;
                                ev.Player.Rotations = point.Direction;
                            }
                            break;
                        }
                    case SpawnLocation.Shelter:
                        {
                            if (!Warhead.IsDetonated)
                            {
                                ev.Player.Position = point.Position;
                                ev.Player.Rotations = point.Direction;
                            }
                            break;
                        }
                }
            }

            var ihandler = new TeamEvents.AddingInventoryItemsEventArgs(ev.Player, ev.Subclass, keepInv:ev.KeepItems);

            ihandler.StartInvoke();

            if (MainPlugin.Singleton.Config.UseHints)
            {
                ev.Player.ShowHint(ev.Subclass.RoleMessage, 10);
            }
            else
            {
                ev.Player.Broadcast(10, ev.Subclass.RoleMessage);
            }

            MainPlugin.Singleton.EventHandlers.coroutineHandle.Add(Timing.CallDelayed(0.2f, () => 
            {
                /*if (MainPlugin.Singleton.EventHandlers.spawnableTeamType == Respawning.SpawnableTeamType.NineTailedFox)
                {
                    p.UnitName = Respawning.RespawnManager.Singleton.NamingManager.AllUnitNames[MainPlugin.Singleton.EventHandlers.respawns].UnitName;
                }*/

                ev.Player.InfoArea &= ~PlayerInfoArea.Role;
                ev.Player.CustomInfo = ev.Subclass.RoleName;
                ev.Player.SetAdvancedTeam(ev.Team);
            }
            ));

            Log.Debug("Changing player " + ev.Player.Nickname + " to " + ev.Team.Name, MainPlugin.Singleton.Config.Debug);
        }

        public void OnAddingInventoryItems(TeamEvents.AddingInventoryItemsEventArgs ev)
        {
            Log.Debug($"Giving Inventory Items of the subclass {ev.Subteam.Name}, to {ev.Player.Nickname}", this.plugin.Config.Debug);
            if (ev.IsAllowed == false)
            {
                return;
            }

            if (ev.KeepInv)
            {
                // This leaves the items in the escape area
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
