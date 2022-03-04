namespace FitnessBuddy.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Exercises;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Exercises;
    using Microsoft.AspNetCore.Mvc;

    public class ExercisesController : AdministrationController
    {
        private readonly IExercisesService exercisesService;

        public ExercisesController(IExercisesService exercisesService)
        {
            this.exercisesService = exercisesService;
        }

        public IActionResult Index()
        {
            var viewModel = this.exercisesService
                .GetAll();

            return this.View(viewModel);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var exercise = this.exercisesService.GetById<ExerciseDetailsModel>(id.Value);

            if (exercise == null)
            {
                return this.NotFound();
            }

            return this.View(exercise);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExerciseInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            await this.exercisesService.AddAsync(this.User.GetUserId(), model);

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var exercise = this.exercisesService.GetById<ExerciseInputModel>(id.Value);

            if (exercise == null)
            {
                return this.NotFound();
            }

            return this.View(exercise);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExerciseInputModel model)
        {
            if (this.exercisesService.IsExist(model.Id) == false)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            await this.exercisesService.EditAsync(model);

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var exercise = this.exercisesService.GetById<ExerciseViewModel>(id.Value);

            if (exercise == null)
            {
                return this.NotFound();
            }

            return this.View(exercise);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this.exercisesService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
