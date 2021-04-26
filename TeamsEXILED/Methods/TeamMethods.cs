using System.Collections.Generic;
using System.Linq;
using TeamsEXILED.API;
using Exiled.API.Features;
using MEC;

namespace TeamsEXILED
{
    public class TeamMethods
    {
        
        public void RefNextTeamSpawn()
        {
            Log.Debug("Getting Team Referances", MainPlugin.Singleton.Config.Debug);
            var list = MainPlugin.Singleton.Config.Teams.Where(x => x.SpawnTypes.ToList().Contains(Respawn.NextKnownTeam) && x.Active == true).ToList();
            Log.Debug("Got list", MainPlugin.Singleton.Config.Debug);
            var team = list[MainPlugin.Singleton.EventHandlers.random.Next(0, list.Count)];
            Log.Debug("Got team", MainPlugin.Singleton.Config.Debug);

            var handler = new Events.General.ReferencingTeamEventArgs(MainPlugin.Singleton.EventHandlers.chosenTeam, Respawning.SpawnableTeamType.None)
            {
                Team = team
            };
            Log.Debug("Got Handler and invoking", MainPlugin.Singleton.Config.Debug);

            handler.StartInvoke();
        }

        public void RefNextTeamSpawn(Respawning.SpawnableTeamType spawnableTeamType)
        {
            Log.Debug("Getting Team Referances", MainPlugin.Singleton.Config.Debug);
            var list = MainPlugin.Singleton.Config.Teams.Where(x => x.SpawnTypes.Contains(spawnableTeamType) && x.Active == true).ToList();
            var team = list[MainPlugin.Singleton.EventHandlers.random.Next(0, list.Count)];

            var handler = new Events.General.ReferencingTeamEventArgs(MainPlugin.Singleton.EventHandlers.chosenTeam, spawnableTeamType)
            {
                Team = team,
                Spawning = spawnableTeamType
            };

            handler.StartInvoke();
        }

        public void ChangePlysToTeam(List<Player> p, Teams team)
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

        public void ChangeTeam(Player p, Teams t, Subteams s, bool keepInv = false)
        {
            var handler = new Events.General.SettingPlayerTeamEventArgs(t, s, p, escaping:keepInv);
            handler.StartInvoke();
        }

        public bool TeamExists(string team)
        {
            return MainPlugin.Singleton.EventHandlers.teamedPlayers.ContainsValue(team);
        }

        public void RemoveTeamReference()
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

        private IEnumerator<float> RemoveReferenceCouroutine()
        {
            yield return Timing.WaitForSeconds(5f);
            if (MainPlugin.assemblyTimer)
            {
                DefaultTimerConfig();
            }

            MainPlugin.Singleton.EventHandlers.chosenTeam = null;
            MainPlugin.Singleton.EventHandlers.HasReference = false;
        }

        public void DefaultTimerConfig()
        {
            var cfg = (RespawnTimer.Config)Methods.GetRespawnTimerCfg();
            cfg.translations.Ci = MainPlugin.Singleton.EventHandlers.chaosTrans;
            cfg.translations.Ntf = MainPlugin.Singleton.EventHandlers.mtfTrans;
        }
    }
}
