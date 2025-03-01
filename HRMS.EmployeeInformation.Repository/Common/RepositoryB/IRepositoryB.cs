using HRMS.EmployeeInformation.DTO.DTOs;

namespace HRMS.EmployeeInformation.Repository.Common.RepositoryB
{
    public interface IRepositoryB
    {
        Task<List<object>> QualificationDocumentsDetails(int QualificationId);
        Task<string> InsertOrUpdateCommunication(SaveCommunicationSDto communications);
        Task<string> InsertOrUpdateCommunicationEmergency(SaveCommunicationSDto communications);
        Task<string> UpdateCommunication(SaveCommunicationSDto communications);
    }
}
