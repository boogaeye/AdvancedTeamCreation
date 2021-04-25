using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using TeamsEXILED.API;

namespace TeamsEXILED.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    class ForceNextTeam : ICommand
    {
        public string Command { get; } = "forcenextteam";

        public string[] Aliases { get; } = { "fnt", "forcent", "fnteam" };

        public string Description { get; } = "Force the next spawneable team";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(sender as CommandSender);
            if (ply.CheckPermission("ATC.forcenextteam"))
            {
                if (arguments.Count <= 0)
                {
                    response = "<color=red>You need to add the team name.</color>\nUsage: fnt teamname";
                    return false;
                }

                Teams team = null;

                foreach (var tm in MainPlugin.Singleton.Config.Teams)
                {
                    if (tm.Name == arguments.At(0))
                    {
                        team = tm;
                    }
                }

                if (team == null)
                {
                    response = "<color=red>The name of the team isn't valid.</color> Teams:";
                    foreach (var tm in MainPlugin.Singleton.Config.Teams)
                    {
                        response += "\n" + tm.Name;
                    }
                    return false;
                }

                MainPlugin.Singleton.EventHandlers.ForcedTeam = true;
                MainPlugin.Singleton.EventHandlers.HasReference = true;
                MainPlugin.Singleton.EventHandlers.chosenTeam = team;

                response = $"<color=green> Done, {arguments.At(0)} team forced";

                return true;
            }

            response = "<color=red>You dont have permission for this command</color>";
            return false;
        }
    }
}
