namespace FitnessBuddy.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Data.Common.Models;
    using FitnessBuddy.Data.Models.Enums;

    public class Exercise : BaseDeletableModel<int>
    {
        public Exercise()
        {
            this.ExerciseTranings = new HashSet<TrainingExercise>();
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public ExerciseDifficulty Difficulty { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        [Url]
        public string VideoUrl { get; set; }

        public int CategoryId { get; set; }

        public virtual ExerciseCategory Category { get; set; }

        public int EquipmentId { get; set; }

        public virtual ExerciseEquipment Equipment { get; set; }

        [Required]
        public string AddedByUserId { get; set; }

        public virtual ApplicationUser AddedByUser { get; set; }

        public virtual ICollection<TrainingExercise> ExerciseTranings { get; set; }
    }
}
