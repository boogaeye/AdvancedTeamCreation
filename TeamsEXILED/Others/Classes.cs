using System;
using System.Collections.Generic;
using System.Linq;
using TeamsEXILED.API;
using Exiled.API.Features;

namespace TeamsEXILED.Classes
{
    public class Classes
    {
        public void RenameUnit(int index, string name)
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

        public void AddUnit(string name, Player[] players)
        {
            Respawning.NamingRules.UnitNamingRules.AllNamingRules[Respawning.SpawnableTeamType.NineTailedFox].AddCombination(name, Respawning.SpawnableTeamType.NineTailedFox);

            foreach (var ply in players)
            {
                ply.ReferenceHub.characterClassManager.NetworkCurUnitName = name;
            }
        }

        public string[] GetFriendlyTeams(Teams team)
        {
            return team.Friendlys;
        }

        public static List<string> GetAllRequirements(string TeamFond)
        {
            List<string> team = new List<string>();

            foreach (Teams t in MainPlugin.Singleton.Config.Teams.Where(x => x.Requirements.Contains(TeamFond)))
            {
                team.Add(t.Name);
            }

            return team;
        }

        public static List<string> GetAllNeutrals(string TeamFond)
        {
            List<string> team = new List<string>();

            foreach (Teams t in MainPlugin.Singleton.Config.Teams.Where(x => x.Neutral.Contains(TeamFond)))
            {
                team.Add(t.Name);
            }

            return team;
        }

        public static List<string> GetAllFriendlyTeams(string TeamFond)
        {
            List<string> team = new List<string>();

            foreach (Teams t in MainPlugin.Singleton.Config.Teams.Where(x => x.Friendlys.Contains(TeamFond)))
            {
                team.Add(t.Name);
            }

            return team;
        }

        public bool IsTeamFriendly(Teams i, string u)
        {
            return i.Friendlys.Contains(u);
        }

        public bool IsTeamEnemy(Teams i, string u)
        {
            return i.Requirements.Contains(u);
        }

        public bool IsTeamNeutral(Teams i, string u)
        {
            return i.Neutral.Contains(u);
        }

        public Teams GetTeamFromString(string s)
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

        public Team ConvertToNormalTeamName(RoleType role)
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
