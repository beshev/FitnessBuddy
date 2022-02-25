namespace FitnessBuddy.Services.Data.TrainingsExercises
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Trainings;

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

        public IEnumerable<TModel> GetTrainingExercises<TModel>(int id)
            => this.trainingExerciseRepository
            .AllAsNoTracking()
            .Where(x => x.TrainingId == id)
            .To<TModel>()
            .AsEnumerable();

        public bool IsExist(int id)
            => this.trainingExerciseRepository
            .AllAsNoTracking()
            .Any(x => x.Id == id);

        public bool IsForUser(int id, string userId)
            => this.trainingExerciseRepository
            .AllAsNoTracking()
            .Any(x => x.Id == id && x.Training.ForUserId == userId);

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
