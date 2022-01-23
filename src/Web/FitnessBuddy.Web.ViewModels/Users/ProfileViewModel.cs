namespace FitnessBuddy.Web.ViewModels.Users
{
    public class ProfileViewModel
    {
        public string UserEmail { get; set; }

        public double CurrentProtein { get; set; }

        public double CurrentCarbohydrates { get; set; }

        public double CurrentFat { get; set; }

        public double CurrentCalories { get; set; }

        public UserViewModel UserInfo { get; set; }
    }
}
