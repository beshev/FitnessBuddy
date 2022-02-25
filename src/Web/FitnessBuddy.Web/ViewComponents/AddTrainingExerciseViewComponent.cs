namespace FitnessBuddy.Web.ViewComponents
{
    using FitnessBuddy.Web.ViewModels.Trainings;
    using Microsoft.AspNetCore.Mvc;

    public class AddTrainingExerciseViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int exerciseId)
        {
            var viewModel = new TrainingExerciseInputModel
            {
                ExerciseId = exerciseId,
            };

            return this.View(viewModel);
        }
    }
}
