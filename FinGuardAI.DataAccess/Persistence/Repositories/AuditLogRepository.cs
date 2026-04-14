using FinGuardAI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGuardAI.DataAccess.Persistence.Repositories
{
    public class AuditLogRepository
    {
        private readonly AppDbContext _dbContext;

        public AuditLogRepository(AppDbContext dbContext)
        { _dbContext = dbContext; }

        public async Task<bool> Add(AuditLog entity)
        {
            if (entity == null)
                return false;

            _dbContext.AuditLogs.Add(entity);

            return await Save();
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _dbContext.AuditLogs.FindAsync(id);

            if (entity == null)
                return false;

            _dbContext.AuditLogs.Remove(entity);

            return await Save();
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _dbContext.AuditLogs.AsNoTracking().ToListAsync();
        }

        public async Task<AuditLog> GetByIdAsync(int id)
        {
            return await _dbContext.AuditLogs.FindAsync(id);
        }

     
        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
     
    }
}
