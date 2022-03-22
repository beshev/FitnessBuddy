namespace FitnessBuddy.Web.ViewModels.Chats
{
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ConversationViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string LastMessage { get; set; }

        public string LastActivity { get; set; }
    }
}
