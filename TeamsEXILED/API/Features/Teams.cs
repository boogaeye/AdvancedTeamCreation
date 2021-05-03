using System.ComponentModel;
using System.Linq;
using Respawning;

namespace TeamsEXILED.API
{
    public class Teams
    {
        [Description("sets if the teams active")]
        public bool Active { get; set; } = false;

        [Description("Sets the team name MUST be lowercase")]
        public string Name { get; set; } = "";

        [Description("Defines subclasses of this team such as commander and rookie(tip: define commander first then Officer then rookie then ect...)")]
        public Subteams[] Subclasses { get; set; } = new Subteams[] { };

        [Description("String values of teams MUST be lowercase to define correctly. you can define Exiled teams too! this defines who cant hurt this team")]
        public string[] Friendlys { get; set; } = new string[] { };

        [Description("String values MUST be lowercase to define correctly. this defines teams that if alive when this team wins will make the round a draw")]
        public string[] Neutral { get; set; } = new string[] { };

        [Description("String values of teams MUST be lowercase to define correctly and you can use Exiled teams too. this ends the round when none of these teams are in the round when this team is active(This is requirements due to the fact that You wouldnt want Scientist to win when MTF is here to try helping them escape. plus requirements are also for which teams are enemies and cant allow them to win)")]
        public string[] Requirements { get; set; } = new string[] { };

        [Description("Defines where this team can spawn by default it can spawn as both Chaos and NineTailedFox spawn locations")]
        public SpawnableTeamType[] SpawnTypes { get; set; } = { };

        [Description("Makes MTF cassie messages when this team spawns set it to nothing if you dont want an MTF cassie message")]
        public string CassieMessageMTFSpawn { get; set; } = "";

        [Description("Makes a Chaos cassie message when the team spawns")]
        public string CassieMessageChaosMessage { get; set; } = "";

        [Description("set this to 0 to prevent the cassie announcement for chaos")]
        public ushort CassieMessageChaosAnnounceChance { get; set; } = 100;

        [Description("Sets where this team spawns")]
        public SpawnLocation SpawnLocation { get; set; } = SpawnLocation.Normal;

        [Description("makes it where if this team is the latest spawn it will spawn the assigned escapees to this team if they are defined in this config")]
        public RoleType[] EscapeChange { get; set; } = { };

        public string Color { get; set; } = "cyan";

        [Description("the chance this team will spawn if its been selected")]
        public ushort Chance { get; set; } = 50;

        public bool TryGetSubteam(string name, out Subteams subteam)
        {
            if (name == null)
            {
                subteam = null;
                return false;
            }

            subteam = GetSubteam(name);
            return subteam != null;
        }

        public Subteams GetSubteam(string name) => Subclasses?.FirstOrDefault(x => x.Name == name);

        public static Teams Get(string name) => MainPlugin.Singleton.Config.Teams?.FirstOrDefault(x => x.Name == name);

        public static bool TryGet(string name, out Teams team)
        {
            if (name == null)
            {
                team = null;
                return false;
            }

            team = Get(name);
            return team != null;
        }
    }
}