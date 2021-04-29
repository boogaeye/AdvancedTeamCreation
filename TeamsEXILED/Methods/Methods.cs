﻿using System.Linq;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using MEC;
using TeamsEXILED.API;
using System.Collections.Generic;

namespace TeamsEXILED
{
    public static class Methods
    {
        public static string PointThings(SpawnLocation loc)
        {
            string name = "";
            switch (loc)
            {
                case SpawnLocation.Escape:
                {
                    name = "Outside:187.4,-7.2,-14.4:356.5,179.7";
                    break;
                }
                case SpawnLocation.SurfaceNuke:
                {
                    name = "Outside:40.6,-11.0,-42.6:1.5,180.3";
                    break;
                }
                case SpawnLocation.Shelter:
                {
                    name = "EZ_Shelter:0.0,1.4,5.8:-3.1,181.5";
                    break;
                }
                case SpawnLocation.SCP173:
                {
                    name = "LCZ_173:12.2,17.2,13.3:0.0,90.3";
                    break;
                }
                case SpawnLocation.SCP106:
                {
                    name = "HCZ_106:19.6,1.4,0.1:0.1,269.9";
                    break;
                }
                case SpawnLocation.SCP096:
                {
                    name = "HCZ_457:-7.7,1.4,0.0:-1.2,90.3";
                    break;
                }
                case SpawnLocation.SCP079:
                {
                    name = "HCZ_079:-1.2,1.4,0.1:0.3,269.7";
                    break;
                }
                case SpawnLocation.SCP012:
                {
                    name = "LCZ_012:6.4,1.4,-0.1:1.5,270.9";
                    break;
                }
            }

            return name;
        }

        public static IEnumerator<float> RespawnTimerPatch()
        {
            var cfg = (RespawnTimer.Config)Methods.GetRespawnTimerCfg();
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(cfg.Interval - 0.01f);

                var rteam = Respawn.NextKnownTeam;

                if (MainPlugin.Singleton.EventHandlers.ForcedTeam)
                {
                    rteam = MainPlugin.Singleton.EventHandlers.chosenTeam.SpawnTypes.FirstOrDefault();
                }

                if (MainPlugin.Singleton.EventHandlers.HasReference && MainPlugin.Singleton.EventHandlers.chosenTeam != null)
                {
                    switch (rteam)
                    {
                        case Respawning.SpawnableTeamType.NineTailedFox:
                            {
                                cfg.translations.Ntf = $"<color={MainPlugin.Singleton.EventHandlers.chosenTeam.Color}>{MainPlugin.Singleton.EventHandlers.chosenTeam.Name}</color>";
                                break;
                            }
                        case Respawning.SpawnableTeamType.ChaosInsurgency:
                            {
                                cfg.translations.Ci = $"<color={MainPlugin.Singleton.EventHandlers.chosenTeam.Color}>{MainPlugin.Singleton.EventHandlers.chosenTeam.Name}</color>";
                                break;
                            }
                    }
                }
            }
        }

        public static IConfig GetRespawnTimerCfg()
        {
            return Exiled.Loader.Loader.Plugins.First(x => x.Name == "RespawnTimer").Config;
        }

        public static IConfig GetUIUCfg()
        {
            return Exiled.Loader.Loader.Plugins.First(x => x.Name == "UIU Rescue Squad").Config;
        }

        public static IConfig GetSerpentCfg()
        {
            return Exiled.Loader.Loader.Plugins.First(x => x.Name == "SerpentsHand").Config;
        }

        public static void StartRT()
        {
            var cfg = (RespawnTimer.Config)GetRespawnTimerCfg();
            Log.Debug("Got respawn timer configs", MainPlugin.Singleton.Config.Debug);
            MainPlugin.Singleton.EventHandlers.mtfTrans = cfg.translations.Ntf;
            MainPlugin.Singleton.EventHandlers.chaosTrans = cfg.translations.Ci;
        }

        public static bool IsUIU()
        {
            return UIURescueSquad.EventHandlers.IsSpawnable;
        }

        public static bool IsSerpentHand()
        {
            return SerpentsHand.EventHandlers.IsSpawnable;
        }

        public static void SpawneableUIUToFalse()
        {
            UIURescueSquad.EventHandlers.IsSpawnable = false;
        }

        public static void SpawneableSerpentToFalse()
        {
            SerpentsHand.EventHandlers.IsSpawnable = false;
        }

        public static void CheckRoundEnd(Teams team)
        {
            if (RoundSummary.singleton._roundEnded)
            {
                return;
            }

            if (team.IsNormalTeam() || team == SerpentHandsTeam || team == UiUTeam)
            {
                return;
            }

            var teamedPlayers = MainPlugin.Singleton.EventHandlers.teamedPlayers;

            // Checking round for end
            foreach (var te in teamedPlayers.Values)
            {
                if (team.Requirements.Contains(te.Name))
                {
                    Log.Debug($"Stopped Round from ending heres some information\n Triggered Team: {team.Name}\n Stopping Team: {te}\n Hope this works", MainPlugin.Singleton.Config.Debug);
                    return;
                }
            }
            
            if (!MainPlugin.Singleton.EventHandlers.AllowNormalRoundEnd)
            {
                MainPlugin.Singleton.EventHandlers.AllowNormalRoundEnd = true;
            }

            MainPlugin.Singleton.EventHandlers.coroutineHandle.Add(Timing.CallDelayed(0.2f, () =>
            {
                if (!RoundSummary.singleton._roundEnded)
                {
                    Round.ForceEnd();
                }
            }));
        }

        public static void DefaultTimerConfig()
        {
            var cfg = (RespawnTimer.Config)GetRespawnTimerCfg();
            cfg.translations.Ci = MainPlugin.Singleton.EventHandlers.chaosTrans;
            cfg.translations.Ntf = MainPlugin.Singleton.EventHandlers.mtfTrans;
        }

        public static Teams UiUTeam = new Teams()
        {
            Active = true,
            Name = "uiu"
        };

        public static Teams SerpentHandsTeam = new Teams()
        {
            Active = true,
            Name = "serpenthands"
        };
    }
}
