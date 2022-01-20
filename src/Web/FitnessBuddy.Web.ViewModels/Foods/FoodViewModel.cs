namespace FitnessBuddy.Web.ViewModels.Foods
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.Infrastructure.Attributes;

    public class FoodViewModel : IMapFrom<Food>
    {
        [Required]
        [MinLength(DataConstants.FoodNameMinLength)]
        [MaxLength(DataConstants.FoodNameMaxLength)]
        [Display(Name = "Food name")]

        // Property name for automapper
        public string FoodNameName { get; set; }

        [Required]
        [MinLength(DataConstants.FoodDescriptionMinLength)]
        [MaxLength(DataConstants.FoodDescriptionMaxLength)]
        public string Description { get; set; }

        [Range(DataConstants.FoodNutritionsMinValue, DataConstants.FoodNutritionsMaxValue)]
        [Display(Name = "Protein (in 100 grams)")]
        public double ProteinIn100Grams { get; set; }

        [Range(DataConstants.FoodNutritionsMinValue, DataConstants.FoodNutritionsMaxValue)]
        [Display(Name = "Carbohydrates (in 100 grams)")]
        public double CarbohydratesIn100Grams { get; set; }

        [Range(DataConstants.FoodNutritionsMinValue, DataConstants.FoodNutritionsMaxValue)]
        [Display(Name = "Fat (in 100 grams)")]
        public double FatIn100Grams { get; set; }

        [NonNegativeNumber]
        public double Sodium { get; set; }

        public string FoodCalories
            => (((this.ProteinIn100Grams + this.CarbohydratesIn100Grams) * 4) + (this.FatIn100Grams * 9)).ToString("F2");

        public string ImageUrl { get; set; }
    }
}
