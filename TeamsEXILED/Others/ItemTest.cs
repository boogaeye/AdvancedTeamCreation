using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.CustomItems.API.Features;
using Exiled.CustomItems.API.Spawn;
using Exiled.CustomItems.API.EventArgs;
using Exiled.Events.EventArgs;
using Grenades;

namespace TeamsEXILED.Others
{
    public class ItemTest : CustomGrenade
    {
        public override bool ExplodeOnCollision { get; set; } = true;
        public override float FuseTime { get; set; } = 1;
        public override uint Id { get; set; } = 1;
        public override string Name { get; set; } = "UndyingGrenade";
        public override string Description { get; set; } = "Oh No";
        public override SpawnProperties SpawnProperties { get; set; }
        public void OnSomeEvent(ThrowingGrenadeEventArgs ev)
        {
            CustomGrenade.TryGive(ev.Player, 1, true);
        }
    }
}
