using System;
using Xunit;
using PlantApp.Core;

namespace PlantApp.Tests
{
    public class InventoryServiceTests
    {
        // ======================================
        // BASIC FUNCTIONALITY (Happy Path)
        // ======================================

        [Theory]
        [InlineData(5, 10, true)]       // unter Minimum -> Restock nötig
        [InlineData(15, 10, false)]     // über Minimum -> kein Restock
        [InlineData(10, 10, false)]     // genau Minimum -> kein Restock
        public void NeedsRestock_ShouldReturnExpected(int stock, int min, bool expected)
        {
            // Arrange
            var plant = new Plant { Stock = stock, MinimumStock = min};
            var service = new InventoryService();

            // Act
            var result = service.NeedsRestock(plant);

            // Assert
            Assert.Equal(expected, result);
        }

        // ======================================
        // EDGE CASES
        // ======================================

        [Fact]
        public void NeedsRestock_ShouldHandleNegativeStock()
        {
            // Arrange
            var plant = new Plant { Stock = -1, MinimumStock = 10 };
            var service = new InventoryService();

            // Act
            var result = service.NeedsRestock(plant);

            // Assert
            Assert.True(result);
        }

        // ======================================
        // EDGE CASES
        // ======================================

        [Fact]
        public void NeedsRestock_ShouldThrowException_WhenPlantIsNull()
        {
            // Arrange
            var service = new InventoryService();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => service.NeedsRestock(null));
        }
    }
}