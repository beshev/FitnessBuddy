namespace FitnessBuddy.Web.ViewModels.Users
{
    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class UserTargetNutrientsViewModel : IMapFrom<ApplicationUser>
    {
        public double DailyProteinGoal { get; set; }

        public double DailyCarbohydratesGoal { get; set; }

        public double DailyFatGoal { get; set; }

        public double DailyCaloriesGoal
            => ((this.DailyProteinGoal + this.DailyCarbohydratesGoal) * GlobalConstants.CaloriesForOneGramProteinAndCarbohydrates) + (this.DailyFatGoal * GlobalConstants.CaloriesForOneGramFats);
    }
}
