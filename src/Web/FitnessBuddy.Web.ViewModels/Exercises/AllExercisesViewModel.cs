namespace FitnessBuddy.Web.ViewModels.Exercises
{
    using System.Collections.Generic;

    public class AllExercisesViewModel : PagingViewModel
    {
        public string CategoryName { get; set; }

        public IEnumerable<ExerciseViewModel> Exercises { get; set; }
    }
}
