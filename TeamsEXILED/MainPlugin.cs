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

        public static MainPlugin Singleton;

        public override Version RequiredExiledVersion { get; } = new Version("2.8.0.0");

        public override PluginPriority Priority { get; } = PluginPriority.High;

        public override string Author { get; } = "BoogaEye";

        public override string Name { get; } = "Advanced Team Creation";

        public override Version Version { get; } = new Version("1.0.4.2");

        public static bool assemblyTimer = false;

        public static bool asseblyUIU = false;

        public static UIURescueSquad.Config uiuConfig = new UIURescueSquad.Config();

        public static RespawnTimer.Config rtconfig = new RespawnTimer.Config();


        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Singleton = this;

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

            Events.EventArgs.SetTeam += EventHandlers.OnTeamSpawn;

            Events.EventArgs.ReferencingTeam += EventHandlers.OnReferanceTeam;

            Events.EventArgs.CreatingTeam += EventHandlers.OnCreatingTeam;

            Exiled.Events.Handlers.Server.SendingConsoleCommand += EventHandlers.OnSendingCommand;

            if (!Server.FriendlyFire)
            {
                Log.Warn("Friendly Fire Is heavily recommended to be enabled on server config as it can lead to problems with people not being able to finish around because a person is supposed to be their enemy");
            }

            Timing.CallDelayed(2f, () =>
            {
                foreach (IPlugin<IConfig> plugin in Loader.Plugins)
                {
                    if (plugin.Name == "RespawnTimer" && plugin.Config.IsEnabled)
                    {
                        assemblyTimer = true;
                        rtconfig = (RespawnTimer.Config)plugin.Config;
                        Log.Debug("Got respawn timer configs", this.Config.Debug);
                        EventHandlers.mtfTrans = rtconfig.translations.Ntf;
                        EventHandlers.chaosTrans = rtconfig.translations.Ci;
                    }
                    if (plugin.Name == "UIU Rescue Squad" && plugin.Config.IsEnabled)
                    {
                        asseblyUIU = true;
                        uiuConfig = (UIURescueSquad.Config)plugin.Config;
                        uiuConfig.Probability = 0;
                        Log.Debug("Converting UIU into a playable team", this.Config.Debug);
                        //TeamConvert.ConvertPluginTeam(new Teams
                        //{
                        //    Active = true,
                        //    Name = "uiu",
                        //    SpawnTypes = new Respawning.SpawnableTeamType[] { Respawning.SpawnableTeamType.NineTailedFox },
                        //    spawnLocation = Enums.SpawnLocation.PluginHandle,
                        //    Chance = (ushort)uiuConfig.Probability,
                        //    Color = uiuConfig.UiuUnitColor,
                        //    Subclasses = new Subteams[] {
                        //        new Subteams { Name = uiuConfig.UiuLeaderRank.ToLower(), Ammo = uiuConfig.UiuLeaderAmmo, HP = uiuConfig.UiuLeaderLife, RoleName = uiuConfig.UiuLeaderRank, RoleHint = uiuConfig.UiuBroadcast, ModelRole = RoleType.NtfCommander, NumOfAllowedPlayers = 1 },
                        //        new Subteams {}
                        //    },
                        //});
                    }
                }
            });

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

            Events.EventArgs.SetTeam -= EventHandlers.OnTeamSpawn;

            Events.EventArgs.ReferencingTeam -= EventHandlers.OnReferanceTeam;

            Events.EventArgs.CreatingTeam -= EventHandlers.OnCreatingTeam;

            Exiled.Events.Handlers.Server.SendingConsoleCommand -= EventHandlers.OnSendingCommand;

            base.OnDisabled();
        }

        public void OnReload()
        {
        }
    }
}
