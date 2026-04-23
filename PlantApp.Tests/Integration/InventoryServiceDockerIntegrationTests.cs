using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlantApp.Core;
using Xunit;

namespace PlantApp.Tests.Integration
{
    [Trait("Category", "Integration")]           // Trait, um in der CI Pipeline leichter filtern zu können
    public class InventoryServiceDockerIntegrationTests
    {
        private AppDbContext CreateDbContext()
        {
            var connectionString =
                Environment.GetEnvironmentVariable("DB_CONNECTION")
                ?? "Host=localhost;Database=plants;Username=plant;Password=plant";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public void Should_Persist_Plant_And_Count_Restock_Correctly()
        {
            // Arrange
            using var context = CreateDbContext();
            context.Database.EnsureCreated();

            // Clean slate for deterministic test
            context.Plants.RemoveRange(context.Plants);
            context.SaveChanges();

            var repo = new PlantRepositoryEf(context);
            var service = new InventoryService(repo);

            var plant1 = new Plant
            {
                Name = "Tomato",
                Stock = 2,
                MinimumStock = 5
            };

            var plant2 = new Plant
            {
                Name = "Rose",
                Stock = 10,
                MinimumStock = 5
            };

            context.Plants.AddRange(plant1, plant2);
            context.SaveChanges();

            // Act
            var result = service.GetPlantsNeedingRestockCount();

            // Assert (persisted correctly + business logic correct)
            var fromDb = context.Plants.ToList();

            Assert.Equal(2, fromDb.Count);                 // DB persistence check
            Assert.Equal(1, result);                       // only Tomato needs restock
        }
    }
}