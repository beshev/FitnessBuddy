﻿namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Services.Data.Exercises;
    using FitnessBuddy.Services.Data.TrainingsExercises;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Exercises;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ExercisesController : Controller
    {
        private readonly IExercisesService exercisesService;
        private readonly ITrainingsExercisesService trainingsExercisesService;

        public ExercisesController(
            IExercisesService exercisesService,
            ITrainingsExercisesService trainingsExercisesService)
        {
            this.exercisesService = exercisesService;
            this.trainingsExercisesService = trainingsExercisesService;
        }

        public IActionResult All()
        {
            var viewModel = this.exercisesService.GetAll();

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
            var viewModel = this.exercisesService.GetById<ExerciseTrainingInputModel>(exerciseId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToTraining(ExerciseTrainingInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            await this.trainingsExercisesService.AddAsync(model);

            return this.Redirect(GlobalConstants.UserTrainings);
        }
    }
}
