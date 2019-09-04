namespace Beryllium.Shared.Session
{
   public class CurrentUserInformation
   {
      public UserDto User { get; set; }
      public AuthorizationInfoDto Authorization { get; set; }
      public EntityInfoDto DefaultTeam { get; set; }
      public EntityInfoDto DefaultSeason { get; set; }
   }
}
