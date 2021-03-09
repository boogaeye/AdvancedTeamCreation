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
using MEC;
using Exiled.CustomItems.API.Features;

namespace TeamsEXILED
{
    public class MainPlugin : Plugin<Config>
    {
        public EventHandlers EventHandlers;
        public override Version RequiredExiledVersion { get; } = new Version("2.8.0.0");
        public override string Author { get; } = "BoogaEye";
        public override Version Version { get; } = new Version("1.0.0.1");
        public override void OnEnabled()
        {
            Timing.CallDelayed(5f, () => new TeamsEXILED.Others.ItemTest().TryRegister());
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.RACommand;
            Exiled.Events.Handlers.Server.EndingRound += EventHandlers.RoundEnding;
            Exiled.Events.Handlers.Player.Died += EventHandlers.OnDied;
            Exiled.Events.Handlers.Server.RespawningTeam += EventHandlers.Respawn;
            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.OnRoleChange;
            Exiled.Events.Handlers.Player.Hurting += EventHandlers.OnHurt;
            Exiled.Events.Handlers.Player.Verified += EventHandlers.OnJoin;
            Exiled.Events.Handlers.Player.Destroying += EventHandlers.OnLeave;
        }

        public override void OnDisabled()
        {
            new TeamsEXILED.Others.ItemTest().TryUnregister();
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.RACommand;
            Exiled.Events.Handlers.Server.EndingRound -= EventHandlers.RoundEnding;
            Exiled.Events.Handlers.Player.Died -= EventHandlers.OnDied;
            Exiled.Events.Handlers.Server.RespawningTeam -= EventHandlers.Respawn;
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.OnRoleChange;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.OnHurt;
            Exiled.Events.Handlers.Player.Verified -= EventHandlers.OnJoin;
            Exiled.Events.Handlers.Player.Destroying -= EventHandlers.OnLeave;
        }
    }
}
