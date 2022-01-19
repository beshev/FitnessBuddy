namespace FitnessBuddy.Web.ViewModels.Users
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;

    public class UserProfileInputModel
    {
        [Required]
        [MaxLength(DataConstants.UserUsernameMaxLength)]
        [MinLength(DataConstants.UserUsernameMinLength)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Range(DataConstants.UserWeightMinValue, DataConstants.UserWeightMaxValue)]
        [Display(Name = "Current weights (in kg)")]
        public double WeightInKg { get; set; }

        [Range(DataConstants.UserWeightMinValue, DataConstants.UserWeightMaxValue)]
        [Display(Name = "Goal weights (in kg)")]
        public double GoalWeightInKg { get; set; }

        [Display(Name = "Profile picture")]
        public string ProfilePicture { get; set; }

        [Range(DataConstants.UserHeightMinValue, DataConstants.UserHeightMaxValue)]
        [Display(Name = "Height (in cm)")]
        public double HeightInCm { get; set; }

        [Range(DataConstants.UserDailyNutritionsMinValue, DataConstants.UserDailyNutritionsMaxValue)]
        [Display(Name = "Daily protein goal")]
        public double DailyProteinGoal { get; set; }

        [Range(DataConstants.UserDailyNutritionsMinValue, DataConstants.UserDailyNutritionsMaxValue)]
        [Display(Name = "Daily carbohydrates goal")]
        public double DailyCarbohydratesGoal { get; set; }

        [Range(DataConstants.UserDailyNutritionsMinValue, DataConstants.UserDailyNutritionsMaxValue)]
        [Display(Name = "Daily fat goal")]
        public double DailyFatGoal { get; set; }

        [Display(Name = "Daily calories goal")]
        public double DailyCaloriesGoal => ((this.DailyProteinGoal + this.DailyCarbohydratesGoal) * 4) + (this.DailyFatGoal * 9);

        [Display(Name = "About Me")]
        public string AboutMe { get; set; }
    }
}
