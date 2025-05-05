using HRMS.EmployeeInformation.DTO.DTOs;
using LEAVE.Dto;
using LEAVE.Service.BasicSettings;
using Microsoft.AspNetCore.Mvc;

namespace LEAVE.Controllers.BasicSettings
{
    [Route("api/[controller]/[action]")]
    public class BasicSettingsController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly IBasicSettingsService _basicSettingsService;
        public BasicSettingsController(IBasicSettingsService basicSettingsService)
        {

            _basicSettingsService = basicSettingsService;
        }

        [HttpGet]
        public async Task<IActionResult> Fillvacationaccrual(int basicsettingsid)
        {
            var fillvacationaccrual = await _basicSettingsService.Fillvacationaccrual(basicsettingsid);
            return Ok(fillvacationaccrual);
        }
        [HttpGet]
        public async Task<IActionResult> GetEditbasicsettings(int Masterid)
        {
            var GetEditbasicsettings = await _basicSettingsService.GetEditbasicsettings(Masterid);
            return Ok(GetEditbasicsettings);
        }
        [HttpGet]
        public async Task<IActionResult> saveleavelinktable(int Masterid)
        {
            var saveleavelinktable = await _basicSettingsService.saveleavelinktable(Masterid);
            return Ok(saveleavelinktable);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteConfirm(int Basicsettingsid)
        {
            var deleteConfirm = await _basicSettingsService.DeleteConfirm(Basicsettingsid);
            return Ok(deleteConfirm);
        }
        [HttpGet]
        public async Task<IActionResult> GetDeletebasics(int Basicsettingsid, int Masterid)
        {
            var getDeletebasics = await _basicSettingsService.GetDeletebasics(Basicsettingsid, Masterid);
            return Ok(getDeletebasics);
        }
        [HttpGet]     
        public async Task<IActionResult> Geteditdetails(string entitlement, int masterId, int? experienceId = null)
        {
            var Geteditdetails = await _basicSettingsService.Geteditdetails(entitlement, masterId, experienceId);
            return new JsonResult(Geteditdetails);
        }
        [HttpPost]
        public async Task<IActionResult> Createbasicsettings([FromBody] CreatebasicsettingsDto CreatebasicsettingsDto)
        {
            var Createbasicsettings = await _basicSettingsService.Createbasicsettings(CreatebasicsettingsDto);
            return Ok(Createbasicsettings);
        }
        [HttpGet]
        public async Task<IActionResult> FillleavetypeListAsync( int SecondEntityId, int Empid)
        {
            var fillleavetypeList = await _basicSettingsService.FillleavetypeListAsync(SecondEntityId, Empid);
            return new JsonResult(fillleavetypeList);
        }
    }
}
