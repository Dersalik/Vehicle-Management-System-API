using Maintenance_API.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Maintenance_API.Data
{
    public interface IRepository
    {
        Task Add(MaintenanceRecord entity);
        public Task<MaintenanceRecord> GetFirstOrDefault(Expression<Func<MaintenanceRecord, bool>> filter);
        Task<IEnumerable<MaintenanceRecord>> GetAll();
        public EntityEntry<MaintenanceRecord> Remove(MaintenanceRecord entity);
        public Task<int> Save();
        public  Task<bool> CheckVehicleExists(int id);

        void UpdateEntity(MaintenanceRecord entity);
        Task<IEnumerable<MaintenanceRecord>> Where(Expression<Func<MaintenanceRecord, bool>> predicate);
    }
}
