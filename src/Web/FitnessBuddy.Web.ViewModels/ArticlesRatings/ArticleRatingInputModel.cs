namespace FitnessBuddy.Web.ViewModels.ArticlesRatings
{
    using System.ComponentModel.DataAnnotations;

    public class ArticleRatingInputModel
    {
        public int ArticleId { get; set; }

        [Range(1, 5)]
        public double Rating { get; set; }
    }
}
