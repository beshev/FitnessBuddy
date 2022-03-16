namespace FitnessBuddy.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using FitnessBuddy.Web.ViewModels.Exercises;
    using FitnessBuddy.Web.ViewModels.Foods;

    public class HomeViewModel
    {
        public IEnumerable<ExerciseViewModel> Exercises { get; set; }

        public IEnumerable<FoodViewModel> Foods { get; set; }
    }
}
