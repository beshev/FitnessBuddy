namespace FitnessBuddy.Services.Data.Foods
{
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;

    public class FoodNamesService : IFoodNamesService
    {
        private readonly IDeletableEntityRepository<FoodName> foodNameRepository;

        public FoodNamesService(IDeletableEntityRepository<FoodName> foodNameRepository)
        {
            this.foodNameRepository = foodNameRepository;
        }

        public async Task<FoodName> GetByNameAsync(string name)
        {
            var foodName = this.foodNameRepository
                  .AllAsNoTracking()
                  .FirstOrDefault(x => x.Name == name);

            if (foodName == null)
            {
                foodName = new FoodName
                {
                    Name = name,
                };

                await this.foodNameRepository.AddAsync(foodName);
                await this.foodNameRepository.SaveChangesAsync();
            }

            return foodName;
        }
    }
}
