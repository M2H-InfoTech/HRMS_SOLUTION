using HRMS.EmployeeInformation.DTO.DTOs;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace HRMS.EmployeeInformation.Repository.Common.RepositoryC
{
    public interface IRepositoryC
    {
        Task<FillTravelTypeDto> FillTravelType ();
        }
}
