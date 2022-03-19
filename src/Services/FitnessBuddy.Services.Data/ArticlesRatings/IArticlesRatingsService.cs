﻿namespace FitnessBuddy.Services.Data.ArticlesRatings
{
    using System.Threading.Tasks;

    public interface IArticlesRatingsService
    {
        public Task RateAsync(int articleId, string userId, double rating);

        public double CalcAvgRate(int articleId);
    }
}
