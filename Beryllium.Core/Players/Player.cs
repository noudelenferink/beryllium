namespace Beryllium.Core.Players
{
   using System.ComponentModel.DataAnnotations;
   using System.ComponentModel.DataAnnotations.Schema;

   [Table("Player")]
   public class Player : Entity
   {
      [Column("PlayerID")]
      public override int Id { get; set; }
      public string FirstName { get; set; }

      [Column("SurNamePrefix")]
      public string LastNamePrefix { get; set; }

      [Column("SurName")]
      public string LastName { get; set; }
   }
}
