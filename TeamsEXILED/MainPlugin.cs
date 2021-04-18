﻿using System;
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

        public static MainPlugin Singleton;

        public override Version RequiredExiledVersion { get; } = new Version("2.8.0.0");

        public override string Author { get; } = "BoogaEye";

        public override string Name { get; } = "Advanced Team Creation";

        public override Version Version { get; } = new Version("1.0.4.0");

        public static bool assemblyTimer = false;

        public static RespawnTimer.Config rtconfig = new RespawnTimer.Config();


        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Singleton = this;

            Exiled.Events.Handlers.Server.EndingRound += EventHandlers.RoundEnding;

            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.WaitingForPlayers;

            Exiled.Events.Handlers.Player.Died += EventHandlers.OnDied;

            Exiled.Events.Handlers.Server.RespawningTeam += EventHandlers.OnRespawning;

            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.OnRoleChange;

            Exiled.Events.Handlers.Player.Hurting += EventHandlers.OnHurt;

            Exiled.Events.Handlers.Player.Verified += EventHandlers.OnVerified;

            Exiled.Events.Handlers.Player.Left += EventHandlers.OnLeave;

            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += EventHandlers.MTFSpawnAnnounce;

            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;

            Events.EventArgs.SetTeam += EventHandlers.OnTeamSpawn;

            Events.EventArgs.ReferencingTeam += EventHandlers.OnReferanceTeam;

            Events.EventArgs.CreatingTeam += EventHandlers.OnCreatingTeam;

            if (!Server.FriendlyFire)
            {
                Log.Warn("Friendly Fire Is heavily recommended to be enabled on server config as it can lead to problems with people not being able to finish around because a person is supposed to be their enemy");
            }

            foreach (IPlugin<IConfig> plugin in Loader.Plugins)
            {
                if (plugin.Name == "RespawnTimer" && plugin.Config.IsEnabled)
                {
                    Timing.CallDelayed(5f, () =>
                    {
                        assemblyTimer = true;
                        rtconfig = (RespawnTimer.Config)plugin.Config;
                        Log.Debug("Got respawn timer configs", this.Config.Debug);
                        EventHandlers.mtfTrans = rtconfig.translations.Ntf;
                        EventHandlers.chaosTrans = rtconfig.translations.Ci;
                    });
                }
            }

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Timing.KillCoroutines();

            Exiled.Events.Handlers.Server.EndingRound -= EventHandlers.RoundEnding;

            Exiled.Events.Handlers.Player.Died -= EventHandlers.OnDied;

            Exiled.Events.Handlers.Server.RespawningTeam -= EventHandlers.OnRespawning;

            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.OnRoleChange;

            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.OnHurt;

            Exiled.Events.Handlers.Player.Verified -= EventHandlers.OnVerified;

            Exiled.Events.Handlers.Player.Left -= EventHandlers.OnLeave;

            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= EventHandlers.MTFSpawnAnnounce;

            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.WaitingForPlayers;

            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;

            Events.EventArgs.SetTeam -= EventHandlers.OnTeamSpawn;

            Events.EventArgs.ReferencingTeam -= EventHandlers.OnReferanceTeam;

            Events.EventArgs.CreatingTeam -= EventHandlers.OnCreatingTeam;

            base.OnDisabled();
        }
    }
}
