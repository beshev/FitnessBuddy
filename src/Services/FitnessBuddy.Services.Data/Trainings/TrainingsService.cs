namespace FitnessBuddy.Services.Data.Trainings
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class TrainingsService : ITrainingsService
    {
        private readonly IDeletableEntityRepository<Training> trainingRepository;

        public TrainingsService(IDeletableEntityRepository<Training> trainingRepository)
        {
            this.trainingRepository = trainingRepository;
        }

        public async Task AddAsync(string trainingName, string userId)
        {
            bool isTrainingExists = this.trainingRepository
                .AllAsNoTracking()
                .Any(x => x.Name == trainingName && x.ForUserId == userId);

            if (isTrainingExists == false)
            {
                var trainig = new Training
                {
                    Name = trainingName,
                    ForUserId = userId,
                };

                await this.trainingRepository.AddAsync(trainig);
                await this.trainingRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int trainingId, string userId)
        {
            var training = this.trainingRepository
                .All()
                .FirstOrDefault(x => x.Id == trainingId && x.ForUserId == userId);

            this.trainingRepository.Delete(training);
            await this.trainingRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(string userId)
            => await this.trainingRepository
            .AllAsNoTracking()
            .Where(x => x.ForUserId == userId)
            .To<TModel>()
            .ToListAsync();

        public async Task<string> GetNameByIdAsync(int trainingId)
            => await this.trainingRepository
            .AllAsNoTracking()
            .Where(x => x.Id == trainingId)
            .Select(x => x.Name)
            .FirstOrDefaultAsync();

        public async Task<int> GetTrainingIdAsync(string name, string userId)
        {
            var training = await this.trainingRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == name && x.ForUserId == userId);

            return training != null ? training.Id : -1;
        }

        public async Task<bool> IsExistAsync(int id)
            => await this.trainingRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.Id == id);

        public async Task<bool> IsUserTrainingAsync(int id, string userId)
            => await this.trainingRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.Id == id && x.ForUserId == userId);
    }
}
