namespace Beryllium.Shared.Trainings
{
   using Beryllium.Shared.Players;
   using System.Collections.Generic;

   public class TrainingSeasonOverviewDto
   {
      public PlayerListDto Player { get; set; }
      public decimal AttendancePercentage { get; set; }
      public int BonusAttended { get; set; }
      public Dictionary<string, bool> Recent { get; set; }
   }
}
