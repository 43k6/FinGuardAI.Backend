using Azure;
using FinGuardAI.DataAccess.Entities;
using FinGuardAI.DataAccess.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Infrastructure.Persistence.Repositories;

namespace FinGuardAI.Business.Services
{
    public class FinancialResponseService
    {

        private FinancialResponseRepository _financialResponseRepository;

        public FinancialResponseService(FinancialResponseRepository financialResponseRepository)
        {
            _financialResponseRepository = financialResponseRepository;
           
        }
        async public Task<IEnumerable<FinancialResponse>?> GetAll()
        {
            IEnumerable<FinancialResponse> responses = await _financialResponseRepository.GetAllAsync();

            return (responses);
        }
        async public Task<FinancialResponse?> GetByID(int id)
        {
            var response = await _financialResponseRepository.GetByIdAsync(id);

            return response;

        }
        async public Task<bool> AddNew(FinancialResponse Entity)
        {
            return await _financialResponseRepository.Add(Entity);
        }
        async public Task<bool> Delete(int id)
        {
            return await _financialResponseRepository.Delete(id);
        }
        public async Task<bool> Update(FinancialResponse Entity)
        {
            return await _financialResponseRepository.Update(Entity);
        }

    
    }
}
