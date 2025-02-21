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
        public async Task<List<FillDocumentTypeDto>> FillDocumentType(int EmpID)
        {
            return await _repositoryB.FillDocumentType(EmpID);
        }
        public async Task<List<DocumentFieldDto>> DocumentField(int DocumentID)
        {
            return await _repositoryB.DocumentField(DocumentID);
        }
        public async Task<List<DocumentGetGeneralSubCategoryListDto>> DocumentGetGeneralSubCategoryList(string Remarks)
        {
            return await _repositoryB.DocumentGetGeneralSubCategoryList(Remarks);
        }
        public async Task<string> InsertDocumentsFieldDetails(List<TmpDocFileUpDto> DocumentBankField, int DocumentID, int In_EntryBy)
        {
            return await _repositoryB.InsertDocumentsFieldDetails(DocumentBankField, DocumentID, In_EntryBy);
        }
        public async Task<string> SetEmpDocuments(TmpFileUpDto DocumentBankField, int DetailID, string Status, int In_EntryBy)
        {
            return await _repositoryB.SetEmpDocuments(DocumentBankField, DetailID, Status, In_EntryBy);
        }

    }
}
