using Microsoft.EntityFrameworkCore;
using PlantApp.Core;
using System;
using System.Linq;
using System.Collections.Generic;

namespace PlantApp.Core
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Plant> Plants { get; set; }
    }
}

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