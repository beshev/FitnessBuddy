namespace FitnessBuddy.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Common.Models;

    public class Training : BaseDeletableModel<int>
    {
        public Training()
        {
            this.TrainingExercises = new HashSet<TrainingExercise>();
        }

        [Required]
        [MinLength(DataConstants.TrainingNameMinLength)]
        [MaxLength(DataConstants.TrainingNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string ForUserId { get; set; }

        public virtual ApplicationUser ForUser { get; set; }

        public virtual ICollection<TrainingExercise> TrainingExercises { get; set; }
    }
}
