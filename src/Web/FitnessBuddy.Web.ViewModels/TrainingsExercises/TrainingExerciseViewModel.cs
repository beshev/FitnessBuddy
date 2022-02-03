namespace FitnessBuddy.Web.ViewModels.TrainingsExercises
{
    using AutoMapper;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Mapping;

    public class TrainingExerciseViewModel : IMapFrom<TrainingExercise>, IHaveCustomMappings
    {
        public int TrainingExerciseId { get; set; }

        public string ExerciseId { get; set; }

        public string ExerciseName { get; set; }

        public string ExerciseImageUrl { get; set; }

        public int Sets { get; set; }

        public int Repetitions { get; set; }

        public double Weight { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<TrainingExercise, TrainingExerciseViewModel>()
                .ForMember(dest => dest.TrainingExerciseId, opt => opt.MapFrom(x => x.Id));
        }
    }
}
