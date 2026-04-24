using System;
using System.Collections.Generic;
using Xunit;
using PlantApp.Core;

namespace PlantApp.Tests.Unit
{
    [Trait("Category", "Unit")]        // Trait, um in der CI Pipeline leichter filtern zu können
    public class InventoryServiceTests
    {
        // ============================================
        // 🟢 UNIT TESTS – BASIC BUSINESS LOGIC
        // ============================================

        [Theory]
        [InlineData(5, 10, true)]   // unter Minimum → Restock nötig
        [InlineData(15, 10, false)] // über Minimum → kein Restock
        [InlineData(10, 10, false)] // genau Minimum → kein Restock
        public void NeedsRestock_ShouldReturnExpected(int stock, int minStock, bool expected)
        {
            // Arrange: einfache Pflanze mit Testdaten
            var plant = new Plant
            {
                Stock = stock,
                MinimumStock = minStock
            };

            var fakeRepo = new FakePlantRepository(new List<Plant> { plant });
            var service = new InventoryService(fakeRepo);

            // Act: Business-Logik ausführen
            var result = service.NeedsRestock(plant);

            // Assert: Ergebnis muss erwartetes Verhalten zeigen
            Assert.Equal(expected, result);
        }

        // ============================================
        // 🟡 EDGE CASES – GRENZFÄLLE
        // ============================================

        [Fact]
        public void NeedsRestock_ShouldHandleNegativeStock()
        {
            // Arrange: negativer Bestand (fehlerhafte oder extreme Daten)
            var plant = new Plant
            {
                Stock = -1,
                MinimumStock = 10
            };

            var fakeRepo = new FakePlantRepository(new List<Plant> { plant });
            var service = new InventoryService(fakeRepo);

            // Act
            var result = service.NeedsRestock(plant);

            // Assert: negativer Bestand → sollte als Restock gelten
            Assert.True(result);
        }

        [Fact]
        public void NeedsRestock_ShouldThrowException_WhenPlantIsNull()
        {
            // Arrange
            var fakeRepo = new FakePlantRepository(new List<Plant>());
            var service = new InventoryService(fakeRepo);

            // Act + Assert: Null darf nicht verarbeitet werden
            Assert.Throws<ArgumentNullException>(() => service.NeedsRestock(null));
        }

        // ============================================
        // 🔵 INTEGRATION LIGHT – REPOSITORY VERHALTEN
        // ============================================

        [Fact]
        public void Should_Count_Plants_Needing_Restock_With_FakeRepository()
        {
            // Arrange: mehrere Pflanzen wie in echter Datenquelle
            var fakeRepo = new FakePlantRepository(new List<Plant>
            {
                new Plant { Stock = 5, MinimumStock = 10 },   // Restock nötig
                new Plant { Stock = 20, MinimumStock = 10 },  // OK
                new Plant { Stock = 3, MinimumStock = 10 }    // Restock nötig
            });

            var service = new InventoryService(fakeRepo);

            // Act: aggregierte Geschäftslogik
            var result = service.GetPlantsNeedingRestockCount();

            // Assert: nur 2 von 3 Pflanzen brauchen Nachbestellung
            Assert.Equal(2, result);
        }
    }
}