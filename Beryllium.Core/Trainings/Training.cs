namespace Beryllium.Core.Trainings
{
   using Beryllium.Core.Players;
   using System;
   using System.Collections.Generic;
   using System.ComponentModel.DataAnnotations;
   using System.ComponentModel.DataAnnotations.Schema;

   [Table("Training")]
   public class Training : Entity
   {
      [Column("TrainingID")]
      public override int Id { get; set; }
      public DateTime TrainingDate { get; set; }
      public bool IsBonus { get; set; }
      public int SeasonID { get; set; }
      public int TeamID { get; set; }

      public virtual List<PlayerTraining> TrainingPlayers { get; set; } = new List<PlayerTraining>();
   }
}