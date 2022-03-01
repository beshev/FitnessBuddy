namespace FitnessBuddy.Web.ViewModels.Users
{
    using System.IO;

    using AutoMapper;
    using FitnessBuddy.Data.Models;

    public class UserFollowers : ShortUserViewModel
    {
        public override void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserFollower, UserFollowers>()
                .ForMember(
                dest => dest.FollowingCount,
                opt => opt.MapFrom(x => x.Follower.Following.Count))
                .ForMember(
                dest => dest.FollowersCount,
                opt => opt.MapFrom(x => x.Follower.Followers.Count))
                .ForMember(
                dest => dest.Username, opt => opt.MapFrom(x => x.Follower.UserName))
                .ForMember(
                dest => dest.Email, opt => opt.MapFrom(x => x.Follower.Email))
                .ForMember(
                dest => dest.ProfilePicture, opt => opt.MapFrom(x => $"/images/profileimages/{Path.GetFileName(x.Follower.ProfilePicture)}"));
        }
    }
}
