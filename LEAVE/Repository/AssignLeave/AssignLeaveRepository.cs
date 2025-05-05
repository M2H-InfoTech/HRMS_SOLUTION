using EMPLOYEE_INFORMATION.Data;
using HRMS.EmployeeInformation.Models;
using LEAVE.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LEAVE.Repository.AssignLeave
{
    public class AssignLeaveRepository : IAssignLeaveRepository
    {
        private readonly EmployeeDBContext _context;
        public AssignLeaveRepository(EmployeeDBContext context)
        {
            _context = context;
        }

        public async Task<int> GetconfirmBsInsert(GetconfirmBsInsert GetconfirmBsInsert)
        {
            var settingIds = GetconfirmBsInsert.leavemasters.Split(',').Select(int.Parse).ToList();
            var employeeIds = GetconfirmBsInsert.employeeids.Split(',').Select(int.Parse).ToList();


            var leaveIds = await _context.HrmLeaveMasterandsettingsLinks
                .Where(x => settingIds.Contains((int)x.SettingsId))
                .Select(x => x.LeaveMasterId)
                .Distinct()
                .ToListAsync();

 
            var updateFirstSet = await _context.HrmLeaveBasicsettingsaccesses
                .Where(x =>
                    settingIds.Contains((int)x.SettingsId) &&
                    employeeIds.Contains((int)x.EmployeeId) &&
                    x.ValidToBs == null &&
                    (
                        (x.Laps.HasValue && x.Laps.Value > 0 && x.FromDateBs != null &&
                            GetconfirmBsInsert.fromDate > x.FromDateBs.Value.AddMonths((int)(12 / x.Laps.Value))) ||
                        ((!x.Laps.HasValue || x.Laps.Value == 0) && x.FromDateBs != null &&
                            GetconfirmBsInsert.fromDate > x.FromDateBs.Value.AddDays(1))
                    ))
                .ToListAsync();

           
            foreach (var item in updateFirstSet)
            {
                item.ValidToBs = GetconfirmBsInsert.fromDate.AddDays(-1);
            }

            await _context.SaveChangesAsync();

            var updateSecondSet = await _context.HrmLeaveBasicsettingsaccesses
                .Where(x =>
                    !settingIds.Contains((int)x.SettingsId) &&
                    leaveIds.Contains((int)x.LeaveMasterId) &&
                    employeeIds.Contains((int)x.EmployeeId) &&
                    x.FromDateBs >= GetconfirmBsInsert.fromDate)
                .ToListAsync();

            foreach (var item in updateSecondSet)
            {
                item.ValidToBs = GetconfirmBsInsert.fromDate.AddDays(-1);
            }

            await _context.SaveChangesAsync();

            var links = await _context.HrmLeaveMasterandsettingsLinks
                .Where(x => settingIds.Contains((int)x.SettingsId))
                .ToListAsync();

            var entitlements = await _context.HrmLeaveEntitlementHeads
                .Where(x => settingIds.Contains((int)x.SettingsId))
                .ToListAsync();

            var existingAccess = await _context.HrmLeaveBasicsettingsaccesses
                .Where(x => employeeIds.Contains((int)x.EmployeeId) && settingIds.Contains((int)x.SettingsId) && x.ValidToBs == null)
                .ToListAsync();

            var newAccessRecords = new List<HrmLeaveBasicsettingsaccess>();

            foreach (var empId in employeeIds)
            {
                foreach (var link in links)
                {
                    if (!existingAccess.Any(x => x.EmployeeId == empId && x.SettingsId == link.SettingsId))
                    {
                        var ent = entitlements.FirstOrDefault(e => e.SettingsId == link.SettingsId);

                        newAccessRecords.Add(new HrmLeaveBasicsettingsaccess
                        {
                            EmployeeId = empId,
                            SettingsId = link.SettingsId,
                            LeaveMasterId = link.LeaveMasterId,
                            IsCompanyLevel = 1,
                            CreatedBy = GetconfirmBsInsert.entryBy,
                            CreatedDate = DateTime.UtcNow,
                            LinkLevel = GetconfirmBsInsert.linkLevel,
                            FromDateBs = GetconfirmBsInsert.fromDate,
                            ValidToBs = GetconfirmBsInsert.validTo,
                            Laps = ent?.Laps ?? 0
                        });
                    }
                }
            }

            if (newAccessRecords.Any())
            {
                _context.HrmLeaveBasicsettingsaccesses.AddRange(newAccessRecords);
                await _context.SaveChangesAsync();
            }

            return newAccessRecords.LastOrDefault()?.LeaveMasterId ?? 0;
        }

    }
}
