using AutoMapper;
using FinGuardAI.DataAccess.Entities;
using static FinGuardAI.DataAccess.DTOs.FinancialResponseDTO;

public class FinancialResponseProfile : Profile
{
    public FinancialResponseProfile()
    {
        // من Entity إلى DTO
        CreateMap<FinancialResponse, FinancialResponseDto>()
            .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.UserName));

        // من DTO إلى Entity
        CreateMap<FinancialResponseDto, FinancialResponse>()
            .ForMember(dest => dest.Request, opt => opt.Ignore())
            .ForMember(dest => dest.Creator, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}