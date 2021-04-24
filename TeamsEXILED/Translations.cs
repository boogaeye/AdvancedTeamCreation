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
    public class Translations : ITranslation
    {
        public string TeamKillBroadcast { get; set; } = "You got teamkilled report this to the admins if you dont think its an accident";
        public string KilledByNonfriendlyPlayer { get; set; } = "You didnt get team killed you where probably killed by someone who looks like you but isnt";
    }
}
