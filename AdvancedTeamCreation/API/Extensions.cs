namespace AdvancedTeamCreation.API
{
    using Exiled.API.Features;
    using System.Collections.Generic;
    using System.Linq;
    using Helper;
    using static AdvancedTeamCreation;

    public static class Extensions
    {
        public static AdvancedTeam AdvancedTeam(this Player player) => Instance.EventHandlers.teamedPlayers[player];

        public static void SetAdvancedTeam(this Player player, AdvancedTeam team)
        {
            Instance.EventHandlers.teamedPlayers[player] = team;
        }

        public static void SetAdvancedTeamSubteam(this Player ply, AdvancedTeam team, SubTeam subclass, bool KeepInv = false)
        {
            TeamMethods.ChangeTeam(ply, team, subclass, KeepInv);
        }

        public static bool IsNormalAdvancedTeam(this Player ply)
        {
            return TeamMethods.NormalTeamsNames.Contains(ply.AdvancedTeam().Name.ToLower());
        }

        public static bool IsTeamFriendly(this AdvancedTeam i, AdvancedTeam u)
        {
            return i.Friendlys.Contains(u.Name);
        }

        public static bool IsTeamEnemy(this AdvancedTeam i, AdvancedTeam u)
        {
            return i.Requirements.Contains(u.Name);
        }

        public static bool IsTeamNeutral(this AdvancedTeam i, AdvancedTeam u)
        {
            return i.Neutral.Contains(u.Name);
        }

        public static bool IsTeamAlive(this AdvancedTeam team)
        {
            return Instance.EventHandlers.teamedPlayers.ContainsValue(team);
        }

        public static string[] GetFriendlyTeams(this AdvancedTeam team)
        {
            return team.Friendlys;
        }

        public static List<string> GetAllRequirements(this AdvancedTeam Fteam)
        {
            List<string> team = new List<string>();
            foreach (AdvancedTeam t in Instance.Config.Teams.Where(x => x.Requirements.Contains(Fteam.Name)))
            {
                team.Add(t.Name);
            }

            return team;
        }

        public static List<string> GetAllNeutrals(this AdvancedTeam Fteam)
        {
            List<string> team = new List<string>();
            foreach (AdvancedTeam t in Instance.Config.Teams.Where(x => x.Neutral.Contains(Fteam.Name)))
            {
                team.Add(t.Name);
            }

            return team;
        }

        public static List<string> GetAllFriendlyTeams(this AdvancedTeam Fteam)
        {
            List<string> team = new List<string>();
            foreach (AdvancedTeam t in Instance.Config.Teams.Where(x => x.Friendlys.Contains(Fteam.Name)))
            {
                team.Add(t.Name);
            }

            return team;
        }

        public static AdvancedTeam GetNormalAdvancedTeam(this Team team)
        {
            return Instance.Config.Teams.First(x => x.Name == team.ToString().ToLower());
        }

        // Not Extensions at all ↓
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

        public static void SetAdvancedTeam(List<Player> plylist, AdvancedTeam team)
        {
            foreach (var ply in plylist)
            {
                Instance.EventHandlers.teamedPlayers[ply] = team;
            }
        }
    }
}
