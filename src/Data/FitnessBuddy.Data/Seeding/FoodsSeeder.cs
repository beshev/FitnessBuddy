namespace FitnessBuddy.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;

    public class FoodsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Foods.Any())
            {
                return;
            }

            var adminId = dbContext.Users
                .Where(x => x.Email == "admin@admin.bg")
                .Select(x => x.Id)
                .FirstOrDefault();

            var foods = new List<Food>
            {
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Egg",
                    },
                    ProteinIn100Grams = 13,
                    CarbohydratesIn100Grams = 1.1,
                    FatIn100Grams = 11,
                    Sodium = 124,
                    ImageUrl = "/images/foods/egg.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Chocolate",
                    },
                    ProteinIn100Grams = 4.9,
                    CarbohydratesIn100Grams = 61,
                    FatIn100Grams = 31,
                    Sodium = 24,
                    ImageUrl = "/images/foods/chocolate.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Banana",
                    },
                    ProteinIn100Grams = 1.1,
                    CarbohydratesIn100Grams = 23,
                    FatIn100Grams = 0.3,
                    Sodium = 1,
                    ImageUrl = "/images/foods/banana.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Nut",
                    },
                    ProteinIn100Grams = 20,
                    CarbohydratesIn100Grams = 21,
                    FatIn100Grams = 54,
                    Sodium = 273,
                    ImageUrl = "/images/foods/nut.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Beef",
                    },
                    ProteinIn100Grams = 26,
                    CarbohydratesIn100Grams = 0,
                    FatIn100Grams = 15,
                    Sodium = 72,
                    ImageUrl = "/images/foods/beef.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Peanut Butter",
                    },
                    ProteinIn100Grams = 25,
                    CarbohydratesIn100Grams = 20,
                    FatIn100Grams = 50,
                    Sodium = 17,
                    ImageUrl = "/images/foods/peanutbutter.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Chicken steak",
                    },
                    ProteinIn100Grams = 31,
                    CarbohydratesIn100Grams = 0,
                    FatIn100Grams = 3.6,
                    Sodium = 74,
                    ImageUrl = "/images/foods/chickensteak.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Salmon",
                    },
                    ProteinIn100Grams = 20,
                    CarbohydratesIn100Grams = 0,
                    FatIn100Grams = 13,
                    Sodium = 59,
                    ImageUrl = "/images/foods/salmon.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "White Rice",
                    },
                    ProteinIn100Grams = 2.7,
                    CarbohydratesIn100Grams = 28,
                    FatIn100Grams = 0.3,
                    Sodium = 1,
                    ImageUrl = "/images/foods/whiterice.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Brown Rice",
                    },
                    ProteinIn100Grams = 2.6,
                    CarbohydratesIn100Grams = 23,
                    FatIn100Grams = 0.9,
                    Sodium = 5,
                    ImageUrl = "/images/foods/brownrice.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Potato",
                    },
                    ProteinIn100Grams = 2,
                    CarbohydratesIn100Grams = 17,
                    FatIn100Grams = 0.1,
                    Sodium = 6,
                    ImageUrl = "/images/foods/potato.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Cheese",
                    },
                    ProteinIn100Grams = 25,
                    CarbohydratesIn100Grams = 1.3,
                    FatIn100Grams = 33,
                    Sodium = 621,
                    ImageUrl = "/images/foods/cheese.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Tomato",
                    },
                    ProteinIn100Grams = 0.9,
                    CarbohydratesIn100Grams = 3.9,
                    FatIn100Grams = 0.2,
                    Sodium = 20,
                    ImageUrl = "/images/foods/tomato.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Lukanka",
                    },
                    ProteinIn100Grams = 29,
                    CarbohydratesIn100Grams = 2,
                    FatIn100Grams = 25,
                    Sodium = 30,
                    ImageUrl = "/images/foods/lukanka.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Cottage Cheese",
                    },
                    ProteinIn100Grams = 11,
                    CarbohydratesIn100Grams = 3.4,
                    FatIn100Grams = 4.3,
                    Sodium = 364,
                    ImageUrl = "/images/foods/cottagecheese.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Pea",
                    },
                    ProteinIn100Grams = 5,
                    CarbohydratesIn100Grams = 14,
                    FatIn100Grams = 0.4,
                    Sodium = 5,
                    ImageUrl = "/images/foods/pea.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Cucumber",
                    },
                    ProteinIn100Grams = 1.3,
                    CarbohydratesIn100Grams = 7.3,
                    FatIn100Grams = 0.2,
                    Sodium = 4,
                    ImageUrl = "/images/foods/cucumber.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Broccoli",
                    },
                    ProteinIn100Grams = 2.5,
                    CarbohydratesIn100Grams = 6,
                    FatIn100Grams = 0.4,
                    Sodium = 30,
                    ImageUrl = "/images/foods/broccoli.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Avocado",
                    },
                    ProteinIn100Grams = 2,
                    CarbohydratesIn100Grams = 9,
                    FatIn100Grams = 15,
                    Sodium = 7,
                    ImageUrl = "/images/foods/avocado.jpg",
                    AddedByUserId = adminId,
                },
                new Food
                {
                    FoodName = new FoodName
                    {
                        Name = "Quattro Formaggi Pizza",
                    },
                    ProteinIn100Grams = 31.7,
                    CarbohydratesIn100Grams = 100.3,
                    FatIn100Grams = 21.1,
                    Sodium = 263,
                    ImageUrl = "/images/foods/quattroformaggipizza.jpg",
                    AddedByUserId = adminId,
                },
            };

            dbContext.Foods.AddRange(foods);

            await dbContext.SaveChangesAsync();
        }
    }
}
