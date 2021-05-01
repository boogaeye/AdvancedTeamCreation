using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using TeamsEXILED.API;
using Exiled.API.Enums;
using System.ComponentModel;

namespace TeamsEXILED.Configs
{
    public class TeamsConfig
    {
        [Description("All team names have to be lowercase otherwise IT WILL NOT LET YOU SPAWN CORRECTLY!")]
        public Teams Team { get; set; } = new Teams()
        {
        };
    }
}
