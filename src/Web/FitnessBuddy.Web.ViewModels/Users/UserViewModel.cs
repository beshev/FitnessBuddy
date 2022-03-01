namespace FitnessBuddy.Web.ViewModels.Users
{
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using AutoMapper;
    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class UserViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        [Display(Name = "Username")]
        public string UserName { get; set; }

        public string Email { get; set; }

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
            => ((this.DailyProteinGoal + this.DailyCarbohydratesGoal) * GlobalConstants.CaloriesForOneGramProteinAndCarbohydrates) + (this.DailyFatGoal * GlobalConstants.CaloriesForOneGramFats);

        [Display(Name = "About Me")]
        public string AboutMe { get; set; }

        public string Gender { get; set; }

        public string UserRoleId { get; set; }

        public string UserRole { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserViewModel>()
                   .ForMember(
                   dest => dest.ProfilePicture,
                   opt => opt.MapFrom(x => $"/images/profileimages/{Path.GetFileName(x.ProfilePicture)}"))
                   .ForMember(
                   dest => dest.UserRoleId, opt => opt.MapFrom(x => x.Roles.FirstOrDefault().RoleId));
        }
    }
}
