﻿namespace FitnessBuddy.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Articles;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Articles;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ArticlesController : Controller
    {
        private const int ArticlesPerPage = 4;

        private readonly IArticlesService articlesService;

        public ArticlesController(IArticlesService articlesService)
        {
            this.articlesService = articlesService;
        }

        [AllowAnonymous]
        public IActionResult All(int id, string search = "")
        {
            int count = this.articlesService.GetCount(search);
            int pagesCount = (int)Math.Ceiling((double)count / ArticlesPerPage);
            var skip = (id - 1) * ArticlesPerPage;

            var articles = this.articlesService.GetAll<ArticleViewModel>(search, skip, ArticlesPerPage);

            var viewModel = new AllArticlesViewModel
            {
                PageNumber = id,
                PagesCount = pagesCount,
                Articles = articles,
            };

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticleInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            model.AddedByUserId = this.User.GetUserId();

            await this.articlesService.CreateAsync(model);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Details(int id)
        {
            var viewModel = this.articlesService.GetById<ArticleDetailsModel>(id);

            return this.View(viewModel);
        }
    }
}
