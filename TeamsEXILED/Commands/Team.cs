using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using TeamsEXILED.API;

namespace TeamsEXILED.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class TeamCommand : ICommand
    {
        public string Command { get; } = "team";

        public string[] Aliases { get; } = { "tm" };

        public string Description { get; } = "Tells you what team you are on";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(sender as CommandSender);
            if (ply.CheckPermission("ATC.team"))
            {
                response = "<color=cyan>Team: </color>" + $"<b><color=white>{ply.AdvancedTeam().Name}</color></b>";
                return true;
            }

            response = "<color=red>You dont have permission for this command</color>";
            return false;
        }
    }
}
