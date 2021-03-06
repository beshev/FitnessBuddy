namespace FitnessBuddy.Web.ViewComponents
{
    using FitnessBuddy.Services.Data.Messages;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Chats;
    using Microsoft.AspNetCore.Mvc;

    public class ChatConversationViewComponent : ViewComponent
    {
        private readonly IMessagesService messagesService;

        public ChatConversationViewComponent(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
        }

        public IViewComponentResult Invoke()
        {
            var userId = this.UserClaimsPrincipal.GetUserId();

            var conversations = this.messagesService.GetConversationsAsync<ConversationViewModel>(userId).GetAwaiter().GetResult();

            foreach (var currentUser in conversations)
            {
                currentUser.LastActivity = this.messagesService.GetLastActivityAsync(userId, currentUser.Id).GetAwaiter().GetResult();
            }

            return this.View(conversations);
        }
    }
}
