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

        public string TeamKillBroadcast { get; set; } = "You got teamkilled report this to the admins if you dont think its an accident";

        public string KilledByNonfriendlyPlayer { get; set; } = "You didnt get team killed you where probably killed by someone who looks like you but isnt";

        [Description("allows friendly teams to hurt eachother no matter what hurts them")]
        public bool FriendlyFire { get; set; } = false;

        [Description("If enabled the RoleHint message will be displayed as a hint, else as a broadcast")]
        public bool UseHints { get; set; } = true;

        [Description("Should display the description of the customitem when given?")]
        public bool DisplayDescription { get; set; } = false;

        public bool Debug { get; set; } = false;

        public string ConfigsFolder { get; set; } = Path.Combine(Paths.Configs, "AdvancedTeamCreation");

        public TeamsConfig TeamsConfigs;

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
        }
    }
}
