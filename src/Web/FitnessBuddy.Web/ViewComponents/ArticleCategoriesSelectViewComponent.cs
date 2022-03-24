namespace FitnessBuddy.Web.ViewComponents
{
    using FitnessBuddy.Services.Data.Articles;
    using FitnessBuddy.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class ArticleCategoriesSelectViewComponent : ViewComponent
    {
        private readonly IArticleCategoriesService articleCategoriesService;

        public ArticleCategoriesSelectViewComponent(IArticleCategoriesService articleCategoriesService)
        {
            this.articleCategoriesService = articleCategoriesService;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = this.articleCategoriesService.GetAllAsync<SelectViewModel>().GetAwaiter().GetResult();

            return this.View(viewModel);
        }
    }
}
