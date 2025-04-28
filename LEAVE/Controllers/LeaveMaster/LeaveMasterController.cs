using LEAVE.Service.LeaveMaster;
using Microsoft.AspNetCore.Mvc;

namespace LEAVE.Controllers.LeaveMaster
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LeaveMasterController : Controller
    {
        private readonly ILeaveMasterService _leaveMasterService;

        public LeaveMasterController(ILeaveMasterService leaveMasterService)
        {
            _leaveMasterService = leaveMasterService;
        }
        //[HttpGet]

        //public IActionResult Index()
        //{
        //    return Ok();
        //}
        [HttpGet]
        public async Task<IActionResult> FillLeaveMasterAsync(int SecondEntityId, int EmpId)
        {
            var leaveMaster = await _leaveMasterService.FillLeaveMasterAsync(SecondEntityId, EmpId);
            return Ok(leaveMaster);
        }
    }
}
