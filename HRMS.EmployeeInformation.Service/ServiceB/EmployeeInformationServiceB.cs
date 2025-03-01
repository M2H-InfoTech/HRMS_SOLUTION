using HRMS.EmployeeInformation.DTO.DTOs;
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

        public async Task<List<object>> QualificationDocumentsDetails(int QualificationId)
        {
            return await _repositoryB.QualificationDocumentsDetails(QualificationId);
        }

        public async Task<string> InsertOrUpdateCommunication(SaveCommunicationSDto communications)
        {
            return await _repositoryB.InsertOrUpdateCommunication(communications);
        }
        public async Task<string> InsertOrUpdateCommunicationEmergency(SaveCommunicationSDto communications)
        {
            return await _repositoryB.InsertOrUpdateCommunicationEmergency(communications);
        }
        public async Task<string> UpdateCommunication(SaveCommunicationSDto communications)
        {
            return await _repositoryB.UpdateCommunication(communications);
        }
    }
}
