namespace FitnessBuddy.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Data.Common.Models;

    public class ExerciseCategory : BaseDeletableModel<int>
    {
        public ExerciseCategory()
        {
            this.Exercises = new HashSet<Exercise>();
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
