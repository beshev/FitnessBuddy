namespace FitnessBuddy.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;

    public class ExerciseEquipmentsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ExerciseEquipment.Any())
            {
                return;
            }

            dbContext.ExerciseEquipment.Add(new ExerciseEquipment
            {
                Name = "Body only",
            });

            dbContext.ExerciseEquipment.Add(new ExerciseEquipment
            {
                Name = "Exercise Ball",
            });

            dbContext.ExerciseEquipment.Add(new ExerciseEquipment
            {
                Name = "Medicine Ball",
            });

            dbContext.ExerciseEquipment.Add(new ExerciseEquipment
            {
                Name = "Foam Roll",
            });

            dbContext.ExerciseEquipment.Add(new ExerciseEquipment
            {
                Name = "Kettlebells",
            });

            dbContext.ExerciseEquipment.Add(new ExerciseEquipment
            {
                Name = "Bands",
            });

            dbContext.ExerciseEquipment.Add(new ExerciseEquipment
            {
                Name = "EZ Curl Bar",
            });

            dbContext.ExerciseEquipment.Add(new ExerciseEquipment
            {
                Name = "Cable",
            });

            dbContext.ExerciseEquipment.Add(new ExerciseEquipment
            {
                Name = "Machine",
            });

            dbContext.ExerciseEquipment.Add(new ExerciseEquipment
            {
                Name = "Barbell",
            });

            dbContext.ExerciseEquipment.Add(new ExerciseEquipment
            {
                Name = "Dumbbell",
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
