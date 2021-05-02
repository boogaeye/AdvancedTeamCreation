using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;

namespace TeamsEXILED.API
{
    public static class Extensions
    {
        public static Teams AdvancedTeam(this Player player) => MainPlugin.Singleton.EventHandlers.teamedPlayers[player];

        public static void SetAdvancedTeam(this Player player, Teams team)
        {
             MainPlugin.Singleton.EventHandlers.teamedPlayers[player] = team;
        }

        public static void SetAdvancedTeamSubteam(this Player ply, Teams team, Subteams subclass, bool KeepInv = false)
        {
            TeamMethods.ChangeTeam(ply, team, subclass, KeepInv);
        }

        public static bool IsTeamFriendly(this Teams i, Teams u)
        {
            return i.Friendlys.Contains(u.Name);
        }

        public static bool IsTeamEnemy(this Teams i, Teams u)
        {
            return i.Requirements.Contains(u.Name);
        }

        public static bool IsTeamNeutral(this Teams i, Teams u)
        {
            return i.Neutral.Contains(u.Name);
        }

        public static bool IsNormalTeam(this Teams i)
        {
            List<string> names = new List<string>();

            foreach (var tm in MainPlugin.Singleton.Config.NormalConfigs.NTeams)
            {
                names.Add(tm.Name.ToLower());
            }

            return names.Contains(i.Name);
        }

        public static bool IsTeamAlive(this Teams team)
        {
            return MainPlugin.Singleton.EventHandlers.teamedPlayers.ContainsValue(team);
        }

        public static string[] GetFriendlyTeams(this Teams team)
        {
            return team.Friendlys;
        }

        public static List<string> GetAllRequirements(this Teams Fteam)
        {
            List<string> team = new List<string>();

            foreach (Teams t in MainPlugin.Singleton.Config.Teams.Where(x => x.Requirements.Contains(Fteam.Name)))
            {
                team.Add(t.Name);
            }

            return team;
        }

        public static List<string> GetAllNeutrals(this Teams Fteam)
        {
            List<string> team = new List<string>();

            foreach (Teams t in MainPlugin.Singleton.Config.Teams.Where(x => x.Neutral.Contains(Fteam.Name)))
            {
                team.Add(t.Name);
            }

            return team;
        }

        public static List<string> GetAllFriendlyTeams(this Teams Fteam)
        {
            List<string> team = new List<string>();

            foreach (Teams t in MainPlugin.Singleton.Config.Teams.Where(x => x.Friendlys.Contains(Fteam.Name)))
            {
                team.Add(t.Name);
            }

            return team;
        }

        public static Teams GetNormalAdvancedTeam(this Team team)
        {
            return MainPlugin.Singleton.Config.Teams.First(x => x.Name == team.ToString().ToLower());
        }

        // Not Extensions at all ↓
        public static Teams GetTeamFromString(string s)
        {
            Teams team = null;
            foreach (Teams t in MainPlugin.Singleton.Config.Teams)
            {
                if (t.Name.ToLower() == s.ToLower())
                {
                    team = t;
                }
            }

            return team;
        }

        // This can be added to Map of EXILED
        public static void RenameUnit(int index, string name)
        {
            var unit = Respawning.RespawnManager.Singleton.NamingManager.AllUnitNames[index];
            unit.UnitName = name;
            Respawning.RespawnManager.Singleton.NamingManager.AllUnitNames.Remove(Respawning.RespawnManager.Singleton.NamingManager.AllUnitNames[index]);
            Respawning.NamingRules.UnitNamingRules.AllNamingRules[Respawning.SpawnableTeamType.NineTailedFox].AddCombination(unit.UnitName, Respawning.SpawnableTeamType.NineTailedFox);

            foreach (var ply in Player.List.Where(x => x.UnitName == unit.UnitName))
            {
                ply.UnitName = unit.UnitName;
            }
        }

        public static void SetAdvancedTeam(List<Player> plylist, Teams team)
        {
            foreach (var ply in plylist)
            {
                MainPlugin.Singleton.EventHandlers.teamedPlayers[ply] = team;
            }
        }
    }
}
