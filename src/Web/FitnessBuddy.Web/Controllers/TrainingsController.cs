namespace FitnessBuddy.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Services.Data.Exercises;
    using FitnessBuddy.Services.Data.Trainings;
    using FitnessBuddy.Services.Data.TrainingsExercises;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels;
    using FitnessBuddy.Web.ViewModels.Trainings;
    using FitnessBuddy.Web.ViewModels.TrainingsExercises;
    using Microsoft.AspNetCore.Mvc;

    public class TrainingsController : BaseController
    {
        private readonly ITrainingsService trainingsService;
        private readonly ITrainingsExercisesService trainingsExercisesService;
        private readonly IExercisesService exercisesService;

        public TrainingsController(
            ITrainingsService trainingsService,
            ITrainingsExercisesService trainingsExercisesService,
            IExercisesService exercisesService)
        {
            this.trainingsService = trainingsService;
            this.trainingsExercisesService = trainingsExercisesService;
            this.exercisesService = exercisesService;
        }

        public IActionResult MyTrainings(string trainingName = "")
        {
            var userId = this.User.GetUserId();

            var trainings = this.trainingsService.GetAll<SelectViewModel>(userId);
            trainingName = string.IsNullOrWhiteSpace(trainingName) ? trainings.FirstOrDefault()?.Name : trainingName;

            var trainingId = this.trainingsService.GetTrainingId(trainingName, userId);

            var viewModel = new AllTrainingsViewModel
            {
                TrainingName = trainingName,
                Trainings = trainings,
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

            return this.RedirectToAction(nameof(this.MyTrainings));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (this.trainingsService.IsExist(id) == false)
            {
                return this.NotFound();
            }

            var userId = this.User.GetUserId();

            if (this.trainingsService.IsUserTraining(id, userId) == false)
            {
                return this.Unauthorized();
            }

            await this.trainingsService.DeleteAsync(id, userId);

            return this.RedirectToAction(nameof(this.MyTrainings));
        }

        [HttpPost]
        public async Task<IActionResult> AddExercise(TrainingExerciseInputModel model)
        {
            if (this.exercisesService.IsExist(model.ExerciseId) == false
                || this.trainingsService.IsExist(model.TrainingId) == false)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid == false)
            {
                return this.RedirectToAction("Details", "Exercises", new { Id = model.ExerciseId });
            }

            await this.trainingsExercisesService.AddAsync(model);

            return this.Redirect(GlobalConstants.UserTrainings);
        }

        public async Task<IActionResult> RemoveExercise(int trainingExerciseId, string name)
        {
            if (this.trainingsExercisesService.IsExist(trainingExerciseId) == false)
            {
                return this.NotFound();
            }

            if (this.trainingsExercisesService.IsForUser(trainingExerciseId, this.User.GetUserId()) == false)
            {
                return this.Unauthorized();
            }

            await this.trainingsExercisesService.RemoveAsync(trainingExerciseId);

            return this.RedirectToAction(nameof(this.MyTrainings), new { TrainingName = name });
        }
    }
}
