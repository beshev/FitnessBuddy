namespace FitnessBuddy.Web.ViewModels.Users
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Current weights (in kg)")]
        public double WeightInKg { get; set; }

        [Display(Name = "Goal weights (in kg)")]
        public double GoalWeightInKg { get; set; }

        [Display(Name = "Profile picture")]
        public string ProfilePicture { get; set; }

        [Display(Name = "Height (in cm)")]
        public double HeightInCm { get; set; }

        [Display(Name = "Daily protein goal")]
        public double DailyProteinGoal { get; set; }

        [Display(Name = "Daily carbohydrates goal")]
        public double DailyCarbohydratesGoal { get; set; }

        [Display(Name = "Daily fat goal")]
        public double DailyFatGoal { get; set; }

        [Display(Name = "Daily calories goal")]
        public double DailyCaloriesGoal 
            => ((this.DailyProteinGoal + this.DailyCarbohydratesGoal) * 4) + (this.DailyFatGoal * 9);

        [Display(Name = "About Me")]
        public string AboutMe { get; set; }
    }
}
