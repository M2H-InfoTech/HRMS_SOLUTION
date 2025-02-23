using HRMS.EmployeeInformation.Repository.Common.RepositoryB;
using HRMS.EmployeeInformation.Service.InterfaceB;

namespace HRMS.EmployeeInformation.Service.ServiceB
{
    public class EmployeeInformationServiceB : IEmployeeInformationServiceB
    {
        private readonly IRepositoryB _repositoryB;
        public EmployeeInformationServiceB(IRepositoryB repositoryB)
        {
            _repositoryB = repositoryB;
        }


    }
}
