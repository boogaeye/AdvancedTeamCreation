using TeamsEXILED.API;

namespace TeamsEXILED.Configs
{
    public class NormalTeams
    {
        public Teams[] Teams { get; set; } = new Teams[] {
            new Teams()
            {
                Active = true,
                Name = "mtf",
                Friendlys = new string[] { "mtf", "rsc" },
                Requirements = new string[] { "scp", "chi", "cdp"},
                Neutral = new string[]{ },
                
            },
            new Teams()
            {
                Active = true,
                Name = "chi",
                Friendlys = new string[] { "chi", "cdp" },
                Requirements = new string[] { "rsc", "mtf"},
                Neutral = new string[]{ }
            },
            new Teams()
            {
                Active = true,
                Name = "cdp",
                Friendlys = new string[] { "chi", "cdp" },
                Requirements = new string[] { "scp", "rsc", "mtf" },
                Neutral = new string[]{ }
            },
            new Teams()
            {
                Active = true,
                Name = "rsc",
                Friendlys = new string[] { "mtf", "rsc" },
                Requirements = new string[] { "scp", "cdp", "chi"},
                Neutral = new string[]{  }
            },
            new Teams()
            {
                Active = true,
                Name = "scp",
                Friendlys = new string[] { "scp" },
                Requirements = new string[] { "rsc", "mtf", "cdp" },
                Neutral = new string[]{ }
            },
            new Teams()
            {
                Active = true,
                Name = "tut",
                Friendlys = new string[] { },
                Requirements = new string[] { },
                Neutral = new string[]{ }
            },
            new Teams()
            {
                Active = true,
                Name = "rip",
            }
        };
    }
}
