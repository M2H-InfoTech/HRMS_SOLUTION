using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.Interface.HrmLeaveBasicSettingInterface;

namespace OFFICEKITCORELEAVE.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class HrmLeaveBasicSettingController : ControllerBase
    {
        private readonly ILeaveBasicSettingService _hrmLeaveBasicSettingService;
        public HrmLeaveBasicSettingController (ILeaveBasicSettingService hrmLeaveBasicSettingService_)
        {
            _hrmLeaveBasicSettingService = hrmLeaveBasicSettingService_;
        }

        [HttpPost]
        public async Task<IActionResult> SaveLeaveBasicSetting (HrmLeaveBasicSettingDto leaveSettingDto)
        {
            if (leaveSettingDto == null)
            {
                return BadRequest ("Please provide data.");
            }

            try
            {
                var result = await _hrmLeaveBasicSettingService.SaveLeaveBasicSetting (leaveSettingDto);
                return Ok (result);
            }
            catch (Exception)
            {
                return StatusCode (500, "An error occurred while saving the settings.");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetLeaveBasicSettingById (int leavebasicSettingsId)
        {
            if (leavebasicSettingsId == 0)
            {
                return BadRequest ("Please provide a Id");
            }
            var result =await _hrmLeaveBasicSettingService.GetLeaveBasicSettingById (leavebasicSettingsId);
            return Ok (result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteLeaveBasicSetting (int leavebasicSettingsId)
        {
            if (leavebasicSettingsId == 0)
            {
                return BadRequest ("Please provide a Id");
            }
            var result =await _hrmLeaveBasicSettingService.DeleteLeaveBasicSetting (leavebasicSettingsId);
            return Ok (result);
        }

        [HttpGet("GetAllLeaveBasicSettings")]
        public async Task<IActionResult>  GetAllLeaveBasicSettings ( )
        {
            var result =await _hrmLeaveBasicSettingService.GetAllLeaveBasicSettings ();
            return Ok (result);
        }
    }
}
