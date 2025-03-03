using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Repository.Common.RepositoryC;
using HRMS.EmployeeInformation.Service.InterfaceC;

namespace HRMS.EmployeeInformation.Service.ServiceC
{
    public class EmployeeInformationServiceC : IEmployeeInformationServiceC
    {
        private readonly IRepositoryC _repositoryC;
        public EmployeeInformationServiceC(IRepositoryC repositoryC)
        {
            _repositoryC = repositoryC;
        }
        public async Task<FillTravelTypeDto> FillTravelType()
        {
            return await _repositoryC.FillTravelType();
        }
        public async Task<List<FillEmployeesBasedOnRoleDto>> FillEmployeesBasedOnRole(int firstEntityId, int secondEntityId, string transactionType)
        {
            return await _repositoryC.FillEmployeesBasedOnRole(firstEntityId, secondEntityId, transactionType);
        }
        public async Task<GetDependentDetailsDto> GetDependentDetails(int employeeId)
        {
            return await _repositoryC.GetDependentDetails(employeeId);
        }
        public async Task<int> SaveDependentEmp(SaveDependentEmpDto SaveDependentEmp)
        {
            return await _repositoryC.SaveDependentEmp(SaveDependentEmp);
        }
        public async Task<object> RetrieveEducation()
        {
            return await _repositoryC.RetrieveEducation();
        }
        public async Task<object> RetrieveCourse()
        {
            return await _repositoryC.RetrieveCourse();
        }
        public async Task<object> RetrieveSpecial()
        {
            return await _repositoryC.RetrieveSpecial();
        }
        public async Task<object> RetrieveUniversity()
        {
            return await _repositoryC.RetrieveUniversity();
        }
        public async Task<List<EditDependentEmpResultDto>> EditDependentEmpNew (int Schemeid,int EmpId)
            {
            return await _repositoryC.EditDependentEmpNew (Schemeid, EmpId);
            }
        public async Task<List<WorkFlowAvailabilityDto>> WorkFlowAvailability (int Emp_Id, string Transactiontype, int ParameterID)
            {
            return await _repositoryC.WorkFlowAvailability (Emp_Id, Transactiontype, ParameterID);
            }
        public async Task<string> InsertDepFields (List<TmpDocFileUpDto> InsertDepFields)
            {
            return await _repositoryC.InsertDepFields (InsertDepFields);
            }
        public async Task<List<FillDocumentTypeDto>> GetDocumentTypeEdit ( )
            {
            return await _repositoryC.GetDocumentTypeEdit ();
            }
        public async Task<List<DocumentFieldCheckBankDto>> DocumentFieldOfCheckBank (int DocumentID)
            {
            return await _repositoryC.DocumentFieldOfCheckBank (DocumentID);
            }
        public async Task<List<DocumentFieldGetEditDocFieldsDto>> DocumentFieldOfGetEditDocFields (int DocumentID, string Status)
            {
            return await _repositoryC.DocumentFieldOfGetEditDocFields (DocumentID, Status);
            }
        public async Task<List<GetCountryNameDto>> DocumentFieldOfGetCountryName ( )
            {
            return await _repositoryC.DocumentFieldOfGetCountryName ( );
            }
        public async Task<object> DocumentFieldOfGetBankTypeEdit ( )
            {
            return await _repositoryC.DocumentFieldOfGetBankTypeEdit ( );
            }
        public async Task<List<DocumentGetFolderNameDto>> DocumentOfGetFolderName (int DocumentID)
            {
            return await _repositoryC.DocumentOfGetFolderName (DocumentID);
            }
        public async Task<string> UpdateEmpDocumentDetailsAsync (object documentDetails, int DetailID, string Status, int EntryBy)
            {
            return await _repositoryC.UpdateEmpDocumentDetails (documentDetails, DetailID, Status, EntryBy);
            }
        }

    }

