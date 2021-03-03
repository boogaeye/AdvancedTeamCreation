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
using Exiled.API.Interfaces;

namespace TeamsEXILED
{
    public class MainPlugin : Plugin<Config>
    {
        public EventHandlers EventHandlers;
        public override Version RequiredExiledVersion { get; } = new Version("2.4.0.0");
        public override string Author { get; } = "BoogaEye";
        public override Version Version { get; } = new Version("1.0.0.0");
        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.RACommand;
            Exiled.Events.Handlers.Server.EndingRound += EventHandlers.RoundEnding;
            Exiled.Events.Handlers.Player.Died += EventHandlers.OnDied;
            Exiled.Events.Handlers.Server.RespawningTeam += EventHandlers.Respawn;
            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.OnRoleChange;
            Exiled.Events.Handlers.Player.Hurting += EventHandlers.OnHurt;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.RACommand;
            Exiled.Events.Handlers.Server.EndingRound -= EventHandlers.RoundEnding;
            Exiled.Events.Handlers.Player.Died -= EventHandlers.OnDied;
            Exiled.Events.Handlers.Server.RespawningTeam -= EventHandlers.Respawn;
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.OnRoleChange;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.OnHurt;
        }
    }
}
