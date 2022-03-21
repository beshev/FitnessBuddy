namespace FitnessBuddy.Web.Hubs
{
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Messages;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessagesService messagesService;

        public ChatHub(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
        }

        public async Task SendMessage(string message, string receiverId)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var authorId = this.Context.User.GetUserId();

            await this.messagesService.SendMessageAsync(authorId, receiverId, message);

            var result = new { Text = message, User = this.Context.User.Identity.Name };

            Task.WaitAll(
                this.Clients.User(receiverId).SendAsync("NewMessage", result),
                this.Clients.User(authorId).SendAsync("NewMessage", result));
        }
    }
}
