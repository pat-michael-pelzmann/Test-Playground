using Microsoft.EntityFrameworkCore;
using PlantApp.Core;
using PlantApp.Tests;
using System;
using System.Linq;
using System.Collections.Generic;

namespace PlantApp.Tests
{



    public class PlantRepositoryEf : IPlantRepository
    {
        private readonly AppDbContext _context;

        public PlantRepositoryEf(AppDbContext context)
        {
            _context = context;
        }

        public List<Plant> GetAllPlants()
        {
            return _context.Plants.ToList();
        }
    }
}
