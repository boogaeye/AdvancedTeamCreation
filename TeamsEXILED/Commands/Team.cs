using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace TeamsEXILED.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    class Team : ICommand
    {
        public string Command { get; } = "team";

        public string[] Aliases { get; } = { "tm" };

        public string Description { get; } = "Tells you what team your on";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(sender as CommandSender);
            if (ply.CheckPermission("ATC.team"))
            {
                response = "<color=cyan>Team: </color>" + $"<b><color=white>{MainPlugin.Singleton.EventHandlers.teamedPlayers[ply]}</color></b>";
                return true;
            }

            response = "<color=red>You dont have permission for this command</color>";
            return false;
        }
    }
}
