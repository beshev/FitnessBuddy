﻿namespace FitnessBuddy.Services.Data.Users
{
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Common.Repositories;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Web.ViewModels.Users;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task EditAsync(string userId, UserProfileInputModel model)
        {
            var user = this.usersRepository
                .All()
                .FirstOrDefault(x => x.Id == userId);

            user.UserName = model.UserName;
            user.WeightInKg = model.WeightInKg;
            user.GoalWeightInKg = model.GoalWeightInKg;
            user.ProfilePicture = model.ProfilePicture;
            user.HeightInCm = model.HeightInCm;
            user.DailyProteinGoal = model.DailyProteinGoal;
            user.DailyCarbohydratesGoal = model.DailyCarbohydratesGoal;
            user.DailyFatGoal = model.DailyFatGoal;
            user.AboutMe = model.AboutMe;

            await this.usersRepository.SaveChangesAsync();
        }

        public UserProfileInputModel GetUserInfo(string userId)
        {
            var user = this.usersRepository
                .AllAsNoTracking()
                .Where(x => x.Id == userId)
                .Select(userData => new UserProfileInputModel
                {
                    UserName = userData.UserName,
                    WeightInKg = userData.WeightInKg,
                    GoalWeightInKg = userData.GoalWeightInKg,
                    ProfilePicture = userData.ProfilePicture,
                    HeightInCm = userData.HeightInCm,
                    DailyProteinGoal = userData.DailyProteinGoal,
                    DailyCarbohydratesGoal = userData.DailyCarbohydratesGoal,
                    DailyFatGoal = userData.DailyFatGoal,
                    AboutMe = userData.AboutMe,
                })
                .FirstOrDefault();

            return user;
        }
    }
}
