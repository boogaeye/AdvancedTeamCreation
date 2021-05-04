namespace AdvancedTeamCreation.API
{
    using Exiled.API.Features;
    public delegate void TeamEvent<TEventArgs>(TEventArgs eventArgs) where TEventArgs : System.EventArgs;

    public class TeamEvents
    {
        public static event TeamEvent<SettingPlayerTeamEventArgs> SettingPlayerTeam;
        public static event TeamEvent<ReferencingTeamEventArgs> ReferencingTeam;
        public static event TeamEvent<AddingInventoryItemsEventArgs> AddingInventoryItems;

        public class SettingPlayerTeamEventArgs : System.EventArgs
        {
            public SettingPlayerTeamEventArgs(AdvancedTeam team, SubTeam subclass, Player player, bool isAllowed = true, bool keepItems = false)
            {
                Team = team;
                Subclass = subclass;
                Player = player;
                IsAllowed = isAllowed;
                KeepItems = keepItems;
            }

            public AdvancedTeam Team { get; set; }
            public SubTeam Subclass { get; set; }
            public Player Player { get; }
            public bool IsAllowed { get; set; }
            public bool KeepItems { get; set; }

            public void StartInvoke()
            {
                SettingPlayerTeam.Invoke(this);
            }
        }

        public class ReferencingTeamEventArgs : System.EventArgs
        {
            public ReferencingTeamEventArgs(AdvancedTeam teams, Respawning.SpawnableTeamType spawnableTeam, bool isAllowed = true, bool forceTeam = false)
            {
                Team = teams;
                IsAllowed = isAllowed;
                ForceTeam = forceTeam;
                Spawning = spawnableTeam;
            }

            public AdvancedTeam Team { get; set; }
            public bool ForceTeam { get; set; }
            public bool IsAllowed { get; set; }
            public Respawning.SpawnableTeamType Spawning { get; set; }

            public void StartInvoke()
            {
                ReferencingTeam.Invoke(this);
            }
        }

        public class AddingInventoryItemsEventArgs : System.EventArgs
        {
            public AddingInventoryItemsEventArgs(Player ply, SubTeam subteam, bool isAllowed = true, bool forceTeam = false, bool keepInv = false)
            {
                Subteam = subteam;
                IsAllowed = isAllowed;
                ForceTeam = forceTeam;
                Player = ply;
                KeepInv = keepInv;
            }

            public Player Player { get; }
            public SubTeam Subteam { get; set; }
            public bool ForceTeam { get; set; }
            public bool IsAllowed { get; set; }
            public bool KeepInv { get; set; }

            public void StartInvoke()
            {
                AddingInventoryItems.Invoke(this);
            }
        }
    }
}
