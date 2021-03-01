using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using TeamsEXILED.API;
using Exiled.API.Enums;
using Exiled.Permissions.Extensions;
using TeamsEXILED.Classes;
using MEC;

namespace TeamsEXILED
{
    public class EventHandlers
    {
        private readonly Plugin<Config> plugin;
        public EventHandlers(Plugin<Config> plugin) => this.plugin = plugin;
        public Dictionary<Player, string> teamedPlayers = new Dictionary<Player, string>() {};
        public Teams chosenTeam;
        public Random random = new Random();
        public Classes.Classes Classes = new Classes.Classes();
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
                            ev.ReplyMessage = "<color=red>Error Player Not Found</color>";
                            if (Player.Get(ev.Arguments[1]).IsVerified)
                            {
                                ev.ReplyMessage = "<color=green>Changed players Team!!!</color>";
                                ChangeTeam(Player.Get(ev.Arguments[1]), ev.Arguments[0].ToLower());
                                return;
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
        }
        public void Respawn(RespawningTeamEventArgs ev)
        {
            chosenTeam = this.plugin.Config.Teams[random.Next(0, this.plugin.Config.Teams.Length)];
            foreach (Player p in ev.Players)
            {
                if (chosenTeam.SpawnTypes.Contains(ev.NextKnownTeam) && chosenTeam.Active)
                {
                    Timing.CallDelayed(1.5f, () =>
                    {
                        ChangeTeam(p, chosenTeam.Name);
                    });
                }
                else
                {
                    chosenTeam = null;
                }
            }
        }
        void ChangeTeam(Player p, string t)
        {
            foreach (Teams team in this.plugin.Config.Teams)
            {
                if (team.Name == t)
                {
                    p.SetRole(team.ModelRole, true);
                    p.ClearInventory();
                    foreach (ItemType i in team.Inventory)
                    {
                        p.AddItem(i);
                    }
                    foreach (System.Collections.Generic.KeyValuePair<AmmoType, uint> a in team.Ammo)
                    {
                        p.Ammo[(int)a.Key] = a.Value;
                    }
                }
            }
        }
        public void MTFSpawnAnnounce(AnnouncingNtfEntranceEventArgs ev)
        {
            if (chosenTeam != null)
            {
                ev.IsAllowed = false;
            }
        }
        public void OnDied(DiedEventArgs ev)
        {
            bool roundEndingAllowed = true;
            foreach (string t in teamedPlayers.Values)
            {
                if (Classes.IsTeamEnemy(Classes.GetTeamFromString(teamedPlayers[ev.Target], this.plugin.Config), t))
                {
                    roundEndingAllowed = false;
                }
            }
            if (roundEndingAllowed) { if (!Round.IsLocked) { Round.ForceEnd(); } }
        }
        public void RoundEnding(EndingRoundEventArgs ev)
        {
            //Not implemented yet
            ev.IsAllowed = true;
        }
    }
}
