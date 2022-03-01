namespace FitnessBuddy.Web.ViewModels.Users
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Meals;

    public class ProfileViewModel : IHaveCustomMappings
    {
        public double CurrentProtein { get; set; }

        public double CurrentCarbohydrates { get; set; }

        public double CurrentFats { get; set; }

        public double CurrentCalories { get; set; }

        public bool IsMyProfile { get; set; }

        public bool IsFollowingByUser { get; set; }

        public UserViewModel UserInfo { get; set; }

        public IEnumerable<ShortUserViewModel> Followers { get; set; }

        public IEnumerable<ShortUserViewModel> Following { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<IEnumerable<MealViewModel>, ProfileViewModel>()
                .ForMember(
                    dest => dest.CurrentProtein,
                    opt => opt.MapFrom(meal => meal.Sum(x => x.CurrentProtein)))
                .ForMember(
                    dest => dest.CurrentCarbohydrates,
                    opt => opt.MapFrom(meal => meal.Sum(x => x.CurrentCarbohydrates)))
                .ForMember(
                    dest => dest.CurrentFats,
                    opt => opt.MapFrom(meal => meal.Sum(x => x.CurrentFats)))
                .ForMember(
                    dest => dest.CurrentCalories,
                    opt => opt.MapFrom(meal => meal.Sum(x => x.TotalCalories)));
        }
    }
}
