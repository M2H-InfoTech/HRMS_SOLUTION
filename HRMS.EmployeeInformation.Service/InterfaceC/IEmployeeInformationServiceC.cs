using HRMS.EmployeeInformation.DTO.DTOs;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace HRMS.EmployeeInformation.Service.InterfaceC
{
    public interface IEmployeeInformationServiceC
    {
        Task<FillTravelTypeDto> FillTravelType ();
        }
}
