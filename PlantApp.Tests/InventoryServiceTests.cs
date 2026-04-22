using System;
using Xunit;
using PlantApp.Core;

namespace PlantApp.Tests
{
    public class InventoryServiceTests
    {
        private readonly InventoryService _service;

        // Konstruktor wird vor jedem Test ausgeführt
        public InventoryServiceTests()
        {
            var plants = new List<Plant>(); // Leer für Unit Tests
            var fakeRepo = new FakePlantRepository(plants);

            _service = new InventoryService(fakeRepo);
        }

        // Helper-Methode für Testdaten
        private Plant CreatePlant(int stock, int minStock)
        {
            return new Plant
            {
                Name = "Tomato",
                Stock = stock,
                MinimumStock = minStock
            };
        }

        [Fact]
        public void Should_Return_True_When_Stock_Is_Below_Minimum()
        {
            // Arrange
            var plant = new Plant { Stock = 5, MinimumStock = 10 };

            // Act
            var result = _service.NeedsRestock(plant);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_Return_False_When_Stock_Equals_Minimum()
        {
            // Arrange
            var plant = CreatePlant(10, 10);

            // Act
            var result = _service.NeedsRestock(plant);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_Return_False_When_Stock_Is_Above_Minimum()
        {
            // Arrange
            var plant = CreatePlant(15, 10);

            // Act
            var result = _service.NeedsRestock(plant);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_Throw_Exception_When_Plant_Is_Null()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.NeedsRestock(null));
        }

        [Fact]
        public void Should_Count_Plants_Needing_Restock()
        {
            // Arrange
            var plants = new List<Plant>
            {
                new Plant { Stock = 5, MinimumStock = 10 },    // needs restock
                new Plant { Stock = 10, MinimumStock = 10 },    // no restock
                new Plant { Stock = 3, MinimumStock = 8 },    // needs restock

            };

            var fakeRepo = new FakePlantRepository(plants);
            var service = new InventoryService(fakeRepo);

            // Act
            var result = service.GetPlantsNeedingRestockCount();

            // Assert
            Assert.Equal(2, result);
        }
    }
} //Kommentar