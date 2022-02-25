namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Foods;
    using FitnessBuddy.Services.Data.Meals;
    using FitnessBuddy.Services.Data.MealsFoodsService;
    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Meals;
    using FitnessBuddy.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class MealsController : Controller
    {
        private readonly IMealsService mealsService;
        private readonly IFoodsService foodsService;
        private readonly IMealsFoodsService mealsFoodsService;
        private readonly IUsersService usersService;

        public MealsController(
            IMealsService mealsService,
            IFoodsService foodsService,
            IMealsFoodsService mealsFoodsService,
            IUsersService usersService)
        {
            this.mealsService = mealsService;
            this.foodsService = foodsService;
            this.mealsFoodsService = mealsFoodsService;
            this.usersService = usersService;
        }

        [Authorize]
        public IActionResult MyMeals()
        {
            string userId = this.User.GetUserId();
            var allMeals = this.mealsService.GetUserMeals<MealViewModel>(userId);
            var userNutrients = this.usersService.GetUserInfo<UserTargetNutrientsViewModel>(userId);

            var viewModel = new AllMealsViewModel
            {
                UserNutrients = userNutrients,
                Meals = allMeals,
            };

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(MealInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            var userId = this.User.GetUserId();
            await this.mealsService.CreateAsync(userId, model);

            return this.RedirectToAction(nameof(this.MyMeals));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int mealId)
        {
            if (this.mealsService.IsExist(mealId) == false)
            {
                return this.NotFound();
            }

            if (this.mealsService.IsUserMeal(mealId, this.User.GetUserId()) == false)
            {
                return this.Unauthorized();
            }

            await this.mealsService.DeleteAsync(mealId);

            return this.RedirectToAction(nameof(this.MyMeals));
        }
    }
}
