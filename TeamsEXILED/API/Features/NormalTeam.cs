using System.ComponentModel;

namespace TeamsEXILED.API
{
    public class NormalTeam
    {
        [Description("Don't change the name")]
        public string Name { get; set; } = "";

        public string[] FriendlyTeams { get; set; } = new string[] { };

        public string[] Requirements { get; set; } = new string[] { };
    }
}