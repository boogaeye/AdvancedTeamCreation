using System;
using CommandSystem;
using TeamsEXILED.API;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace TeamsEXILED.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ForceTeam : ICommand
    {
        public string Command { get; } = "forceteam";

        public string[] Aliases { get; } = { "ft" };

        public string Description { get; } = "Forces someone to be a team";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender.CheckPermission("ATC.forceteam"))
            {
                if (arguments.Count == 0)
                {
                    response = "<color=blue>forceteam <teamName> <subteamName> <playerId></color>";
                    return false;
                }

                if (!Teams.TryGet(arguments.At(0), out Teams team))
                {
                    response = "<color=red>Error Team Does Not Exist</color>";
                    return false;
                }

                if (!team.Active)
                {
                    response = "<color=red>The Team isn't active</color>";
                    return false;
                }

                if (!team.TryGetSubteam(arguments.At(1), out Subteams subteam))
                {
                    response = "<color=red>Error Subteam doesn't exist</color>";
                    return false;
                }

                if (Player.Get(arguments.At(2)) is Player player)
                {
                    player.SetAdvancedTeamSubteam(team, subteam);
                    response = "<color=green>Changed player Team!!!</color>";
                    return true;
                }
                else
                {
                    response = "<color=red>Error Player doesn't exist</color>";
                    return false;
                }
            }

            response = MainPlugin.Singleton.Translation.NoPermissions;
            return false;
        }
    }
}
