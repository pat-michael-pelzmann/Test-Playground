using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;
using PlantApp.Core;

namespace PlantApp.Tests
{
    public class InventoryServiceMockTests
    {
        [Fact]
        public void Should_Count_Plants_Needing_Restock_With_Mock()
        {
            // Arrange
            var plants = new List<Plant>
             {
                new Plant { Stock = 5, MinimumStock = 10 },     // Restock
                new Plant { Stock = 20, MinimumStock = 10 },    // OK
                new Plant { Stock = 3, MinimumStock = 8 },      // Restock
             };

            var mockRepo = new Mock<IPlantRepository>();

            mockRepo
               .Setup(repo => repo.GetAllPlants())
               .Returns(plants);

            var service = new InventoryService(mockRepo.Object);

            // Act
            var result = service.GetPlantsNeedingRestockCount();

            // Assert
            Assert.Equal(2, result);

            // Verify (wichtig: Verhalten prüfen, nicht nur Ergebnis)
            // Frage: wurde die Funktion genau 1-mal aufgerufen?
            mockRepo.Verify(repo => repo.GetAllPlants(), Times.Once);
        }

        [Fact]
        public void Should_Return_Zero_When_No_Plants()
        {
            // Arrange
            var mockRepo = new Mock<IPlantRepository>();

            mockRepo
                .Setup(repo => repo.GetAllPlants())
                .Returns(new List<Plant>());

            var service = new InventoryService(mockRepo.Object);

            // Act
            var result = service.GetPlantsNeedingRestockCount();

            // Assert
            Assert.Equal(0, result);

            // Verify (wichtig: Verhalten prüfen, nicht nur Ergebnis)
            // Frage: wurde die Funktion genau 1-mal aufgerufen?
            mockRepo.Verify(repo => repo.GetAllPlants(), Times.Never);
        }
    }
}