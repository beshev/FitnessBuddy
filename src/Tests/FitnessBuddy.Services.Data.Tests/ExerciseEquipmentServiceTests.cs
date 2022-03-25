namespace FitnessBuddy.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Services.Data.Exercises;
    using FluentAssertions;
    using MockQueryable.Moq;
    using Xunit;

    public class ExerciseEquipmentServiceTests
    {
        [Fact]
        public async Task GetAllAsyncShouldReturnAllExerciceEquipment()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ExerciseEquipment>()
            {
                new ExerciseEquipment { Name = "Test", Id = 1 },
                new ExerciseEquipment { Name = "Test 2", Id = 2 },
            };

            var mockRepo = MockRepo.MockDeletableRepository<ExerciseEquipment>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExerciseEquipmentService(mockRepo.Object);

            var actual = await service.GetAllAsync<ExerciseEquipment>();

            actual.Should().BeEquivalentTo(list);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnAnEmptyCollectionIfThereAreNoEquipment()
        {
            TestMapper.InitializeAutoMapper();

            var list = new List<ExerciseEquipment>();

            var mockRepo = MockRepo.MockDeletableRepository<ExerciseEquipment>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new ExerciseEquipmentService(mockRepo.Object);

            var actual = await service.GetAllAsync<ExerciseEquipment>();

            actual.Should().BeEmpty();
        }
    }
}
