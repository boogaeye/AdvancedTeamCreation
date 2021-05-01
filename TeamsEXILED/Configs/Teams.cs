using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using TeamsEXILED.API;
using Exiled.API.Enums;
using System.ComponentModel;

namespace TeamsEXILED.Configs
{
    public class TeamsConfig
    {
        [Description("All team names have to be lowercase otherwise IT WILL NOT LET YOU SPAWN CORRECTLY!")]
        public Teams Team { get; set; } = new Teams()
        {
            Active = true,
            Name = "goc",
            Subclasses = new Subteams[]{ new Subteams {
                Name = "rookie", Ammo = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato762, 200 } }, Inventory = new string[]{ "KeycardNTFLieutenant", "Radio", "Medkit", "5", "2", "2", "0" }, HP = 135, ModelRole = RoleType.ChaosInsurgency, RoleName = "<color=yellow>GOC</color>", RoleMessage = "You are the GOC"
            } },
            Friendlys = new string[] { "goc" },
            Requirements = new string[] { "scp", "cdp", "tta", "rsc", "opcf", "aes" },
            teamLeaders = LeadingTeam.FacilityForces,
            SpawnTypes = new Respawning.SpawnableTeamType[] { Respawning.SpawnableTeamType.ChaosInsurgency },
            CassieMessageChaosMessage = "pitch_0.6 .g5 .g5 .g5 pitch_1 the g o c has entered the facility bell_end",
            CassieMessageChaosAnnounceChance = 100,
            Chance = 65,
            Color = "yellow",
            Neutral = new string[] { "mtf", "chi", "gru" }
        };
    }
}
