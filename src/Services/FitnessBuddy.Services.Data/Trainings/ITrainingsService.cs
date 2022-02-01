namespace FitnessBuddy.Services.Data.Trainings
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Trainings;

    public interface ITrainingsService
    {
        public Task AddAsync(TrainingInputModel model);

        public IEnumerable<TModel> GetAll<TModel>();
    }
}
