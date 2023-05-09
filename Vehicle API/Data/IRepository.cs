using Maintenance_API.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Vehicle_API.Data
{
    public interface IRepository
    {
        Task Add(Vehicle entity);
        public Task<Vehicle> GetFirstOrDefault(Expression<Func<Vehicle, bool>> filter);
        Task<IEnumerable<Vehicle>> GetAll();
        public EntityEntry<Vehicle> Remove(Vehicle entity);

        void UpdateEntity(Vehicle entity);
        Task<IEnumerable<Vehicle>> Where(Expression<Func<Vehicle, bool>> predicate);
    }
}
