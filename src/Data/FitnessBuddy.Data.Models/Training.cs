namespace FitnessBuddy.Data.Models
{
    using System.Collections.Generic;

    using FitnessBuddy.Data.Common.Models;

    public class Training : BaseDeletableModel<int>
    {
        public Training()
        {
            this.TrainingExercises = new HashSet<TrainingExercise>();
        }

        public string Name { get; set; }

        public string ForUserId { get; set; }

        public virtual ApplicationUser ForUser { get; set; }

        public virtual ICollection<TrainingExercise> TrainingExercises { get; set; }
    }
}
