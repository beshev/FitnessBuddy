namespace FitnessBuddy.Web.ViewModels.Chats
{
    using System;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class MessageViewModel : IMapFrom<Message>
    {
        public string AuthorUsername { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }
    }
}
