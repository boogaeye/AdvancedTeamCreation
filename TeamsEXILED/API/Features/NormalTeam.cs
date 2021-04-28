using UnityEngine;
using System.ComponentModel;
using System.Collections.Generic;
using Exiled.API.Enums;

namespace TeamsEXILED.API
{
    public class NormalTeam
    {
        public bool Active { get; set; } = false;

        public Team Team { get; set; } = Team.TUT;

        [Description("String values of teams MUST be lowercase to define correctly. you can define Exiled teams too! this defines who cant hurt this team")]
        public string[] Friendlys { get; set; } = new string[] { };

        [Description("String values MUST be lowercase to define correctly. this defines teams that if alive when this team wins will make the round a draw")]
        public string[] Neutral { get; set; } = new string[] { };

        [Description("String values of teams MUST be lowercase to define correctly and you can use Exiled teams too. this ends the round when none of these teams are in the round when this team is active(This is requirements due to the fact that You wouldnt want Scientist to win when MTF is here to try helping them escape. plus requirements are also for which teams are enemies and cant allow them to win)")]
        public string[] Requirements { get; set; } = new string[] { };

        public LeadingTeam TeamLeaders { get; set; } = LeadingTeam.Anomalies;
    }
}