namespace FitnessBuddy.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Articles;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Articles;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : AdministrationController
    {
        private readonly IArticlesService articlesService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ArticlesController(
            IArticlesService articlesService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.articlesService = articlesService;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(int id = 1)
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            var articlesPerPage = 15;
            int count = await this.articlesService.GetCountAsync();
            int pagesCount = (int)Math.Ceiling((double)count / articlesPerPage);

            if (pagesCount != 0 && id > pagesCount)
            {
                return this.NotFound();
            }

            var skip = (id - 1) * articlesPerPage;

            var articles = await this.articlesService.GetAllAsync<ArticleViewModel>(skip, articlesPerPage);

            var viewModel = new AllArticlesViewModel
            {
                PageNumber = id,
                PagesCount = pagesCount,
                Articles = articles,
                ForAction = nameof(this.Index),
                ForController = this.GetType().Name.Replace(nameof(Controller), string.Empty),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var article = await this.articlesService.GetByIdAsync<ArticleDetailsModel>(id.Value);

            if (article == null)
            {
                return this.NotFound();
            }

            return this.View(article);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            model.CreatorId = this.User.GetUserId();

            var path = $"{this.webHostEnvironment.WebRootPath}/images";

            await this.articlesService.CreateAsync(model, path);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var article = await this.articlesService.GetByIdAsync<ArticleInputModel>(id.Value);

            if (article == null)
            {
                return this.NotFound();
            }

            return this.View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArticleInputModel model)
        {
            if (await this.articlesService.IsExistAsync(model.Id) == false)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid == false)
            {
                return this.View(model);
            }

            var path = $"{this.webHostEnvironment.WebRootPath}/images";

            await this.articlesService.EditAsync(model, path);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var viewModel = await this.articlesService.GetByIdAsync<ArticleDetailsModel>(id.Value);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this.articlesService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
