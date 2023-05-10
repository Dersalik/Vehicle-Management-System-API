using Maintenance_API.Data;
using Maintenance_API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq.Expressions;

namespace Vehicle_API.Data
{
    public class Repository : IRepository
    {
        private readonly VehicleDbContext _db;
        internal DbSet<Vehicle> dbSet;
        public Repository(VehicleDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<Vehicle>();

        }
        public async Task Add(Vehicle entity)
        {

           await dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            return await dbSet.ToListAsync(); 
        }

        public async Task<Vehicle> GetFirstOrDefault(Expression<Func<Vehicle, bool>> filter)
        {
            return await dbSet.FirstOrDefaultAsync(filter);
        }

        public EntityEntry<Vehicle> Remove(Vehicle entity)
        {
            return dbSet.Remove(entity);

        }



        public void UpdateEntity(Vehicle entity)
        {

            _db.Update(entity);
        }

        public async Task<bool> CheckVehicleExists(int id)
        {
           return await  _db.Vehicles.AnyAsync(x => x.Id == id);
        }   
        public async Task<IEnumerable<Vehicle>> Where(Expression<Func<Vehicle, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }
        public async  Task<int> Save()
        {
    
           return  await  _db.SaveChangesAsync();
        }
    }
}
