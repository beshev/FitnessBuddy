namespace FitnessBuddy.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;

    public class PostCategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.PostCategories.Any())
            {
                return;
            }

            dbContext.PostCategories.Add(new PostCategory
            {
                Name = "Supplements",
            });

            dbContext.PostCategories.Add(new PostCategory
            {
                Name = "Bodyfit",
            });

            dbContext.PostCategories.Add(new PostCategory
            {
                Name = "Workout Equipment",
            });

            dbContext.PostCategories.Add(new PostCategory
            {
                Name = "Workout Programs",
            });

            dbContext.PostCategories.Add(new PostCategory
            {
                Name = "Exercises",
            });

            dbContext.PostCategories.Add(new PostCategory
            {
                Name = "Nutrition",
            });

            dbContext.PostCategories.Add(new PostCategory
            {
                Name = "Loosing Fat",
            });

            dbContext.PostCategories.Add(new PostCategory
            {
                Name = "Teen Bodybuilding",
            });

            dbContext.PostCategories.Add(new PostCategory
            {
                Name = "Female Bodybuilding",
            });

            dbContext.PostCategories.Add(new PostCategory
            {
                Name = "Powerlifting",
            });

            dbContext.PostCategories.Add(new PostCategory
            {
                Name = "Personal Trainers Section",
            });

            dbContext.PostCategories.Add(new PostCategory
            {
                Name = "Relaxation places",
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
