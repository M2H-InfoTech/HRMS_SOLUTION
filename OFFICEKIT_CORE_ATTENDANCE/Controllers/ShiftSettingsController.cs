using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendace.Data;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Response;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Interfaces;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace OFFICEKIT_CORE_ATTENDANCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftSettingsController(IAttendanceLogService attendanceLogService,IShiftSettingsService shiftSettingsService) : ControllerBase
    {


        [HttpPost]
        public async Task<IActionResult> GetShiftAccessDetails(
            int shiftAccessId = 0,
            int entryBy = 19,
            int roleId = 3,
            int status = 1,
            int empStatus = 1,
            int pageNumber = 1,
            int pageSize = 50)
        {
            var result = await attendanceLogService.GetShiftAccessDetails(shiftAccessId, entryBy, roleId, status, empStatus, pageNumber, pageSize);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> FillShiftDayType()
        {
            var result = await shiftSettingsService.FillShiftDayType();
            return Ok(result);
        }

    }
}
