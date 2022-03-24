namespace FitnessBuddy.Web.ViewComponents
{
    using FitnessBuddy.Services.Data.Meals;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class UserMealsSelectViewComponent : ViewComponent
    {
        private readonly IMealsService mealsService;

        public UserMealsSelectViewComponent(IMealsService mealsService)
        {
            this.mealsService = mealsService;
        }

        public IViewComponentResult Invoke()
        {
            var userId = this.UserClaimsPrincipal.GetUserId();

            var viewModel = this.mealsService.GetUserMealsAsync<SelectViewModel>(userId).GetAwaiter().GetResult();

            return this.View(viewModel);
        }
    }
}
