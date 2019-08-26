namespace Beryllium.Core.Players
{
   using System;
   using System.ComponentModel.DataAnnotations;
   using System.ComponentModel.DataAnnotations.Schema;

   [Table("PlayerTeam")]
   public class TeamPlayer : Entity
   {
      [Column("PlayerTeamID")]
      public override int Id { get; set; }
      public int PlayerID { get; set; }
      public virtual Player Player { get; set; }
      public int TeamID { get; set; }
      //public virtual Team Team { get; set; }
      public int SeasonID { get; set; }
      //public virtual Season Season { get; set; }
      public int? JerseyNumber { get; set; }
      public DateTime EffectiveDate { get; set; }
      public DateTime? ExpiryDate { get; set; }

   }
}
