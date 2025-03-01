using HRMS.EmployeeInformation.DTO.DTOs;

namespace HRMS.EmployeeInformation.Repository.Common.RepositoryC
{
    public interface IRepositoryC
    {
        Task<FillTravelTypeDto> FillTravelType();
        Task<List<FillEmployeesBasedOnRoleDto>> FillEmployeesBasedOnRole(int firstEntityId, int secondEntityId, string transactionType);
        Task<GetDependentDetailsDto> GetDependentDetails(int employeeId);
        Task<int> SaveDependentEmp(SaveDependentEmpDto SaveDependentEmp);
        Task<object> RetrieveEducation();
        Task<object> RetrieveCourse();
        Task<object> RetrieveSpecial();
        Task<object> RetrieveUniversity();
        Task<List<EditDependentEmpResultDto>> EditDependentEmpNew (int Schemeid, int EmpId);
        Task<List<WorkFlowAvailabilityDto>> WorkFlowAvailability (int Emp_Id, string Transactiontype, int ParameterID);
        Task<string> InsertDepFields (List<TmpDocFileUpDto> InsertDepFields);
        Task<List<FillDocumentTypeDto>> GetDocumentTypeEdit ();
        Task<List<DocumentFieldCheckBankDto>> DocumentField_CheckBank (int DocumentID);
        Task<List<DocumentFieldGetEditDocFieldsDto>> DocumentField_GetEditDocFields (int DocumentID, string Status);
        Task<List<GetCountryNameDto>> DocumentField_GetCountryName ( );
        Task<object> DocumentField_GetBankTypeEdit ( );
        Task<List<DocumentGetFolderNameDto>> Document_GetFolderName (int DocumentID);
        Task<string> UpdateEmpDocumentDetails (object documentDetails, int DetailID, string Status, int EntryBy);

        }
    }
