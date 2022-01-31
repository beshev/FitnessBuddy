namespace FitnessBuddy.Data.Models
{
    using System.Collections.Generic;

    using FitnessBuddy.Data.Common.Models;

    public class ExerciseCategory : BaseDeletableModel<int>
    {
        public ExerciseCategory()
        {
            this.Exercises = new HashSet<Exercise>();
        }

        public string Name { get; set; }

        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
