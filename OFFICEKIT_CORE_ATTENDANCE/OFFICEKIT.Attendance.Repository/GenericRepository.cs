using Microsoft.EntityFrameworkCore;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendace.Data;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Interfaces;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Repository
{
    public class GenericRepository<TEntity>(AttendanceDbContext context) : IGenericRepository<TEntity> where TEntity : class
    {
        internal readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

        public async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }


        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task RemoveAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        
    }
   
}
