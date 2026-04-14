using FinGuardAI.DataAccess.Entities;
using FinGuardAI.DataAccess.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGuardAI.Business.Services
{
    public class FinancialRequestService
    {

        private FinancialRequestRepository _financialRequestRepository;

        public FinancialRequestService(FinancialRequestRepository financialRequestRepository)
        {
            _financialRequestRepository = financialRequestRepository;
        }
        async public Task<IEnumerable<FinancialRequest>?> GetAll()
        {
            IEnumerable<FinancialRequest> responses = await _financialRequestRepository.GetAllAsync();

            return (responses);
        }
        async public Task<FinancialRequest?> GetByID(int id)
        {
            var response = await _financialRequestRepository.GetByIdAsync(id);

            return response;

        }
        async public Task<bool> AddNew(FinancialRequest Entity)
        {
            return await _financialRequestRepository.Add(Entity);
        }
        async public Task<bool> Delete(int id)
        {
            return await _financialRequestRepository.Delete(id);
        }
        public async Task<bool> Update(FinancialRequest Entity)
        {
            return await _financialRequestRepository.Update(Entity);
        }
    }
}
