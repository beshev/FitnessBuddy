namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Trainings;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Trainings;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class TrainingsController : Controller
    {
        private readonly ITrainingsService trainingsService;

        public TrainingsController(ITrainingsService trainingsService)
        {
            this.trainingsService = trainingsService;
        }

        public IActionResult MyTrainings()
        {
            var viewModel = this.trainingsService.GetAll<AllTrainingsViewModel>();

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
