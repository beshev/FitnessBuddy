namespace FitnessBuddy.Web.ViewModels.Emails
{
    using System.ComponentModel.DataAnnotations;

    public class EmailInputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail address")]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
