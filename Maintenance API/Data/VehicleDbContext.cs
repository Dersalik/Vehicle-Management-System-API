using Maintenance_API.Model;
using Microsoft.EntityFrameworkCore;

namespace Maintenance_API.Data
{
    public class VehicleDbContext:DbContext
    {
        public VehicleDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Vehicle> Vehicles { get; set; }
    }
}
