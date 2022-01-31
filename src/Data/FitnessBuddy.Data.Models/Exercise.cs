namespace FitnessBuddy.Data.Models
{
    using System.Collections.Generic;

    using FitnessBuddy.Data.Common.Models;
    using FitnessBuddy.Data.Models.Enums;

    public class Exercise : BaseDeletableModel<int>
    {
        public Exercise()
        {
            this.ExerciseTranings = new HashSet<TrainingExercise>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public ExerciseDifficulty Difficulty { get; set; }

        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public int CategoryId { get; set; }

        public virtual ExerciseCategory Category { get; set; }

        public int EquipmentId { get; set; }

        public virtual ExerciseEquipment Equipment { get; set; }

        public string AddedByUserId { get; set; }

        public virtual ApplicationUser AddedByUser { get; set; }

        public virtual ICollection<TrainingExercise> ExerciseTranings { get; set; }
    }
}
