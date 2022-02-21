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
        private readonly IArticleCategoriesService articleCategoriesService;

        public ArticlesController(
            IArticlesService articlesService,
            IArticleCategoriesService articleCategoriesService)
        {
            this.articlesService = articlesService;
            this.articleCategoriesService = articleCategoriesService;
        }

        [AllowAnonymous]
        public IActionResult All(int id = 1)
        {
            int count = this.articlesService.GetCount();
            int pagesCount = (int)Math.Ceiling((double)count / ArticlesPerPage);
            var skip = (id - 1) * ArticlesPerPage;

            var articles = this.articlesService.GetAll<ArticleViewModel>(skip, ArticlesPerPage);

            var viewModel = new AllArticlesViewModel
            {
                PageNumber = id,
                PagesCount = pagesCount,
                Articles = articles,
                ForAction = nameof(this.All),
                ForController = this.GetType().Name.Replace(nameof(Controller), string.Empty),
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

            // The binder set Id to 1 for no reason!
            model.Id = 0;
            model.CreatorId = this.User.GetUserId();

            await this.articlesService.CreateAsync(model);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Details(int id)
        {
            var viewModel = this.articlesService.GetById<ArticleDetailsModel>(id);
            viewModel.IsCreator = viewModel.CreatorId == this.User.GetUserId();

            return this.View(viewModel);
        }

        public IActionResult Categories()
        {
            var viewModel = this.articleCategoriesService.GetAll<ArticleCategoryViewModel>();

            return this.View(viewModel);
        }

        public IActionResult Category(string categoryName)
        {
            var viewModel = this.articleCategoriesService.GetCategoryArticles<ArticleViewModel>(categoryName);

            this.ViewData["CategoryName"] = categoryName;

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (this.articlesService.IsUserCreator(this.User.GetUserId(), id) == false)
            {
                return this.Unauthorized();
            }

            await this.articlesService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Edit(int id)
        {
            var viewModel = this.articlesService.GetById<ArticleInputModel>(id);

            if (viewModel.CreatorId != this.User.GetUserId())
            {
                return this.Unauthorized();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ArticleInputModel model)
        {
            if (model.CreatorId != this.User.GetUserId())
            {
                return this.Unauthorized();
            }

            if (this.ModelState.IsValid == false)
            {
                return this.View();
            }

            await this.articlesService.EditAsync(model);

            return this.RedirectToAction(nameof(this.Details), new { model.Id });
        }
    }
}
