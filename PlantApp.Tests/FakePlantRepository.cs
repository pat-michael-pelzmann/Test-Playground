using System;
using System.Collections.Generic;
using System.Text;
using PlantApp.Core;

namespace PlantApp.Tests
{
    public class FakePlantRepository : IPlantRepository
    {
        private readonly List<Plant> _plants;

        public FakePlantRepository(List<Plant> plants)
        {
            _plants = plants;
        }

        public List<Plant> GetAllPlants()
        {
            return _plants;
        }
    }
}
