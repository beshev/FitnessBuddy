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

            var viewModel = this.messagesService.GetConversations<UserChatViewModel>(userId);

            return this.View(viewModel);
        }
    }
}
