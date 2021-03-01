using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsEXILED.API;
using Exiled.API.Features;
using TeamsEXILED.Classes;
using Exiled.API.Enums;

namespace TeamsEXILED
{
    public class MainPlugin : Plugin<Config>
    {
        private static readonly Lazy<MainPlugin> LazyInstance = new Lazy<MainPlugin>(valueFactory: () => new MainPlugin());
        static public MainPlugin instance => LazyInstance.Value;
        public override PluginPriority Priority { get; } = PluginPriority.First;
        public EventHandlers EventHandlers;
        private MainPlugin()
        {

        }
        public override void OnEnabled()
        {
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            Log.Info("Registered");
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.RACommand;
            Exiled.Events.Handlers.Server.EndingRound += EventHandlers.RoundEnding;
            Exiled.Events.Handlers.Player.Died += EventHandlers.OnDied;
            Exiled.Events.Handlers.Server.RespawningTeam += EventHandlers.Respawn;
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
        }

        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.RACommand;
            Exiled.Events.Handlers.Server.EndingRound -= EventHandlers.RoundEnding;
            Exiled.Events.Handlers.Player.Died -= EventHandlers.OnDied;
            Exiled.Events.Handlers.Server.RespawningTeam -= EventHandlers.Respawn;
        }
    }
}
