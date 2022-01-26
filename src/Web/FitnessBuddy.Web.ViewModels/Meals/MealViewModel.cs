namespace FitnessBuddy.Web.ViewModels.Meals
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class MealViewModel : IMapFrom<Meal>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Current meal protein")]
        public double CurrentProtein => this.MealFoods.Sum(x => x.Protein);

        [Display(Name = "Current meal carbohydrates")]
        public double CurrentCarbohydrates => this.MealFoods.Sum(x => x.Carbohydrates);

        [Display(Name = "Current meal fat")]
        public double CurrentFats => this.MealFoods.Sum(x => x.Fats);

        [Display(Name = "Target meal protein")]
        public double TargetProtein { get; set; }

        [Display(Name = "Target meal carbohydrates")]
        public double TargetCarbs { get; set; }

        [Display(Name = "Target meal fat")]
        public double TargetFat { get; set; }

        [Display(Name = "Target meal calories")]
        public double TotalCalories
            => this.MealFoods.Sum(x => x.Calories);

        public IEnumerable<MealFoodViewModel> MealFoods { get; set; }
    }
}
