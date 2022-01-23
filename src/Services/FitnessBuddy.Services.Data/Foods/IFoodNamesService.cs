namespace FitnessBuddy.Services.Data.Foods
{
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;

    public interface IFoodNamesService
    {
        /// <summary>
        /// This method gets the food name, if the name does not exist, it will create it.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<FoodName> GetByNameAsync(string foodName);
    }
}
