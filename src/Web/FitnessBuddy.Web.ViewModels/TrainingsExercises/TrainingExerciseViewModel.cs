namespace FitnessBuddy.Web.ViewModels.TrainingsExercises
{
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class TrainingExerciseViewModel : IMapFrom<TrainingExercise>
    {
        public string ExerciseName { get; set; }

        public string ExerciseImageUrl { get; set; }

        public int Sets { get; set; }

        public int Repetitions { get; set; }

        public double Weight { get; set; }
    }
}
