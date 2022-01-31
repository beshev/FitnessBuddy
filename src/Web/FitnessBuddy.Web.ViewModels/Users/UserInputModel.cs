namespace FitnessBuddy.Web.ViewModels.Users
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Data.Models.Enums;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.Infrastructure.Attributes;
    using Microsoft.AspNetCore.Http;

    public class UserInputModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        [Required]
        [MaxLength(DataConstants.UserUsernameMaxLength)]
        [MinLength(DataConstants.UserUsernameMinLength)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [EnumDataType(typeof(GenderType))]
        public GenderType Gender { get; set; }

        [Range(DataConstants.UserWeightMinValue, DataConstants.UserWeightMaxValue)]
        [Display(Name = "Current weights (in kg)")]
        public double WeightInKg { get; set; }

        [Range(DataConstants.UserWeightMinValue, DataConstants.UserWeightMaxValue)]
        [Display(Name = "Goal weights (in kg)")]
        public double GoalWeightInKg { get; set; }

        [Display(Name = "Profile picture")]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile ProfilePicture { get; set; }

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

        [Display(Name = "About Me")]
        public string AboutMe { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserInputModel>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
        }
    }
}
