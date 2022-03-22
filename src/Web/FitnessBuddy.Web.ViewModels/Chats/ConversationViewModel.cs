namespace FitnessBuddy.Web.ViewModels.Chats
{
    using System.IO;

    using AutoMapper;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ConversationViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string ProfilePicture { get; set; }

        public string FollowersCount { get; set; }

        public string LastActivity { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, ConversationViewModel>()
                .ForMember(
                dest => dest.ProfilePicture,
                opt => opt.MapFrom(x => string.IsNullOrWhiteSpace(x.ProfilePicture)
                ? "/images/profileimages/default-avatar.jpg"
                : $"/images/profileimages/{Path.GetFileName(x.ProfilePicture)}"));
        }
    }
}
