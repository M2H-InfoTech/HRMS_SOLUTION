using AutoMapper;
using EMPLOYEE_INFORMATION.Models.Entity;
using HRMS.EmployeeInformation.DTO.DTOs;

namespace ATTENDANCE.Mapping
{
    public class AttendenceMapper : Profile
    {
        public AttendenceMapper()
        {
            CreateMap<HrmValueType, HrmValueTypeDto>( );
        }
    }
}
