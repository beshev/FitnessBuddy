namespace FitnessBuddy.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Services.Data.Exercises;
    using FitnessBuddy.Services.Data.TrainingsExercises;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Exercises;
    using FitnessBuddy.Web.ViewModels.Trainings;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ExercisesController : Controller
    {
        private const int ExercisesPerPage = 6;

        private readonly IExercisesService exercisesService;
        private readonly ITrainingsExercisesService trainingsExercisesService;

        public ExercisesController(
            IExercisesService exercisesService,
            ITrainingsExercisesService trainingsExercisesService)
        {
            this.exercisesService = exercisesService;
            this.trainingsExercisesService = trainingsExercisesService;
        }

        public IActionResult All(int id = 1, string search = "")
        {
            int count = this.exercisesService.GetCount(search);
            int pagesCount = (int)Math.Ceiling((double)count / ExercisesPerPage);
            var skip = (id - 1) * ExercisesPerPage;

            var exercises = this.exercisesService.GetAll(search, skip, ExercisesPerPage);

            var viewModel = new AllExercisesViewModel
            {
                PageNumber = id,
                PagesCount = pagesCount,
                Exercises = exercises,
                Search = search,
                ForAction = nameof(this.All),
                ForController = this.GetType().Name.Replace(nameof(Controller), string.Empty),
            };

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExerciseInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            var userId = this.User.GetUserId();

            await this.exercisesService.AddAsync(userId, model);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult AddToTraining(int exerciseId)
        {
            var viewModel = this.exercisesService.GetById<ExerciseDetailsModel>(exerciseId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToTraining(ExerciseDetailsModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            await this.trainingsExercisesService.AddAsync(model.TrainingExercise);

            return this.Redirect(GlobalConstants.UserTrainings);
        }
    }
}
