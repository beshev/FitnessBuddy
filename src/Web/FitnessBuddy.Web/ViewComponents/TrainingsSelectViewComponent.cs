namespace FitnessBuddy.Web.ViewComponents
{
    using FitnessBuddy.Services.Data.Trainings;
    using FitnessBuddy.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class TrainingsSelectViewComponent : ViewComponent
    {
        private readonly ITrainingsService trainingsService;

        public TrainingsSelectViewComponent(ITrainingsService trainingsService)
        {
            this.trainingsService = trainingsService;
        }

        public IViewComponentResult Invoke(string userId)
        {
            var viewModel = this.trainingsService.GetAll<SelectViewModel>(userId);

            return this.View(viewModel);
        }
    }
}
