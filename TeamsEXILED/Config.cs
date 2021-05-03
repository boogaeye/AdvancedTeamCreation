using System.ComponentModel;
using Exiled.API.Interfaces;
using TeamsEXILED.Configs;
using System.IO;
using Exiled.API.Features;
using Exiled.Loader;
using TeamsEXILED.API;
using System.Collections.Generic;
using System.Linq;

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
                foreach (Teams tm in TeamMethods.DefaultTeams)
                {
                    File.WriteAllText(Path.Combine(teamsdir, $"{tm.Name}.yml"), Loader.Serializer.Serialize(tm));
                }
            }

            var tfiles = Directory.GetFiles(teamsdir);
            foreach (var file in tfiles.Where(x => x.EndsWith("yml")))
            {
                Teams.Add(Loader.Deserializer.Deserialize<Teams>(File.ReadAllText(file)));
            }

            // This let the people change the normal requeriments and friendly teams of the base game teams
            string npath = Path.Combine(ConfigsFolder, "NormalTeams.yml");
            if (File.Exists(npath) == false)
            {
                NormalConfigs = new NormalTeams();
                File.WriteAllText(npath, Loader.Serializer.Serialize(NormalConfigs));
            }
            else
            {
                NormalConfigs = Loader.Deserializer.Deserialize<NormalTeams>(File.ReadAllText(npath));
            }

            foreach (var ntm in NormalConfigs.NTeams)
            {
                Teams.Add(new Teams
                {
                    Active = false,
                    Name = ntm.Name,
                    Requirements = ntm.Requirements,
                    Friendlys = ntm.FriendlyTeams
                });
            }
        }
    }
}
