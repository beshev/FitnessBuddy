namespace FitnessBuddy.Services.Data.Exercises
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Exercises;

    public class ExercisesService : IExercisesService
    {
        private readonly IDeletableEntityRepository<Exercise> exerciseRepository;

        public ExercisesService(
            IDeletableEntityRepository<Exercise> exerciseRepository)
        {
            this.exerciseRepository = exerciseRepository;
        }

        public async Task AddAsync(string userId, ExerciseInputModel model)
        {
            var exercise = new Exercise
            {
                Name = model.Name,
                Description = model.Description,
                Difficulty = model.Difficulty,
                CategoryId = model.CategoryId,
                EquipmentId = model.EquipmentId,
                ImageUrl = model.ImageUrl,
                VideoUrl = GetYouTubeEmbededLink(model.VideoUrl),
                AddedByUserId = userId,
            };

            await this.exerciseRepository.AddAsync(exercise);
            await this.exerciseRepository.SaveChangesAsync();
        }

        public IEnumerable<ExerciseViewModel> GetAll()
            => this.exerciseRepository
            .AllAsNoTracking()
            .To<ExerciseViewModel>()
            .AsEnumerable();

        public TModel GetById<TModel>(int id)
            => this.exerciseRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<TModel>()
            .FirstOrDefault();

        private static string GetYouTubeEmbededLink(string url)
        {
            var regex = new Regex(@"^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|&v=)([^#&?]*).*");
            var match = regex.Match(url);

            string videoId = (match.Success && match.Groups[2].Length == 11)
                ? match.Groups[2].ToString()
                : null;

            return $"https://youtube.com/embed/{videoId}";
        }
    }
}
