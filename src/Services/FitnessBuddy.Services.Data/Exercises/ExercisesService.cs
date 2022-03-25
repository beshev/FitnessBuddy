namespace FitnessBuddy.Services.Data.Exercises
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using FitnessBuddy.Web.ViewModels.Exercises;
    using Microsoft.EntityFrameworkCore;

    public class ExercisesService : IExercisesService
    {
        private readonly IDeletableEntityRepository<Exercise> exerciseRepository;

        public ExercisesService(
            IDeletableEntityRepository<Exercise> exerciseRepository)
        {
            this.exerciseRepository = exerciseRepository;
        }

        public async Task<int> AddAsync(string userId, ExerciseInputModel model)
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

            return exercise.Id;
        }

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(string search = "", int skip = 0, int? take = null)
        {
            var query = this.exerciseRepository
                .AllAsNoTracking();

            if (string.IsNullOrWhiteSpace(search) == false)
            {
                query = query
                    .Where(x => x.Name
                    .Contains(search));
            }

            query = query
                .OrderByDescending(x => x.ExerciseLikes.Count)
                .ThenByDescending(x => x.CreatedOn);

            if (take.HasValue)
            {
                query = query
                    .Skip(skip)
                    .Take(take.Value);
            }

            return await query
                .To<TModel>()
                .ToListAsync();
        }

        public async Task<TModel> GetByIdAsync<TModel>(int id)
            => await this.exerciseRepository
            .AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<TModel>()
            .FirstOrDefaultAsync();

        public Task<int> GetCountAsync(string search = "")
        {
            var query = this.exerciseRepository
                .AllAsNoTracking();

            if (string.IsNullOrWhiteSpace(search) == false)
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            return query.CountAsync();
        }

        public async Task<bool> IsUserCreatorAsync(string userId, int exerciseId)
            => await this.exerciseRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.AddedByUserId == userId && x.Id == exerciseId);

        public async Task EditAsync(ExerciseInputModel model)
        {
            var exercise = this.exerciseRepository
                .All()
                .FirstOrDefault(x => x.Id == model.Id);

            exercise.Name = model.Name;
            exercise.Description = model.Description;
            exercise.ImageUrl = model.ImageUrl;
            exercise.VideoUrl = GetYouTubeEmbededLink(model.VideoUrl);
            exercise.CategoryId = model.CategoryId;
            exercise.EquipmentId = model.EquipmentId;
            exercise.Difficulty = model.Difficulty;

            this.exerciseRepository.Update(exercise);
            await this.exerciseRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var exercise = this.exerciseRepository
                   .All()
                   .FirstOrDefault(x => x.Id == id);

            this.exerciseRepository.Delete(exercise);
            await this.exerciseRepository.SaveChangesAsync();
        }

        public async Task<bool> IsExistAsync(int id)
            => await this.exerciseRepository
            .AllAsNoTracking()
            .AnyAsync(x => x.Id == id);

        public async Task<IEnumerable<TModel>> GetRandomAsync<TModel>(int count)
            => await this.exerciseRepository
            .AllAsNoTracking()
            .OrderBy(x => Guid.NewGuid())
            .To<TModel>()
            .Take(count)
            .ToListAsync();

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
