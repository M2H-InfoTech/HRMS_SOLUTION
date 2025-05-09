using LEAVE.Dto;
using LEAVE.Repository.LeaveMaster;
using LEAVE.Service.LeaveMaster;
using Microsoft.AspNetCore.Mvc;

namespace LEAVE.Controllers.LeaveMaster
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Authorize]
    public class LeaveMasterController : ControllerBase
    {
        private readonly ILeaveMasterService _leaveMasterService;
        public LeaveMasterController(ILeaveMasterService leaveMasterService)
        {
            _leaveMasterService = leaveMasterService;

        }
        [HttpGet]
        public async Task<IActionResult> FillLeaveMasterAsync(int SecondEntityId, int EmpId)
        {
            var leaveMaster = await _leaveMasterService.FillLeaveMasterAsync(SecondEntityId, EmpId);
            return Ok(leaveMaster);
        }
        [HttpGet]
        public async Task<IActionResult> FillvaluetypesAsync(string type)
        {

            var fillvaluetypes = await _leaveMasterService.FillvaluetypesAsync(type);
            return Ok(fillvaluetypes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMasterAsync(CreateMasterDto createMasterDto)
        {
            if (createMasterDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var createMaster = await _leaveMasterService.CreateMasterAsync(createMasterDto);
            return Ok(createMaster);
        }
        [HttpGet]
        public async Task<IActionResult> FillbasicsettingsAsync(int Masterid, string TransactionType, int SecondEntityId, int EmpId)
        {

            var fillbasicsetting = await _leaveMasterService.FillbasicsettingsAsync(Masterid, TransactionType, SecondEntityId, EmpId);
            return Ok(fillbasicsetting);
        }
        [HttpGet]
        public async Task<IActionResult> GetEntityApplicableStringsAsync(string transactionType, long masterId)
        {
            var result = await _leaveMasterService.GetEntityApplicableStringsAsync(transactionType, masterId);
            return Ok(new
            {
                result.ApplicableLevelsNew,
                result.ApplicableLevelsOne,
                result.EmpIds,
                result.CompanyIds
            });
        }
        [HttpPost]
        public async Task<IActionResult> ProcessEntityApplicableAsync([FromBody] EntityApplicableApiDto entityApplicableApiDtos)
        {
            if (entityApplicableApiDtos == null)
            {
                return BadRequest("Invalid data.");
            }
            var result = await _leaveMasterService.ProcessEntityApplicableAsync(entityApplicableApiDtos);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetEditLeaveMastersAsync(int masterId)
        {
            var getEditleave = await _leaveMasterService.GetEditLeaveMastersAsync(masterId);
            return Ok(getEditleave);
        }
        [HttpGet]
        public async Task<IActionResult> GetDeleteLeaveMastersAsync(int masterId)
        {
            var getdeleteleave = await _leaveMasterService.GetDeleteLeaveMastersAsync(masterId);
            return Ok(getdeleteleave);
        }
    }
}
