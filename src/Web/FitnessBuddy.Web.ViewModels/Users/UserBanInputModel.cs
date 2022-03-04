namespace FitnessBuddy.Web.ViewModels.Users
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;

    public class UserBanInputModel
    {
        public string Username { get; set; }

        [Required]
        [Display(Name = "Tell the reason for the ban")]
        [StringLength(
            DataConstants.UserBanReasonMaxLength,
            MinimumLength = DataConstants.UserBanReasonMinLength,
            ErrorMessage = "The reason must be between 3 and 500 characters long")]
        public string BanReason { get; set; }
    }
}
