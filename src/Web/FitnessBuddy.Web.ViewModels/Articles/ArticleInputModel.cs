namespace FitnessBuddy.Web.ViewModels.Articles
{
    using System.ComponentModel.DataAnnotations;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ArticleInputModel : IMapTo<Article>, IMapFrom<Article>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(DataConstants.ArticleTitleMaxLength, MinimumLength = DataConstants.ArticleTitleMinLength, ErrorMessage = "Title lenght must be betweem {2} and {1}")]
        public string Title { get; set; }

        [Required]
        [StringLength(DataConstants.ArticleContentMaxLength, MinimumLength = DataConstants.ArticleContentMinLength, ErrorMessage = "Content lenght must be betweem {2} and {1}")]
        public string Content { get; set; }

        [Required]
        [Url(ErrorMessage = "The URL is not valid!")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "The field Category is required!")]
        public int? CategoryId { get; set; }

        public string CreatorId { get; set; }
    }
}
