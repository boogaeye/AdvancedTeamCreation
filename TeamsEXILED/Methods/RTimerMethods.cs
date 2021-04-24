using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsEXILED.API;
using Exiled.API.Features;
using TeamsEXILED.Enums;
using Exiled.API.Interfaces;
using MEC;

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

                if (MainPlugin.Singleton.EventHandlers.HasReference && MainPlugin.Singleton.EventHandlers.chosenTeam != null)
                {
                    switch (Respawn.NextKnownTeam)
                    {
                        case Respawning.SpawnableTeamType.NineTailedFox:
                            cfg.translations.Ntf = $"<color={MainPlugin.Singleton.EventHandlers.chosenTeam.Color}>{MainPlugin.Singleton.EventHandlers.chosenTeam.Name}</color>";
                            break;
                        case Respawning.SpawnableTeamType.ChaosInsurgency:
                            cfg.translations.Ci = $"<color={MainPlugin.Singleton.EventHandlers.chosenTeam.Color}>{MainPlugin.Singleton.EventHandlers.chosenTeam.Name}</color>";
                            break;
                    }
                }
            }
        }
    }
}
