using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using TeamsEXILED.API;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace TeamsEXILED.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    class TeamsAlive : ICommand
    {
        public string Command { get; } = "teamsalive";

        public string[] Aliases { get; } = { "ta", "tma", "talive" };

        public string Description { get; } = "Tells all teams";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get((sender as CommandSender));
            if (ply.CheckPermission("ATC.teamsalive"))
            {
                response = "";
                foreach (KeyValuePair<Player, string> t in MainPlugin.Singleton.EventHandlers.teamedPlayers)
                {
                    response = response + "\n" + t.Value + " : " + t.Key.Nickname;
                }
                return true;
            }
            response = "<color=red>You dont have permission for this command</color>";
            return false;
        }
    }
}
