using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Exiled.API.Interfaces;
using TeamsEXILED.API;
using Exiled.API.Enums;

namespace TeamsEXILED
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool RoleNameChangesAllowed { get; set; } = true;
        public bool Debug { get; set; } = false;
        [Description("All team names have to be lowercase otherwise IT WILL NOT LET YOU SPAWN CORRECTLY!")]
        public Teams[] Teams { get; set; } = new Teams[]{ new Teams() {
            Active = true,
            Name = "serpentshand",
            Subclasses = new Subteams[] { new Subteams {
                Name = "commander", Ammo = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato9, 200 } }, Inventory = new ItemType[]{ ItemType.GunProject90, ItemType.KeycardChaosInsurgency, ItemType.Radio, ItemType.Medkit, ItemType.Adrenaline }, HP = 100, ModelRole = RoleType.Tutorial, PlayerListRoleColor = "Green", PlayerListRoleName = "Serpents Hand Commander", RoleHint = "You are a commander of the serpents hand", NumOfAllowedPlayers = 1
            }, new Subteams{
                Name = "officer", Ammo = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato9, 200 } }, Inventory = new ItemType[]{ ItemType.GunProject90, ItemType.KeycardFacilityManager, ItemType.Radio, ItemType.Medkit }, HP = 85, ModelRole = RoleType.Tutorial, PlayerListRoleColor = "Green", PlayerListRoleName = "Serpents Hand Officer", RoleHint = "You are an officer of the serpents hand", NumOfAllowedPlayers = 3
            },
            new Subteams{
            Name = "rookie", Ammo = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato9, 200 } }, Inventory = new ItemType[]{ ItemType.GunProject90, ItemType.KeycardZoneManager, ItemType.Radio, ItemType.Medkit }, HP = 75, ModelRole = RoleType.Tutorial, PlayerListRoleColor = "Green", PlayerListRoleName = "Serpents Hand", RoleHint = "You are a rookie of the serpents hand"
            }
            },
            Friendlys = new string[] { "serpentshand", "scp" },
            Enemies = new string[] { "goc", "mtf", "cdp", "chi" },
            teamLeaders = LeadingTeam.Anomalies,
        }, 
            new Teams() {
            Active = true,
            Name = "goc",
            Subclasses = new Subteams[]{ new Subteams {
                Name = "rookie", Ammo = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato762, 200 } }, Inventory = new ItemType[]{ ItemType.GunMP7, ItemType.KeycardNTFLieutenant, ItemType.Radio, ItemType.Medkit, ItemType.GrenadeFrag, ItemType.GrenadeFrag }, HP = 135, ModelRole = RoleType.ChaosInsurgency, PlayerListRoleColor = "Yellow", PlayerListRoleName = "GOC", RoleHint = ""
            } },
            Friendlys = new string[] { "goc", "gru" },
            Enemies = new string[] { "serpentshand", "chi", "scp" },
            teamLeaders = LeadingTeam.FacilityForces,
            SpawnTypes = new Respawning.SpawnableTeamType[] { Respawning.SpawnableTeamType.ChaosInsurgency }
        }, new Teams(){
            Active = true,
            Name = "gru",
            Subclasses = new Subteams[]{ new Subteams {
                Name = "rookie", Ammo = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato762, 200 } }, Inventory = new ItemType[]{ ItemType.GunMP7, ItemType.KeycardNTFLieutenant, ItemType.Radio, ItemType.Medkit, ItemType.GrenadeFrag, ItemType.GrenadeFrag }, HP = 135, ModelRole = RoleType.ChaosInsurgency, PlayerListRoleColor = "Yellow", PlayerListRoleName = "GOC", RoleHint = ""
            } },
            Friendlys = new string[] { "goc", "gru" },
            Enemies = new string[] { "chi", "scp" },
            teamLeaders = LeadingTeam.FacilityForces,
            SpawnTypes = new Respawning.SpawnableTeamType[] { Respawning.SpawnableTeamType.NineTailedFox }
        }
        };
    }
}
