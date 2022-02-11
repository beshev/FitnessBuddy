namespace FitnessBuddy.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;

    public class ArticleCategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ArticleCategories.Any())
            {
                return;
            }

            var categories = new List<ArticleCategory>
            {
                new ArticleCategory
                {
                    Name = "Nutrition",
                    ImageUrl = "/images/articlecategories/nutrition.jpg",
                },
                new ArticleCategory
                {
                    Name = "Motivation",
                    ImageUrl = "/images/articlecategories/motivation.jpg",
                },
                new ArticleCategory
                {
                    Name = "Training",
                    ImageUrl = "/images/articlecategories/training.jpg",
                },
                new ArticleCategory
                {
                    Name = "Fat Loss",
                    ImageUrl = "/images/articlecategories/weightloss.jpg",
                },
                new ArticleCategory
                {
                    Name = "Injury Prevention",
                    ImageUrl = "/images/articlecategories/injuryprevention.jpg",
                },
                new ArticleCategory
                {
                    Name = "Recipes",
                    ImageUrl = "/images/articlecategories/recipes.jpg",
                },
            };

            await dbContext.ArticleCategories.AddRangeAsync(categories);
            await dbContext.SaveChangesAsync();
        }
    }
}
