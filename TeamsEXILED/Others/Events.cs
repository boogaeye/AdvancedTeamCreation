using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsEXILED.API;
using Exiled.API.Features;

namespace TeamsEXILED.Events
{
    public delegate void TeamEvent();
    public delegate void TeamEvent<TEventArgs>(TEventArgs eventArgs) where TEventArgs : System.EventArgs;

    public class EventArgs
    {
        public static event TeamEvent<SetTeamEventArgs> SetTeam;
        public static event TeamEvent<CreatingTeamEventArgs> CreatingTeam;
        public static event TeamEvent<TeamReferencedEventArgs> ReferencingTeam;

        public class SetTeamEventArgs : System.EventArgs
        {
            public SetTeamEventArgs(string team, string subclass, Player player, bool isAllowed = true)
            {
                Team = team;
                Subclass = subclass;
                Player = player;
                IsAllowed = isAllowed;
            }

            public string Team { get; set; }
            public string Subclass { get; set; }
            public Player Player { get; }
            public bool IsAllowed { get; set; }

            public void StartInvoke()
            {
                SetTeam?.Invoke(this);
            }
        }

        public class CreatingTeamEventArgs : System.EventArgs
        {
            public CreatingTeamEventArgs(Teams team)
            {
                Team = team;
            }

            public Teams Team { get; set; }

            public void StartInvoke()
            {
                CreatingTeam.Invoke(this);
            }
        }

        public class TeamReferencedEventArgs : System.EventArgs
        {
            public TeamReferencedEventArgs(Teams teams, bool isAllowed = true, bool forceTeam = false)
            {
                Team = teams;
                IsAllowed = isAllowed;
                ForceTeam = forceTeam;
            }

            public Teams Team { get; set; }
            public bool ForceTeam { get; set; }
            public bool IsAllowed { get; set; }

            public void StartInvoke()
            {
                ReferencingTeam.Invoke(this);
            }
        }
    }
}
