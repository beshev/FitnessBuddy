namespace FitnessBuddy.Web.Controllers
{
    using System.Diagnostics;

    using FitnessBuddy.Services.Data.Exercises;
    using FitnessBuddy.Services.Data.Foods;
    using FitnessBuddy.Web.Infrastructure.Filters;
    using FitnessBuddy.Web.ViewModels;
    using FitnessBuddy.Web.ViewModels.Exercises;
    using FitnessBuddy.Web.ViewModels.Foods;
    using FitnessBuddy.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    [TypeFilter(typeof(RestrictBannedUsersAttribute))]
    public class HomeController : Controller
    {
        private readonly IExercisesService exercisesService;
        private readonly IFoodsService foodsService;

        public HomeController(
            IExercisesService exercisesService,
            IFoodsService foodsService)
        {
            this.exercisesService = exercisesService;
            this.foodsService = foodsService;
        }

        public IActionResult Index()
        {
            var count = 2;

            var viewModel = new HomeViewModel
            {
                Exercises = this.exercisesService.GetRandom<ExerciseViewModel>(count),
                Foods = this.foodsService.GetRandom<FoodViewModel>(count),
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
