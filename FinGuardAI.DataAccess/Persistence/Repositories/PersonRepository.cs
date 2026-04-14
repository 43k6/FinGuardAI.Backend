using FinGuardAI.DataAccess.Entities;
using FinGuardAI.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Infrastructure.Persistence.Repositories
{
    public class PersonRepository
    {
        private readonly AppDbContext _dbContext;

        public PersonRepository(AppDbContext dbContext)
        { _dbContext = dbContext; }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _dbContext.People.AsNoTracking().ToListAsync();
        }

        public async Task<bool> Add(Person entity)
        {
            if (entity == null)
                return false;

            _dbContext.People.Add(entity);

            return await Save();
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _dbContext.People.FindAsync(id);

            if (entity == null)
                return false;

            _dbContext.People.Remove(entity);

            return await Save();
        }



        public async Task<Person> GetByIdAsync(int id)
        {
            return await _dbContext.People.FindAsync(id);
        }

        public async Task<bool> Update(Person entity)
        {
            if (entity == null)
                return false;

            Person person = await _dbContext.People.FindAsync(entity.Id);

            if (person == null)
                return false;

            _dbContext.Entry(person).CurrentValues.SetValues(entity);

            return await Save();
        }

        public async Task<bool> IsExistByEmailIDAsync(string Email)
        {
            if (!string.IsNullOrEmpty(Email))
                return false;

            return await _dbContext.People
                            .AnyAsync(c => c.Email == Email);
        }
        public async Task<bool> IsExistByNationalIDAsync(string NationalID)
        {
            if (!string.IsNullOrEmpty(NationalID))
                return false;

            return await _dbContext.People
                            .AnyAsync(c => c.NationalId == NationalID);
        }
        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> IsExistByPersonIDAsync(int PersonID)
        {
            if (PersonID <= 0)
                return false;

            return await _dbContext.People
                            .AnyAsync(c => c.Id == PersonID);
        }
    }
}
