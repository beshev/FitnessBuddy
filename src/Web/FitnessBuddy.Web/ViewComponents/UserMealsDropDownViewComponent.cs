namespace FitnessBuddy.Web.ViewComponents
{
    using FitnessBuddy.Services.Data.Meals;
    using FitnessBuddy.Web.ViewModels.Meals;
    using Microsoft.AspNetCore.Mvc;

    public class UserMealsDropDownViewComponent : ViewComponent
    {
        private readonly IMealsService mealsService;

        public UserMealsDropDownViewComponent(IMealsService mealsService)
        {
            this.mealsService = mealsService;
        }

        public IViewComponentResult Invoke(string userId)
        {
            var viewModel = this.mealsService.GetUserMeals<MealDropDownViewModel>(userId);

            return this.View(viewModel);
        }
    }
}
