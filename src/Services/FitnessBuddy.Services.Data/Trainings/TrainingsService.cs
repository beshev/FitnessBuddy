namespace FitnessBuddy.Services.Data.Trainings
{
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
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
    }
}
