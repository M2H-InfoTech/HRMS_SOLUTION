using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Service.InterfaceB;
using Microsoft.AspNetCore.Mvc;

namespace EMPLOYEE_INFORMATION.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeeBController : Controller
    {
        private readonly IEmployeeInformationServiceB _employeeInformationB;
        public EmployeeBController(IEmployeeInformationServiceB employeeInformationB)
        {
            _employeeInformationB = employeeInformationB;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> FillDocumentType(int EmpID)    //dropdown in document add button
        {
            var FillDocumentType = await _employeeInformationB.FillDocumentType(EmpID);
            return new JsonResult(FillDocumentType);
        }
        [HttpGet]
        public async Task<IActionResult> DocumentField(int DocumentID)   //textbox field name inside document add button
        {
            var DocumentField = await _employeeInformationB.DocumentField(DocumentID);
            return new JsonResult(DocumentField);
        }
        [HttpGet]
        public async Task<IActionResult> DocumentGetGeneralSubCategoryList(string Remarks)   // "bank name" drop down inside document add button
        {
            var DocumentGetGeneralSubCategoryList = await _employeeInformationB.DocumentGetGeneralSubCategoryList(Remarks);
            return new JsonResult(DocumentGetGeneralSubCategoryList);
        }


        [HttpPost]
        public async Task<IActionResult> InsertDocumentsFieldDetails([FromBody] List<TmpDocFileUpDto> DocumentBankField, int DocumentID, int In_EntryBy)   //InsertOrUpdate document & bank
        {
            var FieldDetails = await _employeeInformationB.InsertDocumentsFieldDetails(DocumentBankField, DocumentID, In_EntryBy);

            if (FieldDetails == null || !FieldDetails.Any())
            {
                return NotFound();
            }

            return Ok(FieldDetails);
        }

        [HttpPost]
        public async Task<IActionResult> SetEmpDocuments([FromBody] TmpFileUpDto DocumentBankField, int DetailID, string Status, int In_EntryBy)   //InsertOrUpdate document & bank upload file
        {
            var SetEmpDocuments = await _employeeInformationB.SetEmpDocuments(DocumentBankField, DetailID, Status, In_EntryBy);

            if (SetEmpDocuments == null || !SetEmpDocuments.Any())
            {
                return NotFound();
            }

            return Ok(SetEmpDocuments);
        }


    }
}
