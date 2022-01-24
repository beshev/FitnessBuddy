namespace FitnessBuddy.Web.ViewModels.Meals
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class MealInputModel : IMapTo<Meal>
    {
        [Required]
        [MaxLength(DataConstants.MealNameMaxLength)]
        [MinLength(DataConstants.MealNameMinLength)]
        public string Name { get; set; }

        [Range(DataConstants.MealNutritionsMinValue, DataConstants.MealNutritionsMaxValue)]
        [Display(Name = "Target meal protein")]
        public double TargetProtein { get; set; }

        [Range(DataConstants.MealNutritionsMinValue, DataConstants.MealNutritionsMaxValue)]
        [Display(Name = "Target meal carbohydrates")]
        public double TargetCarbs { get; set; }

        [Range(DataConstants.MealNutritionsMinValue, DataConstants.MealNutritionsMaxValue)]
        [Display(Name = "Target meal fat")]
        public double TargetFat { get; set; }
    }
}
