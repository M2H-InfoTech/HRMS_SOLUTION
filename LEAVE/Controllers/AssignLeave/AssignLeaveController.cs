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
        [HttpGet]//Created by Shan Lal K
        public async Task<IActionResult> GetBasicAssignmentAsync (int roleId, int entryBy)
        {
            var result = await _assignleaveservice.GetBasicAssignmentAsync (roleId, entryBy);
            return Ok (result);
        }
        [HttpDelete]//Created by Shan Lal K
        public async Task<IActionResult> DeleteSingleEmpBasicSettingAsync (int leavemasters, int empid)
        {
            if (leavemasters <= 0 || empid <= 0)
                return BadRequest ("Invalid leave master ID or employee ID.");
            try
            {
                bool result = await _assignleaveservice.DeleteSingleEmpBasicSettingAsync (leavemasters, empid);

                if (result)
                    return Ok ("Deleted Successfully");
                else
                    return NotFound ("No matching record found to delete.");
            }
            catch (Exception ex)
            {
                return StatusCode (500, "An error occurred while processing your request.");
            }
        }
        [HttpPost]//Created by Shan Lal K
        public async Task<IActionResult> AssignBasicsAsync ([FromBody]LeaveAssignSaveDto Dto)
        {
            if (Dto == null)
            {
                return BadRequest ("Leave assignment data cannot be null.");
            }
            var result = await _assignleaveservice.AssignBasicsAsync (Dto);
            return Ok (result);
        }

    }
}
