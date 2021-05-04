namespace AdvancedTeamCreation.Commands
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using API;
    using static AdvancedTeamCreation;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class TeamCommand : ICommand
    {
        public string Command { get; } = "team";
        public string[] Aliases { get; } = { "tm" };
        public string Description { get; } = "Tells you what team you are on";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender.CheckPermission("ATC.team"))
            {
                response = "<color=cyan>Team: </color>" + $"<b><color=white>{(sender as Player).AdvancedTeam().Name}</color></b>";
                return true;
            }

            response = Instance.Translation.NoPermissions;
            return false;
        }
    }
}
