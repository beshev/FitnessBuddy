namespace FitnessBuddy.Web.ViewModels.Trainings
{
    using System.Collections.Generic;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.TrainingsExercises;

    public class AllTrainingsViewModel : IMapFrom<Training>
    {
        public string Name { get; set; }

        public IEnumerable<TrainingExerciseViewModel> TrainingExercises { get; set; }
    }
}
