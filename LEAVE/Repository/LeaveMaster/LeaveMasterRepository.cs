using EMPLOYEE_INFORMATION.Data;
using EMPLOYEE_INFORMATION.Models;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Models;
using HRMS.EmployeeInformation.Models.Models.Entity;
using LEAVE.Dto;
using LEAVE.Helpers.AccessMetadataService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LEAVE.Repository.LeaveMaster
{
    public class LeaveMasterRepository : ILeaveMasterRepository
    {
        private readonly EmployeeDBContext _context;
        private readonly EmployeeSettings _employeeSettings;
        private IAccessMetadataService _accessMetadataService;
        public LeaveMasterRepository(EmployeeDBContext dbContext, IAccessMetadataService accessMetadataService, IOptions<EmployeeSettings> employeeSettings)
        {
            _context = dbContext;
            _accessMetadataService = accessMetadataService;
            _employeeSettings = employeeSettings.Value;
        }

        private static IEnumerable<string> SplitStrings_XML(string list, char delimiter = ',') =>
            string.IsNullOrWhiteSpace(list)
                ? Enumerable.Empty<string>()
                : list.Split(delimiter)
                      .Select(item => item.Trim())
                      .Where(item => !string.IsNullOrEmpty(item));

        private async Task<List<LeaveDetailModelDto>> FillLeaveMasterAsyncNoAccessMode(int empId, int roleId, int? lnklev, int transid)
        {

            var newHigh = await _accessMetadataService.GetNewHighListAsync(empId, roleId, transid, lnklev);

            // Final Fetch
            return await _context.HrmLeaveMasters
                .Where(l => newHigh.Contains(l.LeaveMasterId))
                .GroupJoin(_context.AdmUserMasters,
                    l => l.CreatedBy,
                    u => u.UserId,
                    (l, users) => new { Leave = l, User = users.FirstOrDefault() })
                .Select(x => new LeaveDetailModelDto
                {
                    UserName = x.User.UserName,
                    LeaveMasterId = x.Leave.LeaveMasterId,
                    LeaveCode = x.Leave.LeaveCode,
                    Description = x.Leave.Description,
                    PayType = x.Leave.PayType,
                    LeaveUnit = x.Leave.LeaveUnit,
                    Active = x.Leave.Active,
                    CreatedDate = x.Leave.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<List<LeaveDetailModelDto>> FillLeaveMasterAsync(int secondEntityId, int empId)
        {

            var accessMetadata = await _accessMetadataService.GetAccessMetadataAsync("Leave", secondEntityId, empId);
            if (accessMetadata.HasAccessRights)
            {
                // Fetch leave details if entity access rights are valid
                return await _context.HrmLeaveMasters
                    .GroupJoin(_context.AdmUserMasters,
                        l => l.CreatedBy,
                        u => u.UserId,
                        (l, users) => new { Leave = l, User = users.FirstOrDefault() })
                    .Select(x => new LeaveDetailModelDto
                    {
                        UserName = x.User.UserName,
                        LeaveMasterId = x.Leave.LeaveMasterId,
                        LeaveCode = x.Leave.LeaveCode,
                        Description = x.Leave.Description,
                        PayType = x.Leave.PayType,
                        LeaveUnit = x.Leave.LeaveUnit,
                        Active = x.Leave.Active,
                        CreatedDate = x.Leave.CreatedDate
                    })
                    .ToListAsync();
            }
            else
            {
                // If no entity access, fetch the result from dd method
                return await FillLeaveMasterAsyncNoAccessMode(empId, secondEntityId, accessMetadata.LinkLevel, accessMetadata.TransactionId);
            }
        }
        private static string FormatDate(DateTime? date, string format)
        {
            return date.HasValue ? date.Value.ToString(format) : string.Empty; // Or any other default value
        }
        public async Task<List<object>> FillvaluetypesAsync(string type)
        {
            if (type == "Entitlement")
            {
                var data = await _context.HrmValueTypes
                    .Where(v => v.Type == type && v.Code != "N")
                    .Select(v => new
                    {
                        v.Value,
                        v.Description
                    })
                    .ToListAsync();

                return data.Cast<object>().ToList();
            }
            else if (type == "ReasonMaster")
            {
                var data = await _context.GeneralCategories
                    .Select(g => new
                    {
                        g.Id,
                        g.Description,
                        g.Code
                    })
                    .ToListAsync();

                return data.Cast<object>().ToList();
            }
            else
            {
                var data = await _context.HrmValueTypes
                    .Where(v => v.Type == type)
                    .Select(v => new
                    {
                        v.Value,
                        v.Description
                    })
                    .ToListAsync();
                return data.Cast<object>().ToList();
            }
        }

        public async Task<int?> CreateMasterAsync(CreateMasterDto dto)
        {

            string camelCaseDescription = string.IsNullOrWhiteSpace(dto.Description) ? dto.Description : dto.Description.ToUpper();


            // If updating
            if (dto.MasterId != 0)
            {
                var existing = await _context.HrmLeaveMasters
                    .FirstOrDefaultAsync(x => x.LeaveMasterId == dto.MasterId);

                if (existing != null)
                {
                    existing.LeaveCode = dto.LeaveCode;
                    existing.Description = camelCaseDescription;
                    existing.PayType = dto.PayType;
                    existing.LeaveUnit = dto.LeaveUnit;
                    existing.Active = dto.Active;
                    existing.Colour = dto.Colour;

                    await _context.SaveChangesAsync();
                    return existing.LeaveMasterId;
                }
            }
            else
            {

                var exists = await _context.HrmLeaveMasters
                    .AnyAsync(x => x.LeaveCode == dto.LeaveCode);

                if (!exists)
                {
                    var newEntity = new HrmLeaveMaster
                    {
                        LeaveCode = dto.LeaveCode,
                        Description = camelCaseDescription,
                        PayType = dto.PayType,
                        LeaveUnit = dto.LeaveUnit,
                        Active = dto.Active,
                        CreatedBy = dto.CreatedBy,
                        Colour = dto.Colour,
                        CreatedDate = DateTime.UtcNow
                    };

                    _context.HrmLeaveMasters.Add(newEntity);
                    await _context.SaveChangesAsync();

                    return newEntity.LeaveMasterId;
                }
            }

            return null;
        }
        public async Task<List<object>> FillbasicsettingsAsync(int Masterid, string TransactionType, int SecondEntityId, int EmpId)
        {
            if (Masterid == 0)
            {
                var accessMetadata = await _accessMetadataService.GetAccessMetadataAsync(TransactionType, SecondEntityId, EmpId);//"Leave_BS"
                if (accessMetadata.HasAccessRights)
                {
                    var result = await (from b in _context.HrmLeaveBasicSettings
                                        join a in _context.AdmUserMasters
                                            on b.CreatedBy equals a.UserId into gj
                                        from a in gj.DefaultIfEmpty()
                                        select new
                                        {
                                            UserName = a != null ? a.UserName : null,
                                            b.SettingsId,
                                            b.SettingsName,
                                            b.SettingsDescription,
                                            CreatedDate = b.CreatedDate.HasValue ? FormatDate(b.CreatedDate, _employeeSettings.DateFormat) : null
                                        })
                                        .Distinct()
                                        .ToListAsync<object>();

                    return result;
                }
                var newHigh = await _accessMetadataService.GetNewHighListAsync(EmpId, SecondEntityId, accessMetadata.TransactionId, accessMetadata.LinkLevel);
                var finalResult = await (from b in _context.HrmLeaveBasicSettings
                                         join a in _context.AdmUserMasters
                                             on b.CreatedBy equals a.UserId into gj
                                         from a in gj.DefaultIfEmpty()
                                         where newHigh.Contains(b.SettingsId)
                                         select new
                                         {
                                             UserName = a != null ? a.UserName : null,
                                             b.SettingsId,
                                             b.SettingsName,
                                             b.SettingsDescription,
                                             CreatedDate = b.CreatedDate.HasValue ? FormatDate(b.CreatedDate, _employeeSettings.DateFormat) : null
                                         })
                                         .Distinct()
                                         .ToListAsync<object>();

                return finalResult;
            }
            else
            {
                var finalResult = await (from lmsl in _context.HrmLeaveMasterandsettingsLinks
                                         join lm in _context.HrmLeaveMasters
                                             on lmsl.LeaveMasterId equals lm.LeaveMasterId
                                         join lbs in _context.HrmLeaveBasicSettings
                                             on lmsl.SettingsId equals lbs.SettingsId
                                         join um in _context.AdmUserMasters
                                             on lbs.CreatedBy equals um.UserId into userJoin
                                         from um in userJoin.DefaultIfEmpty()
                                         where lm.LeaveMasterId == Masterid
                                         select new
                                         {
                                             UserName = um != null ? um.UserName : null,
                                             lbs.SettingsId,
                                             lbs.SettingsName,
                                             lbs.SettingsDescription,
                                             CreatedDate = lbs.CreatedDate.HasValue ? FormatDate(lbs.CreatedDate, _employeeSettings.DateFormat) : null
                                         })
                                         .Distinct()
                                         .ToListAsync<object>();

                return finalResult;
            }
        }
        public async Task<(string ApplicableLevelsNew, string ApplicableLevelsOne, string EmpIds, string CompanyIds)>



    GetEntityApplicableStringsAsync(string transactionType, long masterId)
        {
            // Step 1: Get relevant Transaction IDs
            var transactionIds = await _context.TransactionMasters
                .Where(t => t.TransactionType == transactionType)
                .Select(t => t.TransactionId)
                .ToListAsync();

            // Step 2: Get LinkId (LinkLevel != 1)
            var applicableLevelsNew = await (
                                from ea in _context.EntityApplicable00s
                                join tm in _context.TransactionMasters
                                    on ea.TransactionId equals tm.TransactionId
                                where tm.TransactionType == transactionType
                                   && ea.MasterId == masterId
                                   && ea.LinkLevel != 1
                                select ea.LinkId.ToString()
                            ).ToListAsync();

            var ApplicableLevelsNew = string.Join(",", applicableLevelsNew);


            // Step 3: Get LinkId (LinkLevel == 1)
            var applicableLevelsOne = await (
                                   from ea in _context.EntityApplicable00s
                                   join tm in _context.TransactionMasters
                                       on ea.TransactionId equals tm.TransactionId
                                   where tm.TransactionType == transactionType
                                      && ea.MasterId == masterId
                                      && ea.LinkLevel == 1
                                   select ea.LinkId.ToString()
                               ).ToListAsync();

            var ApplicableLevelsOne = string.Join(",", applicableLevelsOne.Where(x => !string.IsNullOrWhiteSpace(x)));




            // Step 4: Get EmpId from EntityApplicable01
            var empIds = await (
                       from ea in _context.EntityApplicable01s
                       join tm in _context.TransactionMasters
                           on ea.TransactionId equals tm.TransactionId
                       where tm.TransactionType == transactionType
                          && ea.MasterId == masterId
                       select ea.EmpId.ToString()
                   ).ToListAsync();

            var EmpIds = string.Join(",", empIds);

            // Step 5: Get LinkLevel = 15 as Company IDs
            var companyIds = await (
    from ea in _context.EntityApplicable00s
    join tm in _context.TransactionMasters
        on ea.TransactionId equals tm.TransactionId
    where tm.TransactionType == transactionType
       && ea.MasterId == masterId
       && ea.LinkLevel == 15
    select ea.LinkLevel.ToString()
).ToListAsync();

            var CompanyIds = string.Join(",", companyIds);

            // Step 6: Return all comma-separated results
            return (
                ApplicableLevelsNew: string.Join(",", applicableLevelsNew),
                ApplicableLevelsOne: string.Join(",", applicableLevelsOne),
                EmpIds: string.Join(",", empIds),
                CompanyIds: string.Join(",", companyIds)
            );
        }

        public async Task<string> ProcessEntityApplicableAsync(EntityApplicableApiDto entityApplicableApiDtos)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var tranId = await _accessMetadataService.GetTransactionIdByTransactionTypeAsync(entityApplicableApiDtos.TransactionType);

                if (entityApplicableApiDtos.FirstEntityId == 0)
                {
                    var applicable00 = await _context.EntityApplicable00s
                        .Where(e => e.TransactionId == tranId && e.MasterId == entityApplicableApiDtos.MasterId)
                        .ToListAsync();

                    if (applicable00.Any())
                    {
                        _context.EntityApplicable00s.RemoveRange(applicable00);
                        await _context.SaveChangesAsync();//--------newly added code
                    }


                    if (!string.IsNullOrEmpty(entityApplicableApiDtos.LinkIds))
                    {
                        var linkGroupList = entityApplicableApiDtos.LinkIds.Split('+', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var group in linkGroupList)
                        {
                            var linkIdList = group.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(int.Parse)
                                                  .ToList();

                            var linkLevel = await _context.SubCategoryLinksNews
                                .Where(s => linkIdList.Contains(s.LinkId))
                                .Select(s => s.LinkLevel)
                                .FirstOrDefaultAsync();

                            bool alreadyExists = await _context.EntityApplicable00s
                                .AnyAsync(e => e.TransactionId == tranId && e.LinkLevel == linkLevel && e.MasterId == entityApplicableApiDtos.MasterId);

                            if (!alreadyExists)
                            {
                                var insertLinks = await _context.SubCategoryLinksNews
                                    .Where(s => linkIdList.Contains(s.LinkId))
                                    .Select(s => new EntityApplicable00
                                    {
                                        TransactionId = tranId,
                                        LinkLevel = s.LinkLevel,
                                        LinkId = s.LinkId,
                                        MasterId = entityApplicableApiDtos.MasterId,
                                        MainMasterId = entityApplicableApiDtos.SecondEntityId,
                                        EntryBy = entityApplicableApiDtos.EntryBy,
                                        EntryDate = DateTime.Now
                                    }).ToListAsync();

                                _context.EntityApplicable00s.AddRange(insertLinks);
                            }
                        }
                    }
                }
                else
                {
                    var applicable01 = await _context.EntityApplicable01s
                        .Where(e => e.TransactionId == tranId && e.MasterId == entityApplicableApiDtos.MasterId)
                        .ToListAsync();
                    var applicable00 = await _context.EntityApplicable00s
                        .Where(e => e.TransactionId == tranId && e.MasterId == entityApplicableApiDtos.MasterId)
                        .ToListAsync();

                    _context.EntityApplicable01s.RemoveRange(applicable01);
                    _context.EntityApplicable00s.RemoveRange(applicable00);

                    _context.EntityApplicable00s.Add(new EntityApplicable00
                    {
                        TransactionId = tranId,
                        LinkLevel = entityApplicableApiDtos.FirstEntityId,
                        LinkId = 0,
                        MasterId = entityApplicableApiDtos.MasterId,
                        MainMasterId = entityApplicableApiDtos.SecondEntityId,
                        EntryBy = entityApplicableApiDtos.EntryBy,
                        EntryDate = DateTime.Now
                    });
                }

                if (!string.IsNullOrEmpty(entityApplicableApiDtos.EmployeeIds) && entityApplicableApiDtos.EmployeeIds != "0")
                {
                    var empIdList = entityApplicableApiDtos.EmployeeIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    var newEmpEntries = await _context.HrEmpMasters
                        .Where(e => empIdList.Contains(e.EmpId))
                        .Select(e => new EntityApplicable01
                        {
                            TransactionId = tranId,
                            LinkLevel = 13,
                            EmpId = e.EmpId,
                            MasterId = entityApplicableApiDtos.MasterId,
                            MainMasterId = entityApplicableApiDtos.SecondEntityId,
                            EntryBy = entityApplicableApiDtos.EntryBy,
                            EntryDate = DateTime.Now
                        }).ToListAsync();

                    var existing = await _context.EntityApplicable01s
                        .Where(e => e.TransactionId == tranId && e.MasterId == entityApplicableApiDtos.MasterId)
                        .ToListAsync();

                    _context.EntityApplicable01s.RemoveRange(existing);
                    _context.EntityApplicable01s.AddRange(newEmpEntries);
                }

                if (!string.IsNullOrEmpty(entityApplicableApiDtos.EntityList))
                {
                    var entityIds = entityApplicableApiDtos.EntityList.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    var existingLevelOne = await _context.EntityApplicable00s
                        .Where(e => e.TransactionId == tranId && e.MasterId == entityApplicableApiDtos.MasterId && e.LinkLevel == 1)
                        .ToListAsync();

                    _context.EntityApplicable00s.RemoveRange(existingLevelOne);

                    var EntityLevelOnes = from a in _context.Categorymasters
                                          join b in _context.Subcategories on a.EntityId equals b.EntityId
                                          where a.SortOrder == 1
                                          select new
                                          {
                                              LevelOneId = b.SubEntityId,
                                              LevelOneCode = b.Code,
                                              LinkableSubcategory = b.SubEntityId,
                                              LevelOneDescription = b.Description
                                          };

                    var result = await EntityLevelOnes.ToListAsync();

                    var newLevelOne = (from e in EntityLevelOnes
                                       where entityIds.Contains(e.LevelOneId)
                                       select new EntityApplicable00
                                       {
                                           TransactionId = tranId,
                                           LinkLevel = 1,
                                           LinkId = e.LevelOneId,
                                           MasterId = entityApplicableApiDtos.MasterId,
                                           MainMasterId = entityApplicableApiDtos.SecondEntityId,
                                           EntryBy = entityApplicableApiDtos.EntryBy,
                                           EntryDate = DateTime.Now
                                       }).ToList();

                    // Add the new entries to the EntityApplicable00 table
                    _context.EntityApplicable00s.AddRange(newLevelOne);
                }

                // Commit the transaction
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "success"; // Return success message
            }
            catch (Exception)
            {
                // Rollback the transaction if something goes wrong
                await transaction.RollbackAsync();
                throw; // Rethrow the exception to be handled at a higher level if needed
            }
        }

        public async Task<List<object>> GetEditLeaveMastersAsync(int masterId)
        {

            var result = await _context.HrmLeaveMasters
                .Where(lm => lm.LeaveMasterId == masterId)
                .Select(lm => new
                {
                    lm.LeaveCode,
                    lm.Description,
                    lm.PayType,
                    lm.LeaveUnit,
                    lm.Active,
                    lm.Colour
                })
                .ToListAsync();

            return result.Cast<object>().ToList(); // Because return type is List<object>

        }

        public async Task<int> GetDeleteLeaveMastersAsync(int masterId)
        {
            // Step 1: Get the related SettingsId
            var settingsLink = await _context.HrmLeaveMasterandsettingsLinks
                .FirstOrDefaultAsync(l => l.LeaveMasterId == masterId);

            if (settingsLink == null)
                return masterId;

            int? settingsId = settingsLink.SettingsId;

            // Step 2: Get related SettingsDetailsId
            var detail = await _context.HrmLeaveBasicsettingsDetails
                .FirstOrDefaultAsync(d => d.SettingsId == settingsId);

            int? settingsDetailsId = detail?.SettingsDetailsId;

            // Step 3: Get related LeaveEntitlementId
            var entitlement = await _context.HrmLeaveEntitlementHeads
                .FirstOrDefaultAsync(e => e.SettingsId == settingsId);

            int? entitlementId = entitlement?.LeaveEntitlementId;

            // Step 4: Delete in the correct order

            if (settingsDetailsId.HasValue)
            {
                var exceptions = _context.HrmLeaveExceptionalEligibilities
                    .Where(e => e.SettingsDetailsHeadId == settingsDetailsId.Value);
                _context.HrmLeaveExceptionalEligibilities.RemoveRange(exceptions);

                var details = _context.HrmLeaveBasicsettingsDetails
                    .Where(d => d.SettingsId == settingsId);
                _context.HrmLeaveBasicsettingsDetails.RemoveRange(details);
            }

            if (entitlementId.HasValue)
            {
                var entitlementRegs = _context.HrmLeaveEntitlementRegs
                    .Where(r => r.LeaveEntitlementId == entitlementId.Value);
                _context.HrmLeaveEntitlementRegs.RemoveRange(entitlementRegs);

                _context.HrmLeaveEntitlementHeads.Remove(entitlement);
            }

            var basicSettings = _context.HrmLeaveBasicSettings
                .Where(s => s.SettingsId == settingsId);
            _context.HrmLeaveBasicSettings.RemoveRange(basicSettings);

            _context.HrmLeaveMasterandsettingsLinks.Remove(settingsLink);

            var leaveMaster = await _context.HrmLeaveMasters
                .FirstOrDefaultAsync(lm => lm.LeaveMasterId == masterId);
            if (leaveMaster != null)
                _context.HrmLeaveMasters.Remove(leaveMaster);

            await _context.SaveChangesAsync();

            return masterId;
        }
        private HashSet<int> ParseStatusList(string? empStatus)
        {
            return string.IsNullOrWhiteSpace(empStatus)
                ? new HashSet<int>()
                : empStatus.Split(',')
                           .Select(s => int.Parse(s.Trim()))
                           .ToHashSet();
        }
        //public async Task<List<balancedetailsDto>> LeaveBalanceDetails(string employeeIds)
        //{
        //    var employeeIdList = ParseStatusList(employeeIds);

        //    var filteredEmployees = await _context.HrEmpMasters
        //        .Where(emp => employeeIdList.Contains(emp.EmpId) && emp.JoinDt <= DateTime.UtcNow)
        //        .Select(emp => new
        //        {
        //            emp.EmpId,
        //            emp.JoinDt,
        //            emp.PublicHoliday
        //        }).ToListAsync();

        //    var leaveSettings =
        //       from emp in _context.HrEmpMasters
        //       where employeeIdList.Contains(emp.EmpId) && emp.JoinDt <= DateTime.UtcNow
        //       join setting in _context.LeaveFinalSettings on emp.EmpId equals setting.EmployeeId into empSettings
        //       from setting in empSettings.DefaultIfEmpty()
        //       join detail in _context.HrmLeaveBasicsettingsDetails on setting.SettingsId equals detail.SettingsId into settingDetails
        //       from detail in settingDetails.DefaultIfEmpty()
        //       where detail == null || detail.Casualholiday == 0 || detail.Casualholiday == 1 && emp.PublicHoliday == true
        //       select new
        //       {
        //           EmployeeId = emp.EmpId,
        //           emp.JoinDt,
        //           setting.SettingsId,
        //           setting.LeaveMaster
        //       }
        //   ;

        //    var tempLeaveSetup =
        //       from a in leaveSettings
        //       join emp in _context.HrEmpMasters on a.EmployeeId equals emp.EmpId
        //       join basic in _context.HrmLeaveBasicsettingsDetails on a.SettingsId equals basic.SettingsId into basicGroup
        //       from basic in basicGroup.DefaultIfEmpty()

        //       join ent in _context.HrmLeaveEntitlementHeads on a.SettingsId equals ent.SettingsId into entGroup
        //       from ent in entGroup.DefaultIfEmpty()

        //       let grant = _context.HrmLeaveServicedbasedleaves
        //           .Where(g => ent != null && g.LeaveEntitlementId == ent.LeaveEntitlementId)
        //           .FirstOrDefault()

        //       let calculatedExperience = emp.JoinDt.HasValue
        //           ? (float)(DateTime.UtcNow.Year - emp.JoinDt.Value.Year +
        //                     (DateTime.UtcNow.Month - emp.JoinDt.Value.Month) / 12.0)
        //           : 0f

        //       where ent != null && ent.LeaveEntitlementId != null
        //       where grant == null ||
        //           grant.Checkcase == 1 && calculatedExperience < (grant.FromYear ?? float.MaxValue) ||
        //           grant.Checkcase == 3 && calculatedExperience > (grant.FromYear ?? float.MinValue) ||
        //           grant.Checkcase == 2 && calculatedExperience >= (grant.FromYear ?? float.MinValue) &&
        //                                  calculatedExperience <= (grant.ToYear ?? float.MaxValue)


        //       select new
        //       {
        //           emp.EmpId,
        //           a.LeaveMaster,
        //           a.SettingsId,
        //           emp.InstId,
        //           emp.JoinDt,
        //           GrantType = grant != null ? grant.ExperiancebasedGrant : ent.LeaveGrantType ?? 0,
        //           IsCarryForward = ent.Carryforward ?? 0,
        //           FullLeaveProRata = ent.FullleaveProRata ?? 0,
        //           ent.LeaveCount,

        //       }
        //   ;

        //    var finalLeaveSummary = await (
        //                        from setup in tempLeaveSetup
        //                        join leave in _context.HrmLeaveMasters on setup.LeaveMaster equals leave.LeaveMasterId into leaveGroup
        //                        from leave in leaveGroup.DefaultIfEmpty()

        //                        join empDetails in _context.EmployeeDetails on setup.EmpId equals empDetails.EmpId into empGroup
        //                        from emp in empGroup.DefaultIfEmpty()

        //                        join access in _context.LeavepolicyMasterAccesses
        //                            .Where(x => employeeIdList.Contains(x.EmployeeId ?? 0))
        //                            on setup.EmpId equals access.EmployeeId into accessGroup
        //                        from policyAccess in accessGroup.DefaultIfEmpty()

        //                        join limits in _context.LeavePolicyInstanceLimits
        //                            on new { policyAccess.PolicyId, LeaveId = setup.LeaveMaster }
        //                            equals new { PolicyId = limits.LeavePolicyMasterId, limits.LeaveId } into limitsGroup
        //                        from limit in limitsGroup.DefaultIfEmpty()

        //                        where setup.JoinDt == null || setup.JoinDt <= DateTime.Now

        //                        select new balancedetailsDto
        //                        {
        //                            EmpId = setup.EmpId,
        //                            EmpCode = emp.EmpCode,
        //                            Name = $"{emp.Name}'[{emp.EmpCode}]",
        //                            LeaveMasterId = setup.LeaveMaster,
        //                            LeaveCode = leave.LeaveCode,
        //                            Description = leave.Description,
        //                            LeaveCredited = setup.LeaveCount,
        //                            Accrued = setup.LeaveCount,
        //                            Leavebalance = setup.LeaveCount, /// (limit.Nobalance ?? 0) == 1 && 1 > setup.LeaveCount? setup.LeaveCount: (setup.LeaveCount ?? setup.LeaveCount)
        //                            Colour = leave.Colour,
        //                            MonthlyLimit = limit.MaximamLimit,
        //                        }
        //                    ).ToListAsync();


        //    // You would typically return actual data here
        //    return finalLeaveSummary; // Placeholder
        //}

        public async Task<List<LeaveBalanceBaseDto>> GetLeaveBalanceDetails(string employeeIds, string submode, int leaveBalanceFormat)
        {
            var employeeIdList = ParseStatusList(employeeIds);

            var leaveSettings =
                from emp in _context.HrEmpMasters
                where employeeIdList.Contains(emp.EmpId) && emp.JoinDt <= DateTime.UtcNow
                join setting in _context.LeaveFinalSettings on emp.EmpId equals setting.EmployeeId into empSettings
                from setting in empSettings.DefaultIfEmpty()
                join detail in _context.HrmLeaveBasicsettingsDetails on setting.SettingsId equals detail.SettingsId into settingDetails
                from detail in settingDetails.DefaultIfEmpty()
                where detail == null || detail.Casualholiday == 0 || (detail.Casualholiday == 1 && emp.PublicHoliday == true)
                select new
                {
                    EmployeeId = emp.EmpId,
                    emp.JoinDt,
                    setting.SettingsId,
                    setting.LeaveMaster
                };

            var tempLeaveSetup =
                from a in leaveSettings
                join emp in _context.HrEmpMasters on a.EmployeeId equals emp.EmpId
                join basic in _context.HrmLeaveBasicsettingsDetails on a.SettingsId equals basic.SettingsId into basicGroup
                from basic in basicGroup.DefaultIfEmpty()

                join ent in _context.HrmLeaveEntitlementHeads on a.SettingsId equals ent.SettingsId into entGroup
                from ent in entGroup.DefaultIfEmpty()

                let grant = _context.HrmLeaveServicedbasedleaves
                    .Where(g => ent != null && g.LeaveEntitlementId == ent.LeaveEntitlementId)
                    .FirstOrDefault()

                let calculatedExperience = emp.JoinDt.HasValue
                    ? (float)(DateTime.UtcNow.Year - emp.JoinDt.Value.Year +
                              (DateTime.UtcNow.Month - emp.JoinDt.Value.Month) / 12.0)
                    : 0f

                where ent != null && ent.LeaveEntitlementId != null
                where grant == null ||
                      (grant.Checkcase == 1 && calculatedExperience < (grant.FromYear ?? float.MaxValue)) ||
                      (grant.Checkcase == 3 && calculatedExperience > (grant.FromYear ?? float.MinValue)) ||
                      (grant.Checkcase == 2 && calculatedExperience >= (grant.FromYear ?? float.MinValue) &&
                                                calculatedExperience <= (grant.ToYear ?? float.MaxValue))

                select new
                {
                    emp.EmpId,
                    emp.EmpCode,
                    emp.FirstName,
                    a.LeaveMaster,
                    a.SettingsId,
                    emp.InstId,
                    emp.JoinDt,
                    GrantType = grant != null ? grant.ExperiancebasedGrant : ent.LeaveGrantType ?? 0,
                    IsCarryForward = ent.Carryforward ?? 0,
                    FullLeaveProRata = ent.FullleaveProRata ?? 0,
                    LeaveCount = ent.LeaveCount,
                    ApplyWithoutBalance = basic.Applywithoutbalance ?? 0
                };

            var finalLeaveSummary = await (
                from setup in tempLeaveSetup
                join leave in _context.HrmLeaveMasters on setup.LeaveMaster equals leave.LeaveMasterId into leaveGroup
                from leave in leaveGroup.DefaultIfEmpty()

                join empDetails in _context.EmployeeDetails on setup.EmpId equals empDetails.EmpId into empGroup
                from emp in empGroup.DefaultIfEmpty()

                join access in _context.LeavepolicyMasterAccesses
                    .Where(x => employeeIdList.Contains(x.EmployeeId ?? 0))
                    on setup.EmpId equals access.EmployeeId into accessGroup
                from policyAccess in accessGroup.DefaultIfEmpty()

                    //join limits in _context.LeavePolicyInstanceLimits 
                    //    on new { PolicyId = policyAccess.PolicyId, LeaveId = setup.LeaveMaster }
                    //    equals new { limits.LeavePolicyMasterId, limits.LeaveId } into limitsGroup
                    //from limit in limitsGroup.DefaultIfEmpty()
                from limit in _context.LeavePolicyInstanceLimits
       .Where(l => l.LeavePolicyMasterId == policyAccess.PolicyId
                && l.LeaveId == setup.LeaveMaster)
       .DefaultIfEmpty()



                select new
                {
                    setup,
                    leave,
                    emp,
                    policyAccess,
                    limit
                }
            ).ToListAsync();

            if (submode == "leavebalancefulldetails")
            {
                return finalLeaveSummary.Select(x => new leavebalancefulldetailsDto
                {
                    EmpId = x.setup.EmpId,
                    EmpCode = x.setup.EmpCode,
                    Name = $"{x.setup.FirstName} [{x.setup.EmpCode}]",
                    LeaveMasterId = (int)x.setup.LeaveMaster,
                    LeaveCode = x.leave?.LeaveCode,
                    Description = x.leave?.Description,

                    LeaveCredited = x.setup.ApplyWithoutBalance == 1 ? 0 : x.setup.LeaveCount,
                    Accrued = x.setup.ApplyWithoutBalance == 1 ? 0 : x.setup.LeaveCount,
                    LeaveBalance = x.setup.ApplyWithoutBalance == 1 ? 0 : x.setup.LeaveCount,

                    Used = leaveBalanceFormat == 2 ? (x.setup.LeaveCount ?? 0) : (x.setup.LeaveCount ?? 0),
                    Granted = 0, // Optional: adjust based on requirement
                    Carryforward = x.setup.IsCarryForward,
                    CreditedLeave = x.setup.ApplyWithoutBalance == 1 ? 0 : x.setup.LeaveCount,

                    Colour = x.leave?.Colour,
                    MonthlyLimit = (decimal?)(x.limit?.MaximamLimit),
                    LapsedLeaves = 0,
                    Leavebalanceformat = leaveBalanceFormat

                }).Cast<LeaveBalanceBaseDto>().ToList();
            }
            else // balancedetails
            {
                return finalLeaveSummary.Select(x => new balancedetailsDto
                {
                    EmpId = x.setup.EmpId,
                    EmpCode = x.setup.EmpCode,
                    Name = $"{x.setup.FirstName} [{x.setup.EmpCode}]",
                    LeaveMasterId = (int)x.setup.LeaveMaster,
                    LeaveCode = x.leave?.LeaveCode,
                    Description = x.leave?.Description,
                    LeaveCredited = x.setup.LeaveCount,
                    Accrued = x.setup.LeaveCount,
                    Leavebalance = x.setup.LeaveCount,
                    Colour = x.leave?.Colour,
                    MonthlyLimit = (decimal?)(x.limit?.MaximamLimit)
                }).Cast<LeaveBalanceBaseDto>().ToList();
            }
        }
        public class LeaveBalanceBaseDto
        {
            public int EmpId { get; set; }
            public string EmpCode { get; set; }
            public string Name { get; set; }
            public int LeaveMasterId { get; set; }
            public string LeaveCode { get; set; }
            public string Description { get; set; }
            public decimal? LeaveCredited { get; set; }
            public decimal? Accrued { get; set; }
            public string Colour { get; set; }
            public decimal? MonthlyLimit { get; set; }
        }
        public class balancedetailsDto : LeaveBalanceBaseDto
        {
            public decimal? Leavebalance { get; set; }
        }
        public class leavebalancefulldetailsDto : LeaveBalanceBaseDto
        {
            public decimal? LeaveBalance { get; set; }
            public decimal? Used { get; set; }
            public decimal? Granted { get; set; }
            public decimal? Carryforward { get; set; }
            public decimal? CreditedLeave { get; set; }
            public decimal? LapsedLeaves { get; set; }
            public int Leavebalanceformat { get; set; }
        }

    }
}


