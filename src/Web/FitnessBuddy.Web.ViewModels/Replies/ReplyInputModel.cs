namespace FitnessBuddy.Web.ViewModels.Replies
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ReplyInputModel : IMapTo<Reply>
    {
        [Required]
        [StringLength(DataConstants.ReplyDescriptionMaxLength, MinimumLength = DataConstants.ReplyDescriptionMinLength)]
        public string Description { get; set; }

        public int? ParentId { get; set; }

        public string AuthorId { get; set; }

        public int PostId { get; set; }
    }
}
