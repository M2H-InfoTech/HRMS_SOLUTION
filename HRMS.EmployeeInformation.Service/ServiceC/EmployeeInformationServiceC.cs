using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Repository.Common.RepositoryC;
using HRMS.EmployeeInformation.Service.InterfaceC;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace HRMS.EmployeeInformation.Service.ServiceC
{
    public class EmployeeInformationServiceC : IEmployeeInformationServiceC
    {
        private readonly IRepositoryC _repositoryC;
        public EmployeeInformationServiceC(IRepositoryC repositoryC)
        {
            _repositoryC = repositoryC;
        }
        public async Task<FillTravelTypeDto> FillTravelType ()
            {
            return await _repositoryC.FillTravelType ();
            }
        }
}
