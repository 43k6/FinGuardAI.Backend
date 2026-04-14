using AutoMapper;
using FinGuardAI.DataAccess.DTOs;
using FinGuardAI.DataAccess.Entities;
using static FinGuardAI.DataAccess.DTOs.FinancialResponseDTO;

namespace FinGuardAI.Business.Mappings
{

    public class FinancialRequestProfile : Profile
    {
        public FinancialRequestProfile()
        {
            // من Entity إلى DTO (للعرض)
            CreateMap<FinancialRequest, FinancialResponseDTO.FinancialRequestDto>()
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.UserName));

            // من DTO إلى Entity (للإضافة والتحديث)
            CreateMap<FinancialResponseDTO.FinancialRequestDto, FinancialRequest>()
                .ForMember(dest => dest.Creator, opt => opt.Ignore())   // تجاهل كائن المستخدم كاملاً
                .ForMember(dest => dest.Response, opt => opt.Ignore()) // تجاهل كائن الرد
                .ForMember(dest => dest.Id, opt => opt.Ignore());      // تجاهل الـ ID في حالة الإضافة (Add)
        }
    }


}