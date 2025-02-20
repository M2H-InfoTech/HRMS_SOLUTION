using HRMS.EmployeeInformation.Repository.Common.RepositoryA;
using HRMS.EmployeeInformation.Service.InterfaceA;

namespace HRMS.EmployeeInformation.Service.ServiceA
{
    public class EmployeeInformationServiceA : IEmployeeInformationServiceA
    {
        private readonly IRepositoryA _repositoryA;
        public EmployeeInformationServiceA(IRepositoryA repositoryA)
        {
            _repositoryA = repositoryA;
        }
    }
}
