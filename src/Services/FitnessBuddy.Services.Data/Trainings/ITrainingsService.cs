namespace FitnessBuddy.Services.Data.Trainings
{
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Trainings;

    public interface ITrainingsService
    {
        public Task AddAsync(TrainingInputModel model);
    }
}
