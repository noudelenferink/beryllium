namespace Beryllium.Shared.Trainings
{
    using System;

    public class TrainingListDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public bool IsBonus { get; set; }

        public int NumAttendees { get; set; }

    }
}
