namespace FitnessBuddy.Web.ViewComponents
{
    using FitnessBuddy.Services.Data.Meals;
    using FitnessBuddy.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class UserMealsSelectViewComponent : ViewComponent
    {
        private readonly IMealsService mealsService;

        public UserMealsSelectViewComponent(IMealsService mealsService)
        {
            this.mealsService = mealsService;
        }

        public IViewComponentResult Invoke(string userId)
        {
            var viewModel = this.mealsService.GetUserMeals<SelectViewModel>(userId);

            return this.View(viewModel);
        }
    }
}
