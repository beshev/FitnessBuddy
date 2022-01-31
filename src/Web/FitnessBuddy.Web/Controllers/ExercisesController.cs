namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Exercises;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Exercises;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ExercisesController : Controller
    {
        private readonly IExercisesService exercisesService;

        public ExercisesController(IExercisesService exercisesService)
        {
            this.exercisesService = exercisesService;
        }

        public IActionResult All()
        {
            var viewModel = this.exercisesService.GetAll();

            return this.View(viewModel);
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ExerciseInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            var userId = this.User.GetUserId();

            await this.exercisesService.AddAsync(userId, model);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Details(int exerciseId)
        {
            var viewModel = this.exercisesService.GetById<ExerciseDetailsModel>(exerciseId);

            return this.View(viewModel);
        }
    }
}
