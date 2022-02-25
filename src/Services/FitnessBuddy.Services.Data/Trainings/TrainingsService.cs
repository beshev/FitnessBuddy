namespace FitnessBuddy.Services.Data.Trainings
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

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

        public IEnumerable<TModel> GetAll<TModel>(string userId)
            => this.trainingRepository
            .AllAsNoTracking()
            .Where(x => x.ForUserId == userId)
            .To<TModel>()
            .AsEnumerable();

        public int GetTrainingId(string name, string userId)
        {
            var training = this.trainingRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == name && x.ForUserId == userId);

            return training != null ? training.Id : -1;
        }

        public bool IsExist(int id)
            => this.trainingRepository
            .AllAsNoTracking()
            .Any(x => x.Id == id);

        public bool IsUserTraining(int id, string userId)
            => this.trainingRepository
            .AllAsNoTracking()
            .Any(x => x.Id == id && x.ForUserId == userId);
    }
}
