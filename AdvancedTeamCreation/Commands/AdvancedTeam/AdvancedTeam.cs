namespace AdvancedTeamCreation.Commands
{
	using CommandSystem;
	using Exiled.Permissions.Extensions;
	using System;
	using System.Linq;
	using static AdvancedTeamCreation;

	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class AdvancedTeamCommand : ParentCommand
	{
		public AdvancedTeamCommand() => LoadGeneratedCommands();

		public override string Command { get; } = "advancedteamcreation";
		public override string[] Aliases { get; } = { "adtm", "adt", "atc" };
		public override string Description { get; } = "AdvancedTeam main command";
		public override void LoadGeneratedCommands()
		{
			RegisterCommand(new ForceNextTeam());
			RegisterCommand(new ForceTeam());
			RegisterCommand(new TeamsAlive());
		}

		protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (sender.CheckPermission("ATC.main"))
            {
				if (arguments.Count == 0)
                {
					response = "You need to enter an argument\n<b>forcenextteam</b>\n<b>forceteam</b>\n<b>teamsalive</b>\n<b>reload</b>";
					return true;
                }
				else if (arguments.Contains("reload"))
                {
					if (sender.CheckPermission("ATC.reload"))
                    {
						Instance.Config.LoadConfigs();
						response = "Done";
						return true;
					}
                    else 
					{
						response = Instance.Translation.NoPermissions;
						return false;
					}
				}
				else
                {
					response = "Invalid argument";
					return false;
                }
            }

			response = Instance.Translation.NoPermissions;
			return false;
		}
	}
}