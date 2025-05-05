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

        public async Task<List<BsemployeedataDto>> Bsemployeedata(int employeeId)
        {
            var firstQuery = from a in _context.HrmLeaveBasicsettingsaccesses
                             join b in _context.HrmLeaveBasicSettings on a.SettingsId equals b.SettingsId into ab
                             from b in ab.DefaultIfEmpty()
                             join c in _context.EmployeeDetails on a.EmployeeId equals c.EmpId into ac
                             from c in ac.DefaultIfEmpty()
                             where a.EmployeeId == employeeId && b.SettingsDescription != null
                             select new BsemployeedataDto
                             {
                                 EmployeeId = (int)a.EmployeeId,
                                 SettingsId = (int)a.SettingsId,
                                 SettingsDescription = b.SettingsDescription,
                                 Name = c.Name
                             };

            var secondQuery = from a in _context.HrmLeaveEmployeeleaveaccesses
                              join b in _context.HrmLeaveMasters on a.LeaveMaster equals b.LeaveMasterId into ab
                              from b in ab.DefaultIfEmpty()
                              join c in _context.EmployeeDetails on a.EmployeeId equals c.EmpId into ac
                              from c in ac.DefaultIfEmpty()
                              where a.EmployeeId == employeeId && b.Description != null && a.ValidTo == null
                              select new BsemployeedataDto
                              {
                                  EmployeeId = (int)a.EmployeeId,
                                  SettingsId = (int)a.LeaveMaster,
                                  SettingsDescription = b.Description,
                                  Name = c.Name
                              };

            var result = await firstQuery.Union(secondQuery).ToListAsync();

            return result;
        }

        public async Task<List<FillchildBSdetailsDto>> FillchildBSdetails(int employeeId)
        {
            var firstQuery = from a in _context.HrmLeaveBasicsettingsaccesses
                             join b in _context.HrmLeaveBasicSettings
                             on a.SettingsId equals b.SettingsId
                             where a.EmployeeId == employeeId && a.ValidToBs == null
                             select new
                             {
                                 a.SettingsId,
                                 b.SettingsName,
                                 b.SettingsDescription,
                                 a.AssignPeriodBs,
                                 a.FromDateBs
                             };

            var secondQuery = from a in _context.HrmLeaveEmployeeleaveaccesses
                              join b in _context.HrmLeaveMasters
                              on a.LeaveMaster equals b.LeaveMasterId
                              where a.EmployeeId == employeeId && a.ValidTo == null
                              select new
                              {
                                  SettingsId = a.LeaveMaster,
                                  SettingsName = b.LeaveCode,
                                  SettingsDescription = b.Description,
                                  AssignPeriodBs = a.AssignPeriod,
                                  FromDateBs = a.FromDate
                              };

            var unionQuery = firstQuery.Union(secondQuery);

            var result = await unionQuery.Select(x => new FillchildBSdetailsDto
            {
                SettingsId = x.SettingsId ?? 0,
                SettingsName = x.SettingsName,
                SettingsDescription = x.SettingsDescription,
                AssignPeriodBs = x.AssignPeriodBs ?? 0,
                FromDate = x.FromDateBs.HasValue
                    ? x.FromDateBs.Value.ToString("dd/MM/yyyy")
                    : null
            }).ToListAsync();

            return result;
        }

        public async Task<object> Getallbasics(string linkid, int levelid, string transaction, int empid)
        {
            int? rotatingLinkId = null;
            int? levelOneId = null;
            List<int> settingsIds1 = new List<int>();
            List<int> settingsIds2 = new List<int>();

            string totalLinkId = string.Empty;
            object result = null;

            int? transactionId = _context.TransactionMasters
                .Where(t => t.TransactionType == transaction)
                .Select(t => (int?)t.TransactionId)
                .FirstOrDefault();

            if (levelid == 0 && empid == 0)
            {

                var entityList = linkid.Split(',').Select(int.Parse).ToList();

                var masterIds = _context.EntityApplicable00s
                    .Where(e =>
                        (e.LinkLevel == 1 && e.LinkId.HasValue && entityList.Contains((int)e.LinkId.Value) && e.TransactionId == transactionId) ||
                        (e.LinkLevel == 15 && e.TransactionId == transactionId))
                    .Select(e => e.MasterId)
                    .ToList();

                result = await _context.HrmLeaveBasicSettings
                    .Where(s => masterIds.Contains(s.SettingsId))
                    .Select(s => new
                    {
                        s.SettingsId,
                        SettingsCode = s.SettingsDescription + "[" + s.SettingsName + "]",
                        s.LeaveMasterId
                    })
                    .ToListAsync();
            }

            else
            {
                int? count;

                if (empid == 0)
                {
                    int parsedLinkId = int.Parse(linkid);
                    rotatingLinkId = _context.SubCategoryLinksNews
                        .Where(s => s.LinkId == parsedLinkId)
                        .Select(s => (int?)s.Root)
                        .FirstOrDefault();

                    if (rotatingLinkId == 0 || rotatingLinkId == null)
                    {
                         parsedLinkId = int.Parse(linkid);
                        levelOneId = _context.SubCategoryLinksNews
                            .Where(s => s.LinkId == parsedLinkId)
                            .Select(s => (int?)s.LinkableSubcategory)
                            .FirstOrDefault();

                        totalLinkId = linkid + ",";
                    }
                    else
                    {
                        totalLinkId = $"{linkid},{rotatingLinkId},";
                    }
                     parsedLinkId = int.Parse(linkid); // Consider using int.TryParse for safety
                    count = _context.SubCategoryLinksNews
                        .Where(s => s.LinkId == parsedLinkId)
                        .Select(s => (int?)s.LinkLevel)
                        .FirstOrDefault() - 2;

                  
                }
                else
                {

                    int? lastEntityId = _context.HrEmpMasters
                        .Where(s => s.EmpId == empid)
                        .Select(s => (int?)s.LastEntity)
                        .FirstOrDefault();

                    rotatingLinkId = _context.SubCategoryLinksNews
                        .Where(s => s.LinkId == lastEntityId)
                        .Select(s => (int?)s.Root)
                        .FirstOrDefault();

                    totalLinkId = $"{lastEntityId},{rotatingLinkId},";

                    count = _context.LicensedCompanyDetails
                        .Select(s => (int?)s.EntityLimit)
                        .FirstOrDefault() - 2;
                }

                while (count > 0)
                {
                    if (count == 1)
                    {
                        levelOneId = _context.SubCategoryLinksNews
                            .Where(s => s.Root == 0 && s.LinkId == rotatingLinkId)
                            .Select(s => (int?)s.LinkableSubcategory)
                            .FirstOrDefault();
                    }
                    else
                    {
                        rotatingLinkId = _context.SubCategoryLinksNews
                            .Where(s => s.LinkId == rotatingLinkId)
                            .Select(s => (int?)s.Root)
                            .FirstOrDefault();

                        if (rotatingLinkId != null)
                        {
                            totalLinkId += $"{rotatingLinkId},";
                        }
                    }
                    count--;
                }

                if (empid > 0)
                {
                    var totalLinkIdList = totalLinkId
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToList();

                    var masterIds = _context.EntityApplicable00s
                        .Where(e =>
                            (e.LinkLevel == 1 && e.LinkId == levelOneId && e.TransactionId == transactionId) ||
                            (e.LinkLevel == 15 && e.TransactionId == transactionId) ||
                            (e.LinkLevel != 1 && e.LinkId.HasValue && totalLinkIdList.Contains((int)e.LinkId.Value) && e.TransactionId == transactionId))
                        .Select(e => e.MasterId)
                        .ToList();

                    settingsIds1 = _context.HrmLeaveBasicSettings
                                      .Where(s => masterIds.Contains(s.SettingsId))
                                      .Select(s => s.SettingsId)
                                       .ToList();

                    settingsIds2 = _context.EntityApplicable01s
                                    .Where(s => s.TransactionId == transactionId && s.EmpId == empid && s.MasterId.HasValue)
                                    .Select(s => (int)s.MasterId.Value)
                                    .ToList();



                    result = await _context.HrmLeaveBasicSettings
                            .Where(s => settingsIds1.Contains(s.SettingsId) || settingsIds2.Contains(s.SettingsId))
                            .Select(s => new
                            {
                                s.SettingsId,
                                SettingsCode = s.SettingsDescription + "[" + s.SettingsName + "]",
                                s.LeaveMasterId
                            })
                                .ToListAsync();

                }
                else
                {
                    result = await _context.HrmLeaveBasicSettings
                            .Where(s => settingsIds1.Contains(s.SettingsId))
                            .Select(s => new
                            {
                                s.SettingsId,
                                SettingsCode = s.SettingsDescription + "[" + s.SettingsName + "]",
                                s.LeaveMasterId
                            })
                                .ToListAsync();

                }
            }

            return result;
        }

    }
}
