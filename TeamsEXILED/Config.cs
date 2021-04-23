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
        public bool Allow1Player { get; set; } = true;
        [Description("allows friendly teams to hurt eachother no matter what hurts them")]
        public bool FriendlyFire { get; set; } = false;
        public string TeamKillBroadcast { get; set; } = "You got teamkilled report this to the admins if you dont think its an accident";
        public string KilledByNonfriendlyPlayer { get; set; } = "You didnt get team killed you where probably killed by someone who looks like you but isnt";
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
            Requirements = new string[] { "goc", "mtf", "cdp", "chi", "gru", "tta", "rsc", "tta", "opcf", "aes" },
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
            Requirements = new string[] { "serpentshand", "scp", "cdp", "tta", "rsc", "opcf", "aes" },
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
            Requirements = new string[] { "scp", "serpentshand", "tta", "cdp", "rsc", "aes" },
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
            Requirements = new string[] { "chi", "scp", "serpentshand", "goc", "gru", "mtf", "cdp", "rsc", "opcf", "aes" },
            teamLeaders = LeadingTeam.Anomalies,
            SpawnTypes = new Respawning.SpawnableTeamType[] { Respawning.SpawnableTeamType.NineTailedFox, Respawning.SpawnableTeamType.ChaosInsurgency },
            CassieMessageMTFSpawn = "pitch_0.1 .g3 .g3 .g3 pitch_1 The Tactical Target Agent C has entered the facility bell_end",
            CassieMessageChaosAnnounceChance = 75,
            CassieMessageChaosMessage = "pitch_0.1 .g3 .g3 .g3 pitch_1 The Tactical Target Agent C unit {nato} {SCP} has entered the facility bell_end",
            Chance = 50,
            Color = "red",
            spawnLocation = Enums.SpawnLocation.SurfaceNuke
        },new Teams(){
            Active = true,
            Name = "opcf",
            Subclasses = new Subteams[]{ 
                new Subteams{ 
                    Name = "commander",
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
                    Name = "officer",
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
                    Name = "cadet",
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
            Requirements = new string[] { "chi", "scp", "serpentshand", "goc", "gru", "cdp", "tta", "aes" },
            teamLeaders = LeadingTeam.FacilityForces,
            SpawnTypes = new Respawning.SpawnableTeamType[] { Respawning.SpawnableTeamType.ChaosInsurgency },
            CassieMessageChaosAnnounceChance = 100,
            CassieMessageChaosMessage = "pitch_0.1 .g5 .g5 .g5 pitch_1 operation chaos force has entered the facility bell_end",
            Chance = 100,
            Color = "green",
            Neutral = new string[]{ "rsc" },
            spawnLocation = Enums.SpawnLocation.Escape
        }, new Teams{
            Active = true,
            Name = "aes",
            Subclasses = new Subteams[]{
                new Subteams{
                    Name = "commander",
                    Ammo = new Dictionary<AmmoType, uint>{ { AmmoType.Nato9, 450 }, { AmmoType.Nato762, 450 } },
                    HP = 100,
                    ModelRole = RoleType.ChaosInsurgency,
                    RoleName = "AES Commander",
                    RoleHint = "You are to eliminate all scp subjects",
                    Inventory = new ItemType[]{ ItemType.GunProject90, ItemType.KeycardO5, ItemType.Medkit, ItemType.Medkit, ItemType.WeaponManagerTablet, ItemType.Radio },
                    CustomItemIds = new int[]{ 6 },
                    NumOfAllowedPlayers = 1
                },
                new Subteams{
                    Name = "firstofficer",
                    Ammo = new Dictionary<AmmoType, uint>{ { AmmoType.Nato9, 450 }, { AmmoType.Nato762, 450 } },
                    HP = 100,
                    ModelRole = RoleType.ChaosInsurgency,
                    RoleName = "AES First Officer",
                    RoleHint = "You are to eliminate all scp subjects",
                    Inventory = new ItemType[]{ ItemType.GunProject90, ItemType.KeycardChaosInsurgency, ItemType.Medkit, ItemType.Medkit, ItemType.WeaponManagerTablet, ItemType.Radio },
                    CustomItemIds = new int[]{ },
                    NumOfAllowedPlayers = 1
                },
                new Subteams{
                    Name = "officer",
                    Ammo = new Dictionary<AmmoType, uint>{ { AmmoType.Nato9, 450 }, { AmmoType.Nato762, 450 } },
                    HP = 100,
                    ModelRole = RoleType.ChaosInsurgency,
                    RoleName = "AES Officer",
                    RoleHint = "You are to eliminate all scp subjects",
                    Inventory = new ItemType[]{ ItemType.GunMP7, ItemType.KeycardZoneManager, ItemType.Medkit, ItemType.Medkit, ItemType.WeaponManagerTablet },
                    CustomItemIds = new int[]{ },
                    NumOfAllowedPlayers = 2
                },
                new Subteams{
                    Name = "rookie",
                    Ammo = new Dictionary<AmmoType, uint>{ { AmmoType.Nato9, 450 }, { AmmoType.Nato762, 450 } },
                    HP = 100,
                    ModelRole = RoleType.ChaosInsurgency,
                    RoleName = "AES rookie",
                    RoleHint = "You are to eliminate all scp subjects",
                    Inventory = new ItemType[]{ ItemType.GunMP7, ItemType.KeycardZoneManager, ItemType.Medkit, ItemType.WeaponManagerTablet, ItemType.WeaponManagerTablet },
                    CustomItemIds = new int[]{ },
                    NumOfAllowedPlayers = -1
                },
            },
            Friendlys = new string[] { "aes" },
            Requirements = new string[] { "scp", "serpentshand", "tta" },
            teamLeaders = LeadingTeam.FacilityForces,
            SpawnTypes = new Respawning.SpawnableTeamType[] { Respawning.SpawnableTeamType.NineTailedFox },
            CassieMessageChaosAnnounceChance = 0,
            CassieMessageChaosMessage = "",
            Chance = 50,
            Color = "red",
            Neutral = new string[]{ "mtf", "opcf", "goc", "gru", "chi", "rsc", "cdp" },
            spawnLocation = Enums.SpawnLocation.SCP106,
            CassieMessageMTFSpawn = "cassie pitch_0.6 .g3 .g3 pitch_1 the arrival of the anomaly emergency squad {nato} {unit} has entered the facility. pitch_0.96 please escort2 all scpsubjects to surface zone. pitch_1 all scpsubjects need to be secured. please wait inside of your designated evacuation shelters until this emergency has been completed. there are {SCP} scpsubjects remaining."
        }
        };
        public NormalTeam[] TeamRedefine { get; set; } = new NormalTeam[] {
            new NormalTeam()
            {
                Active = true,
                Team = Team.MTF,
                Friendlys = new string[] { "opcf", "mtf", "rsc" },
                Requirements = new string[] { "scp", "rsc", "chi", "cdp", "serpentshand", "tta"  },
                Neutral = new string[]{ "aes", "goc", "gru" }
            },
            new NormalTeam()
            {
                Active = true,
                Team = Team.CHI,
                Friendlys = new string[] { "chi", "cdp" },
                Requirements = new string[] { "scp", "rsc", "mtf", "cdp", "serpentshand", "tta", "opcf"  },
                Neutral = new string[]{ "aes", "goc", "gru" }
            },
            new NormalTeam()
            {
                Active = true,
                Team = Team.CDP,
                Friendlys = new string[] { "chi", "cdp" },
                Requirements = new string[] { "scp", "rsc", "mtf", "serpentshand", "tta", "opcf"  },
                Neutral = new string[]{ "aes", "goc", "gru" }
            },
            new NormalTeam()
            {
                Active = true,
                Team = Team.RSC,
                Friendlys = new string[] { "mtf", "rsc" },
                Requirements = new string[] { "scp", "rsc", "mtf", "cdp", "serpentshand", "tta", "opcf"  },
                Neutral = new string[]{ "aes", "goc", "gru", "opcf" }
            },
            new NormalTeam()
            {
                Active = true,
                Team = Team.SCP,
                Friendlys = new string[] { "scp", "serpentshand" },
                Requirements = new string[] { "rsc", "mtf", "cdp", "tta", "opcf", "aes", "goc", "gru"  },
                Neutral = new string[]{ }
            }
        };
    }
}
