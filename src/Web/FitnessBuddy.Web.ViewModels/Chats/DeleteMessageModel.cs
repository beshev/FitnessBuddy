namespace FitnessBuddy.Web.ViewModels.Chats
{
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class DeleteMessageModel : IMapFrom<Message>
    {
        public string AuthorId { get; set; }

        public string ReceiverId { get; set; }

        public string ReceiverUsername { get; set; }
    }
}
