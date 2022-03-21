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

        public const double UserHeightMinValue = 1;
        public const double UserHeightMaxValue = 300;

        public const double UserDailyNutritionsMinValue = 0;
        public const double UserDailyNutritionsMaxValue = 1000;

        public const int UserAboutMeMinLength = 10;
        public const int UserAboutMeMaxLength = 500;

        public const int UserBanReasonMinLength = 3;
        public const int UserBanReasonMaxLength = 500;

        // Food model
        public const int FoodDescriptionMinLength = 10;
        public const int FoodDescriptionMaxLength = 500;

        public const double FoodNutritionsMinValue = 0;
        public const double FoodNutritionsMaxValue = 1000;

        // FoodName model
        public const int FoodNameMinLength = 3;
        public const int FoodNameMaxLength = 50;

        // Meal model
        public const int MealNameMinLength = 1;
        public const int MealNameMaxLength = 50;

        public const double MealNutritionsMinValue = 0;
        public const double MealNutritionsMaxValue = 1000;

        public const double MealFoodQuantityMinValue = 1;
        public const double MealFoodQuantityMaxValue = 5000;

        // Exercise model
        public const int ExerciseNameMinLength = 3;
        public const int ExerciseNameMaxLength = 100;

        public const int ExerciseDifficultyMinValue = 1;
        public const int ExerciseDifficultyMaxValue = 3;

        public const int ExerciseSetsMinValue = 1;
        public const int ExerciseSetsMaxValue = 100;

        public const int ExerciseRepetitionMinValue = 1;
        public const int ExerciseRepetitionMaxValue = 100;

        // Training model
        public const int TrainingNameMinLength = 3;
        public const int TrainingNameMaxLength = 100;

        // Article model
        public const int ArticleTitleMinLength = 2;
        public const int ArticleTitleMaxLength = 100;

        public const int ArticleContentMinLength = 2;
        public const int ArticleContentMaxLength = 10000;

        // ArticleCategory model
        public const int ArticleCategoryNameMinLength = 2;
        public const int ArticleCategoryNameMaxLength = 20;

        // Post model
        public const int PostTitleMinLength = 1;
        public const int PostTitleMaxLength = 100;

        public const int PostDescriptionMinLength = 10;
        public const int PostDescriptionMaxLength = 5000;

        // Reply model
        public const int ReplyDescriptionMinLength = 1;
        public const int ReplyDescriptionMaxLength = 5000;

        // PostCategory model
        public const int PostCategoryNameMinLength = 1;
        public const int PostCategoryNameMaxLength = 100;

        // Message model
        public const int MessageContentMaxLength = 300;
        public const int MessageContentMinLength = 1;
    }
}
