using Exiled.API.Features;
using HarmonyLib;
using System;

namespace TeamsEXILED.Patches
{
    [HarmonyPatch(typeof(Respawning.RespawnTickets), nameof(Respawning.RespawnTickets.DrawRandomTeam))]
    public class RefNextTeam
    {
        public static void Postfix(ref Respawning.SpawnableTeamType __result)
        {
            try
            {
                if (!MainPlugin.Singleton.EventHandlers.HasReference)
                {
                    MainPlugin.Singleton.EventHandlers.TmMethods.RefNextTeamSpawn();
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}