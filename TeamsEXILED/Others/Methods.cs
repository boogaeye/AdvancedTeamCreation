using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsEXILED.API;
using Exiled.API.Features;
using TeamsEXILED.Enums;

namespace TeamsEXILED
{
    public static class Methods
    {
        public static string PointThings(SpawnLocation loc)
        {
            string name = "";
            switch (loc)
            {
                case SpawnLocation.Escape:
                {
                    name = "Outside:187.4,-7.2,-14.4:356.5,179.7";
                    break;
                }
                case SpawnLocation.SurfaceNuke:
                {
                    name = "Outside:40.6,-11.0,-42.6:1.5,180.3";
                    break;
                }
                case SpawnLocation.Shelter:
                {
                    name = "EZ_Shelter:0.0,1.4,5.8:-3.1,181.5";
                    break;
                }
                case SpawnLocation.SCP173:
                {
                    name = "LCZ_173:12.2,17.2,13.3:0.0,90.3";
                    break;
                }
                case SpawnLocation.SCP106:
                {
                    name = "HCZ_106:19.6,1.4,0.1:0.1,269.9";
                    break;
                }
                case SpawnLocation.SCP096:
                {
                    name = "HCZ_457:-7.7,1.4,0.0:-1.2,90.3";
                    break;
                }
                case SpawnLocation.SCP079:
                {
                    name = "HCZ_079:-1.2,1.4,0.1:0.3,269.7";
                    break;
                }
                case SpawnLocation.SCP012:
                {
                    name = "LCZ_012:6.4,1.4,-0.1:1.5,270.9";
                    break;
                }
            }

            return name;
        }
    }
}
