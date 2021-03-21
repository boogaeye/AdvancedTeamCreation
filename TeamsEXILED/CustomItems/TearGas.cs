using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.CustomItems.API.Features;
using Exiled.CustomItems.API.Spawn;
using Exiled.Events.EventArgs;
using Exiled.API.Features;
using CustomPlayerEffects;

namespace TeamsEXILED.CustomItems
{
    class TearGas : CustomGrenade
    {
        public override float FuseTime { get; set; } = 10;
        public override string Name { get; set; } = "teargas";
        public override uint Id { get; set; } = 1;
        public override bool ExplodeOnCollision { get; set; } = false;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties { DynamicSpawnPoints = new List<DynamicSpawnPoint> { new DynamicSpawnPoint { Chance = 75, Location = Exiled.CustomItems.API.SpawnLocation.Inside096 }, new DynamicSpawnPoint { Chance = 50, Location = Exiled.CustomItems.API.SpawnLocation.Inside173Gate } } };
        public override string Description { get; set; } = "Will cause people to bleed and be slow";
        public override ItemType Type { get; set; } = ItemType.GrenadeFlash;
        protected override void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            foreach (Player p in ev.Targets)
            {
                p.EnableEffect<Bleeding>(60, true);
                p.EnableEffect<Disabled>(120, false);
            }
            base.OnExploding(ev);
        }
    }
}
