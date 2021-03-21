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
        
        public string[] GetFriendlyTeams(Teams team)
        {
            string[] r = team.Friendlys;
            return r;
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
            if (i.Friendlys.Contains(u))
            {
                return true;
            }
            return false;
        }
        public bool IsTeamEnemy(Teams i, String u)
        {
            if (i.Requirements.Contains(u))
            {
                return true;
            }
            return false;
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
            if (s == "mtf")
            {
                return Teams.NTF(config);
            }
            if (s == "chi")
            {
                return Teams.CHI(config);
            }
            if (s == "cdp")
            {
                return Teams.CDP(config);
            }
            if (s == "scp")
            {
                return Teams.SCP(config);
            }
            if (s == "rsc")
            {
                return Teams.RSC(config);
            }
            List<string> friendlys = new List<string>() { s };
            foreach (string i in GetAllFriendlyTeams(s, config)) {
                friendlys.Add(i);
            }
            return new Teams { Name = s, Requirements = GetAllRequirements(s, config).ToArray<string>(), Friendlys = friendlys.ToArray() };
        }
    }
}
