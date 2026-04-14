using AutoMapper;
using FinGuardAI.DataAccess.Entities;
using static FinGuardAI.DataAccess.DTOs.FinancialResponseDTO;

namespace FinGuardAI.Business.Mappings
{
    public class AuditLogProfile : Profile
    {
        public AuditLogProfile()
        {
            CreateMap<AuditLog, AuditLogDto>().ReverseMap();

        }
    }


}
