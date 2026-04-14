using AutoMapper;
using FinGuardAI.DataAccess.DTOs;
using FinGuardAI.DataAccess.Entities;
using WMS.Infrastructure.Persistence.Repositories;

namespace FinGuardAI.Business.Services
{
    public class PersonService
    {
        private PersonRepository _personRepository;
 


        public PersonService(PersonRepository personRepository)
        {
            _personRepository = personRepository;
           
        }
        async public Task<IEnumerable<Person>?> GetAll()
        {
            IEnumerable<Person> People = await _personRepository.GetAllAsync();

            return (People);
        }
        async public Task<Person?> GetByID(int id)
        {
            var Person = await _personRepository.GetByIdAsync(id);

            return Person;

        }
        async public Task<bool> AddNew(Person Entity)
        {
            return await _personRepository.Add(Entity);
        }
        async public Task<bool> Delete(int id)
        {
            return await _personRepository.Delete(id);
        }
        public async Task<bool> Update(Person Entity)
        {
            return await _personRepository.Update(Entity);
        }

        public async Task<bool> IsExistByNationalID(string NationalID)
        {
            return await _personRepository.IsExistByNationalIDAsync(NationalID);
        }

        public async Task<bool> IsExistByEmail(string Email)
        {
            return await _personRepository.IsExistByEmailIDAsync(Email);
        }

        public async Task<bool> IsExistByPersonID(int PersonID)
        {
            return await _personRepository.IsExistByPersonIDAsync(PersonID);
        }

    }
}
