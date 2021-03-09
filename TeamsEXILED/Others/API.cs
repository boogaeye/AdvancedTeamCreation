using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using TeamsEXILED.enums;
using Respawning;

namespace TeamsEXILED.API
{
    public class Subteams
    {
        [Description("Sets the Subclasses name")]
        public string Name { get; set; }
        [Description("sets the subclasses HP")]
        public int HP { get; set; } = 100;
        [Description("sets what this class has")]
        public ItemType[] Inventory { get; set; }
        [Description("Define the ids for custom items to be used")]
        public int[] CustomItemIds { get; set; }
        [Description("Define ammo so that this class has this ammo")]
        public Dictionary<AmmoType, uint> Ammo { get; set; } = new Dictionary<AmmoType, uint>() { { AmmoType.Nato556, 0u }, { AmmoType.Nato762, 0u }, { AmmoType.Nato9, 0u } };
        [Description("Sets what this role is supposed to look like")]
        public RoleType ModelRole { get; set; }
        [Description("sets the role name on the playerlist to easily define who you are")]
        public String PlayerListRoleName { get; set; }
        [Description("Rank Color")]
        public String PlayerListRoleColor { get; set; } = "red";
        [Description("What the Hint says for the role itself")]
        public string RoleHint { get; set; }
        [Description("the amount of players that can be changed to this role when spawning")]
        public int NumOfAllowedPlayers { get; set; } = -1;
    }
    public class Teams
    {
        [Description("sets if the teams active")]
        public bool Active { get; set; }
        [Description("Sets the team name MUST be lowercase")]
        public string Name { get; set; }
        [Description("Defines subclasses of this team such as commander and rookie(tip: define commander first then Officer then rookie then ect...)")]
        public Subteams[] Subclasses { get; set; }
        [Description("String values of teams MUST be lowercase to define correctly. you can define Exiled teams too! this defines who cant hurt this team")]
        public string[] Friendlys { get; set; }
        [Description("String values of teams MUST be lowercase to define correctly and you can use Exiled teams too. this ends the round when none of these teams are in the round when this team is active")]
        public string[] Enemies { get; set; }
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
    }
}
