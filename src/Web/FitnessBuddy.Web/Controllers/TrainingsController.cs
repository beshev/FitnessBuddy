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
            var trainingId = this.trainingsService.GetIdByName(trainingName);

            var viewModel = new AllTrainingsViewModel
            {
                Trainings = this.trainingsService.GetNames(),
                TrainingExercises = this.trainingsExercisesService
                .GetTrainingExercises<TrainingExerciseViewModel>(trainingId),
            };

            return this.View(viewModel);
        }

        public IActionResult AddTraining()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTraining(TrainingInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                this.View(model);
            }

            var userId = this.User.GetUserId();
            model.UserId = userId;

            await this.trainingsService.AddAsync(model);

            return this.RedirectToAction(nameof(this.MyTrainings));
        }
    }
}
