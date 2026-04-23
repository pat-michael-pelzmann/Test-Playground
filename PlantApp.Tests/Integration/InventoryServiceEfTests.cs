using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using PlantApp.Core;

namespace PlantApp.Tests.Integration
{
    [Trait("Category", "Integration")]           // Trait, um in der CI Pipeline leichter filtern zu können
    public class InventoryServiceEfTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // wichtig: isolierte DB pro Test
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public void Should_Count_Plants_Needing_Restock_With_InMemoryDb()
        {
            // Arrange
            using var context = CreateContext();

            context.Plants.AddRange(
                new Plant { Name = "Tomate", Stock = 5, MinimumStock = 10 },   // Restock
                new Plant { Name = "Gurke", Stock = 20, MinimumStock = 10 },  // OK
                new Plant { Name = "Rose", Stock = 3, MinimumStock = 10 }    // Restock
            );

            context.SaveChanges();

            var repo = new PlantRepositoryEf(context);
            var service = new InventoryService(repo);

            // Act
            var result = service.GetPlantsNeedingRestockCount();

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void Should_Return_Zero_When_No_Plants_In_Db()
        {
            // Arrange
            using var context = CreateContext();

            var repo = new PlantRepositoryEf(context);
            var service = new InventoryService(repo);

            // Act
            var result = service.GetPlantsNeedingRestockCount();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Should_Persist_And_Read_Plants_Correctly()
        {
            // Arrange
            using var context = CreateContext();

            var plant = new Plant { Name = "Erdbeere", Stock = 7, MinimumStock = 10 };

            context.Plants.Add(plant);
            context.SaveChanges();

            var repo = new PlantRepositoryEf(context);

            // Act
            var plantsFromDb = repo.GetAllPlants();

            // Assert
            Assert.Single(plantsFromDb);
            Assert.Equal(7, plantsFromDb.First().Stock);
        }
    }
}