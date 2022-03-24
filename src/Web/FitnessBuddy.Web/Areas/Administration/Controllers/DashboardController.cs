namespace FitnessBuddy.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using FitnessBuddy.Services.Data.Articles;
    using FitnessBuddy.Services.Data.Exercises;
    using FitnessBuddy.Services.Data.Foods;
    using FitnessBuddy.Services.Data.Posts;
    using FitnessBuddy.Services.Data.Replies;
    using FitnessBuddy.Services.Data.Users;
    using FitnessBuddy.Web.ViewModels.Administration.Dashboard;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly IUsersService usersService;
        private readonly IExercisesService exercisesService;
        private readonly IArticlesService articlesService;
        private readonly IPostsService postsService;
        private readonly IFoodsService foodsService;
        private readonly IRepliesService repliesService;

        public DashboardController(
            IUsersService usersService,
            IExercisesService exercisesService,
            IArticlesService articlesService,
            IPostsService postsService,
            IFoodsService foodsService,
            IRepliesService repliesService)
        {
            this.usersService = usersService;
            this.exercisesService = exercisesService;
            this.articlesService = articlesService;
            this.postsService = postsService;
            this.foodsService = foodsService;
            this.repliesService = repliesService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel
            {
                UsersCount = await this.usersService.GetCountAsync(),
                ExercisesCount = await this.exercisesService.GetCountAsync(),
                ArticlesCount = await this.articlesService.GetCountAsync(),
                PostsCount = this.postsService.GetCount(),
                FoodsCount = this.foodsService.GetCount(),
                RepliesCount = this.repliesService.GetCount(),
            };

            return this.View(viewModel);
        }
    }
}
