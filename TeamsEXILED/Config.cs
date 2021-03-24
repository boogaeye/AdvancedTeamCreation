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
        [Description("if this is true then it will allow one player to be alive in the round when someone kills themselves(This is basically to give them another chance before ending the game)")]
        public bool Allow1Player { get; set; } = false;
        public bool Debug { get; set; } = false;
        [Description("All team names have to be lowercase otherwise IT WILL NOT LET YOU SPAWN CORRECTLY!")]
        public Teams[] Teams { get; set; } = new Teams[]{ new Teams() {
            Active = true,
            Name = "serpentshand",
            Subclasses = new Subteams[] { new Subteams {
                Name = "commander", Ammo = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato9, 200 } }, Inventory = new ItemType[]{ ItemType.GunProject90, ItemType.Radio, ItemType.Medkit, ItemType.Adrenaline }, HP = 100, ModelRole = RoleType.Tutorial, RoleName = "Serpents Hand Commander", RoleHint = "You are a commander of the serpents hand", CustomItemIds = new int[] { 9 } , NumOfAllowedPlayers = 1
            }, new Subteams{
                Name = "officer", Ammo = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato9, 200 } }, Inventory = new ItemType[]{ ItemType.GunProject90, ItemType.Radio, ItemType.Medkit }, HP = 85, ModelRole = RoleType.Tutorial, RoleName = "Serpents Hand Officer", RoleHint = "You are an officer of the serpents hand", CustomItemIds = new int[] { 8 } , NumOfAllowedPlayers = 3
            },
            new Subteams{
            Name = "rookie", Ammo = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato9, 200 } }, Inventory = new ItemType[]{ ItemType.GunProject90, ItemType.Radio, ItemType.Medkit }, HP = 75, ModelRole = RoleType.Tutorial, RoleName = "Serpents Hand", RoleHint = "You are a rookie of the serpents hand", CustomItemIds = new int[] { 8 }
            }
            },
            Friendlys = new string[] { "serpentshand", "scp" },
            Requirements = new string[] { "goc", "mtf", "cdp", "chi", "gru", "tta", "rsc", "tta", "opcf" },
            teamLeaders = LeadingTeam.Anomalies,
            Chance = 95,
            Color = "green"
        }, 
            new Teams() {
            Active = true,
            Name = "goc",
            Subclasses = new Subteams[]{ new Subteams {
                Name = "rookie", Ammo = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato762, 200 } }, Inventory = new ItemType[]{ ItemType.KeycardNTFLieutenant, ItemType.Radio, ItemType.Medkit }, HP = 135, ModelRole = RoleType.ChaosInsurgency, RoleName = "<color=yellow>GOC</color>", RoleHint = "You are the GOC", CustomItemIds = new int[] { 5, 2, 2, 0, }
            } },
            Friendlys = new string[] { "goc" },
            Requirements = new string[] { "serpentshand", "scp", "cdp", "tta", "rsc", "opcf" },
            teamLeaders = LeadingTeam.FacilityForces,
            SpawnTypes = new Respawning.SpawnableTeamType[] { Respawning.SpawnableTeamType.ChaosInsurgency },
            CassieMessageChaosMessage = "pitch_0.6 .g5 .g5 .g5 pitch_1 the g o c has entered the facility bell_end",
            CassieMessageChaosAnnounceChance = 100,
            Chance = 65,
            Color = "yellow",
            Neutral = new string[]{ "mtf", "chi", "gru" }
        }, new Teams(){
            Active = true,
            Name = "gru",
            Subclasses = new Subteams[]{ new Subteams {
                Name = "rookie", Ammo = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato762, 200 } }, Inventory = new ItemType[]{ ItemType.GunE11SR, ItemType.KeycardChaosInsurgency, ItemType.Adrenaline, ItemType.WeaponManagerTablet, ItemType.GrenadeFlash }, HP = 150, ModelRole = RoleType.FacilityGuard, RoleName = "<color=yellow>GRU</color>", RoleHint = "", CustomItemIds = new int[] { 7, 6 }
            } },
            Friendlys = new string[] { "gru" },
            Requirements = new string[] { "scp", "serpentshand", "tta", "cdp", "rsc"  },
            teamLeaders = LeadingTeam.FacilityForces,
            SpawnTypes = new Respawning.SpawnableTeamType[] { Respawning.SpawnableTeamType.NineTailedFox },
            CassieMessageMTFSpawn = ".g5 .g5 the g r u has entered the facility. there are {SCP} scpsubjects",
            Chance = 65,
            Color = "yellow",
            Neutral = new string[]{ "mtf", "chi", "goc" }
        },new Teams(){
            Active = true,
            Name = "tta",
            Subclasses = new Subteams[]{ new Subteams {
                Name = "officer", Ammo = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato9, 200 } }, Inventory = new ItemType[]{ ItemType.GunUSP, ItemType.KeycardFacilityManager, ItemType.Adrenaline, ItemType.WeaponManagerTablet }, HP = 100, ModelRole = RoleType.NtfScientist, RoleName = "<color=red>TTA</color>", RoleHint = "You are the TTA\nKill everything in sight", CustomItemIds = new int[] { 0, 0 }
            } },
            Friendlys = new string[] { "tta" },
            Requirements = new string[] { "chi", "scp", "serpentshand", "goc", "gru", "mtf", "cdp", "rsc", "opcf" },
            teamLeaders = LeadingTeam.Anomalies,
            SpawnTypes = new Respawning.SpawnableTeamType[] { Respawning.SpawnableTeamType.NineTailedFox, Respawning.SpawnableTeamType.ChaosInsurgency },
            CassieMessageMTFSpawn = "pitch_0.1 .g3 .g3 .g3 pitch_1 The Tactical Target Agent C has entered the facility bell_end",
            CassieMessageChaosAnnounceChance = 75,
            CassieMessageChaosMessage = "pitch_0.1 .g3 .g3 .g3 pitch_1 The Tactical Target Agent C unit {nato} {SCP} has entered the facility bell_end",
            Chance = 50,
            Color = "red",
        },new Teams(){
            Active = true,
            Name = "opcf",
            Subclasses = new Subteams[]{ 
                new Subteams{ 
                    Name = "Commander",
                    Ammo = new Dictionary<AmmoType, uint>{ { AmmoType.Nato9, 450 } },
                    HP = 160,
                    ModelRole = RoleType.FacilityGuard,
                    RoleName = "OPCF Commander",
                    RoleHint = "You are part of the branch of the NTF\n<color=blue>Operation Chaos Force</color>\nKill everything you see and help the MTF",
                    Inventory = new ItemType[]{ ItemType.GunLogicer, ItemType.GunProject90, ItemType.KeycardChaosInsurgency, ItemType.Radio, ItemType.Medkit, ItemType.Medkit },
                    CustomItemIds = new int[]{ 0 },
                    NumOfAllowedPlayers = 1
                },
                new Subteams{
                    Name = "Officer",
                    Ammo = new Dictionary<AmmoType, uint>{ { AmmoType.Nato9, 450 } },
                    HP = 120,
                    ModelRole = RoleType.NtfLieutenant,
                    RoleName = "OPCF Officer",
                    RoleHint = "You are part of the branch of the NTF\n<color=blue>Operation Chaos Force</color>\nKill everything you see and help the MTF",
                    Inventory = new ItemType[]{ ItemType.GunLogicer, ItemType.GunProject90, ItemType.KeycardNTFLieutenant, ItemType.Radio, ItemType.Medkit, ItemType.Painkillers },
                    CustomItemIds = new int[]{ 0 },
                    NumOfAllowedPlayers = 2
                },
                new Subteams{
                    Name = "Cadet",
                    Ammo = new Dictionary<AmmoType, uint>{ { AmmoType.Nato9, 450 } },
                    HP = 100,
                    ModelRole = RoleType.NtfCadet,
                    RoleName = "OPCF Cadet",
                    RoleHint = "You are part of the branch of the NTF\n<color=blue>Operation Chaos Force</color>\nKill everything you see and help the MTF",
                    Inventory = new ItemType[]{ ItemType.GunProject90, ItemType.KeycardGuard, ItemType.Radio, ItemType.Painkillers, ItemType.GrenadeFlash },
                    CustomItemIds = new int[]{ 0 },
                    NumOfAllowedPlayers = -1
                },
            },
            Friendlys = new string[] { "opcf", "mtf" },
            Requirements = new string[] { "chi", "scp", "serpentshand", "goc", "gru", "cdp", "tta" },
            teamLeaders = LeadingTeam.FacilityForces,
            SpawnTypes = new Respawning.SpawnableTeamType[] { Respawning.SpawnableTeamType.ChaosInsurgency },
            CassieMessageChaosAnnounceChance = 100,
            CassieMessageChaosMessage = "pitch_0.1 .g5 .g5 .g5 pitch_1 operation chaos force has entered the facility bell_end",
            Chance = 100,
            Color = "green",
            Neutral = new string[]{ "rsc" }
        }
        };
    }
}
