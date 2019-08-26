namespace Beryllium.Shared.Trainings
{
   using System;
   using System.Collections.Generic;
   using System.Text;

   public class UpdateTrainingDto
   {
      public int Id { get; set; }

      public DateTime TrainingDate { get; set; }
      public bool IsBonus { get; set; }
      public List<int> Attendees { get; set; }
   }
}
