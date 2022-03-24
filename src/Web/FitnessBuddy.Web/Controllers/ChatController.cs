namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Messages;
    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Chats;
    using Microsoft.AspNetCore.Mvc;

    public class ChatController : BaseController
    {
        private readonly IMessagesService messagesService;
        private readonly IUsersService usersService;

        public ChatController(
            IMessagesService messagesService,
            IUsersService usersService)
        {
            this.messagesService = messagesService;
            this.usersService = usersService;
        }

        public async Task<IActionResult> WithUser(string username = "")
        {
            var authorId = this.User.GetUserId();
            var receiverId = await this.usersService.GetIdByUsernameAsync(username);

            var viewModel = new UserChatViewModel
            {
                ReceiverUsername = username,
                ReceiverId = receiverId,
                Messages = await this.messagesService.GetMessagesAsync<MessageViewModel>(authorId, receiverId),
            };

            return this.View(viewModel);
        }
    }
}
