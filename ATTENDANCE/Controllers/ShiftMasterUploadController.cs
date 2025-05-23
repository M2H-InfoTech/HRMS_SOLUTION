using ATTENDANCE.Service.ShiftMasterUpload;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ATTENDANCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftMasterUploadController (IShiftMasterUploadService _shiftmasterService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetMasterShiftUploadDetails()
        {
            var result = await _shiftmasterService.GetMasterShiftUploadDetails( );
            return Ok(result);
        }
    }
}
