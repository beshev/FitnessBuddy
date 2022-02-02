namespace FitnessBuddy.Services.Data.Trainings
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Trainings;

    public interface ITrainingsService
    {
        public Task AddAsync(string trainingName, string userId);

        public IEnumerable<TModel> GetAll<TModel>(string userId);

        public IEnumerable<string> GetNames(string userId);

        public int GetTrainingId(string name, string userId);
    }
}
