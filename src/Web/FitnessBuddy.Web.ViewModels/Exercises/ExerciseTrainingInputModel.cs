namespace FitnessBuddy.Web.ViewModels.Exercises
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ExerciseTrainingInputModel : IMapFrom<Exercise>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Difficulty { get; set; }

        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public string CategoryName { get; set; }

        public string EquipmentName { get; set; }

        public int TrainingId { get; set; }

        public string UserId { get; set; }

        [Range(DataConstants.ExerciseSetsMinValue, DataConstants.ExerciseSetsMaxValue)]
        public int Sets { get; set; }

        [Range(DataConstants.ExerciseRepetitionMinValue, DataConstants.ExerciseRepetitionMaxValue)]
        public int Repetitions { get; set; }

        public double Weight { get; set; }
    }
}
