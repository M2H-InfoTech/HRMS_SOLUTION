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
        public async Task<string> SubmitAssetDetailsNewAsync(SubmitAssetNewDto submitAssetNewDto)
        {
            return await _repositoryB.SubmitAssetDetailsNewAsync(submitAssetNewDto);
        }
        public async Task<string> UpdateAssetDetailsNewAsync(SubmitAssetNewDto submitAssetNewDto, int AssetRole)
        {
            return await _repositoryB.UpdateAssetDetailsNewAsync(submitAssetNewDto, AssetRole);
        }

        public async Task<List<AssetParameterDto>> GetAssetParameterAsync()
        {
            return await _repositoryB.GetAssetParameterAsync();
        }
        public async Task<List<dynamic>> FillAssetsubOnchange1Async(int ComFieldID, string AssignAssetStatus)
        {
            return await _repositoryB.FillAssetsubOnchange1Async(ComFieldID, AssignAssetStatus);
        }


        public async Task<List<dynamic>> GenrlCategoryFieldsReasonAsync(int Reason_Id)
        {
            return await _repositoryB.GenrlCategoryFieldsReasonAsync(Reason_Id);
        }


        public async Task<string> SavefieldsReasonsAsync(SaveReasonDto saveReasonDto)
        {
            return await _repositoryB.SavefieldsReasonsAsync(saveReasonDto);
        }

    }
}
