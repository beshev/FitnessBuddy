namespace FitnessBuddy.Services.Data.Trainings
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Trainings;

    public class TrainingsService : ITrainingsService
    {
        private readonly IDeletableEntityRepository<Training> trainingRepository;

        public TrainingsService(IDeletableEntityRepository<Training> trainingRepository)
        {
            this.trainingRepository = trainingRepository;
        }

        public async Task AddAsync(TrainingInputModel model)
        {
            var trainig = new Training
            {
                Name = model.Name,
                ForUserId = model.UserId,
            };

            await this.trainingRepository.AddAsync(trainig);
            await this.trainingRepository.SaveChangesAsync();
        }

        public IEnumerable<TModel> GetAll<TModel>()
            => this.trainingRepository
            .AllAsNoTracking()
            .To<TModel>()
            .AsEnumerable();

        public int GetIdByName(string name)
        {
            var training = this.trainingRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == name);

            return training != null ? training.Id : -1;
        }

        public IEnumerable<string> GetNames()
            => this.trainingRepository
            .AllAsNoTracking()
            .Select(x => x.Name)
            .AsEnumerable();
    }
}
