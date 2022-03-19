﻿namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Articles;
    using FitnessBuddy.Services.Data.ArticlesRatings;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.ArticlesRatings;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesRatingsController : ApiController
    {
        private readonly IArticlesRatingsService articlesRatingsService;
        private readonly IArticlesService articlesService;

        public ArticlesRatingsController(
            IArticlesRatingsService articlesRatingsService,
            IArticlesService articlesService)
        {
            this.articlesRatingsService = articlesRatingsService;
            this.articlesService = articlesService;
        }

        [HttpPost]
        public async Task<ActionResult> Post(ArticleRatingInputModel model)
        {
            if (this.articlesService.IsExist(model.ArticleId) == false)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest();
            }

            await this.articlesRatingsService.RateAsync(model.ArticleId, this.User.GetUserId(), model.Rating);

            var avgRating = this.articlesRatingsService.CalcAvgRate(model.ArticleId);

            return new JsonResult(new { AvgRating = avgRating });
        }
    }
}