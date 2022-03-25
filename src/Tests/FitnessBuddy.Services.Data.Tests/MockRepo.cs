namespace FitnessBuddy.Services.Data.Tests
{
    using FitnessBuddy.Data.Common.Models;
    using FitnessBuddy.Data.Common.Repositories;
    using Moq;

    public static class MockRepo
    {
        public static Mock<IDeletableEntityRepository<TEntity>> MockDeletableRepository<TEntity>()
            where TEntity : class, IDeletableEntity
        {
            return new Mock<IDeletableEntityRepository<TEntity>>();
        }

        public static Mock<IRepository<TEntity>> MockRepository<TEntity>()
            where TEntity : class
        {
            return new Mock<IRepository<TEntity>>();
        }
    }
}
