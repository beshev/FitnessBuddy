namespace FitnessBuddy.Data.Models
{
    using FitnessBuddy.Data.Common.Models;

    public class TrainingExercise : BaseDeletableModel<int>
    {
        public int ExerciseId { get; set; }

        public virtual Exercise Exercise { get; set; }

        public int TrainingId { get; set; }

        public virtual Training Training { get; set; }

        public int Sets { get; set; }

        public int Repetitions { get; set; }

        public double Weight { get; set; }
    }
}
