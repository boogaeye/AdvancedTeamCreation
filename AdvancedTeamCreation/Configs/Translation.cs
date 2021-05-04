namespace AdvancedTeamCreation.Configs
{
    using Exiled.API.Interfaces;

    public class Translation : ITranslation
    {
        public string TeamKillBroadcast { get; set; } = "You got teamkilled report this to the admins if you dont think its an accident";
        public string KilledByNonfriendlyPlayer { get; set; } = "You didnt get team killed you where probably killed by someone who looks like you but isnt";
        public string FriendlyFireHint { get; set; } = "You can't hurt teams teamed with you!";
        public string NoPermissions { get; set; } = "<color=red>You dont have permission for this command</color>";
    }
}
