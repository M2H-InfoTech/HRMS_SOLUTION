using LEAVE.Dto;
using LEAVE.Service.AssignLeave;
using Microsoft.AspNetCore.Mvc;

namespace LEAVE.Controllers.AssignLeave
{
    [Route("api/[controller]/[action]")]
    public class AssignLeaveController : Controller
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
       
    }
}
