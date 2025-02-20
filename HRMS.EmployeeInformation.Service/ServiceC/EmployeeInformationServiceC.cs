using HRMS.EmployeeInformation.Repository.Common.RepositoryC;
using HRMS.EmployeeInformation.Service.InterfaceC;

namespace HRMS.EmployeeInformation.Service.ServiceC
{
    public class EmployeeInformationServiceC : IEmployeeInformationServiceC
    {
        private readonly IRepositoryC _repositoryC;
        public EmployeeInformationServiceC(IRepositoryC repositoryC)
        {
            _repositoryC = repositoryC;
        }
    }
}
