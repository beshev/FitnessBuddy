namespace FitnessBuddy.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Exercises;
    using FitnessBuddy.Services.Data.Foods;
    using FitnessBuddy.Web.ViewModels;
    using FitnessBuddy.Web.ViewModels.Exercises;
    using FitnessBuddy.Web.ViewModels.Foods;
    using FitnessBuddy.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    public class HomeController : BaseController
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

        public async Task<IActionResult> Index()
        {
            var count = 2;

            var viewModel = new HomeViewModel
            {
                Exercises = await this.exercisesService.GetRandomAsync<ExerciseViewModel>(count),
                Foods = await this.foodsService.GetRandomAsync<FoodViewModel>(count),
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
