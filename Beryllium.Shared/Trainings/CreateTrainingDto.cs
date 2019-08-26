namespace Beryllium.Shared.Trainings
{
   using System;
   using System.Collections.Generic;
   using System.Text;

   public class CreateTrainingDto
   {
      public DateTime TrainingDate { get; set; }
      public bool IsBonus { get; set; }
      public int TeamId { get; set; }
      public int SeasonId { get; set; }
   }
}
