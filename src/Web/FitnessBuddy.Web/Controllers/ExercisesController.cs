namespace FitnessBuddy.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Services.Data.Exercises;
    using FitnessBuddy.Services.Data.ExercisesLikes;
    using FitnessBuddy.Services.Data.TrainingsExercises;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Exercises;
    using Microsoft.AspNetCore.Mvc;

    public class ExercisesController : BaseController
    {
        private const int ExercisesPerPage = 6;

        private readonly IExercisesService exercisesService;
        private readonly IExerciseCategoriesService exerciseCategoriesService;
        private readonly ITrainingsExercisesService trainingsExercisesService;
        private readonly IExercisesLikesService exercisesLikesService;

        public ExercisesController(
            IExercisesService exercisesService,
            IExerciseCategoriesService exerciseCategoriesService,
            ITrainingsExercisesService trainingsExercisesService,
            IExercisesLikesService exercisesLikesService)
        {
            this.exercisesService = exercisesService;
            this.exerciseCategoriesService = exerciseCategoriesService;
            this.trainingsExercisesService = trainingsExercisesService;
            this.exercisesLikesService = exercisesLikesService;
        }

        public async Task<IActionResult> All(int id = 1, string search = "")
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            int count = await this.exercisesService.GetCountAsync(search);
            int pagesCount = (int)Math.Ceiling((double)count / ExercisesPerPage);

            if (pagesCount != 0 && id > pagesCount)
            {
                return this.NotFound();
            }

            var skip = (id - 1) * ExercisesPerPage;

            var exercises = await this.exercisesService.GetAllAsync<ExerciseViewModel>(search, skip, ExercisesPerPage);

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

            var exerciseId = await this.exercisesService.AddAsync(userId, model);

            return this.RedirectToAction(nameof(this.Details), new { Id = exerciseId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (await this.exercisesService.IsExistAsync(id) == false)
            {
                return this.NotFound();
            }

            if (await this.exercisesService.IsUserCreatorAsync(this.User.GetUserId(), id) == false)
            {
                return this.Unauthorized();
            }

            var viewModel = await this.exercisesService.GetByIdAsync<ExerciseInputModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ExerciseInputModel model)
        {
            if (await this.exercisesService.IsExistAsync(model.Id) == false)
            {
                return this.NotFound();
            }

            if (await this.exercisesService.IsUserCreatorAsync(this.User.GetUserId(), model.Id) == false)
            {
                return this.Unauthorized();
            }

            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            await this.exercisesService.EditAsync(model);

            return this.RedirectToAction(nameof(this.Details), new { model.Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (await this.exercisesService.IsExistAsync(id) == false)
            {
                return this.NotFound();
            }

            if (await this.exercisesService.IsUserCreatorAsync(this.User.GetUserId(), id) == false)
            {
                return this.Unauthorized();
            }

            await this.exercisesService.DeleteAsync(id);

            this.TempData[GlobalConstants.NameOfDelete] = string.Format(GlobalConstants.DeleteMessage, GlobalConstants.NameOfExercise);

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Details(int id)
        {
            if (await this.exercisesService.IsExistAsync(id) == false)
            {
                return this.NotFound();
            }

            var userId = this.User.GetUserId();

            var viewModel = await this.exercisesService.GetByIdAsync<ExerciseDetailsModel>(id);
            viewModel.IsCreator = await this.exercisesService.IsUserCreatorAsync(userId, id);
            viewModel.IsUserLikeExercise = await this.exercisesLikesService.IsExistsAsync(userId, id);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Category(string categoryName, int id = 1)
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            int count = await this.exerciseCategoriesService.GetCategoryExercisesCountAsync(categoryName);
            int pagesCount = (int)Math.Ceiling((double)count / ExercisesPerPage);

            if (pagesCount != 0 && id > pagesCount)
            {
                return this.NotFound();
            }

            var skip = (id - 1) * ExercisesPerPage;

            var exercises = await this.exerciseCategoriesService.GetCategoryExercisesAsync<ExerciseViewModel>(categoryName, skip, ExercisesPerPage);

            var viewModel = new AllExercisesViewModel
            {
                PageNumber = id,
                PagesCount = pagesCount,
                Exercises = exercises,
                CategoryName = categoryName,
            };

            return this.View(viewModel);
        }
    }
}
