﻿namespace FitnessBuddy.Services.Data.Exercises
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FitnessBuddy.Web.ViewModels.Exercises;

    public interface IExercisesService
    {
        public IEnumerable<ExerciseViewModel> GetAll();

        public Task AddAsync(string userId, ExerciseInputModel model);
    }
}
