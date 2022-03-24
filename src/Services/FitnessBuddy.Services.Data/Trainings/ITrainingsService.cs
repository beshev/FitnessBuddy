namespace FitnessBuddy.Services.Data.Trainings
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Trainings;

    public interface ITrainingsService
    {
        public Task AddAsync(string trainingName, string userId);

        public Task DeleteAsync(int trainingId, string userId);

        public Task<string> GetNameByIdAsync(int trainingId);

        public Task<IEnumerable<TModel>> GetAllAsync<TModel>(string userId);

        public Task<int> GetTrainingIdAsync(string name, string userId);

        public Task<bool> IsExistAsync(int id);

        public Task<bool> IsUserTrainingAsync(int id, string userId);
    }
}
