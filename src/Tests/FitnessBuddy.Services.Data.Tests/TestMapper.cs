namespace FitnessBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using AutoMapper;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels;
    using FitnessBuddy.Web.ViewModels.Articles;
    using FitnessBuddy.Web.ViewModels.Meals;
    using FitnessBuddy.Web.ViewModels.Users;

    public static class TestMapper
    {
        /// <summary>
        /// Some of the tests need an auto mapper. AutoMapperConfig is a static class and for that, all tests use one instance of him when you run it all in once.
        /// </summary>
        public static void InitializeAutoMapper()
        {
            // Because all tests use one instance of mapper there will be no need to instance it every time.
            // For now, this is the solution I found. Maybe it has a better way!.
            if (AutoMapperConfig.MapperInstance != null)
            {
                return;
            }

            AutoMapperConfig.MapperInstance = RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        public static Mapper RegisterMappings(params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(a => a.GetExportedTypes()).ToList();

            var config = new MapperConfigurationExpression();
            config.CreateProfile(
                "ReflectionProfile",
                configuration =>
                {
                    configuration.CreateMap<ExerciseEquipment, ExerciseEquipment>();
                    configuration.CreateMap<Exercise, Exercise>();
                    configuration.CreateMap<ApplicationUser, ApplicationUser>();
                    configuration.CreateMap<ApplicationUser, UserViewModel>();
                    configuration.CreateMap<Article, Article>();
                    configuration.CreateMap<ExerciseCategory, ExerciseCategory>();
                    configuration.CreateMap<ArticleInputModel, Article>();
                    configuration.CreateMap<ArticleCategory, ArticleCategory>();
                    configuration.CreateMap<Meal, Meal>();
                    configuration.CreateMap<UserFollower, UserFollower>();
                    configuration.CreateMap<Food, Food>();
                    configuration.CreateMap<MealFood, MealFoodViewModel>();
                    configuration.CreateMap<Meal, MealViewModel>();
                    configuration.CreateMap<IEnumerable<MealViewModel>, ProfileViewModel>();
                    configuration.CreateMap<MealInputModel, Meal>();

                    // IHaveCustomMappings
                    foreach (var map in GetCustomMappings(types))
                    {
                        map.CreateMappings(configuration);
                    }
                });
            return new Mapper(new MapperConfiguration(config));
        }

        private static IEnumerable<IHaveCustomMappings> GetCustomMappings(IEnumerable<Type> types)
        {
            var customMaps = from t in types
                             from i in t.GetTypeInfo().GetInterfaces()
                             where typeof(IHaveCustomMappings).GetTypeInfo().IsAssignableFrom(t) &&
                                   !t.GetTypeInfo().IsAbstract &&
                                   !t.GetTypeInfo().IsInterface
                             select (IHaveCustomMappings)Activator.CreateInstance(t);

            return customMaps;
        }
    }
}
