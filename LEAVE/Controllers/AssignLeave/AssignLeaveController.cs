using LEAVE.Dto;
using LEAVE.Service.AssignLeave;
using Microsoft.AspNetCore.Mvc;

namespace LEAVE.Controllers.AssignLeave
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    // [Authorize]
    public class AssignLeaveController : ControllerBase
    {
        private readonly IAssignLeaveService _assignleaveservice;
        public AssignLeaveController(IAssignLeaveService assignleaveservice)
        {
            _assignleaveservice = assignleaveservice;
        }
        [HttpPost]
        public async Task<IActionResult> GetconfirmBsInsert([FromBody] GetconfirmBsInsert GetconfirmBsInsert)
        {
            var GetconfirmInsert = await _assignleaveservice.GetconfirmBsInsert(GetconfirmBsInsert);
            return Ok(GetconfirmBsInsert);
        }
        [HttpGet]
        public async Task<IActionResult> Bsemployeedata(int employeeId)
        {
            var bsemployeedata = await _assignleaveservice.Bsemployeedata(employeeId);
            return Ok(bsemployeedata);
        }
        [HttpGet]
        public async Task<IActionResult> FillchildBSdetails(int employeeId)
        {
            var fillchildBSdetails = await _assignleaveservice.FillchildBSdetails(employeeId);
            return Ok(fillchildBSdetails);
        }
        [HttpGet]
        public async Task<IActionResult> Getallbasics(string linkid, int levelid, string transaction, int empid)
        {
            var getallbasics = await _assignleaveservice.Getallbasics(linkid, levelid, transaction, empid);
            return Ok(getallbasics);
        }

    }
}
