namespace FitnessBuddy.Services.Data.Trainings
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Trainings;

    public interface ITrainingsService
    {
        public Task AddAsync(string trainingName, string userId);

        public Task DeleteAsync(int trainingId, string userId);

        public IEnumerable<TModel> GetAll<TModel>(string userId);

        public int GetTrainingId(string name, string userId);

        public bool IsExist(int id);

        public bool IsUserTraining(int id, string userId);
    }
}
