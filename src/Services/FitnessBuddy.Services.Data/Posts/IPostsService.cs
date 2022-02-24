﻿namespace FitnessBuddy.Services.Data.Posts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Posts;

    public interface IPostsService
    {
        public IEnumerable<TModel> GetPostsByCategory<TModel>(int categoryId);

        public TModel GetById<TModel>(int id);

        public bool IsExist(int id);

        public bool IsUserAuthor(int postId, string userId);

        public Task CreateAsync(PostInputModel model);

        public Task DeleteAsync(int id);

        public Task EditAsync(PostInputModel model);

        public Task IncreaseViewsAsync(int id);
    }
}