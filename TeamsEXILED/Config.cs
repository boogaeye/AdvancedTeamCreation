using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Interfaces;
using TeamsEXILED.API;
using Exiled.API.Enums;

namespace TeamsEXILED
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public Teams[] Teams { get; set; } = new Teams[]{ new Teams() {
            Active = false,
            Name = "serpentshand",
            Friendlys = new string[] { "serpentshand" },
            Enemies = new string[] { "goc" },
            CanHurtSCPs = false,
            Ammo = new Dictionary<AmmoType, uint>{ { AmmoType.Nato9, 200 } },
            Inventory = new ItemType[]{ ItemType.GunProject90, ItemType.GrenadeFlash },
            ModelRole = RoleType.Tutorial,
        }, 
            new Teams() {
            Active = false,
            Name = "goc",
            Friendlys = new string[] { "goc" },
            Enemies = new string[] { "serpentshand" },
            CanHurtSCPs = true,
            Ammo = new Dictionary<AmmoType, uint>{ { AmmoType.Nato9, 200 } },
            Inventory = new ItemType[]{ ItemType.GunProject90, ItemType.GrenadeFlash, ItemType.Adrenaline, ItemType.Adrenaline, ItemType.Adrenaline },
            ModelRole = RoleType.ChaosInsurgency,
        }};
    }
}
