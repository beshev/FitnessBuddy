namespace FitnessBuddy.Web.ViewModels.Foods
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Web.Infrastructure.Attributes;

    public class FoodViewModel
    {
        [Required]
        [MinLength(DataConstants.FoodNameMinLength)]
        [MaxLength(DataConstants.FoodNameMaxLength)]
        [Display(Name = "Food name")]
        public string FoodName { get; set; }

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

        public string ImageUrl { get; set; }
    }
}
