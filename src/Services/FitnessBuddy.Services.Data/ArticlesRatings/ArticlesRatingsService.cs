namespace FitnessBuddy.Services.Data.ArticlesRatings
{
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class ArticlesRatingsService : IArticlesRatingsService
    {
        private readonly IRepository<ArticleRating> articlesRatingsRepository;

        public ArticlesRatingsService(IRepository<ArticleRating> articlesRatingsRepository)
        {
            this.articlesRatingsRepository = articlesRatingsRepository;
        }

        public async Task<double> CalcAvgRateAsync(int articleId)
        {
            var ratings = await this.articlesRatingsRepository
                .AllAsNoTracking()
                .Where(x => x.ArticleId == articleId)
                .Select(x => x.Rating)
                .ToListAsync();

            double avgRating = 0;

            if (ratings.Any())
            {
                avgRating = ratings.Average(x => x);
            }

            return avgRating;
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
