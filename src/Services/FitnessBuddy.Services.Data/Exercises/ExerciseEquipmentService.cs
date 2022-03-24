namespace FitnessBuddy.Services.Data.Exercises
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ExerciseEquipmentService : IExerciseEquipmentService
    {
        private readonly IDeletableEntityRepository<ExerciseEquipment> exerciseEquipmentRepository;

        public ExerciseEquipmentService(IDeletableEntityRepository<ExerciseEquipment> exerciseEquipmentRepository)
        {
            this.exerciseEquipmentRepository = exerciseEquipmentRepository;
        }

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>()
            => await this.exerciseEquipmentRepository
            .AllAsNoTracking()
            .To<TModel>()
            .ToListAsync();
    }
}
