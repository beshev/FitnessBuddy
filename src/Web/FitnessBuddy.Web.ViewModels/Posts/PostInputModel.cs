namespace FitnessBuddy.Web.ViewModels.Posts
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class PostInputModel : IMapTo<Post>, IMapFrom<Post>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(DataConstants.PostTitleMaxLength, MinimumLength = DataConstants.PostTitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(DataConstants.PostDescriptionMaxLength, MinimumLength = DataConstants.PostDescriptionMinLength)]
        public string Description { get; set; }

        public string AuthorId { get; set; }

        public int CategoryId { get; set; }
    }
}
