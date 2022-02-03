namespace FitnessBuddy.Web.ViewModels.Trainings
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.TrainingsExercises;

    public class AllTrainingsViewModel : IMapFrom<Training>
    {
        [Required(ErrorMessage = "Field is required!")]
        [StringLength(DataConstants.TrainingNameMaxLength, MinimumLength = DataConstants.TrainingNameMinLength, ErrorMessage = "Name must be between {1} and {2}.")]
        public string TrainingName { get; set; }

        public IEnumerable<SelectViewModel> Trainings { get; set; }

        public IEnumerable<TrainingExerciseViewModel> TrainingExercises { get; set; }
    }
}
