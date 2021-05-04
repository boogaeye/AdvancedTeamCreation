namespace AdvancedTeamCreation.Patches
{
    using Exiled.API.Features;
    using Helper;
    using HarmonyLib;
    using System;
    using static AdvancedTeamCreation;

    [HarmonyPatch(typeof(Respawning.RespawnTickets), nameof(Respawning.RespawnTickets.DrawRandomTeam))]
    public class RefNextTeam
    {
        public static void Postfix(ref Respawning.SpawnableTeamType __result)
        {
            try
            {
                if (!Instance.EventHandlers.HasReference)
                {
                    TeamMethods.RefNextTeamSpawn(__result);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}