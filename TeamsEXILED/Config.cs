﻿using System.ComponentModel;
using Exiled.API.Interfaces;
using TeamsEXILED.Configs;
using System.IO;
using Exiled.API.Features;
using Exiled.Loader;
using TeamsEXILED.API;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;

namespace TeamsEXILED
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
		

        [Description("allows friendly teams to hurt eachother no matter what hurts them")]
        public bool FriendlyFire { get; set; } = false;

        [Description("If enabled the RoleHint message will be displayed as a hint, else as a broadcast")]
        public bool UseHints { get; set; } = true;

        [Description("Should display the description of the customitem when given?")]
        public bool DisplayDescription { get; set; } = false;

        [Description("This plugin takes priority over UIU and SerpentHands?")]
        public bool DominantPlugin { get; set; } = false;

        public bool Debug { get; set; } = false;

        public string ConfigsFolder { get; set; } = Path.Combine(Paths.Configs, "AdvancedTeamCreation");

        public List<Teams> Teams = new List<Teams>();
        public NormalTeams NormalConfigs;

        public void LoadConfigs()
        {
            Teams.Clear();

            if (Directory.Exists(ConfigsFolder) == false)
            {
                Directory.CreateDirectory(ConfigsFolder);

            }

            var teamsdir = Path.Combine(ConfigsFolder, "Teams");
            if (Directory.Exists(teamsdir) == false)
            {
                Directory.CreateDirectory(teamsdir);
                foreach (var tm in DefaultTeams)
                {
                    File.WriteAllText(Path.Combine(teamsdir, $"{tm.Name}.yml"), Loader.Serializer.Serialize(tm));
                }
            }

            var tfiles = Directory.GetFiles(teamsdir);
            foreach (var file in tfiles.Where(x => x.EndsWith("yml")))
            {
                var des = Loader.Deserializer.Deserialize<TeamsConfig>(File.ReadAllText(file));
                File.WriteAllText(file, Loader.Serializer.Serialize(des));
                Teams.Add(des.Team);
            }

            string npath = Path.Combine(ConfigsFolder, "NormalTeams.yml");
            if (File.Exists(npath) == false)
            {
                NormalConfigs = new NormalTeams();
                File.WriteAllText(npath, Loader.Serializer.Serialize(NormalConfigs));
            }
            else
            {
                NormalConfigs = Loader.Deserializer.Deserialize<NormalTeams>(File.ReadAllText(npath));
                File.WriteAllText(npath, Loader.Serializer.Serialize(NormalConfigs));
            }
        }

        public Teams[] DefaultTeams = new Teams[]{ new Teams() {
            Active = true,
            Name = "goc",
            Subclasses = new Subteams[]{ new Subteams {
                Name = "rookie", Ammo = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato762, 200 } }, Inventory = new string[]{ "KeycardNTFLieutenant", "Radio", "Medkit", "5", "2", "2", "0" }, HP = 135, ModelRole = RoleType.ChaosInsurgency, RoleName = "<color=yellow>GOC</color>", RoleMessage = "You are the GOC"
            } },
            Friendlys = new string[] { "goc" },
            Requirements = new string[] { "scp", "cdp", "tta", "rsc", "opcf", "aes" },
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
                Name = "rookie", Ammo = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato762, 200 } }, Inventory = new string[]{ "GunE11SR", "KeycardChaosInsurgency", "Adrenaline", "WeaponManagerTablet", "GrenadeFlash", "7", "6" }, HP = 150, ModelRole = RoleType.FacilityGuard, RoleName = "<color=yellow>GRU</color>", RoleMessage = ""
            } },
            Friendlys = new string[] { "gru" },
            Requirements = new string[] { "scp", "tta", "cdp", "rsc", "aes" },
            SpawnTypes = new Respawning.SpawnableTeamType[] { Respawning.SpawnableTeamType.NineTailedFox },
            CassieMessageMTFSpawn = ".g5 .g5 the g r u has entered the facility. there are {SCP} scpsubjects",
            Chance = 65,
            Color = "yellow",
            Neutral = new string[]{ "mtf", "chi", "goc" },
        },new Teams(){
            Active = true,
            Name = "tta",
            Subclasses = new Subteams[]{ new Subteams {
                Name = "officer", Ammo = new Dictionary<AmmoType, uint>(){ { AmmoType.Nato9, 200 } }, Inventory = new string[]{ "GunUSP", "KeycardFacilityManager", "Adrenaline", "WeaponManagerTablet", "0", "0" }, HP = 100, ModelRole = RoleType.NtfScientist, RoleName = "<color=red>TTA</color>", RoleMessage = "You are the TTA\nKill everything in sight"
            } },
            Friendlys = new string[] { "tta" },
            Requirements = new string[] { "chi", "scp", "goc", "gru", "mtf", "cdp", "rsc", "opcf", "aes" },
            SpawnTypes = new Respawning.SpawnableTeamType[] { Respawning.SpawnableTeamType.NineTailedFox, Respawning.SpawnableTeamType.ChaosInsurgency },
            CassieMessageMTFSpawn = "pitch_0.1 .g3 .g3 .g3 pitch_1 The Tactical Target Agent C has entered the facility bell_end",
            CassieMessageChaosAnnounceChance = 75,
            CassieMessageChaosMessage = "pitch_0.1 .g3 .g3 .g3 pitch_1 The Tactical Target Agent C unit {nato} {SCP} has entered the facility bell_end",
            Chance = 50,
            Color = "red",
            spawnLocation = SpawnLocation.SurfaceNuke
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
                    RoleMessage = "You are part of the branch of the NTF\n<color=blue>Operation Chaos Force</color>\nKill everything you see and help the MTF",
                    Inventory = new string[]{ "GunLogicer", "GunProject90", "KeycardChaosInsurgency", "Radio", "Medkit", "Medkit", "0" },
                    NumOfAllowedPlayers = 1
                },
                new Subteams{
                    Name = "officer",
                    Ammo = new Dictionary<AmmoType, uint>{ { AmmoType.Nato9, 450 } },
                    HP = 120,
                    ModelRole = RoleType.NtfLieutenant,
                    RoleName = "OPCF Officer",
                    RoleMessage = "You are part of the branch of the NTF\n<color=blue>Operation Chaos Force</color>\nKill everything you see and help the MTF",
                    Inventory = new string[]{ "GunLogicer", "GunProject90", "KeycardNTFLieutenant", "Radio", "Medkit", "Painkillers", "0" },
                    NumOfAllowedPlayers = 2
                },
                new Subteams{
                    Name = "cadet",
                    Ammo = new Dictionary<AmmoType, uint>{ { AmmoType.Nato9, 450 } },
                    HP = 100,
                    ModelRole = RoleType.NtfCadet,
                    RoleName = "OPCF Cadet",
                    RoleMessage = "You are part of the branch of the NTF\n<color=blue>Operation Chaos Force</color>\nKill everything you see and help the MTF",
                    Inventory = new string[]{ "GunProject90", "KeycardGuard", "Radio", "Painkillers", "GrenadeFlash", "0" },
                    NumOfAllowedPlayers = -1
                },
            },
            Friendlys = new string[] { "opcf", "mtf" },
            Requirements = new string[] { "chi", "scp", "goc", "gru", "cdp", "tta", "aes" },
            SpawnTypes = new Respawning.SpawnableTeamType[] { Respawning.SpawnableTeamType.ChaosInsurgency },
            CassieMessageChaosAnnounceChance = 100,
            CassieMessageChaosMessage = "pitch_0.1 .g5 .g5 .g5 pitch_1 operation chaos force has entered the facility bell_end",
            Chance = 100,
            Color = "green",
            Neutral = new string[]{ "rsc" },
            spawnLocation = SpawnLocation.Escape
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
                    RoleMessage = "You are to eliminate all scp subjects",
                    Inventory = new string[]{ "GunProject90", "KeycardO5", "Medkit", "Medkit", "WeaponManagerTablet", "Radio", "6" },
                    NumOfAllowedPlayers = 1
                },
                new Subteams{
                    Name = "firstofficer",
                    Ammo = new Dictionary<AmmoType, uint>{ { AmmoType.Nato9, 450 }, { AmmoType.Nato762, 450 } },
                    HP = 100,
                    ModelRole = RoleType.ChaosInsurgency,
                    RoleName = "AES First Officer",
                    RoleMessage = "You are to eliminate all scp subjects",
                    Inventory = new string[]{ "GunProject90", "KeycardChaosInsurgency", "Medkit", "Medkit", "WeaponManagerTablet", "Radio" },
                    NumOfAllowedPlayers = 1
                },
                new Subteams{
                    Name = "officer",
                    Ammo = new Dictionary<AmmoType, uint>{ { AmmoType.Nato9, 450 }, { AmmoType.Nato762, 450 } },
                    HP = 100,
                    ModelRole = RoleType.ChaosInsurgency,
                    RoleName = "AES Officer",
                    RoleMessage = "You are to eliminate all scp subjects",
                    Inventory = new string[]{ "GunMP7", "KeycardZoneManager", "Medkit", "Medkit", "WeaponManagerTablet" },
                    NumOfAllowedPlayers = 2
                },
                new Subteams{
                    Name = "rookie",
                    Ammo = new Dictionary<AmmoType, uint>{ { AmmoType.Nato9, 450 }, { AmmoType.Nato762, 450 } },
                    HP = 100,
                    ModelRole = RoleType.ChaosInsurgency,
                    RoleName = "AES rookie",
                    RoleMessage = "You are to eliminate all scp subjects",
                    Inventory = new string[]{ "GunMP7", "KeycardZoneManager", "Medkit", "WeaponManagerTablet", "WeaponManagerTablet" },
                    NumOfAllowedPlayers = -1
                },
            },
            Friendlys = new string[] { "aes" },
            Requirements = new string[] { "scp", "tta" },
            SpawnTypes = new Respawning.SpawnableTeamType[] { Respawning.SpawnableTeamType.NineTailedFox },
            CassieMessageChaosAnnounceChance = 0,
            CassieMessageChaosMessage = "",
            Chance = 50,
            Color = "red",
            Neutral = new string[]{ "mtf", "opcf", "goc", "gru", "chi", "rsc", "cdp" },
            spawnLocation = SpawnLocation.SCP106,
            CassieMessageMTFSpawn = "cassie pitch_0.6 .g3 .g3 pitch_1 the arrival of the anomaly emergency squad {nato} {unit} has entered the facility. pitch_0.96 please escort2 all scpsubjects to surface zone. pitch_1 all scpsubjects need to be secured. please wait inside of your designated evacuation shelters until this emergency has been completed. there are {SCP} scpsubjects remaining."
        }
        };
    }
}
