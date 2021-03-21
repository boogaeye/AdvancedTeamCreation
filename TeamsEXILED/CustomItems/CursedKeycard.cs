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
using Exiled.CustomItems.API.EventArgs;

namespace TeamsEXILED.CustomItems
{
    class CursedKeycard : CustomItem
    {
        public override string Name { get; set; } = "cursedkeycard";
        public override uint Id { get; set; } = 2;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties { DynamicSpawnPoints = new List<DynamicSpawnPoint> { new DynamicSpawnPoint { Chance = 75, Location = Exiled.CustomItems.API.SpawnLocation.Inside096 }, new DynamicSpawnPoint { Chance = 50, Location = Exiled.CustomItems.API.SpawnLocation.InsideNukeArmory }, new DynamicSpawnPoint { Chance = 100, Location = Exiled.CustomItems.API.SpawnLocation.Inside914 } } };
        public override string Description { get; set; } = "Just a normal keycard";
        public override ItemType Type { get; set; } = ItemType.KeycardScientist;
        protected override void OnDropping(DroppingItemEventArgs ev)
        {
            ev.Player.ShowHint("You made a grave mistake", 5);
            ev.Player.EnableEffect<Bleeding>();
            base.OnDropping(ev);
        }
    }
}
