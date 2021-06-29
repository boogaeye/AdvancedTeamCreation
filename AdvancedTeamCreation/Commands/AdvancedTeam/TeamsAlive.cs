namespace AdvancedTeamCreation.Commands
{
    using System;
    using System.Collections.Generic;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using API;
    using static AdvancedTeamCreation;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TeamsAlive : ICommand
    {
        public string Command { get; } = "teamsalive";
        public string[] Aliases { get; } = { "ta", "tma", "talive" };
        public string Description { get; } = "Tells all teams";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender.CheckPermission("ATC.teamsalive"))
            {
                response = "";
                foreach (KeyValuePair<Player, AdvancedTeam> t in Instance.EventHandlers.teamedPlayers)
                {
                    response = response + "\n" + t.Value.Name + " : " + t.Key.Nickname;
                }

                return true;
            }

            response = Instance.Translation.NoPermissions;
            return false;
        }
    }
}
