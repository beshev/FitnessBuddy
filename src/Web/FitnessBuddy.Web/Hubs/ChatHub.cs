namespace FitnessBuddy.Web.Hubs
{
    using System;
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Messages;
    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Services.Format;
    using FitnessBuddy.Services.Hubs;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Chats;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessagesService messagesService;
        private readonly IGroupNameProvider groupNameProvider;
        private readonly IDateTimeFormatProvider dateTimeFormatProvider;

        public ChatHub(
            IMessagesService messagesService,
            IGroupNameProvider groupNameProvider,
            IDateTimeFormatProvider dateTimeFormatProvider)
        {
            this.messagesService = messagesService;
            this.groupNameProvider = groupNameProvider;
            this.dateTimeFormatProvider = dateTimeFormatProvider;
        }

        public async Task JoinGroup(string receiverId)
        {
            var authorId = this.Context.User.GetUserId();

            if (
                string.IsNullOrWhiteSpace(authorId)
                || string.IsNullOrWhiteSpace(receiverId)
                || authorId == receiverId)
            {
                return;
            }

            var groupName = this.groupNameProvider.GetGroupName(authorId, receiverId);

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
        }

        public async Task SendMessage(string message, string receiverId)
        {
            if (
                string.IsNullOrWhiteSpace(message)
                || string.IsNullOrWhiteSpace(receiverId))
            {
                return;
            }

            var authorId = this.Context.User.GetUserId();

            var groupName = this.groupNameProvider.GetGroupName(authorId, receiverId);

            await this.messagesService.SendMessageAsync(authorId, receiverId, message);

            var result = new MessageViewModel
            {
                AuthorUsername = this.Context.User.Identity.Name,
                Content = message,
                CreatedOn = this.dateTimeFormatProvider.GetDateFormat(DateTime.Now),
            };

            await this.Clients.Group(groupName).SendAsync("NewMessage", result);
        }
    }
}
