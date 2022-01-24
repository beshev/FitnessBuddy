namespace FitnessBuddy.Web.ViewModels.Meals
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;

    public class MealFoodInputModel
    {
        public string FoodName { get; set; }

        public int FoodId { get; set; }

        public int MealId { get; set; }

        [Range(
            DataConstants.MealFoodQuantityMinValue,
            DataConstants.MealFoodQuantityMaxValue,
            ErrorMessage = "Quantity must be between {1} and {2}.")]
        [Display(Name = "Quantity in grams")]
        public double QuantityInGrams { get; set; }
    }
}
