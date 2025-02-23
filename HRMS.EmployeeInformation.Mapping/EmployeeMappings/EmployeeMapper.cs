using AutoMapper;
using EMPLOYEE_INFORMATION.Models;
using EMPLOYEE_INFORMATION.Models.Entity;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Models;
using HRMS.EmployeeInformation.Repository.Common;
using MPLOYEE_INFORMATION.DTO.DTOs;


namespace EMPLOYEE_INFORMATION.Services.Mapping
{
    public class EmployeeMapper : Profile
    {
        public EmployeeMapper()
        {
            CreateMap<CompanyParameter, CompanyParameterDto>().ReverseMap();
            CreateMap<EmployeeLanguageSkill, LanguageSkillResultDto>().ReverseMap();
            CreateMap<HrEmpProfdtlsApprl, HrEmpProfdtlsApprlDto>().ReverseMap();
            CreateMap<HrEmpProfdtlsApprl, HrEmpProfdtl>().ReverseMap();
            CreateMap<HrEmpreference, ReferenceSaveDto>().ReverseMap();
            CreateMap<PersonalDetailsHistoryDto, PersonalDetailsHistory>().ReverseMap();
            CreateMap<HrEmpMaster, EmployeeParametersDto>().ReverseMap();
            // Convert DateTime to String (for DTO)
            //.ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.HasValue ? src.DateOfBirth.Value.ToString("yyyy-MM-dd") : null))
            //.ForMember(dest => dest.JoinDt, opt => opt.MapFrom(src => src.JoinDt.HasValue ? src.JoinDt.Value.ToString("yyyy-MM-dd") : null))
            //.ForMember(dest => dest.ReviewDt, opt => opt.MapFrom(src => src.ReviewDt.HasValue ? src.ReviewDt.Value.ToString("yyyy-MM-dd") : null))
            //.ForMember(dest => dest.ProbationDt, opt => opt.MapFrom(src => src.ProbationDt.HasValue ? src.ProbationDt.Value.ToString("yyyy-MM-dd") : null))
            //.ForMember(dest => dest.EntryDt, opt => opt.MapFrom(src => src.EntryDt.ToString("yyyy-MM-dd")))
            //.ForMember(dest => dest.GratuityStrtDate, opt => opt.MapFrom(src => src.GratuityStrtDate.HasValue ? src.GratuityStrtDate.Value.ToString("yyyy-MM-dd") : null))
            //.ForMember(dest => dest.FirstEntryDate, opt => opt.MapFrom(src => src.FirstEntryDate.HasValue ? src.FirstEntryDate.Value.ToString("yyyy-MM-dd") : null))
            //.ForMember(dest => dest.WeddingDate, opt => opt.MapFrom(src => src.WeddingDate.HasValue ? src.WeddingDate.Value.ToString("yyyy-MM-dd") : null))
            //.ForMember(dest => dest.EffectDate, opt => opt.MapFrom(src => src.ModifiedDate.HasValue ? src.ModifiedDate.Value.ToString("yyyy-MM-dd") : null))

            //// Reverse Mapping (DTO to Entity)
            //.ReverseMap()
            //.ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.DateOfBirth) ? DateTime.Parse(src.DateOfBirth) : (DateTime?)null))
            //.ForMember(dest => dest.JoinDt, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.JoinDt) ? DateTime.Parse(src.JoinDt) : (DateTime?)null))
            //.ForMember(dest => dest.ReviewDt, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ReviewDt) ? DateTime.Parse(src.ReviewDt) : (DateTime?)null))
            //.ForMember(dest => dest.ProbationDt, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ProbationDt) ? DateTime.Parse(src.ProbationDt) : (DateTime?)null))
            //.ForMember(dest => dest.EntryDt, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.EntryDt) ? DateTime.Parse(src.EntryDt) : DateTime.Now))
            //.ForMember(dest => dest.GratuityStrtDate, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.GratuityStrtDate) ? DateTime.Parse(src.GratuityStrtDate) : (DateTime?)null))
            //.ForMember(dest => dest.FirstEntryDate, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.FirstEntryDate) ? DateTime.Parse(src.FirstEntryDate) : (DateTime?)null))
            //.ForMember(dest => dest.WeddingDate, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.WeddingDate) ? DateTime.Parse(src.WeddingDate) : (DateTime?)null))
            //.ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.EffectDate) ? DateTime.Parse(src.EffectDate) : (DateTime?)null));

            CreateMap<EmployeeCertification, CertificationSaveDto>().ReverseMap();
            CreateMap<HrEmpTechnicalApprl, SaveSkillSetDto>().ReverseMap();
            CreateMap<EmployeeParametersDto, EmployeeDetailsUpdateDto>().ReverseMap();
            CreateMap<HrEmpMaster, EmployeeDetailsUpdateDto>().ReverseMap();
        }
    }
}
