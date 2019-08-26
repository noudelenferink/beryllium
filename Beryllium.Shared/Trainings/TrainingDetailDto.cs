namespace Beryllium.Shared.Trainings
{
    using System;
    using System.Collections.Generic;
    using Beryllium.Shared.Players;

    public class TrainingDetailDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsBonus { get; set; }
        public List<PlayerListDto> Attendees { get; set; }
      public int SeasonId { get; set; }
      public int TeamId { get; set; }
   }
}
