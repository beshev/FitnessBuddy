namespace FitnessBuddy.Web.ViewComponents
{
    using FitnessBuddy.Services.Data.Exercises;
    using FitnessBuddy.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class ExerciseEquipmentSelectViewComponent : ViewComponent
    {
        private readonly IExerciseEquipmentService exerciseEquipmentService;

        public ExerciseEquipmentSelectViewComponent(IExerciseEquipmentService exerciseEquipmentService)
        {
            this.exerciseEquipmentService = exerciseEquipmentService;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = this.exerciseEquipmentService.GetAll<SelectViewModel>();

            return this.View(viewModel);
        }
    }
}
