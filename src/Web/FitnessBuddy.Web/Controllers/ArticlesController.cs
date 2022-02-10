namespace FitnessBuddy.Web.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Articles;
    using FitnessBuddy.Web.Infrastructure.Extensions;
    using FitnessBuddy.Web.ViewModels.Articles;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ArticlesController : Controller
    {
        private readonly IArticlesService articlesService;

        public ArticlesController(IArticlesService articlesService)
        {
            this.articlesService = articlesService;
        }

        [AllowAnonymous]
        public IActionResult All()
        {
            var viewModel = new AllArticlesViewModel
            {
                Articles = this.articlesService.GetAll<ArticleViewModel>(),
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
    }
}
