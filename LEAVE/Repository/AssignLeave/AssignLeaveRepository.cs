using EMPLOYEE_INFORMATION.Data;
using HRMS.EmployeeInformation.Models;
using LEAVE.Dto;
using Microsoft.EntityFrameworkCore;
using MPLOYEE_INFORMATION.DTO.DTOs;
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
        public async Task<Object> GetBasicAssignmentAsync (int roleId, int entryBy)
        {

            var newEmpId = await _context.HrEmployeeUserRelations
                .Where (x => x.UserId == entryBy)
                .Select (x => x.EmpId)
            .FirstOrDefaultAsync ( );

            var lnklev = await _context.SpecialAccessRights
                .Where (x => x.RoleId == roleId)
                .Select (x => x.LinkLevel)
            .FirstOrDefaultAsync ( );

            var hasLevel15 = _context.EntityAccessRights02s
                .Where (s => s.RoleId == roleId && s.LinkLevel == 15).ToList ( )
                .SelectMany (s => SplitStrings_XML (s.LinkId))
                .Any ( );

            if (hasLevel15)
            {
                var result = await (from a in _context.EmployeeDetails
                                    join b in _context.HighLevelViewTables on a.LastEntity equals b.LastEntityId into joined
                                    from b in joined.DefaultIfEmpty ( )
                                    where _context.HrmLeaveBasicsettingsaccesses
                                        .Select (x => x.EmployeeId).Contains (a.EmpId)
                                    select new
                                    {
                                        EmpId = a.EmpId,
                                        EmpCode = a.EmpCode,
                                        Name = a.Name,
                                        LevelOneDescription = b.LevelOneDescription,
                                        LevelTwoDescription = b.LevelTwoDescription,
                                        LevelThreeDescription = b.LevelThreeDescription
                                    }).ToListAsync ( );
                return result;
            }
            else
            {
                var empEntity = await _context.HrEmpMasters
                    .Where (x => x.EmpId == newEmpId)
                    .Select (x => x.EmpEntity)
                    .FirstOrDefaultAsync ( );

                var ctnew = SplitStrings_XML (empEntity)
               .Select ((item, index) => new LinkItemDto { Item = item, LinkLevel = index + 2 })
               .ToList ( );

                var applicableFinal = _context.EntityAccessRights02s
                    .Where (s => s.RoleId == roleId).ToList ( )
                    .SelectMany (s => SplitStrings_XML (s.LinkId).Select (x => new { item = x, LinkLevel = s.LinkLevel }));

                if (lnklev > 0)
                {
                    var applicableFinal1 = _context.EntityAccessRights02s
               .Where (s => !string.IsNullOrEmpty (s.LinkId)).ToList ( )
               .SelectMany (s => SplitStrings_XML (s.LinkId),
                   (s, item) => new LinkItemDto { Item = item, LinkLevel = s.LinkLevel });
                }
                var empdetails = await _context.EmployeeDetails.ToListAsync ( );
                var HrmLeaveBasicsettingsaccesses = await _context.HrmLeaveBasicsettingsaccesses.ToListAsync ( );
                var HrmLeaveEmployeeleaveaccesses = await _context.HrmLeaveEmployeeleaveaccesses.ToListAsync ( );
                var HighLevelViewTables = await _context.HighLevelViewTables.ToListAsync ( );
                var result = (from d in empdetails
                              join e in HrmLeaveBasicsettingsaccesses on d.EmpId equals e.EmployeeId
                              join f in HrmLeaveEmployeeleaveaccesses on d.EmpId equals f.EmployeeId into tempF
                              from f in tempF.DefaultIfEmpty ( )
                              join a in HighLevelViewTables on d.LastEntity equals a.LastEntityId
                              join b in applicableFinal on 1 equals 1
                              where (a.LevelOneId == int.Parse (b.item) && b.LinkLevel == 1)
                                 || (a.LevelTwoId == int.Parse (b.item) && b.LinkLevel == 2)
                                 || (a.LevelThreeId == int.Parse (b.item) && b.LinkLevel == 3)
                                 || (a.LevelFourId == int.Parse (b.item) && b.LinkLevel == 4)
                                 || (a.LevelFiveId == int.Parse (b.item) && b.LinkLevel == 5)
                                 || (a.LevelSixId == int.Parse (b.item) && b.LinkLevel == 6)
                                 || (a.LevelSevenId == int.Parse (b.item) && b.LinkLevel == 7)
                                 || (a.LevelEightId == int.Parse (b.item) && b.LinkLevel == 8)
                                 || (a.LevelNineId == int.Parse (b.item) && b.LinkLevel == 9)
                                 || (a.LevelTenId == int.Parse (b.item) && b.LinkLevel == 10)
                                 || (a.LevelElevenId == int.Parse (b.item) && b.LinkLevel == 11)
                                 || (a.LevelTwelveId == int.Parse (b.item) && b.LinkLevel == 12)
                              select new
                              {
                                  EmpId = d.EmpId,
                                  EmpCode = d.EmpCode,
                                  Name = d.Name,
                                  LevelOneDescription = a.LevelOneDescription,
                                  LevelTwoDescription = a.LevelTwoDescription,
                                  LevelThreeDescription = a.LevelThreeDescription
                              }).Distinct ( );

                return result;
            }
        }

        public async Task<bool> DeleteSingleEmpBasicSettingAsync (int leavemasters, int empid)
        {
            try
            {
                var recordsToDelete = await _context.HrmLeaveBasicsettingsaccesses
                    .Where (x => x.SettingsId == leavemasters && x.EmployeeId == empid)
                    .ToListAsync ( );

                if (recordsToDelete.Any ( ))
                {
                    _context.HrmLeaveBasicsettingsaccesses.RemoveRange (recordsToDelete);
                    await _context.SaveChangesAsync ( );
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<int> AssignBasicsAsync (LeaveAssignSaveDto dto)
        {
            var leaveMasterIds = SplitToIntList (dto.LeaveMasters);
            var employeeIds = SplitToIntList (dto.EmployeeIds);
            await using var transaction = await _context.Database.BeginTransactionAsync ( );
            try
            {
                // Get LeaveMaster IDs associated with the settings
                var leaveIds = await _context.HrmLeaveMasterandsettingsLinks
                    .Where (x => leaveMasterIds.Contains ((int)x.SettingsId))
                    .Select (x => x.LeaveMasterId.ToString ( ))
                    .ToListAsync ( );

                // Check if valid basic settings already exist
                var existsBasicSettings = await _context.HrmLeaveBasicsettingsaccesses.AnyAsync (x =>
                    leaveMasterIds.Contains ((int)x.SettingsId) &&
                    employeeIds.Contains ((int)x.EmployeeId) &&
                    x.FromDateBs >= dto.FromDate);

                if (existsBasicSettings)
                    return 0;

                // Check for conflicting leave settings
                var conflictingExists = await _context.HrmLeaveBasicsettingsaccesses.AnyAsync (x =>
                    !leaveMasterIds.Contains ((int)x.SettingsId) &&
                    leaveIds.Contains (x.LeaveMasterId.ToString ( )) &&
                    employeeIds.Contains ((int)x.EmployeeId) &&
                    x.FromDateBs >= dto.FromDate);

                if (conflictingExists)
                    return -5;

                // Delete expired valid settings
                var expiredSettings = await _context.HrmLeaveBasicsettingsaccesses
                    .Where (x =>
                        leaveMasterIds.Contains ((int)x.SettingsId) &&
                        leaveIds.Contains (x.LeaveMasterId.ToString ( )) &&
                        employeeIds.Contains ((int)x.EmployeeId) &&
                        x.ValidToBs != null)
                    .ToListAsync ( );

                _context.HrmLeaveBasicsettingsaccesses.RemoveRange (expiredSettings);

                // Update ValidToBs for overlapping current entries
                var candidates = await _context.HrmLeaveBasicsettingsaccesses
                    .Where (x =>
                        leaveMasterIds.Contains ((int)x.SettingsId) &&
                        employeeIds.Contains ((int)x.EmployeeId) &&
                        x.ValidToBs == null)
                    .ToListAsync ( );

                foreach (var record in candidates)
                {
                    var limit = record.Laps > 0 && record.FromDateBs.HasValue
                        ? record.FromDateBs.Value.AddMonths ((int)(12 / record.Laps))
                        : dto.FromDate.AddDays (1);

                    if (dto.FromDate > limit)
                        record.ValidToBs = dto.FromDate.AddDays (-1);
                }

                // Delete subordinate-level records
                var subDelete = await _context.HrmLeaveBasicsettingsaccesses
                    .Where (x =>
                        employeeIds.Contains ((int)x.EmployeeId) &&
                        x.LinkLevel < dto.LinkLevel &&
                        dto.FromDate > (
                            x.Laps > 0
                                ? x.FromDateBs.HasValue
                                    ? x.FromDateBs.Value.AddMonths ((int)(12 / x.Laps))
                                    : DateTime.MaxValue
                                : dto.FromDate.AddDays (1)))
                    .ToListAsync ( );

                _context.HrmLeaveBasicsettingsaccesses.RemoveRange (subDelete);

                // Insert new basic settings (cross join: employeeIds × settings)
                var query = from a in employeeIds
                            from x in _context.HrmLeaveMasterandsettingsLinks
                            join y in _context.HrmLeaveEntitlementHeads
                                on x.SettingsId equals y.SettingsId into yJoin
                            from y in yJoin.DefaultIfEmpty ( )
                            where leaveMasterIds.Contains ((int)x.SettingsId)
                            select new HrmLeaveBasicsettingsaccess
                            {
                                EmployeeId = a,
                                SettingsId = x.SettingsId,
                                LeaveMasterId = x.LeaveMasterId,
                                IsCompanyLevel = 0,
                                CreatedBy = dto.EntryBy,
                                CreatedDate = DateTime.UtcNow,
                                LinkLevel = dto.LinkLevel,
                                FromDateBs = dto.FromDate,
                                ValidToBs = dto.ValidTo,
                                Laps = y != null ? y.Laps : null
                            };

                await _context.HrmLeaveBasicsettingsaccesses.AddRangeAsync (query);

                // Update ValidTo of existing LEAVE_ACCESS entries
                var leaveAccessToUpdate = await _context.HrmLeaveEmployeeleaveaccesses
                    .Where (x =>
                        leaveMasterIds.Contains ((int)x.LeaveMaster) &&
                        employeeIds.Contains ((int)x.EmployeeId) &&
                        x.FromDate < dto.FromDate &&
                        x.ValidTo == null)
                    .ToListAsync ( );

                foreach (var access in leaveAccessToUpdate)
                    access.ValidTo = dto.FromDate.AddDays (-1);

                // Insert new LEAVE_ACCESS entries
                var newLeaveAccesses = from eId in employeeIds
                                       from lm in _context.HrmLeaveMasters
                                       where leaveMasterIds.Contains (lm.LeaveMasterId)
                                       select new HrmLeaveEmployeeleaveaccess
                                       {
                                           EmployeeId = eId,
                                           LeaveMaster = lm.LeaveMasterId,
                                           IsCompanyLevel = 0,
                                           CreatedBy = dto.EntryBy,
                                           CreatedDate = DateTime.UtcNow,
                                           Status = 1,
                                           FromDate = dto.FromDate,
                                           ValidTo = dto.ValidTo
                                       };

                await _context.HrmLeaveEmployeeleaveaccesses.AddRangeAsync (newLeaveAccesses);

                await _context.SaveChangesAsync ( );

                var lastInserted = await _context.HrmLeaveBasicsettingsaccesses
                    .OrderBy (x => x.SettingsId)
                    .LastOrDefaultAsync ( );
                await transaction.CommitAsync ( );
                return (int)(lastInserted?.IdEmployeeSettinsAccess ?? 0);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync ( );
                throw;
            }
        }


        private List<int> SplitToIntList (string csv) => csv.Split (',', StringSplitOptions.RemoveEmptyEntries).Select (int.Parse).ToList ( );


        public List<string> SplitStrings_XML (string input)
        {
            if (string.IsNullOrWhiteSpace (input))
                return new List<string> ( );

            return input.Split (',', StringSplitOptions.RemoveEmptyEntries)
                        .Select (x => x.Trim ( ))
                        .ToList ( );
        }

    }
}
