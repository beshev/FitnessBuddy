﻿namespace FitnessBuddy.Web.Controllers
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
        public IActionResult AddFood(int foodId)
        {
            var food = this.foodsService.GetById(foodId);

            if (food == null)
            {
                return this.NotFound();
            }

            var viewModel = new MealFoodInputModel
            {
                FoodId = food.Id,
                FoodName = food.FoodName.Name,
            };

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddFood(MealFoodInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            if (this.mealsService.Contains(model.MealId) == false
                || this.foodsService.Contains(model.FoodId) == false)
            {
                return this.NotFound();
            }

            await this.mealsFoodsService.AddAsync(model);

            return this.RedirectToAction(nameof(this.MyMeals));
        }

        [Authorize]
        public async Task<IActionResult> RemoveFood(int mealFoodId)
        {
            var mealFood = this.mealsFoodsService.GetById(mealFoodId);

            if (mealFood == null || mealFood.Meal.ForUserId != this.User.GetUserId())
            {
                return this.NotFound();
            }

            await this.mealsFoodsService.DeleteAsync(mealFood);

            return this.RedirectToAction(nameof(this.MyMeals));
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
            var meal = this.mealsService.GetById(mealId);
            var userId = this.User.GetUserId();

            if (meal != null && meal.ForUserId == userId)
            {
                await this.mealsService.DeleteAsync(mealId);
            }

            return this.RedirectToAction(nameof(this.MyMeals));
        }
    }
}
