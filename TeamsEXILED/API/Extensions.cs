using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;

namespace TeamsEXILED.API
{
    public static class Extensions
    {
        public static Teams AdvancedTeam(this Player player) => GetTeamFromString(MainPlugin.Singleton.EventHandlers.teamedPlayers[player]);

        public static void SetPlayerTeamName(this Player player, string name)
        {
             MainPlugin.Singleton.EventHandlers.teamedPlayers[player] = name;
        }

        public static void SetPlayerTeamName(List<Player> plylist, string name)
        {
            foreach (var ply in plylist)
            {
                MainPlugin.Singleton.EventHandlers.teamedPlayers[ply] = name;
            }
        }

        public static bool IsTeamFriendly(this Teams i, string u)
        {
            return i.Friendlys.Contains(u);
        }

        public static bool IsTeamEnemy(this Teams i, string u)
        {
            return i.Requirements.Contains(u);
        }

        public static bool IsTeamNeutral(this Teams i, string u)
        {
            return i.Neutral.Contains(u);
        }

        public static bool TeamExists(string team)
        {
            return MainPlugin.Singleton.EventHandlers.teamedPlayers.ContainsValue(team);
        }

        public static Teams GetTeamFromString(string s)
        {
            Teams team = null;
            foreach (Teams t in MainPlugin.Singleton.Config.TeamsConfigs.Teams)
            {
                if (t.Name.ToLower() == s.ToLower())
                {
                    team = t;
                }
            }

            return team;
        }

        public static void RenameUnit(int index, string name)
        {
            var unit = Respawning.RespawnManager.Singleton.NamingManager.AllUnitNames[index];
            unit.UnitName = name;
            Respawning.RespawnManager.Singleton.NamingManager.AllUnitNames.Remove(Respawning.RespawnManager.Singleton.NamingManager.AllUnitNames[index]);
            Respawning.NamingRules.UnitNamingRules.AllNamingRules[Respawning.SpawnableTeamType.NineTailedFox].AddCombination($"{unit.UnitName}", Respawning.SpawnableTeamType.NineTailedFox);

            foreach (var ply in Player.List.Where(x => x.ReferenceHub.characterClassManager.CurUnitName == unit.UnitName))
            {
                ply.ReferenceHub.characterClassManager.NetworkCurUnitName = unit.UnitName;
            }
        }

        public static void AddUnit(string name, Player[] players)
        {
            Respawning.NamingRules.UnitNamingRules.AllNamingRules[Respawning.SpawnableTeamType.NineTailedFox].AddCombination(name, Respawning.SpawnableTeamType.NineTailedFox);

            foreach (var ply in players)
            {
                ply.ReferenceHub.characterClassManager.NetworkCurUnitName = name;
            }
        }

        public static string[] GetFriendlyTeams(Teams team)
        {
            return team.Friendlys;
        }

        public static List<string> GetAllRequirements(string TeamFond)
        {
            List<string> team = new List<string>();

            foreach (Teams t in MainPlugin.Singleton.Config.TeamsConfigs.Teams.Where(x => x.Requirements.Contains(TeamFond)))
            {
                team.Add(t.Name);
            }

            return team;
        }

        public static List<string> GetAllNeutrals(string TeamFond)
        {
            List<string> team = new List<string>();

            foreach (Teams t in MainPlugin.Singleton.Config.TeamsConfigs.Teams.Where(x => x.Neutral.Contains(TeamFond)))
            {
                team.Add(t.Name);
            }

            return team;
        }

        public static List<string> GetAllFriendlyTeams(string TeamFond)
        {
            List<string> team = new List<string>();

            foreach (Teams t in MainPlugin.Singleton.Config.TeamsConfigs.Teams.Where(x => x.Friendlys.Contains(TeamFond)))
            {
                team.Add(t.Name);
            }

            return team;
        }

        public static Team ConvertToNormalTeamName(RoleType role)
        {
            RoleType[] mtf = { RoleType.NtfCadet, RoleType.NtfCommander, RoleType.NtfLieutenant, RoleType.NtfScientist, RoleType.FacilityGuard, RoleType.Scientist };
            RoleType[] ci = { RoleType.ChaosInsurgency, RoleType.ClassD };
            RoleType[] scp = { RoleType.Scp173, RoleType.Scp106, RoleType.Scp096, RoleType.Scp079, RoleType.Scp0492, RoleType.Scp049, RoleType.Scp93989, RoleType.Scp93953 };

            if (mtf.Contains(role))
            {
                return Team.MTF;
            }
            else if (ci.Contains(role))
            {
                return Team.CHI;
            }
            else if (scp.Contains(role))
            {
                return Team.SCP;
            }

            return Team.TUT;
        }
    }
}
