using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Service.InterfaceC;
using Microsoft.AspNetCore.Mvc;

namespace EMPLOYEE_INFORMATION.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeeCController : Controller
    {
        private readonly IEmployeeInformationServiceC _employeeInformationC;
        public EmployeeCController(IEmployeeInformationServiceC employeeInformationC)
        {
            _employeeInformationC = employeeInformationC;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FillTravelType()
        {
            var fillTravelType = await _employeeInformationC.FillTravelType();
            return new JsonResult(fillTravelType);
        }
        [HttpGet]
        public async Task<IActionResult> FillEmployeesBasedOnRole(int firstEntityId, int secondEntityId, string transactionType)
        {
            var fillEmployeesBasedOnRole = await _employeeInformationC.FillEmployeesBasedOnRole(firstEntityId, secondEntityId, transactionType);
            return new JsonResult(fillEmployeesBasedOnRole);
        }
        [HttpGet]
        public async Task<IActionResult> GetDependentDetails(int employeeId)
        {
            var getDependentDetails = await _employeeInformationC.GetDependentDetails(employeeId);
            return new JsonResult(getDependentDetails);
        }
        [HttpPost]
        public async Task<IActionResult> SaveDependentEmp([FromBody] SaveDependentEmpDto SaveDependentEmp)   // For Document and Bank Insertion
        {
            var saveDependentEmp = await _employeeInformationC.SaveDependentEmp(SaveDependentEmp);

            return Ok(saveDependentEmp);
        }
        [HttpGet]
        public async Task<IActionResult> RetrieveEducation()
        {
            var retrieveEducation = await _employeeInformationC.RetrieveEducation();
            return new JsonResult(retrieveEducation);

        }
        [HttpGet]
        public async Task<IActionResult> RetrieveCourse()
        {
            var retrieveCourse = await _employeeInformationC.RetrieveCourse();
            return new JsonResult(retrieveCourse);

        }
        [HttpGet]
        public async Task<IActionResult> RetrieveSpecial()
        {
            var retrieveSpecial = await _employeeInformationC.RetrieveSpecial();
            return new JsonResult(retrieveSpecial);

        }
        [HttpGet]
        public async Task<IActionResult> RetrieveUniversity()
        {
            var retrieveUniversity = await _employeeInformationC.RetrieveUniversity();
            return new JsonResult(retrieveUniversity);

        }
        [HttpGet]
        public async Task<IActionResult> EditDependentEmpNew (int Schemeid,int EmpId)
            {
            var EditDependentEmpNew = await _employeeInformationC.EditDependentEmpNew (Schemeid, EmpId);
            return new JsonResult (EditDependentEmpNew);
            }

        [HttpGet]
        public async Task<IActionResult> WorkFlowAvailability (int Emp_Id, string Transactiontype, int ParameterID)
            {
            var WorkFlowAvailability = await _employeeInformationC.WorkFlowAvailability (Emp_Id, Transactiontype, ParameterID);
            return new JsonResult (WorkFlowAvailability);
            }

        [HttpPost]
        public async Task<IActionResult> InsertDepFields ([FromBody] List<TmpDocFileUpDto> InsertDepFields)   //InsertOrUpdate dependent function
            {
            var FieldDetails = await _employeeInformationC.InsertDepFields (InsertDepFields);

            if (FieldDetails == null || !FieldDetails.Any ( ))
                {
                return NotFound ( );
                }

            return Ok (FieldDetails);
            }

        [HttpGet]
        public async Task<IActionResult> GetDocumentTypeEdit ()    //dropdown in document edit button
            {
            var GetDocumentTypeEdit = await _employeeInformationC.GetDocumentTypeEdit ();
            return new JsonResult (GetDocumentTypeEdit);
            }
        [HttpGet]
        public async Task<IActionResult> DocumentField_CheckBank (int DocumentID)   //checking if bank is the type inside document edit button
            {
            var DocumentField_CheckBank = await _employeeInformationC.DocumentField_CheckBank (DocumentID);
            return new JsonResult (DocumentField_CheckBank);
            }
        [HttpGet]
        public async Task<IActionResult> DocumentField_GetEditDocFields (int DocumentID, string Status)   //fetching doc field name inside document edit button
            {
            var DocumentField_GetEditDocFields = await _employeeInformationC.DocumentField_GetEditDocFields (DocumentID, Status);
            return new JsonResult (DocumentField_GetEditDocFields);
            }
        [HttpGet]
        public async Task<IActionResult> DocumentField_GetCountryName ( )   //fetching Country name inside document edit button
            {
            var DocumentField_GetCountryName = await _employeeInformationC.DocumentField_GetCountryName ( );
            return new JsonResult (DocumentField_GetCountryName);
            }
        [HttpGet]
        public async Task<IActionResult> DocumentField_GetBankTypeEdit ( )   //if bank dropdown is clicked inside document edit button
            {
            var DocumentField_GetBankTypeEdit = await _employeeInformationC.DocumentField_GetBankTypeEdit ( );
            return new JsonResult (DocumentField_GetBankTypeEdit);
            }

        [HttpGet]
        public async Task<IActionResult> Document_GetFolderName (int DocumentID)   //retrieve folder name in edit document tab
            {
            var Document_GetFolderName = await _employeeInformationC.Document_GetFolderName (DocumentID);
            return new JsonResult (Document_GetFolderName);
            }

        [HttpPost]
        public async Task<IActionResult> UpdateEmpDocumentDetails ([FromBody] object documentDetails, int DetailID, string Status, int EntryBy) // insertion on edit of document tab
            {
            var result = await _employeeInformationC.UpdateEmpDocumentDetailsAsync (documentDetails, DetailID, Status, EntryBy);

            if (string.IsNullOrEmpty (result))
                {
                return NotFound ( );
                }

            return Ok (new { Message = result });
            }


        }


    }
