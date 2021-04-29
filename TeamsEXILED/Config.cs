using System.ComponentModel;
using Exiled.API.Interfaces;
using TeamsEXILED.Configs;
using System.IO;
using Exiled.API.Features;
using Exiled.Loader;

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

        public bool Debug { get; set; } = false;

        public string ConfigsFolder { get; set; } = Path.Combine(Paths.Configs, "AdvancedTeamCreation");

        public TeamsConfig TeamsConfigs;
        public Translations TransConfigs;
        public NormalTeams NormalConfigs;

        public void LoadTeamsConfig()
        {
            if (Directory.Exists(ConfigsFolder) == false)
            {
                Directory.CreateDirectory(ConfigsFolder);
            }

            string fpath = Path.Combine(ConfigsFolder, "Teams.yml");
            if (File.Exists(fpath) == false)
            {
                TeamsConfigs = new TeamsConfig();
                File.WriteAllText(fpath, Loader.Serializer.Serialize(TeamsConfigs));
            }
            else
            {
                TeamsConfigs = Loader.Deserializer.Deserialize<TeamsConfig>(File.ReadAllText(fpath));
                File.WriteAllText(fpath, Loader.Serializer.Serialize(TeamsConfigs));
            }

            string tpath = Path.Combine(ConfigsFolder, "Translations.yml");
            if (File.Exists(tpath) == false)
            {
                TransConfigs = new Translations();
                File.WriteAllText(tpath, Loader.Serializer.Serialize(TransConfigs));
            }
            else
            {
                TransConfigs = Loader.Deserializer.Deserialize<Translations>(File.ReadAllText(tpath));
                File.WriteAllText(tpath, Loader.Serializer.Serialize(TransConfigs));
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
    }
}
