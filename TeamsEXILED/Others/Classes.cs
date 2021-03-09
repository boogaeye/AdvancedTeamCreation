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
        public List<string> GetAllEnemyTeams(string TeamFond, Config config)
        {
            List<string> team = new List<string>();
            foreach (Teams t in config.Teams)
            {
                if (t.Enemies.Contains(TeamFond))
                {
                    team.Add(t.Name);
                }
            }
            return team;
        }
        public List<string> GetAllFriendlyTeams(string TeamFond, Config config)
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
            if (i.Enemies.Contains(u))
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
            return new Teams { Name = s, Enemies = GetAllEnemyTeams(s, config).ToArray<string>(), Friendlys = GetAllFriendlyTeams(s, config).ToArray<string>() };
        }
    }
}
