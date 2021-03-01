using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Respawning;

namespace TeamsEXILED.API
{
    public class Teams
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public string[] Friendlys { get; set; }
        public bool CanHurtSCPs { get; set; } = true;
        public string[] Enemies { get; set; }
        public SpawnableTeamType[] SpawnTypes { get; set; } = { SpawnableTeamType.ChaosInsurgency, SpawnableTeamType.NineTailedFox };
        public ItemType[] Inventory { get; set; }
        public Dictionary<AmmoType, uint> Ammo { get; set; } = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato556, 0u }, { AmmoType.Nato762, 0u }, { AmmoType.Nato9, 0u } };
        public RoleType ModelRole { get; set; }
        public string CassieMessageMTFSpawn { get; set; }
        public string CassieMessageChaosMessage { get; set; }
        public ushort CassieMessageChaosAnnounceChance { get; set; } = 100;
    }
}
