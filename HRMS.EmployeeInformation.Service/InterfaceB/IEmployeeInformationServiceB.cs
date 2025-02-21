using System.Reflection.Metadata;
using HRMS.EmployeeInformation.DTO.DTOs;


namespace HRMS.EmployeeInformation.Service.InterfaceB
{
    public interface IEmployeeInformationServiceB
    {
        Task<List<FillDocumentTypeDto>> FillDocumentType(int EmpID);
        Task<List<DocumentFieldDto>> DocumentField(int DocumentID);
        Task<List<DocumentGetGeneralSubCategoryListDto>> DocumentGetGeneralSubCategoryList(string Remarks);
        Task<string> InsertDocumentsFieldDetails(List<TmpDocFileUpDto> DocumentBankField, int DocumentID, int In_EntryBy);
        Task<string> SetEmpDocuments(TmpFileUpDto DocumentBankField, int DetailID, string Status, int In_EntryBy);

    }
}
