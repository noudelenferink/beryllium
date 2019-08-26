namespace Beryllium.Shared.Trainings
{
   using System.ComponentModel.DataAnnotations;

   public class GetTrainingsInputDto
   {
      [Required]
      public int SeasonId { get; set; }

      [Required]
      public int TeamId { get; set; }

      [Required]
      public bool IncludeBonus { get; set; }
   }
}