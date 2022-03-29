namespace FitnessBuddy.Web.ViewModels.Chats
{
    using System.IO;

    using AutoMapper;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ConversationViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string ProfilePicture { get; set; }

        public string FollowersCount { get; set; }

        public string LastActivity { get; set; }
    }
}
