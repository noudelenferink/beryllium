using Beryllium.Core.Players;

namespace Beryllium.Core.Trainings
{
   public class PlayerTraining
   {
      public int PlayerID { get; set; }
      public virtual Player Player { get; set; }

      public int TrainingID { get; set; }
      public virtual Training Training { get; set; }
   }
}
