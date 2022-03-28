namespace FitnessBuddy.Web.Controllers
{
    using System.Text;
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Services.Messaging;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Emails;
    using Microsoft.AspNetCore.Mvc;

    public class Emails : BaseController
    {
        private readonly IEmailSender emailSender;

        public Emails(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public IActionResult SendEmail()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(EmailInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            var senderUsername = this.User.GetUsername();
            var senderEmail = this.User.GetUserEmail();

            var html = new StringBuilder();
            html.AppendLine($"<h4>{senderUsername}({senderEmail}) invites you to join our app.</h4>");
            html.AppendLine("<hr />");
            html.AppendLine($"<p>{model.Content}</p>");

            await this.emailSender
                .SendEmailAsync("bigdoom@abv.bg", senderUsername, model.Email, model.Subject, html.ToString());

            this.TempData[GlobalConstants.NameOfSuccess] = GlobalConstants.SentEmailMessage;

            return this.RedirectToAction(nameof(this.SendEmail));
        }
    }
}
