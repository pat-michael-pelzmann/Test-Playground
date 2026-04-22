using System;
using System.Collections.Generic;
using System.Text;

namespace PlantApp.Core
{
    public class Plant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public int MinimumStock { get; set; }
    }
}