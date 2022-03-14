namespace FitnessBuddy.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Services.Data.Articles;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Articles;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : BaseController
    {
        private const int ArticlesPerPage = 4;

        private readonly IArticlesService articlesService;
        private readonly IArticleCategoriesService articleCategoriesService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ArticlesController(
            IArticlesService articlesService,
            IArticleCategoriesService articleCategoriesService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.articlesService = articlesService;
            this.articleCategoriesService = articleCategoriesService;
            this.webHostEnvironment = webHostEnvironment;
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

            model.CreatorId = this.User.GetUserId();

            var path = $"{this.webHostEnvironment.WebRootPath}/images";

            await this.articlesService.CreateAsync(model, path);

            this.TempData[GlobalConstants.NameOfSuccess] = string.Format(GlobalConstants.SuccessMessage, GlobalConstants.NameOfArticle);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Details(int id)
        {
            if (this.articlesService.IsExist(id) == false)
            {
                return this.NotFound();
            }

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
            if (this.articleCategoriesService.IsExist(categoryName) == false)
            {
                return this.NotFound();
            }

            var viewModel = this.articleCategoriesService.GetCategoryArticles<ArticleViewModel>(categoryName);

            this.ViewData["CategoryName"] = categoryName;

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (this.articlesService.IsExist(id) == false)
            {
                return this.NotFound();
            }

            if (this.articlesService.IsUserCreator(this.User.GetUserId(), id) == false)
            {
                return this.Unauthorized();
            }

            await this.articlesService.DeleteAsync(id);

            this.TempData[GlobalConstants.NameOfDelete] = string.Format(GlobalConstants.DeleteMessage, GlobalConstants.NameOfArticle);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Edit(int id)
        {
            if (this.articlesService.IsExist(id) == false)
            {
                return this.NotFound();
            }

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
            if (this.articlesService.IsExist(model.Id) == false)
            {
                return this.NotFound();
            }

            if (model.CreatorId != this.User.GetUserId())
            {
                return this.Unauthorized();
            }

            if (this.ModelState.IsValid == false)
            {
                return this.View();
            }

            var path = $"{this.webHostEnvironment.WebRootPath}/images";

            await this.articlesService.EditAsync(model, path);

            return this.RedirectToAction(nameof(this.Details), new { model.Id });
        }
    }
}
