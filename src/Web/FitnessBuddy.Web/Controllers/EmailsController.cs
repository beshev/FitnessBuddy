namespace FitnessBuddy.Web.Controllers
{
    using System.Text;
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Services.Messaging;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Emails;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Mvc;

    public class EmailsController : BaseController
    {
        private readonly IEmailSender emailSender;
        private readonly IHtmlSanitizer htmlSanitizer;

        public EmailsController(
            IEmailSender emailSender,
            IHtmlSanitizer htmlSanitizer)
        {
            this.emailSender = emailSender;
            this.htmlSanitizer = htmlSanitizer;
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
            html
                .AppendLine($"<h4>{senderUsername}({senderEmail}) invites you to join our app.</h4>: <a href=\"https://fitness-buddy.azurewebsites.net/\">FitnessBuddy</a>");
            html.AppendLine("<hr />");
            html.AppendLine(this.htmlSanitizer.Sanitize(model.Content));

            await this.emailSender
                .SendEmailAsync("bigdoom@abv.bg", senderUsername, model.Email, model.Subject, html.ToString());

            this.TempData[GlobalConstants.NameOfSuccess] = GlobalConstants.SentEmailMessage;

            return this.RedirectToAction(nameof(this.SendEmail));
        }
    }
}
