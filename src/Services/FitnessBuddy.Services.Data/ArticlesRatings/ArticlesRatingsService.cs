namespace FitnessBuddy.Services.Data.ArticlesRatings
{
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;

    public class ArticlesRatingsService : IArticlesRatingsService
    {
        private readonly IRepository<ArticleRating> articlesRatingsRepository;

        public ArticlesRatingsService(IRepository<ArticleRating> articlesRatingsRepository)
        {
            this.articlesRatingsRepository = articlesRatingsRepository;
        }

        public double CalcAvgRate(int articleId)
        {
            var query = this.articlesRatingsRepository
                .AllAsNoTracking()
                .Where(x => x.ArticleId == articleId);

            return query.Count() == 0 ? 0 : query.Average(x => x.Rating);
        }

        public async Task RateAsync(int articleId, string userId, double rating)
        {
            var articleRating = this.articlesRatingsRepository
                .All()
                .FirstOrDefault(x => x.ArticleId == articleId && x.UserId == userId);

            if (articleRating == null)
            {
                articleRating = new ArticleRating
                {
                    ArticleId = articleId,
                    UserId = userId,
                };

                await this.articlesRatingsRepository.AddAsync(articleRating);
            }

            articleRating.Rating = rating;

            await this.articlesRatingsRepository.SaveChangesAsync();
        }
    }
}
