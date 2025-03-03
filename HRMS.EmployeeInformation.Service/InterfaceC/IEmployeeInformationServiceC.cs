using HRMS.EmployeeInformation.DTO.DTOs;

namespace HRMS.EmployeeInformation.Service.InterfaceC
{
    public interface IEmployeeInformationServiceC
    {
        Task<FillTravelTypeDto> FillTravelType();
        Task<List<FillEmployeesBasedOnRoleDto>> FillEmployeesBasedOnRole(int firstEntityId, int secondEntityId, string transactionType);
        Task<GetDependentDetailsDto> GetDependentDetails(int employeeId);
        Task<int> SaveDependentEmp(SaveDependentEmpDto SaveDependentEmp);
        Task<object> RetrieveEducation();
        Task<object> RetrieveCourse();
        Task<object> RetrieveSpecial();
        Task<object> RetrieveUniversity();
        Task<List<EditDependentEmpResultDto>> EditDependentEmpNew (int Schemeid,int EmpId);
        Task<List<WorkFlowAvailabilityDto>> WorkFlowAvailability (int Emp_Id, string Transactiontype, int ParameterID);
        Task<string> InsertDepFields (List<TmpDocFileUpDto> InsertDepFields);
        Task<List<FillDocumentTypeDto>> GetDocumentTypeEdit ();
        Task<List<DocumentFieldCheckBankDto>> DocumentFieldOfCheckBank (int DocumentID);
        Task<List<DocumentFieldGetEditDocFieldsDto>> DocumentFieldOfGetEditDocFields (int DocumentID, string Status);
        Task<List<GetCountryNameDto>> DocumentFieldOfGetCountryName ( );
        Task<object> DocumentFieldOfGetBankTypeEdit ( );
        Task<List<DocumentGetFolderNameDto>> DocumentOfGetFolderName (int DocumentID);
        Task<string> UpdateEmpDocumentDetailsAsync (object documentDetails, int detailID, string status, int entryBy);



        }
    }
