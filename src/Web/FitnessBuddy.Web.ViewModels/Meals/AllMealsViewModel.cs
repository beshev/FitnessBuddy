namespace FitnessBuddy.Web.ViewModels.Meals
{
    using System.Collections.Generic;
    using System.Linq;

    using FitnessBuddy.Web.ViewModels.Users;

    public class AllMealsViewModel
    {
        public double TotalProtein => this.Meals.Sum(x => x.CurrentProtein);

        public double TotalCarbohydrates => this.Meals.Sum(x => x.CurrentCarbohydrates);

        public double TotalFats => this.Meals.Sum(x => x.CurrentFats);

        public double TotalCalories => this.Meals.Sum(x => x.TotalCalories);

        public double RemainingCalories => this.UserNutrients.DailyCaloriesGoal - this.TotalCalories;

        public double RemainingProtein => this.UserNutrients.DailyProteinGoal - this.TotalProtein;

        public double RemainingCarbohydrates => this.UserNutrients.DailyCarbohydratesGoal - this.TotalCarbohydrates;

        public double RemainingFat => this.UserNutrients.DailyFatGoal - this.TotalFats;

        public UserTargetNutrientsViewModel UserNutrients { get; set; }

        public IEnumerable<MealViewModel> Meals { get; set; }
    }
}
