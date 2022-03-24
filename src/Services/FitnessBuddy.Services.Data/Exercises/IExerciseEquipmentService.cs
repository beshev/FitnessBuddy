namespace FitnessBuddy.Services.Data.Exercises
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IExerciseEquipmentService
    {
        public Task<IEnumerable<TModel>> GetAllAsync<TModel>();
    }
}
