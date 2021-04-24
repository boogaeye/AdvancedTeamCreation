using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsEXILED.API;
using Exiled.API.Features;
using Exiled.Loader;
using TeamsEXILED.Classes;
using Exiled.API.Enums;
using System.Reflection;
using HarmonyLib;
using Exiled.API.Interfaces;
using MEC;
using Exiled.CustomItems.API.Features;

namespace TeamsEXILED
{
    public class MainPlugin : Plugin<Config>
    {
        public EventHandlers EventHandlers;

        public TeamsEvents TeamsHandlers;

        private Harmony Harmony;

        public static MainPlugin Singleton;

        public Classes.Classes Classes = new Classes.Classes();
        public TeamMethods TmMethods = new TeamMethods();

        public override Version RequiredExiledVersion { get; } = new Version("2.10.0");

        public override PluginPriority Priority { get; } = PluginPriority.High;

        public override string Author { get; } = "BoogaEye";

        public override string Name { get; } = "Advanced Team Creation";

        public override Version Version { get; } = new Version("1.0.4.2");

        public static bool assemblyTimer = false;

        public static bool assemblyUIU = false;

        public static bool assemblySerpentHands = false;

        public override void OnEnabled()
        {
            Singleton = this;
            TeamsHandlers = new TeamsEvents(this);
            EventHandlers = new EventHandlers(this);

            CheckPlugins();

            Harmony = new Harmony($"teamsexiled.{DateTime.Now.Ticks}");
            Harmony.PatchAll();

            Exiled.Events.Handlers.Server.EndingRound += EventHandlers.RoundEnding;
            Exiled.Events.Handlers.Server.RestartingRound += EventHandlers.OnRestartRound;
            Exiled.Events.Handlers.Player.Died += EventHandlers.OnDied;
            Exiled.Events.Handlers.Server.RespawningTeam += EventHandlers.OnRespawning;
            Exiled.Events.Handlers.Player.ChangedRole += EventHandlers.OnRoleChange;
            Exiled.Events.Handlers.Player.Hurting += EventHandlers.OnHurt;
            Exiled.Events.Handlers.Player.Verified += EventHandlers.OnVerified;
            Exiled.Events.Handlers.Player.Left += EventHandlers.OnLeave;
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += EventHandlers.MTFSpawnAnnounce;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;

            Events.EventArgs.SetTeam += TeamsHandlers.OnTeamSpawn;
            Events.EventArgs.AddingInventoryItems += TeamsHandlers.OnAddingInventoryItems;
            Events.EventArgs.ReferencingTeam += TeamsHandlers.OnReferanceTeam;
            Events.EventArgs.CreatingTeam += TeamsHandlers.OnCreatingTeam;
            

            if (!Server.FriendlyFire)
            {
                Log.Warn("Friendly Fire Is heavily recommended to be enabled on server config as it can lead to problems with people not being able to finish around because a person is supposed to be their enemy");
            }

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.EndingRound -= EventHandlers.RoundEnding;
            Exiled.Events.Handlers.Player.Died -= EventHandlers.OnDied;
            Exiled.Events.Handlers.Server.RespawningTeam -= EventHandlers.OnRespawning;
            Exiled.Events.Handlers.Player.ChangedRole -= EventHandlers.OnRoleChange;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.OnHurt;
            Exiled.Events.Handlers.Player.Verified -= EventHandlers.OnVerified;
            Exiled.Events.Handlers.Player.Left -= EventHandlers.OnLeave;
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= EventHandlers.MTFSpawnAnnounce;
            Exiled.Events.Handlers.Server.RestartingRound -= EventHandlers.OnRestartRound;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;

            Events.EventArgs.SetTeam -= TeamsHandlers.OnTeamSpawn;
            Events.EventArgs.AddingInventoryItems -= TeamsHandlers.OnAddingInventoryItems;
            Events.EventArgs.ReferencingTeam -= TeamsHandlers.OnReferanceTeam;
            Events.EventArgs.CreatingTeam -= TeamsHandlers.OnCreatingTeam;

            Harmony.UnpatchAll();

            Singleton = null;
            EventHandlers = null;
            TeamsHandlers = null;
            Harmony = null;

            base.OnDisabled();
        }

        public void CheckPlugins()
        {
            foreach (IPlugin<IConfig> plugin in Loader.Plugins)
            {
                if (plugin.Name == "RespawnTimer" && plugin.Config.IsEnabled)
                {
                    assemblyTimer = true;
                    Methods.StartRT();
                    Log.Debug("RespawnTimer assembly found", this.Config.Debug);
                }

                if (plugin.Name == "UIU Rescue Squad" && plugin.Config.IsEnabled)
                {
                    assemblyUIU = true;
                    Log.Debug("UIU assembly found", this.Config.Debug);
                }

                if (plugin.Name == "SerpentsHand" && plugin.Config.IsEnabled)
                {
                    assemblySerpentHands = true;
                    Log.Debug("SerpentHands assembly found", this.Config.Debug);
                }
            }
        }
    }
}
