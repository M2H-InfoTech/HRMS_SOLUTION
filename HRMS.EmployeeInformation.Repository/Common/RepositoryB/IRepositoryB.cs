using HRMS.EmployeeInformation.DTO.DTOs;

namespace HRMS.EmployeeInformation.Repository.Common.RepositoryB
{
    public interface IRepositoryB
    {
        Task<List<FillDocumentTypeDto>> FillDocumentType(int EmpID);
        Task<List<DocumentFieldDto>> DocumentField(int DocumentID);
        Task<List<DocumentGetGeneralSubCategoryListDto>> DocumentGetGeneralSubCategoryList(string Remarks);
        Task<string> InsertDocumentsFieldDetails(List<TmpDocFileUpDto> DocumentBankField, int DocumentID, int In_EntryBy);
        Task<string> SetEmpDocuments(TmpFileUpDto DocumentBankField, int DetailID, string Status, int In_EntryBy);

    }
}
