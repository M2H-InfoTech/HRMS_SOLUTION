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
            CreateMap<CompanyParameter,CompanyParameterDto>().ReverseMap();
            CreateMap<EmployeeLanguageSkill,LanguageSkillResultDto>().ReverseMap();
            CreateMap<HrEmpProfdtlsApprl, HrEmpProfdtlsApprlDto>().ReverseMap();
            CreateMap<HrEmpProfdtlsApprl, HrEmpProfdtl>().ReverseMap();
            CreateMap<HrEmpreference, ReferenceSaveDto>().ReverseMap();
            CreateMap<EmployeeCertification, CertificationSaveDto> ( ).ReverseMap ( );
            CreateMap<HrEmpTechnicalApprl, SaveSkillSetDto>().ReverseMap();
        }
        }
}
