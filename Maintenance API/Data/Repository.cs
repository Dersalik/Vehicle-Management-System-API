using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq.Expressions;
using Maintenance_API.Model;

namespace Maintenance_API.Data
{
    public class Repository : IRepository
    {
        private readonly MaintenanceDbContext _db;
        internal DbSet<MaintenanceRecord> dbSet;
        public Repository(MaintenanceDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<MaintenanceRecord>();

        }
        public async Task Add(MaintenanceRecord entity)
        {

           await dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<MaintenanceRecord>> GetAll()
        {
            return await dbSet.ToListAsync(); 
        }

        public async Task<MaintenanceRecord> GetFirstOrDefault(Expression<Func<MaintenanceRecord, bool>> filter)
        {
            return await dbSet.FirstOrDefaultAsync(filter);
        }

        public EntityEntry<MaintenanceRecord> Remove(MaintenanceRecord entity)
        {
            return dbSet.Remove(entity);

        }



        public void UpdateEntity(MaintenanceRecord entity)
        {

            _db.Update(entity);
        }

        public async Task<bool> CheckRecordExists(int id)
        {
           return await  _db.Vehicles.AnyAsync(x => x.Id == id);
        }   
        public async Task<IEnumerable<MaintenanceRecord>> Where(Expression<Func<MaintenanceRecord, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }
        public async  Task<int> Save()
        {
    
           return  await  _db.SaveChangesAsync();
        }
    }
}
