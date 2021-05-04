namespace AdvancedTeamCreation.Handlers
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Exiled.API.Enums;
    using Exiled.CustomItems.API.Features;
    using System.Linq;
    using MEC;
    using API;
    using Configs;
    using Helper;
    using static AdvancedTeamCreation;

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
                Instance.EventHandlers.chosenTeam = ev.Team;
                Instance.EventHandlers.HasReference = true;
                Instance.EventHandlers.ForcedTeam = true;
            }
            else
            {
                Log.Debug("Next Known Spawn is " + ev.Spawning, Instance.Config.Debug);

                if (Rand.Next(0, 100) <= ev.Team.Chance)
                {
                    Instance.EventHandlers.chosenTeam = ev.Team;
                    Instance.EventHandlers.HasReference = true;
                    Log.Debug("Next Known Chosen Team is " + Instance.EventHandlers.chosenTeam.Name, Instance.Config.Debug);
                }
                else
                {
                    Instance.EventHandlers.chosenTeam = null;
                }
            }
        }

        public void OnSettingPlayerTeam(TeamEvents.SettingPlayerTeamEventArgs ev)
        {
            if (ev.IsAllowed == false)
            {
                return;
            }

            ev.Player.SetRole(ev.Subclass.ModelRole, true);
            if (assemblyAdvancedSubclass)
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

            ev.Player.Health = ev.Subclass.HP;
            ev.Player.MaxHealth = ev.Subclass.HP;
            if (ev.Team.SpawnLocation != SpawnLocation.Normal)
            {
                var point = Instance.EventHandlers.fixedpoints.First(x => x.Type == ev.Team.SpawnLocation);
                if (ev.Team.SpawnLocation == SpawnLocation.SCP012 || ev.Team.SpawnLocation == SpawnLocation.SCP173)
                {
                    if (Warhead.IsDetonated || Map.IsLCZDecontaminated)
                    {
                        Log.Debug("Not changing player position because the warhead is detonated or the lcz is decontaminated", Instance.Config.Debug);
                    }
                    else
                    {
                        Instance.EventHandlers.coroutineHandle.Add(Timing.CallDelayed(0.2f, () =>
                        {
                            ev.Player.Position = point.Position;
                        }));
                    }
                } 
                else if (ev.Team.SpawnLocation == SpawnLocation.SCP079 || ev.Team.SpawnLocation == SpawnLocation.SCP096 || ev.Team.SpawnLocation == SpawnLocation.SCP106 || ev.Team.SpawnLocation == SpawnLocation.Shelter)
                {
                    if (Warhead.IsDetonated || Map.IsLCZDecontaminated)
                    {
                        Log.Debug("Not changing player position because the warhead is detonated", Instance.Config.Debug);
                    }
                    else
                    {
                        Instance.EventHandlers.coroutineHandle.Add(Timing.CallDelayed(0.2f, () =>
                        {
                            ev.Player.Position = point.Position;
                        }));
                    }
                }
                else
                {
                    Instance.EventHandlers.coroutineHandle.Add(Timing.CallDelayed(0.2f, () =>
                    {
                        ev.Player.Position = point.Position;
                    }));
                }
            }

            var ihandler = new TeamEvents.AddingInventoryItemsEventArgs(ev.Player, ev.Subclass, keepInv:ev.KeepItems);
            ihandler.StartInvoke();
            if (Instance.Config.UseHints)
            {
                ev.Player.ShowHint(ev.Subclass.RoleMessage, 10);
            }
            else
            {
                ev.Player.Broadcast(10, ev.Subclass.RoleMessage);
            }

            Instance.EventHandlers.coroutineHandle.Add(Timing.CallDelayed(0.2f, () => 
            {
                if (Instance.EventHandlers.spawnableTeamType == Respawning.SpawnableTeamType.NineTailedFox)
                {
                    ev.Player.UnitName = Respawning.RespawnManager.Singleton.NamingManager.AllUnitNames[Instance.EventHandlers.respawns].UnitName;
                }

                ev.Player.InfoArea &= ~PlayerInfoArea.Role;
                ev.Player.CustomInfo = ev.Subclass.RoleName;
                ev.Player.SetAdvancedTeam(ev.Team);
            }
            ));

            Log.Debug("Changing player " + ev.Player.Nickname + " to " + ev.Team.Name, Instance.Config.Debug);
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
