using System;
using System.Collections.Generic;
using System.Text;

namespace PlantApp.Core
{
    public interface IPlantRepository
    {
        List<Plant> GetAllPlants();
    }
}
