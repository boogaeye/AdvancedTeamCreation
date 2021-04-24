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
using Exiled.API.Extensions;

namespace TeamsEXILED
{
    public class TeamsEvents
    {
        private readonly Plugin<Config> plugin;

        public TeamsEvents(Plugin<Config> plugin)
        {
            this.plugin = plugin;
        }

        public void OnReferanceTeam(Events.EventArgs.TeamReferencedEventArgs ev)
        {
            Log.Debug($"Forceteam: {ev.ForceTeam}\nIsAllowed: {ev.IsAllowed}\nTeamName: {ev.Team.Name}", this.plugin.Config.Debug);
        }

        public void OnCreatingTeam(Events.EventArgs.CreatingTeamEventArgs ev)
        {
            Log.Debug("Creating Team Event Called", this.plugin.Config.Debug);
            /*if (Teams.IsDefinedInConfig(ev.Team.Name, this.plugin.Config))
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
            }*/
        }

        public void OnTeamSpawn(Events.EventArgs.SetTeamEventArgs ev)
        {
            if (!Round.IsStarted)
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint("Couldnt spawn you in before the round started");
            }

            if (MainPlugin.Singleton.EventHandlers.HasReference)
            {
                if (ev.Player.Role == RoleType.Spectator)
                {
                    ev.IsAllowed = false;
                    ev.Player.ShowHint("Couldnt spawn you in because you are already spawning in");
                }
            }
        }

        public void OnAddingInventoryItems(Events.EventArgs.AddingInventoryItemsEventArgs ev)
        {
            Log.Debug($"Giving Inventory Items of the subclass {ev.Subteam.Name}, to {ev.Player.Nickname}", this.plugin.Config.Debug);
            if (ev.IsAllowed == false)
            {
                return;
            }

            ev.Player.ClearInventory();

            foreach (string i in ev.Subteam.Inventory)
            {
                if (ItemType.TryParse<ItemType>(i, out ItemType item))
                {
                    ev.Player.AddItem(item);
                }
                else if (int.TryParse(i, out int citem))
                {
                    CustomItem.TryGive(ev.Player, citem, plugin.Config.DisplayDescription);
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
