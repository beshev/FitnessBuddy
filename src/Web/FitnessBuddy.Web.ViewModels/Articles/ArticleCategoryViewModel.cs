namespace FitnessBuddy.Web.ViewModels.Articles
{
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ArticleCategoryViewModel : IMapFrom<ArticleCategory>
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }
    }
}
