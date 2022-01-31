namespace FitnessBuddy.Services.Data.Exercises
{
    using System.Collections.Generic;
    using System.Linq;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class ExerciseEquipmentService : IExerciseEquipmentService
    {
        private readonly IDeletableEntityRepository<ExerciseEquipment> exerciseEquipmentRepository;

        public ExerciseEquipmentService(IDeletableEntityRepository<ExerciseEquipment> exerciseEquipmentRepository)
        {
            this.exerciseEquipmentRepository = exerciseEquipmentRepository;
        }

        public IEnumerable<TModel> GetAll<TModel>()
            => this.exerciseEquipmentRepository
            .AllAsNoTracking()
            .To<TModel>()
            .AsEnumerable();
    }
}
