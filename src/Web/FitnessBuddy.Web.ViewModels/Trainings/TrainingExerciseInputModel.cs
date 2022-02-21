namespace FitnessBuddy.Web.ViewModels.Trainings
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;

    public class TrainingExerciseInputModel
    {
        public int TrainingId { get; set; }

        public int ExerciseId { get; set; }

        [Range(DataConstants.ExerciseSetsMinValue, DataConstants.ExerciseSetsMaxValue)]
        public int Sets { get; set; }

        [Range(DataConstants.ExerciseRepetitionMinValue, DataConstants.ExerciseRepetitionMaxValue)]
        public int Repetitions { get; set; }

        public double Weight { get; set; }
    }
}
