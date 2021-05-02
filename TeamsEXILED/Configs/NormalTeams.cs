using TeamsEXILED.API;

namespace TeamsEXILED.Configs
{
    public class NormalTeams
    {
        public NormalTeam[] NTeams { get; set; } = new NormalTeam[] {
            new NormalTeam
            {
                Name = "mtf",
                FriendlyTeams = new string[] { "mtf", "rsc" },
                Requirements = new string[] { "scp", "chi", "cdp"}
            },
            new NormalTeam
            {
                Name = "chi",
                FriendlyTeams = new string[] { "chi", "cdp" },
                Requirements = new string[] { "rsc", "mtf"}
            },
            new NormalTeam
            {
                Name = "cdp",
                FriendlyTeams = new string[] { "chi", "cdp" },
                Requirements = new string[] { "scp", "rsc", "mtf" }
            },
            new NormalTeam
            {
                Name = "rsc",
                FriendlyTeams = new string[] { "mtf", "rsc" },
                Requirements = new string[] { "scp", "cdp", "chi"}
            },
            new NormalTeam
            {
                Name = "scp",
                FriendlyTeams = new string[] { "scp" },
                Requirements = new string[] { "rsc", "mtf", "cdp" }
            },
            new NormalTeam
            {
                Name = "tut",
                FriendlyTeams = new string[] { },
                Requirements = new string[] { },
            },
            new NormalTeam
            {
                Name = "rip",
            }
        };
    }
}
