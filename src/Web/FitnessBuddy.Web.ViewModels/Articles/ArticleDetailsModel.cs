namespace FitnessBuddy.Web.ViewModels.Articles
{
    using System;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ArticleDetailsModel : IMapFrom<Article>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string CategoryName { get; set; }

        public string CreatorUsername { get; set; }

        public string CreatorId { get; set; }

        public bool IsCreator { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
