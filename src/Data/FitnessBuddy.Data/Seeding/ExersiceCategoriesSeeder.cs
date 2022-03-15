namespace FitnessBuddy.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;

    public class ExersiceCategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ExerciseCategories.Any())
            {
                return;
            }

            dbContext.ExerciseCategories.Add(new ExerciseCategory
            {
                Name = "Shoulders",
            });

            dbContext.ExerciseCategories.Add(new ExerciseCategory
            {
                Name = "Triceps",
            });

            dbContext.ExerciseCategories.Add(new ExerciseCategory
            {
                Name = "Biceps",
            });

            dbContext.ExerciseCategories.Add(new ExerciseCategory
            {
                Name = "Back",
            });

            dbContext.ExerciseCategories.Add(new ExerciseCategory
            {
                Name = "Chest",
            });

            dbContext.ExerciseCategories.Add(new ExerciseCategory
            {
                Name = "Forearm",
            });

            dbContext.ExerciseCategories.Add(new ExerciseCategory
            {
                Name = "Traps",
            });

            dbContext.ExerciseCategories.Add(new ExerciseCategory
            {
                Name = "Abs",
            });

            dbContext.ExerciseCategories.Add(new ExerciseCategory
            {
                Name = "Glutes",
            });

            dbContext.ExerciseCategories.Add(new ExerciseCategory
            {
                Name = "Quadriceps",
            });

            dbContext.ExerciseCategories.Add(new ExerciseCategory
            {
                Name = "Hamstrings",
            });

            dbContext.ExerciseCategories.Add(new ExerciseCategory
            {
                Name = "Calves",
            });

            dbContext.ExerciseCategories.Add(new ExerciseCategory
            {
                Name = "Lower back",
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
