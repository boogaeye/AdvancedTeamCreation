using System;
using System.Linq;
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
            Player ply = Player.Get(sender as CommandSender);

            if (ply.CheckPermission("ATC.forceteam"))
            {
                if (arguments.Count == 0)
                {
                    response = "<color=blue>forceteam <teamName> <subteamName> <playerId></color>";
                    return false;
                }

                response = "<color=red>Error Team Does Not Exist</color>";
                foreach (Teams t in MainPlugin.Singleton.Config.Teams)
                {
                    if (t.Name == arguments.ToList()[0].ToLower())
                    {
                        if (!t.Active && !ply.CheckPermission("ATC.bypass"))
                        {
                            response = "<color=red>Error you cant force team this team as you dont have the ATC.bypass Permission</color>";
                            return false;
                        }

                        Teams team = null;
                        foreach (var tm in MainPlugin.Singleton.Config.Teams)
                        {
                            if (tm.Name.ToLower() == arguments.ToList()[0].ToLower())
                            {
                                team = tm;
                            }
                        }

                        if (team == null)
                        {
                            response = "<color=red>Team not found</color>";
                            return false;
                        }

                        Subteams steam = null;
                        foreach (var sb in team.Subclasses)
                        {
                            if (sb.Name.ToLower() == arguments.ToList()[0].ToLower())
                            {
                                steam = sb;
                            }
                        }

                        if (steam == null)
                        {
                            response = "<color=red>SubClass not found</color>";
                            return false;
                        }

                        var player = Player.Get(arguments.ToList()[2].ToLower());

                        player.SetAdvancedTeamSubteam(team, steam);

                        response = "<color=green>Changed player Team!!!</color>";
                        return true;
                    }
                }
            }
            else
            {
                response = MainPlugin.Singleton.Translation.NoPermissions;
            }
            return false;
        }
    }
}
