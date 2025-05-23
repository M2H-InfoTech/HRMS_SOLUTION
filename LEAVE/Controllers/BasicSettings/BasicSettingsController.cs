﻿using LEAVE.Dto;
using LEAVE.Service.BasicSettings;
using Microsoft.AspNetCore.Mvc;

namespace LEAVE.Controllers.BasicSettings
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Authorize]
    public class BasicSettingsController : ControllerBase
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
        public async Task<IActionResult> saveleavelinktable(int masterId, int basicSettingsId, int createdBy)
        {
            var saveleavelinktable = await _basicSettingsService.saveleavelinktable(masterId, basicSettingsId, createdBy);
            return Ok(saveleavelinktable);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteConfirm(int Basicsettingsid)
        {
            var deleteConfirm = await _basicSettingsService.DeleteConfirm(Basicsettingsid);
            return Ok(deleteConfirm);
        }
        [HttpGet]
        public async Task<IActionResult> GetDeletebasics(int Basicsettingsid, int Masterid, string transactionType)
        {
            var getDeletebasics = await _basicSettingsService.GetDeletebasics(Basicsettingsid, Masterid, transactionType);
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
        public async Task<IActionResult> FillleavetypeListAsync(int SecondEntityId, int Empid)
        {
            var fillleavetypeList = await _basicSettingsService.FillleavetypeListAsync(SecondEntityId, Empid);
            return new JsonResult(fillleavetypeList);
        }

        [HttpGet("GetEditbasicsettingsAsync")]
        public async Task<IActionResult> GetEditbasicsettingsAsync(int masterid)
        {
            var fillleavetypeListss = await _basicSettingsService.GetEditbasicsettingsAsync(masterid);
            return new JsonResult(fillleavetypeListss);
        }
        //[HttpGet]
        //public async Task<IActionResult> GetEditbasicsettingsAsync(int masterid)
        //{
        //    var getEditbasicsettings = await _basicSettingsService.getEditbasicsettings(masterid);
        //    return new JsonResult(getEditbasicsettings);
        //}
        //[HttpGet]
        //public async Task<IActionResult> UpdatetLeaveMasterAndSettingsLinkAsync(int masterId, int basicSettingsId, int createdBy)
        //{
        //    var updatetLeaveMasterAndSettingsLink = await _basicSettingsService.UpdatetLeaveMasterAndSettingsLinkAsync(masterId, basicSettingsId, createdBy);
        //    return new JsonResult(updatetLeaveMasterAndSettingsLink);
        //}
        [HttpPost]
        public async Task<IActionResult> UpdateLeaveMasterAndSettingsLink([FromBody] LeaveEntitlementDto leaveEntitlementDto)
        {
            var upsertLeaveMasterAndSettingsLink = await _basicSettingsService.UpdateLeaveMasterAndSettingsLinkAsync(leaveEntitlementDto);
            return new JsonResult(upsertLeaveMasterAndSettingsLink);
        }
    }
}
