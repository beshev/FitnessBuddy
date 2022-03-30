namespace FitnessBuddy.Web.ViewModels.Users
{
    using AutoMapper;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class UserFollowing : ShortUserViewModel, IHaveCustomMappings
    {
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserFollower, UserFollowing>()
                .ForMember(
                dest => dest.FollowingCount,
                opt => opt.MapFrom(x => x.User.Following.Count))
                .ForMember(
                dest => dest.FollowersCount,
                opt => opt.MapFrom(x => x.User.Followers.Count))
                .ForMember(
                dest => dest.Username, opt => opt.MapFrom(x => x.User.UserName))
                .ForMember(
                dest => dest.Email, opt => opt.MapFrom(x => x.User.Email))
                .ForMember(
                dest => dest.ProfilePicture, opt => opt.MapFrom(x => x.User.ProfilePicture));
        }
    }
}
