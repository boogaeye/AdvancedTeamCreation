﻿using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;
using System;
using System.Linq;
using TeamsEXILED.Commands;

namespace TeamsEXILED.Commands
{
	[CommandHandler(typeof(ClientCommandHandler))]
	public class AdvancedTeamCommand : ParentCommand
	{
		public AdvancedTeamCommand() => LoadGeneratedCommands();

		public override string Command { get; } = "advancedteam";

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
			Player ply = Player.Get(((CommandSender)sender).Nickname);
			if (ply.CheckPermission("ATC.main"))
            {
				if (arguments.Count == 0)
                {
					response = "<color=cyan>You need to enter an argument</color>\n<b>forcenextteam</b>\n<b>forceteam</b>\n<b>teamsalive</b>";
					return true;
                }
            }

			response = MainPlugin.Singleton.Config.TransConfigs.NoPermissions;
			return false;
		}
	}
}