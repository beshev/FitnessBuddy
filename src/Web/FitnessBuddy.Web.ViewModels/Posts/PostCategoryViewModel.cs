namespace FitnessBuddy.Web.ViewModels.Posts
{
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class PostCategoryViewModel : IMapFrom<PostCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PostsCount { get; set; }
    }
}
