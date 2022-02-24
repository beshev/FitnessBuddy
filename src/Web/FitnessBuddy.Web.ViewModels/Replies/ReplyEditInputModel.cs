namespace FitnessBuddy.Web.ViewModels.Replies
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ReplyEditInputModel : IMapFrom<Reply>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(DataConstants.ReplyDescriptionMaxLength, MinimumLength = DataConstants.ReplyDescriptionMinLength)]
        public string Description { get; set; }
    }
}
