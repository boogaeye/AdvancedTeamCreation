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
    class ForceTeam : ICommand
    {
        public string Command { get; } = "forceteam";

        public string[] Aliases { get; } = { "ft" };

        public string Description { get; } = "Forces someone to be a team";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get((sender as CommandSender));
            if (arguments.ToList().Count == 0)
            {
                response = "<color=blue>forceteam <teamName> <subteamName> <playerId></color>";
                return false;
            }
            if (ply.CheckPermission("ATC.forceteam"))
            {
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
                        response = "<color=red>Error Could not find subclass</color>";
                        foreach (Subteams st in t.Subclasses)
                        {
                            if (arguments.ToList()[1].ToLower() == st.Name)
                            {
                                response = "<color=red>Error Player Not Found</color>";
                                if (Player.Get(arguments.ToList()[2].ToLower()).IsVerified)
                                {
                                    response = "<color=green>Changed players Team!!!</color>";
                                    MainPlugin.Singleton.EventHandlers.ChangeTeam(Player.Get(arguments.ToList()[2].ToLower()), arguments.ToList()[0].ToLower(), arguments.ToList()[1].ToLower());
                                    return true;
                                }
                                return false;
                            }
                        }
                        return false;
                    }
                }
            }
            else
            {
                response = "<color=red>You do not have permission to use this command aka you dont have ATC.forceteam</color>";
            }
            return false;
    }
    }
}
