using Maintenance_API.Model;
using Microsoft.EntityFrameworkCore;
 
namespace Maintenance_API.Data
{
    public class MaintenanceDbContext:DbContext
    {
        public MaintenanceDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<MaintenanceRecord> Vehicles { get; set; }
    }
}
