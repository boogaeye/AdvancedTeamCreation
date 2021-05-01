using System.Collections.Generic;
using System.Linq;
using TeamsEXILED.API;
using Exiled.API.Features;
using MEC;

namespace TeamsEXILED
{
    public static class TeamMethods
    {
        // Not in use
        public static void RefNextTeamSpawn()
        {
            Log.Debug("Getting Team Referances", MainPlugin.Singleton.Config.Debug);
            var list = MainPlugin.Singleton.Config.Teams.Where(x => x.SpawnTypes.ToList().Contains(Respawn.NextKnownTeam) && x.Active == true).ToList();
            Log.Debug("Got list", MainPlugin.Singleton.Config.Debug);
            var team = list[MainPlugin.Singleton.EventHandlers.random.Next(0, list.Count)];
            Log.Debug("Got team", MainPlugin.Singleton.Config.Debug);

            var handler = new TeamEvents.ReferencingTeamEventArgs(team, Respawning.SpawnableTeamType.None);
            Log.Debug("Got Handler and invoking", MainPlugin.Singleton.Config.Debug);

            handler.StartInvoke();
        }

        public static void RefNextTeamSpawn(Respawning.SpawnableTeamType spawnableTeamType)
        {
            Log.Debug("Getting Team Referances", MainPlugin.Singleton.Config.Debug);
            Log.Debug($"Spawning on side {spawnableTeamType}", MainPlugin.Singleton.Config.Debug);
            var list = MainPlugin.Singleton.Config.Teams.Where(x => x.SpawnTypes.Contains(spawnableTeamType) && x.Active == true).ToList();
            if (list.Count == 0)
            {
                MainPlugin.Singleton.EventHandlers.HasReference = true;
                return;
            }

            var team = list[MainPlugin.Singleton.EventHandlers.random.Next(0, list.Count)];

            var handler = new TeamEvents.ReferencingTeamEventArgs(team, spawnableTeamType);

            handler.StartInvoke();
        }

        public static void ChangePlysToTeam(List<Player> p, Teams team)
        {
            //finding teams
            Log.Debug("Got team " + team.Name + " from referance method", MainPlugin.Singleton.Config.Debug);
            int i = 0;
            int selectedSubclass = 0;

            foreach (Player y in p)
            {
                if (team.Subclasses[selectedSubclass].NumOfAllowedPlayers > i)
                {
                    ChangeTeam(y, team, team.Subclasses[selectedSubclass]);
                    Log.Debug("allowed subteam " + team.Subclasses[selectedSubclass].Name + " from referance method", MainPlugin.Singleton.Config.Debug);
                }
                else if (team.Subclasses[selectedSubclass].NumOfAllowedPlayers == -1)
                {
                    ChangeTeam(y, team, team.Subclasses[selectedSubclass]);
                    Log.Debug("allowed subteam " + team.Subclasses[selectedSubclass].Name + " from referance method with -1 players allowed(making everyone else this role)", MainPlugin.Singleton.Config.Debug);
                }
                else
                {
                    i = 0;
                    selectedSubclass++;
                    Log.Debug("Going to the next subclass, because the max number of players is full", MainPlugin.Singleton.Config.Debug);
                }
                i++;
            }
        }

        public static void ChangeTeam(Player p, Teams t, Subteams s, bool keepInv = false)
        {
            var handler = new TeamEvents.SettingPlayerTeamEventArgs(t, s, p, keepItems:keepInv);
            handler.StartInvoke();
        }

        public static void RemoveTeamReference()
        {
            if (MainPlugin.Singleton.EventHandlers.RemoveChosenTeam != null)
            {
                if (MainPlugin.Singleton.EventHandlers.RemoveChosenTeam.IsRunning)
                {
                    Timing.KillCoroutines(MainPlugin.Singleton.EventHandlers.RemoveChosenTeam);
                }

                var cor = Timing.RunCoroutine(RemoveReferenceCouroutine());
                MainPlugin.Singleton.EventHandlers.RemoveChosenTeam = cor;
                MainPlugin.Singleton.EventHandlers.coroutineHandle.Add(cor);
            }
        }

        private static IEnumerator<float> RemoveReferenceCouroutine()
        {
            yield return Timing.WaitForSeconds(5f);
            if (MainPlugin.assemblyTimer)
            {
                Methods.DefaultTimerConfig();
            }

            MainPlugin.Singleton.EventHandlers.chosenTeam = null;
            MainPlugin.Singleton.EventHandlers.HasReference = false;
        }
    }
}
