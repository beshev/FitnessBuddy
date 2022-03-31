namespace FitnessBuddy.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "FitnessBuddy";
        public const string AntiforgeryHeaderName = "X-CSRF-TOKEN";

        public const string AdministratorRoleName = "Administrator";
        public const string UserRoleName = "User";

        // Nutritions constants
        public const int CaloriesForOneGramProteinAndCarbohydrates = 4;
        public const int CaloriesForOneGramFats = 9;

        // Constants for redirect
        public const string MyMealsUrl = "/Meals/MyMeals";
        public const string UserTrainings = "/Trainings/MyTrainings?trainingName={0}";
        public const string RestrictionBan = "/Restrictions/Ban";

        // Names
        public const string NameOfFollowers = "Followers";
        public const string NameOfFollowing = "Following";
        public const string NameOfTab = "Tab";
        public const string NameOfDelete = "Delete";
        public const string NameOfSuccess = "Success";
        public const string NameOfExercise = "Exercise";
        public const string NameOfFood = "Food";
        public const string NameOfArticle = "Article";
        public const string NameOfPost = "Post";
        public const string NameOfCategoryName = "CategoryName";
        public const string NameOfSelected = "Selected";
        public const string NameOfCategory = "Category";

        // CRUD messages
        public const string DeleteMessage = "The {0} was deleted successfully";
        public const string SuccessMessage = "The {0} was added successfully";
        public const string AddFoodToFavoriteMessage = "The food was added successfully to favorites";
        public const string RemoveFoodFromFavoriteMessage = "The food was removed successfully from favorites";
        public const string EditFoodMessage = "The food was edited successfully";
        public const string SentEmailMessage = "The email was sent successfully";
        public const string InvalidCategoryMessage = "Invalid category!";

        // Format
        public const string DateTimeFormat = "MM/dd/yyyy HH:mm";
        public const string HubGroupNameFormat = "{0}{1}";
    }
}
