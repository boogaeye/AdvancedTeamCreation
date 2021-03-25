using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Features;
using TeamsEXILED.enums;
using Respawning;

namespace TeamsEXILED.API
{
    public class Subteams
    {
        #region Subteams Config Zone
        [Description("Sets the Subclasses name")]
        public string Name { get; set; }
        [Description("sets the subclasses HP")]
        public int HP { get; set; } = 100;
        [Description("sets what this class has")]
        public ItemType[] Inventory { get; set; }
        [Description("Define the ids for custom items to be used(already defined REMOVE THESE CUSTOM IDS IF YOU DONT HAVE BoogasCustomItems Installed. you will have to redefine these!)")]
        public int[] CustomItemIds { get; set; }
        [Description("Define ammo so that this class has this ammo")]
        public Dictionary<AmmoType, uint> Ammo { get; set; } = new Dictionary<AmmoType, uint>() { { AmmoType.Nato556, 0u }, { AmmoType.Nato762, 0u }, { AmmoType.Nato9, 0u } };
        [Description("Sets what this role is supposed to look like")]
        public RoleType ModelRole { get; set; }
        [Description("sets the role name on the playerlist to easily define who you are")]
        public String RoleName { get; set; }
        [Description("What the Hint says for the role itself")]
        public string RoleHint { get; set; }
        [Description("the amount of players that can be changed to this role when spawning")]
        public int NumOfAllowedPlayers { get; set; } = -1;
        #endregion
    }
    public class Teams
    {
        #region team Config Zone
        [Description("sets if the teams active")]
        public bool Active { get; set; } = false;
        [Description("Sets the team name MUST be lowercase")]
        public string Name { get; set; } = "noteam";
        [Description("Defines subclasses of this team such as commander and rookie(tip: define commander first then Officer then rookie then ect...)")]
        public Subteams[] Subclasses { get; set; } = new Subteams[] { };
        [Description("String values of teams MUST be lowercase to define correctly. you can define Exiled teams too! this defines who cant hurt this team")]
        public string[] Friendlys { get; set; } = new string[] { };
        [Description("String values MUST be lowercase to define correctly. this defines teams that if alive when this team wins will make the round a draw")]
        public string[] Neutral { get; set; } = new string[] { };
        [Description("String values of teams MUST be lowercase to define correctly and you can use Exiled teams too. this ends the round when none of these teams are in the round when this team is active(This is requirements due to the fact that You wouldnt want Scientist to win when MTF is here to try helping them escape. plus requirements are also for which teams are enemies and cant allow them to win)")]
        public string[] Requirements { get; set; } = new string[] { };
        [Description("Defines where this team can spawn by default it can spawn as both Chaos and NineTailedFox spawn locations")]
        public SpawnableTeamType[] SpawnTypes { get; set; } = { SpawnableTeamType.ChaosInsurgency, SpawnableTeamType.NineTailedFox };
        [Description("defines who wins at the end of the round if the Enemies perameter is accepted")]
        public LeadingTeam teamLeaders { get; set; } = LeadingTeam.Anomalies;
        [Description("Makes MTF cassie messages when this team spawns set it to nothing if you dont want an MTF cassie message")]
        public string CassieMessageMTFSpawn { get; set; }
        [Description("Makes a Chaos cassie message when the team spawns")]
        public string CassieMessageChaosMessage { get; set; }
        [Description("set this to 0 to prevent the cassie announcement for chaos")]
        public ushort CassieMessageChaosAnnounceChance { get; set; } = 100;
        [Description("Sets where this team spawns")]
        public SpawnLocation spawnLocation { get; set; } = SpawnLocation.Normal;
        public string Color { get; set; } = "cyan";
        [Description("the chance this team will spawn if its been selected")]
        public ushort Chance { get; set; } = 50;
        #endregion
        #region Static Zone
        public static Teams NTF(Config config)
        {
            List<string> friendlys = new List<string>() { "mtf", "rsc" };
            List<string> enemies = new List<string>() { "chi", "cdp", "scp", "rsc" };
            foreach (string i in Classes.Classes.GetAllFriendlyTeams("mtf", config))
            {
                friendlys.Add(i);
            }
            foreach (string i in Classes.Classes.GetAllRequirements("mtf", config).ToArray())
            {
                enemies.Add(i);
            }
            return new Teams
            {
                Name = "mtf",
                Requirements = enemies.ToArray(),
                Friendlys = friendlys.ToArray(),
                Neutral = Classes.Classes.GetAllNeutrals("mtf", config).ToArray(),
                teamLeaders = LeadingTeam.FacilityForces
            };
        }
        public static Teams CHI(Config config)
        {
            List<string> friendlys = new List<string>() { "chi", "cdp" };
            List<string> enemies = new List<string>() { "mtf", "scp", "cdp" };
            foreach (string i in Classes.Classes.GetAllFriendlyTeams("chi", config))
            {
                friendlys.Add(i);
            }
            foreach (string i in Classes.Classes.GetAllRequirements("chi", config).ToArray())
            {
                enemies.Add(i);
            }
            return new Teams
            {
                Name = "chi",
                Requirements = enemies.ToArray(),
                Friendlys = friendlys.ToArray(),
                Neutral = Classes.Classes.GetAllNeutrals("chi", config).ToArray(),
                teamLeaders = LeadingTeam.ChaosInsurgency
            };
        }
        public static Teams CDP(Config config)
        {
            List<string> friendlys = new List<string>() { "cdp", "chi" };
            List<string> enemies = new List<string>() { "mtf", "scp", "rsc" };
            foreach (string i in Classes.Classes.GetAllFriendlyTeams("cdp", config))
            {
                friendlys.Add(i);
            }
            foreach (string i in Classes.Classes.GetAllRequirements("cdp", config))
            {
                enemies.Add(i);
            }
            return new Teams
            {
                Name = "cdp",
                Requirements = enemies.ToArray(),
                Friendlys = friendlys.ToArray(),
                Neutral = Classes.Classes.GetAllNeutrals("cdp", config).ToArray(),
                teamLeaders = LeadingTeam.Anomalies
            };
        }
        public static Teams SCP(Config config)
        {
            List<string> friendlys = new List<string>() { "scp" };
            List<string> enemies = new List<string>() { "mtf", "cdp", "chi", "rsc" };
            foreach (string i in Classes.Classes.GetAllFriendlyTeams("scp", config))
            {
                friendlys.Add(i);
            }
            foreach (string i in Classes.Classes.GetAllRequirements("scp", config).ToArray())
            {
                enemies.Add(i);
            }
            return new Teams
            {
                Name = "scp",
                Requirements = enemies.ToArray(),
                Friendlys = friendlys.ToArray(),
                Neutral = Classes.Classes.GetAllNeutrals("scp", config).ToArray(),
                teamLeaders = LeadingTeam.Anomalies
            };
        }
        public static Teams RSC(Config config)
        {
            List<string> friendlys = new List<string>() { "rsc", "mtf" };
            List<string> enemies = new List<string>() { "cdp", "chi", "scp" };
            foreach (string i in Classes.Classes.GetAllFriendlyTeams("rsc", config))
            {
                friendlys.Add(i);
            }
            foreach (string i in Classes.Classes.GetAllRequirements("rsc", config).ToArray())
            {
                enemies.Add(i);
            }
            return new Teams
            {
                Name = "rsc",
                Requirements = enemies.ToArray(),
                Friendlys = friendlys.ToArray(),
                Neutral = Classes.Classes.GetAllNeutrals("rsc", config).ToArray(),
                teamLeaders = LeadingTeam.Anomalies
            };
        }
        #endregion
    }
}
