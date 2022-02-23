namespace FitnessBuddy.Web.ViewComponents
{
    using FitnessBuddy.Services.Data.Posts;
    using FitnessBuddy.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class PostCategoriesSelectViewComponent : ViewComponent
    {
        private readonly IPostCategoriesService postCategoriesService;

        public PostCategoriesSelectViewComponent(IPostCategoriesService postCategoriesService)
        {
            this.postCategoriesService = postCategoriesService;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = this.postCategoriesService.GetAll<SelectViewModel>();

            return this.View(viewModel);
        }
    }
}
