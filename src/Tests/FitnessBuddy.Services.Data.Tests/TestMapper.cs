namespace FitnessBuddy.Services.Data.Tests
{
    using AutoMapper;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Articles;
    using FitnessBuddy.Web.ViewModels.Meals;

    public static class TestMapper
    {
        /// <summary>
        /// Some of the tests need an auto mapper. AutoMapperConfig is a static class and for that, all tests use one instance of him when you run it all in once.
        /// </summary>
        public static void InitializeAutoMapper()
        {
            var config = new MapperConfigurationExpression();
            config.CreateMap<Article, Article>();
            config.CreateMap<ArticleInputModel, Article>();
            config.CreateMap<ArticleCategory, ArticleCategory>();
            config.CreateMap<Meal, Meal>();
            config.CreateMap<MealInputModel, Meal>();

            // Because all tests use one instance of mapper there will be no need to instance it every time.
            // For now, this is the solution I found. Maybe it has a better way!.
            if (AutoMapperConfig.MapperInstance == null)
            {
                AutoMapperConfig.MapperInstance = new Mapper(new MapperConfiguration(config));
            }
        }
    }
}
