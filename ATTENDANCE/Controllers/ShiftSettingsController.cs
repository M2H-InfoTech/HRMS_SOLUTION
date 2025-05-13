using ATTENDANCE.DTO.Request;
using ATTENDANCE.Service.ShiftSettings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ATTENDANCE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShiftSettingsController(IShiftSettingsService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetShiftDayTypes()
        {
            var result = await service.GetShiftDayTypesAsync();
            return Ok(result);
        }
        //[HttpGet]
        //public async Task<IActionResult> ViewShiftDetails(int shiftId, int entryBy, int roleId)
        //{
        //    var result = await service.ViewAllshiftAsync(shiftId, entryBy, roleId);
        //    return Ok(result);
        //}
        [HttpPost]
        public async Task<IActionResult> CreateShiftAsync([FromBody] ShiftInsertRequestDto shiftSettingsDto)
        {
            var result = await service.CreateSplitShiftAsync(shiftSettingsDto);


            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNormalShiftAsync([FromBody] ShiftInsertRequestDto shiftSettingsDto)
        {
            var result = await service.CreateNormalShiftAsync(shiftSettingsDto);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOpenShiftAsync([FromBody] ShiftInsertRequestDto shiftSettingsDto)
        {
            var result = await service.CreateOpenShiftAsync(shiftSettingsDto);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOpenShiftAsync([FromBody] ShiftInsertRequestDto shiftSettingsDto)
        {
            var result = await service.UpdateOpenShiftAsync(shiftSettingsDto);
            if (result.ErrorID == 0)
            {
                return Ok(result.ErrorMessage);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertShiftNormalSeasonAsync([FromBody] ShiftInsertRequestDto shiftSettingsDto)
        {
            var result = await service.InsertShiftNormalSeasonAsync(shiftSettingsDto);
            return Ok(result);

        }

    }
}