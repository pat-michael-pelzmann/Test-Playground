using System;
using System.Collections.Generic;
using System.Text;

namespace PlantApp.Core
{
    public class InventoryService
    {
        private readonly IPlantRepository _repository;
        
        public InventoryService(IPlantRepository repository)
        {
            _repository = repository;
        }
                
        public bool NeedsRestock(Plant plant)
        {
            if (plant == null)
                throw new ArgumentNullException(nameof(plant));

            return plant.Stock < plant.MinimumStock;
        }

        public int GetPlantsNeedingRestockCount()
        {
            var plants = _repository.GetAllPlants();

            return plants.Count(p => NeedsRestock(p));
        }
    }
}