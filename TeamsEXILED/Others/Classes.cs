using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static List<string> GetAllRequirements(string TeamFond, Config config)
        {
            List<string> team = new List<string>();
            foreach (Teams t in config.Teams)
            {
                if (t.Requirements.Contains(TeamFond))
                {
                    team.Add(t.Name);
                }
            }
            return team;
        }
        public static List<string> GetAllNeutrals(string TeamFond, Config config)
        {
            List<string> team = new List<string>();
            foreach (Teams t in config.Teams)
            {
                if (t.Neutral.Contains(TeamFond))
                {
                    team.Add(t.Name);
                }
            }
            return team;
        }
        public static List<string> GetAllFriendlyTeams(string TeamFond, Config config)
        {
            List<string> team = new List<string>();
            foreach (Teams t in config.Teams)
            {
                if (t.Friendlys.Contains(TeamFond))
                {
                    team.Add(t.Name);
                }
            }
            return team;
        }
        public bool IsTeamFriendly(Teams i, string u)
        {
            return i.Friendlys.Contains(u);
        }
        public bool IsTeamEnemy(Teams i, String u)
        {
            return i.Requirements.Contains(u);
        }
        [Obsolete("No longer in use")]
        public static bool Exists(string look, string[] teams)
        {
            if (teams.Contains(look))
            {
                return true;
            }
            return false;
        }
        public Teams GetTeamFromString(string s, Config config)
        {
            foreach (Teams t in config.Teams)
            {
                if (t.Name == s)
                {
                    return t;
                }
            }
            if (Teams.IsDefinedInConfig(s, config))
            {
                foreach (NormalTeam nt in config.TeamRedefine)
                {
                    if (nt.Team.ToString().ToLower() == s)
                    {
                        return new Teams { Active = nt.Active, Name = nt.Team.ToString().ToLower(), Friendlys = nt.Friendlys, Requirements = nt.Requirements, Neutral = nt.Neutral, teamLeaders = nt.TeamLeaders };
                    }
                }
            }
            List<string> friendlys = new List<string>() { s };
            foreach (string i in GetAllFriendlyTeams(s, config)) {
                friendlys.Add(i);
            }
            Teams teams = new Teams { Name = s, Requirements = GetAllRequirements(s, config).ToArray<string>(), Friendlys = friendlys.ToArray() };
            var handler = new Events.EventArgs.CreatingTeamEventArgs(teams);
            handler.StartInvoke();
            teams = handler.Team;
            return teams;
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
