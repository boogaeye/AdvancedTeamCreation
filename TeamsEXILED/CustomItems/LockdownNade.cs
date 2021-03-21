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
    class LockdownRoom : CustomItem
    {
        public override string Name { get; set; } = "lockdownhackdevice";
        public override uint Id { get; set; } = 3;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties { DynamicSpawnPoints = new List<DynamicSpawnPoint> { new DynamicSpawnPoint { Chance = 75, Location = Exiled.CustomItems.API.SpawnLocation.Inside096 }, new DynamicSpawnPoint { Chance = 50, Location = Exiled.CustomItems.API.SpawnLocation.Inside173Gate } } };
        public override string Description { get; set; } = "Will close down a room";
        public override ItemType Type { get; set; } = ItemType.KeycardChaosInsurgency;
        protected override void OnDropping(DroppingItemEventArgs ev)
        {
            ev.Player.CurrentRoom.TurnOffLights(10);
            base.OnDropping(ev);
        }

    }
}
