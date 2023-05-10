using Vehicle_API.Model;
using Microsoft.EntityFrameworkCore;

namespace Vehicle_API.Data
{
    public class VehicleDbContext:DbContext
    {
        public VehicleDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Vehicle> Vehicles { get; set; }
    }
}
