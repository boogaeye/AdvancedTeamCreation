using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using System.Linq;

namespace TeamsEXILED
{
    public static class RTimerMethods
    {
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
    }
}
