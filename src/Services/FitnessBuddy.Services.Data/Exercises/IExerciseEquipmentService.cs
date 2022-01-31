namespace FitnessBuddy.Services.Data.Exercises
{
    using System.Collections.Generic;

    using FitnessBuddy.Data.Models;

    public interface IExerciseEquipmentService
    {
        public IEnumerable<TModel> GetAll<TModel>();
    }
}
