namespace FitnessBuddy.Data.Models
{
    using FitnessBuddy.Data.Common.Models;

    public class MealFood : BaseDeletableModel<int>
    {
        public int FoodId { get; set; }

        public Food Food { get; set; }

        public int MealId { get; set; }

        public Meal Meal { get; set; }

        public double QuantityInGrams { get; set; }
    }
}
