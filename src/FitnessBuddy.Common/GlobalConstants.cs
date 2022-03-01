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

        // Followers, Following and Tab
        public const string NameOfFollowers = "followers";
        public const string NameOfFollowing = "following";
        public const string NameOfTab = "tab";
    }
}
