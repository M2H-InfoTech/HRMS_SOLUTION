using Microsoft.AspNetCore.Mvc;
using OFFICEKITCORELEAVE.OfficeKit.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.Repository;

namespace OFFICEKITCORELEAVE.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveMasterController : ControllerBase
    {
        private readonly ILeaveRepository _leaveMasterService;
        public LeaveMasterController(ILeaveRepository leaveMasterService_)
        {
            _leaveMasterService = leaveMasterService_;
        }

        [HttpPost("SaveLeaveMaster")]
        public async Task<IActionResult> SaveLeaveMaster(HrmLeaveMasterDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Please provide data");
            }
            if (await _leaveMasterService.Checkexistance(dto.LeaveCode))
            {
                return BadRequest("A leave master with this code already exists");
            }
            try
            {
                var result = await _leaveMasterService.SaveLeaveMaster(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to save leave master");
            }
        }
        [HttpGet("GetLeaveMasterById")]
        public async Task<IActionResult> GetLeaveMasterById(int LeaveMasterId)
        {
            if (LeaveMasterId == 0)
            {
                return BadRequest("Please Provide a Id");
            }
            try
            {
                var result = await _leaveMasterService.GetLeaveMasterById(LeaveMasterId);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpPost("GetAllLeaveMasters")]
        public async Task<IActionResult> GetAllLeaveMasters(HrmLeaveMasterSearchDto sortDto)
        {
            var result = await _leaveMasterService.GetAllLeaveMasters(sortDto);
            if (result == null)
            {
                return BadRequest("No Data To Show");
            }
            return Ok(result);
        }
        [HttpDelete("DeleteLeaveMaster")]
        public async Task<IActionResult> DeleteLeaveMaster(int LeaveMasterId)
        {
            if (LeaveMasterId == 0)
            {
                return BadRequest("Please Provide A Id");
            }
            var result = await _leaveMasterService.DeleteLeaveMaster(LeaveMasterId);
            return Ok(result);
        }
        [HttpPost("EntityApplicableSave")]
        public async Task<IActionResult> EntityApplicableSave(SaveApplicableParameters Dto)
        {
            var result = await _leaveMasterService.EntityApplicableSave(Dto);
            return Ok(result);
        }
    }
}
