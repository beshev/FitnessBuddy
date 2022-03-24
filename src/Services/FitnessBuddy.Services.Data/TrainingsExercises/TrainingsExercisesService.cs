namespace FitnessBuddy.Services.Data.TrainingsExercises
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Trainings;
    using Microsoft.EntityFrameworkCore;

    public class TrainingsExercisesService : ITrainingsExercisesService
    {
        private readonly IDeletableEntityRepository<TrainingExercise> trainingExerciseRepository;

        public TrainingsExercisesService(IDeletableEntityRepository<TrainingExercise> trainingExerciseRepository)
        {
            this.trainingExerciseRepository = trainingExerciseRepository;
        }

        public async Task AddAsync(TrainingExerciseInputModel model)
        {
            var trainingExercise = this.trainingExerciseRepository
                .All()
                .FirstOrDefault(x => x.ExerciseId == model.ExerciseId && x.TrainingId == model.TrainingId);

            if (trainingExercise == null)
            {
                trainingExercise = new TrainingExercise
                {
                    ExerciseId = model.ExerciseId,
                    TrainingId = model.TrainingId,
                    Sets = model.Sets,
                    Repetitions = model.Repetitions,
                    Weight = model.Weight,
                };

                await this.trainingExerciseRepository.AddAsync(trainingExercise);
            }
            else
            {
                trainingExercise.Sets = model.Sets;
                trainingExercise.Repetitions = model.Repetitions;
                trainingExercise.Weight = model.Weight;
            }

            await this.trainingExerciseRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetTrainingExercisesAsync<TModel>(int id)
            => await this.trainingExerciseRepository
            .AllAsNoTracking()
            .Where(x => x.TrainingId == id)
            .To<TModel>()
            .ToListAsync();

        public async Task<bool> IsExistAsync(int id)
            => await this.trainingExerciseRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.Id == id);

        public async Task<bool> IsForUserAsync(int id, string userId)
            => await this.trainingExerciseRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.Id == id && x.Training.ForUserId == userId);

        public async Task RemoveAsync(int id)
        {
            var trainingExercise = this.trainingExerciseRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            this.trainingExerciseRepository.Delete(trainingExercise);

            await this.trainingExerciseRepository.SaveChangesAsync();
        }
    }
}
