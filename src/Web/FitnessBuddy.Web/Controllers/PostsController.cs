namespace FitnessBuddy.Web.Controllers
{
    using FitnessBuddy.Services.Data.Posts;
    using FitnessBuddy.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostCategoriesService postCategoriesService;

        public PostsController(IPostCategoriesService postCategoriesService)
        {
            this.postCategoriesService = postCategoriesService;
        }

        public IActionResult Categories()
        {
            var viewModel = this.postCategoriesService.GetAll<PostCategoryViewModel>();

            return this.View(viewModel);
        }
    }
}
