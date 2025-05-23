﻿using AutoMapper;
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
            CreateMap<HrEmpProfdtlsApprlDto, HrEmpProfdtlsApprl>()
                .ForMember(dest => dest.MasterId, opt => opt.Ignore())
                .ForMember(dest => dest.ProfId, opt => opt.Ignore())// Ignore Identity Column
                .ForMember(dest => dest.EntryDt, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<HrEmpProfdtlsApprl, HrEmpProfdtl>().ReverseMap();
            CreateMap<HrEmpreference, ReferenceSaveDto>().ReverseMap();
            CreateMap<PersonalDetailsHistoryDto, PersonalDetailsHistory>().ReverseMap(); ;
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
            CreateMap<HrEmpMaster, EmployeeDetailsUpdateDto>().ReverseMap();
            CreateMap<LetterMaster01, LetterMaster01Dto>().ReverseMap();
            CreateMap<HrEmpProfdtl, HrEmpProfdtlsApprl>()
           .ForMember(dest => dest.Status, opt => opt.Ignore()) // Extra fields
           .ForMember(dest => dest.FlowStatus, opt => opt.Ignore())
           .ForMember(dest => dest.RequestId, opt => opt.Ignore())
           .ForMember(dest => dest.DateFrom, opt => opt.Ignore())
           .ForMember(dest => dest.MasterId, opt => opt.Ignore())
           .ReverseMap()
           .ForMember(dest => dest.ApprlId, opt => opt.Ignore()); // Extra in HrEmpProfdtl
            CreateMap<HighLevelViewTable, HighLevelTableDto>()
            .ForMember(dest => dest.LevelTwoDescription,
                opt => opt.MapFrom(src => $"{src.LevelTwoDescription} ({src.LevelOneDescription})"))

            .ForMember(dest => dest.LevelThreeDescription,
                opt => opt.MapFrom(src => $"{src.LevelThreeDescription} ({src.LevelOneDescription}-{src.LevelTwoDescription})"))

            .ForMember(dest => dest.LevelFourDescription,
                opt => opt.MapFrom(src => $"{src.LevelFourDescription} ({src.LevelThreeDescription})"))

            .ForMember(dest => dest.LevelFiveDescription,
                opt => opt.MapFrom(src => $"{src.LevelFiveDescription} ({src.LevelThreeDescription}-{src.LevelFourDescription})"))

            .ForMember(dest => dest.LevelSixDescription,
                opt => opt.MapFrom(src => $"{src.LevelSixDescription} ({src.LevelFourDescription}-{src.LevelFiveDescription})"))

            .ForMember(dest => dest.LevelSevenDescription,
                opt => opt.MapFrom(src => $"{src.LevelSevenDescription} ({src.LevelThreeDescription}-{src.LevelSixDescription})"))

            .ForMember(dest => dest.LevelEightDescription,
                opt => opt.MapFrom(src => $"{src.LevelEightDescription} ({src.LevelThreeDescription}-{src.LevelSevenDescription})"))

            .ForMember(dest => dest.LevelNineDescription,
                opt => opt.MapFrom(src => $"{src.LevelNineDescription} ({src.LevelSevenDescription}-{src.LevelEightDescription})"))

            .ForMember(dest => dest.LevelTenDescription,
                opt => opt.MapFrom(src => $"{src.LevelTenDescription} ({src.LevelEightDescription}-{src.LevelNineDescription})"))

            .ForMember(dest => dest.LevelElevenDescription,
                opt => opt.MapFrom(src => $"{src.LevelElevenDescription} ({src.LevelNineDescription}-{src.LevelTenDescription})"))
             .ForMember(dest => dest.LevelTwelveDescription,
                opt => opt.MapFrom(src => $"{src.LevelTwelveDescription} ({src.LevelTenDescription}-{src.LevelElevenDescription})"));

        }
    }
}
