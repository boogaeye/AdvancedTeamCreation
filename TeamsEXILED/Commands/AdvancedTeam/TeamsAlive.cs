using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using TeamsEXILED.API;

namespace TeamsEXILED.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TeamsAlive : ICommand
    {
        public string Command { get; } = "teamsalive";

        public string[] Aliases { get; } = { "ta", "tma", "talive" };

        public string Description { get; } = "Tells all teams";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(sender as CommandSender);
            if (ply.CheckPermission("ATC.teamsalive"))
            {
                response = "";
                foreach (KeyValuePair<Player, Teams> t in MainPlugin.Singleton.EventHandlers.teamedPlayers)
                {
                    response = response + "\n" + t.Value + " : " + t.Key.Nickname;
                }

                return true;
            }

            response = MainPlugin.Singleton.Translation.NoPermissions;
            return false;
        }
    }
}
