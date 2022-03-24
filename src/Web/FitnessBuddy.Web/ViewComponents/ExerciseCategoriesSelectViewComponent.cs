namespace FitnessBuddy.Web.ViewComponents
{
    using FitnessBuddy.Services.Data.Exercises;
    using FitnessBuddy.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class ExerciseCategoriesSelectViewComponent : ViewComponent
    {
        private readonly IExerciseCategoriesService exerciseCategoriesService;

        public ExerciseCategoriesSelectViewComponent(IExerciseCategoriesService exerciseCategoriesService)
        {
            this.exerciseCategoriesService = exerciseCategoriesService;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = this.exerciseCategoriesService.GetAllAsync<SelectViewModel>().GetAwaiter().GetResult();

            return this.View(viewModel);
        }
    }
}
