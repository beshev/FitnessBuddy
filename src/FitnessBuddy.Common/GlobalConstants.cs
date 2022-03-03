namespace FitnessBuddy.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "FitnessBuddy";

        public const string AdministratorRoleName = "Administrator";
        public const string UserRoleName = "User";

        // Nutritions constants
        public const int CaloriesForOneGramProteinAndCarbohydrates = 4;
        public const int CaloriesForOneGramFats = 9;

        // Constants for redirect
        public const string MyMealsUrl = "/Meals/MyMeals";
        public const string UserTrainings = "/Trainings/MyTrainings";

        // Names
        public const string NameOfFollowers = "followers";
        public const string NameOfFollowing = "following";
        public const string NameOfTab = "tab";
        public const string NameOfDelete = "delete";
        public const string NameOfSuccess = "success";
        public const string NameOfExercise = "exercise";
        public const string NameOfFood = "food";

        // CRUD messages
        public const string DeleteMessage = "The {0} was deleted successfully";
        public const string SuccessMessage = "The {0} was added successfully";
        public const string AddFoodToFavoriteMessage = "The food was added successfully to favorites";
        public const string RemoveFoodFromFavoriteMessage = "The food was removed successfully from favorites";
    }
}
