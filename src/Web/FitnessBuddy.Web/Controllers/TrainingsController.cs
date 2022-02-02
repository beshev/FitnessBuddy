namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Trainings;
    using FitnessBuddy.Services.Data.TrainingsExercises;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Trainings;
    using FitnessBuddy.Web.ViewModels.TrainingsExercises;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class TrainingsController : Controller
    {
        private readonly ITrainingsService trainingsService;
        private readonly ITrainingsExercisesService trainingsExercisesService;

        public TrainingsController(
            ITrainingsService trainingsService,
            ITrainingsExercisesService trainingsExercisesService)
        {
            this.trainingsService = trainingsService;
            this.trainingsExercisesService = trainingsExercisesService;
        }

        public IActionResult MyTrainings(string trainingName = "")
        {
            var userId = this.User.GetUserId();
            var trainingId = this.trainingsService.GetTrainingId(trainingName, userId);

            var viewModel = new AllTrainingsViewModel
            {
                TrainingName = trainingName,
                Trainings = this.trainingsService.GetNames(userId),
                TrainingExercises = this.trainingsExercisesService
                .GetTrainingExercises<TrainingExerciseViewModel>(trainingId),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddTraining(AllTrainingsViewModel model)
        {
            var userId = this.User.GetUserId();

            if (this.ModelState.IsValid)
            {
                await this.trainingsService.AddAsync(model.TrainingName, userId);
            }

            return this.RedirectToAction(nameof(this.MyTrainings), new { model.TrainingName });
        }
    }
}
