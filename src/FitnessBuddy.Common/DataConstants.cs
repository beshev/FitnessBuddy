namespace FitnessBuddy.Common
{
    public class DataConstants
    {
        // UserFavoriteFoods table
        public const string UserFavoriteFoodTableName = "UsersFavoriteFoods";
        public const string FoodIdPropertyName = "FoodId";
        public const string UserIdPropertyName = "UserId";

        // ApplicationUser model
        public const int UserUsernameMinLength = 4;
        public const int UserUsernameMaxLength = 25;

        public const double UserWeightMinValue = 1;
        public const double UserWeightMaxValue = 300;

        public const double UserHeightMinValue = 0;
        public const double UserHeightMaxValue = 300;

        public const double UserDailyNutritionsMinValue = 0;
        public const double UserDailyNutritionsMaxValue = 1000;

        public const int UserAboutMeMinLength = 10;
        public const int UserAboutMeMaxLength = 500;

        // Food model
        public const int FoodDescriptionMinLength = 10;
        public const int FoodDescriptionMaxLength = 500;

        public const double FoodNutritionsMinValue = 0;
        public const double FoodNutritionsMaxValue = 1000;

        // FoodName model
        public const int FoodNameMinLength = 1;
        public const int FoodNameMaxLength = 50;

        // Meal model
        public const int MealNameMinLength = 1;
        public const int MealNameMaxLength = 50;
    }
}
