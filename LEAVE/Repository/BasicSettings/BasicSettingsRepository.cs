using EMPLOYEE_INFORMATION.Data;
using EMPLOYEE_INFORMATION.Models;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Models.Models.Entity;
using LEAVE.Dto;
using LEAVE.Helpers.AccessMetadataService;
using Microsoft.EntityFrameworkCore;

namespace LEAVE.Repository.BasicSettings
{
    public class BasicSettingsRepository : IBasicSettingsRepository
    {
        private readonly EmployeeDBContext _context;
        private readonly HttpClient _httpClient;

        private IAccessMetadataService _accessMetadataService;
        public BasicSettingsRepository(EmployeeDBContext dbContext, HttpClient httpClient, IAccessMetadataService accessMetadataService)
        {
            _context = dbContext;
            _httpClient = httpClient;
            _accessMetadataService = accessMetadataService;
        }

        public async Task<List<FillvacationaccrualDto>> Fillvacationaccrual(int basicsettingsid)
        {
            var result = await (from a in _context.LeaveScheme00s
                                join b in _context.Leavescheme02s
                                    on a.LeaveSchemeId equals b.LeaveSchemeId
                                where b.Basicsettingsid == basicsettingsid
                                select new FillvacationaccrualDto
                                {
                                    VacationSchemeId = a.LeaveSchemeId,
                                    Scheme = a.SchemeDescription + " [ " + a.SchemeCode + " ]"
                                }).ToListAsync();

            return result;
        }


        public async Task<List<GetEditbasicsettingsdto>> GetEditbasicsettings(int basicsettingsid)
        {
            var result = from a in _context.HrmLeaveBasicSettings
                         join b in _context.HrmLeaveMasterandsettingsLinks
                             on a.SettingsId equals b.SettingsId
                         where a.SettingsId == basicsettingsid
                         select new GetEditbasicsettingsdto
                         {
                             SettingsName = a.SettingsName,
                             SettingsDescription = a.SettingsDescription,
                             LeaveMasterId = b.LeaveMasterId,
                             DaysOrHours = a.DaysOrHours
                         };

            return await result.ToListAsync();

        }

        public async Task<List<HrmLeaveMasterandsettingsLinksDto>> saveleavelinktable(int masterId, int basicSettingsId, int createdBy)
        {
            var existingLink = await _context.HrmLeaveMasterandsettingsLinks
                .FirstOrDefaultAsync(link => link.SettingsId == basicSettingsId);

            if (existingLink == null)
            {
                var newLink = new HrmLeaveMasterandsettingsLink
                {
                    LeaveMasterId = masterId,
                    SettingsId = basicSettingsId,
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.UtcNow
                };

                _context.HrmLeaveMasterandsettingsLinks.Add(newLink);
                await _context.SaveChangesAsync();
            }
            else
            {
                existingLink.LeaveMasterId = masterId;
                await _context.SaveChangesAsync();
            }

            var result = await _context.HrmLeaveMasterandsettingsLinks
                .Where(l => l.SettingsId == basicSettingsId)
                .Select(l => new HrmLeaveMasterandsettingsLinksDto
                {
                    SettingsId = (int)l.SettingsId,
                    LeaveMasterId = l.LeaveMasterId,
                    CreatedBy = (int)l.CreatedBy,
                    CreatedDate = DateTime.UtcNow
                })
                .ToListAsync();

            return result;
        }

        public async Task<int?> DeleteConfirm(int basicSettingsId)
        {
            bool exists = await _context.HrmLeaveBasicsettingsaccesses
                .AnyAsync(x => x.SettingsId == basicSettingsId);

            if (exists)
                return -1;

            return 1; // or null or 0 if no restriction, based on your logic
        }


        public async Task<int?> GetDeletebasics(int basicSettingsId, int masterId, string transactionType)
        {
            var tranId = await _accessMetadataService.GetTransactionIdByTransactionTypeAsync(transactionType);//Leave_BS
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (masterId == 0)
                {
                    // Delete from HRM_LEAVE_BASIC_SETTINGS
                    var basicSettings = await _context.HrmLeaveBasicSettings
                        .Where(x => x.SettingsId == basicSettingsId)
                        .ToListAsync();
                    _context.HrmLeaveBasicSettings.RemoveRange(basicSettings);



                    // Delete from EntityApplicable00
                    var applicableEntities = await _context.EntityApplicable00s
                        .Where(e => e.MasterId == basicSettingsId && e.TransactionId == tranId)
                        .ToListAsync();
                    _context.EntityApplicable00s.RemoveRange(applicableEntities);

                    // Delete from HRM_LEAVE_MASTERANDSETTINGS_LINK
                    var masterLinks = await _context.HrmLeaveMasterandsettingsLinks
                        .Where(link => link.SettingsId == basicSettingsId)
                        .ToListAsync();
                    _context.HrmLeaveMasterandsettingsLinks.RemoveRange(masterLinks);

                    // Get SettingsDetailsId (head)
                    var detail = await _context.HrmLeaveBasicsettingsDetails
                        .Where(d => d.SettingsId == basicSettingsId)
                        .FirstOrDefaultAsync();

                    if (detail != null)
                    {
                        int headId = detail.SettingsDetailsId;

                        // Delete from HRM_LEAVE_BASICSETTINGS_DETAILS
                        _context.HrmLeaveBasicsettingsDetails.Remove(detail);

                        // Delete from HRM_LEAVE_EXCEPTIONAL_ELIGIBILITY
                        var eligibility = await _context.HrmLeaveExceptionalEligibilities
                            .Where(e => e.SettingsDetailsHeadId == headId)
                            .ToListAsync();
                        _context.HrmLeaveExceptionalEligibilities.RemoveRange(eligibility);
                    }

                    // Get LeaveEntitlementId
                    var entitlement = await _context.HrmLeaveEntitlementHeads
                        .Where(e => e.SettingsId == basicSettingsId)
                        .FirstOrDefaultAsync();

                    if (entitlement != null)
                    {
                        int entId = entitlement.LeaveEntitlementId;

                        _context.HrmLeaveEntitlementHeads.Remove(entitlement);

                        var regs = await _context.HrmLeaveEntitlementRegs
                            .Where(r => r.LeaveEntitlementId == entId)
                            .ToListAsync();
                        _context.HrmLeaveEntitlementRegs.RemoveRange(regs);

                        var serviceLeaves = await _context.HrmLeaveServicedbasedleaves
                            .Where(s => s.LeaveEntitlementId == entId)
                            .ToListAsync();
                        _context.HrmLeaveServicedbasedleaves.RemoveRange(serviceLeaves);

                        var partials = await _context.HrmLeavePartialPayments
                            .Where(p => p.SettingsDetailsId == entId)
                            .ToListAsync();
                        _context.HrmLeavePartialPayments.RemoveRange(partials);
                    }
                }
                else
                {
                    // Delete single mapping if exists
                    var specificLink = await _context.HrmLeaveMasterandsettingsLinks
                        .Where(link => link.SettingsId == basicSettingsId && link.LeaveMasterId == masterId)
                        .ToListAsync();
                    _context.HrmLeaveMasterandsettingsLinks.RemoveRange(specificLink);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return basicSettingsId;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<object> Geteditdetails(string entitlement, int masterId, int? experienceId = null)
        {
            //var emplist= await _context.ViewLeaveBasicsettingsDetails.ToListAsync();

            switch (entitlement.ToLower())
            {
                case "exception":
                    return await (from d in _context.HrmLeaveBasicsettingsDetails
                                  join e in _context.HrmLeaveExceptionalEligibilities
                                  on d.SettingsDetailsId equals e.SettingsDetailsHeadId
                                  where d.SettingsId == masterId
                                  select new
                                  {
                                      e.Year,
                                      e.Month,
                                      e.Count
                                  }).ToListAsync();

                case "prorata":
                    return await (from h in _context.HrmLeaveEntitlementHeads
                                  join r in _context.HrmLeaveEntitlementRegs
                                  on h.LeaveEntitlementId equals r.LeaveEntitlementId
                                  where h.SettingsId == masterId && r.Newjoin == 0
                                  select new
                                  {
                                      Frommonth = r.Year,
                                      Tomonth = r.Month,
                                      r.Count
                                  }).ToListAsync();

                case "serviceperiod":
                    return await (from h in _context.HrmLeaveEntitlementHeads
                                  join s in _context.HrmLeaveServicedbasedleaves
                                      on h.LeaveEntitlementId equals s.LeaveEntitlementId
                                  where h.SettingsId == masterId
                                  select new
                                  {
                                      s.FromYear,
                                      s.ToYear,
                                      h.LeaveCount,
                                      s.IdServiceLeave,
                                      s.ExperiancebasedGrant,
                                      s.Experiancebasedrollover,
                                      s.Checkcase,
                                      s.ExperiancebasedVacation
                                  }).ToListAsync();

                case "newjoinprorata":
                    return await (from h in _context.HrmLeaveEntitlementHeads
                                  join r in _context.HrmLeaveEntitlementRegs
                                  on h.LeaveEntitlementId equals r.LeaveEntitlementId
                                  where h.SettingsId == masterId && r.Newjoin == 1
                                  select new
                                  {
                                      Frommonth = r.Year,
                                      Tomonth = r.Month,
                                      r.Count
                                  }).ToListAsync();

                case "leavelink":
                    return await (from a in _context.HrmLeaveMasterandsettingsLinks
                                  join b in _context.HrmLeaveMasters
                                  on a.LeaveMasterId equals b.LeaveMasterId
                                  where a.SettingsId == masterId
                                  select new
                                  {
                                      a.LeaveMasterId,
                                      b.PayType
                                  }).ToListAsync();

                case "partialpayment":
                    return await (from a in _context.HrmLeaveEntitlementHeads
                                  join b in _context.HrmLeavePartialPayments
                                  on a.LeaveEntitlementId equals b.SettingsDetailsId
                                  where a.SettingsId == masterId && b.ExperiancetabId == 0 && b.NewjnStatus == 0
                                  select new
                                  {
                                      b.Daysfrom,
                                      b.Daysto,
                                      b.PayPercentage,
                                      b.Ondemandpartial
                                  }).ToListAsync();

                case "servicepartialpayment":
                    return await (from a in _context.HrmLeaveEntitlementHeads
                                  join b in _context.HrmLeavePartialPayments
                                  on a.LeaveEntitlementId equals b.SettingsDetailsId
                                  where a.SettingsId == masterId && b.ExperiancetabId == experienceId && b.NewjnStatus == 0
                                  select new
                                  {
                                      b.Daysfrom,
                                      b.Daysto,
                                      b.PayPercentage,
                                      b.Ondemandpartial
                                  }).ToListAsync();

                case "newjnpartialpayment":
                    return await (from a in _context.HrmLeaveEntitlementHeads
                                  join b in _context.HrmLeavePartialPayments
                                  on a.LeaveEntitlementId equals b.SettingsDetailsId
                                  where a.SettingsId == masterId && b.ExperiancetabId == 0 && b.NewjnStatus == 1
                                  select new
                                  {
                                      b.Daysfrom,
                                      b.Daysto,
                                      b.PayPercentage,
                                      b.Ondemandpartial
                                  }).ToListAsync();

                case "details":

                    return await (from b in _context.ViewLeaveBasicsettingsDetails
                                  join a in _context.HrmLeaveMasterandsettingsLinks
                                      on b.SettingsId equals a.SettingsId
                                  join c in _context.HrmLeaveBasicSettings
                                      on b.SettingsId equals c.SettingsId
                                  join d in _context.HrmLeaveEntitlementHeads
                                      on new { SettingsId = b.SettingsId, LeaveEntitlementId = b.LeaveEntitlementId ?? 0 }
                                      equals new { SettingsId = d.SettingsId ?? 0, LeaveEntitlementId = d.LeaveEntitlementId } into dJoin
                                  from d in dJoin.DefaultIfEmpty()
                                  where b.SettingsId == masterId
                                  select new
                                  {
                                      b.SettingsId,
                                      b.EmployeeType,
                                      b.Lopcheck,
                                      b.Gender,
                                      b.MaritalStatus,
                                      b.Carryforward,
                                      b.Rollovercount,
                                      b.AllemployeeLeaveCount,
                                      b.DateofJoiningCheck,
                                      b.JoinedDate,
                                      b.LeaveCount,
                                      b.LeaveGrantType,
                                      b.OndemandLeaveGrand,
                                      b.Maternity,
                                      b.Compensatory,
                                      b.Experiance,
                                      a.LeaveMasterId,
                                      b.Monthwise,
                                      b.NewjoinGranttype,
                                      b.NewjoinLeavecount,
                                      b.NewjoinMonthwise,
                                      b.MinServiceDays,
                                      b.CsectionMaxLeave,
                                      b.EligibleCount,
                                      b.LeaveType,
                                      b.CompCaryfrwrd,
                                      b.Defaultreturndate,
                                      b.Laps,
                                      StartDate = b.StartDate.HasValue ? b.StartDate.Value.ToString("dd/MM/yyyy") : null,
                                      b.Attachment,
                                      b.Eligibility,
                                      b.EligibilityGrant,
                                      b.Cfbasedon,
                                      b.CarryforwardNj,
                                      b.CfbasedonNj,
                                      b.RollovercountNj,
                                      b.Salaryadvance,
                                      b.Roledeligation,
                                      b.Firstmonthleavecount,
                                      b.Credetedon,
                                      b.Weeklyleaveday,
                                      b.Yearcount,
                                      b.JoinmonthdayaftrNyear,
                                      b.JoinmonthleaveaftrNyear,
                                      b.Beginningcarryfrwrd,
                                      b.Vacationaccrualtype,
                                      b.Ishalfday,
                                      b.Previousexperiance,
                                      b.LeaveInclude,
                                      b.LeavedaysSalaryadvance,
                                      b.SalaryadvanceApplybeforedays,
                                      b.Returnrequest,
                                      b.Attachmentmandatory,
                                      b.Applywithoutbalance,
                                      b.Casualholiday,
                                      b.PassageeligibilityEnable,
                                      b.Passageeligibilitydays,
                                      b.PassportRequest,
                                      b.GrantfullleaveforAll,
                                      b.FullleaveProRata,
                                      b.Settingspaymode,
                                      b.PartialpaymentBalancedays,
                                      b.PartialpaymentNextcount,
                                      b.EnableLeaveGrander,
                                      b.Autocarryforward,
                                      b.Yearlylimit,
                                      b.Yearlylimitcount,
                                      b.ApplicableOnnotice,
                                      Isallowleavecancel = b.Allowleavecancel,
                                      Blockpreviouslap = b.Blockpreviouslap ?? 1,
                                      b.LeaveEncashment,
                                      b.Disableyearlylimit,
                                      b.LeaveAccrual,
                                      b.DaysOrHours,
                                      b.LeaveHours,
                                      b.LeaveCriteria,
                                      b.CalculateOnFirst,
                                      b.CalculateOnSecond,
                                      b.LeaveHoursNewjoin,
                                      b.LeaveCriteriaNewjoin,
                                      b.CalculateOnFirstNewjoin,
                                      b.CalculateOnSecondNewjoin,
                                      ShowcurrentmonthWeekoff = b.ShowcurrentmonthWeekoff ?? 0,
                                      Leavebalanceroundoption = b.Leavebalanceroundoption ?? 0,
                                      c.RejoinWarningShow,
                                      c.RejoinWarningShowDaysMax,
                                      NyearBasedOnJoinDate = d != null ? d.NyearBasedOnJoinDate ?? 0 : 0,
                                      LeaveEncashmentMnthly = b.LeaveEncashmentMnthly ?? 0,
                                      LeaveReductionForLateIn = b.LeaveReductionForLateIn ?? 0,
                                      ConsiderProbationDate = d != null ? d.ConsiderProbationDate ?? 0 : 0,
                                      IsShowPartialPaymentDays = d != null ? d.IsShowPartialPaymentDays ?? 0 : 0,
                                      ExtraLeaveCountProxy = d != null ? d.ExtraLeaveCountProxy ?? 0 : 0,
                                      //d.JoinDateIn,
                                      //d.ToJoinDate,
                                      // d.LeaveCountBtw
                                  }).ToListAsync();



                default:
                    throw new ArgumentException("Invalid entitlement type");
            }
        }

        public async Task<int> Createbasicsettings(CreatebasicsettingsDto CreatebasicsettingsDto)
        {
            // description = CamelCase(description); // If you have a method for CamelCase conversion    

            if (CreatebasicsettingsDto.masterId == 0)
            {
                if (CreatebasicsettingsDto.basicSettingsId != 0)
                {
                    var existingSetting = await _context.HrmLeaveBasicSettings
                        .FirstOrDefaultAsync(x => x.SettingsId == CreatebasicsettingsDto.basicSettingsId);

                    if (existingSetting != null)
                    {
                        existingSetting.SettingsName = CreatebasicsettingsDto.leaveCode;
                        existingSetting.SettingsDescription = CreatebasicsettingsDto.description;
                        existingSetting.DaysOrHours = CreatebasicsettingsDto.basedOn;

                        await _context.SaveChangesAsync();
                        return CreatebasicsettingsDto.basicSettingsId;
                    }
                }
                else
                {
                    var duplicateSetting = await _context.HrmLeaveBasicSettings
                        .FirstOrDefaultAsync(x => x.SettingsName == CreatebasicsettingsDto.leaveCode && x.SettingsDescription == CreatebasicsettingsDto.description);

                    if (duplicateSetting == null)
                    {
                        var newSetting = new HrmLeaveBasicSetting
                        {
                            SettingsName = CreatebasicsettingsDto.leaveCode,
                            SettingsDescription = CreatebasicsettingsDto.description,
                            DaysOrHours = CreatebasicsettingsDto.basedOn,
                            CreatedBy = CreatebasicsettingsDto.createdBy,
                            CreatedDate = DateTime.UtcNow
                        };

                        _context.HrmLeaveBasicSettings.Add(newSetting);
                        await _context.SaveChangesAsync();

                        return newSetting.SettingsId;
                    }
                }
            }
            else
            {
                var duplicateSetting = await _context.HrmLeaveBasicSettings
                    .FirstOrDefaultAsync(x => x.SettingsName == CreatebasicsettingsDto.leaveCode && x.SettingsDescription == CreatebasicsettingsDto.description);

                if (duplicateSetting == null)
                {
                    var newSetting = new HrmLeaveBasicSetting
                    {
                        SettingsName = CreatebasicsettingsDto.leaveCode,
                        SettingsDescription = CreatebasicsettingsDto.description,
                        DaysOrHours = CreatebasicsettingsDto.basedOn,
                        CreatedBy = CreatebasicsettingsDto.createdBy,
                        CreatedDate = DateTime.UtcNow
                    };

                    _context.HrmLeaveBasicSettings.Add(newSetting);
                    await _context.SaveChangesAsync();

                    duplicateSetting = newSetting;
                }

                var link = new HrmLeaveMasterandsettingsLink
                {
                    LeaveMasterId = CreatebasicsettingsDto.masterId,
                    SettingsId = duplicateSetting.SettingsId,
                    CreatedBy = CreatebasicsettingsDto.createdBy,
                    CreatedDate = DateTime.UtcNow
                };

                _context.HrmLeaveMasterandsettingsLinks.Add(link);
                await _context.SaveChangesAsync();

                return link.LeaveMasterId;
            }

            return 0;
        }
        private async Task<List<LeaveDetailModelDto>> FillleavetypeListAsyncNoAccessMode(int empId, int roleId, int? lnklev, int transid)
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

                    LeaveMasterId = x.Leave.LeaveMasterId,
                    LeaveCode = x.Leave.LeaveCode


                })
                .ToListAsync();
        }

        public async Task<List<LeaveDetailModelDto>> FillleavetypeListAsync(int secondEntityId, int empId)
        {
            var accessMetadata = await _accessMetadataService.GetAccessMetadataAsync("Leave", secondEntityId, empId);

            if (accessMetadata.HasAccessRights)
            {
                var leaveTypes = await _context.HrmLeaveMasters
                    .Where(l => l.Active == 1)
                    .GroupBy(l => new { l.LeaveMasterId, l.Description, l.LeaveCode })
                    .Select(g => new LeaveDetailModelDto
                    {
                        LeaveMasterId = g.Key.LeaveMasterId,
                        LeaveCode = g.Key.Description + "[" + g.Key.LeaveCode + "]"
                    })
                    .ToListAsync();

                return leaveTypes;
            }
            else
            {
                return await FillleavetypeListAsyncNoAccessMode(empId, secondEntityId, accessMetadata.LinkLevel, accessMetadata.TransactionId);
            }
        }



        public async Task<List<BasicSettingDto>> GetEditbasicsettingsAsync(int masterid)
        {
            return await _context.HrmLeaveBasicSettings
                 .Join(_context.HrmLeaveMasterandsettingsLinks,
                     setting => setting.SettingsId,
                     link => link.SettingsId,
                     (setting, link) => new { setting, link })
                 .Where(x => x.link.LeaveMasterId == masterid)
                 .Select(x => new BasicSettingDto
                 {
                     SettingsName = x.setting.SettingsName,
                     SettingsDescription = x.setting.SettingsDescription,
                     LeaveMasterId = x.link.LeaveMasterId,
                     DaysOrHours = x.setting.DaysOrHours
                 })
                 .ToListAsync();
        }


        public async Task<long?> UpdateLeaveMasterAndSettingsLinkAsync(LeaveEntitlementDto leaveEntitlementDto)
        {
            var lastInsertedId = 0;
            if (leaveEntitlementDto.MasterId != 0)
            {

                var existing = await _context.HrmLeaveMasterandsettingsLinks
                    .FirstOrDefaultAsync(x => x.SettingsId == leaveEntitlementDto.BasicSettingsId);

                if (existing == null)
                {
                    var newLink = new HrmLeaveMasterandsettingsLink
                    {
                        LeaveMasterId = leaveEntitlementDto.MasterId,
                        SettingsId = leaveEntitlementDto.BasicSettingsId,
                        CreatedBy = leaveEntitlementDto.CreatedBy,
                        CreatedDate = DateTime.UtcNow
                    };

                    _context.HrmLeaveMasterandsettingsLinks.Add(newLink);
                    await _context.SaveChangesAsync();

                    lastInsertedId = newLink.IdMasterandSettingsLink;
                }
                else
                {
                    existing.LeaveMasterId = leaveEntitlementDto.MasterId;
                    _context.HrmLeaveMasterandsettingsLinks.Update(existing);
                    await _context.SaveChangesAsync();

                    lastInsertedId = existing.IdMasterandSettingsLink;
                }
            }
            if (leaveEntitlementDto.HeadProrataId != 0)
            {
                if (leaveEntitlementDto.RegType == "Prorata")
                {
                    var prorataEntry = new HrmLeaveEntitlementReg
                    {
                        LeaveEntitlementId = leaveEntitlementDto.HeadProrataId,
                        Year = leaveEntitlementDto.Year,
                        Month = leaveEntitlementDto.Month,
                        Count = leaveEntitlementDto.LeaveCount,
                        Newjoin = 0
                    };

                    _context.HrmLeaveEntitlementRegs.Add(prorataEntry);
                    await _context.SaveChangesAsync();

                    lastInsertedId = prorataEntry.LeaveentitlementregId;
                }
                else if (leaveEntitlementDto.RegType == "Service")
                {
                    var serviceEntry = new HrmLeaveServicedbasedleave
                    {
                        LeaveEntitlementId = leaveEntitlementDto.HeadProrataId,
                        FromYear = leaveEntitlementDto.Year,
                        ToYear = leaveEntitlementDto.Month,
                        LeaveCount = leaveEntitlementDto.LeaveCount,
                        ExperiancebasedGrant = leaveEntitlementDto.GrantType,
                        Experiancebasedrollover = leaveEntitlementDto.Rollover,
                        Checkcase = leaveEntitlementDto.CheckCase == 1 ? 1 : 0,
                        ExperiancebasedVacation = leaveEntitlementDto.VacationType
                    };

                    _context.HrmLeaveServicedbasedleaves.Add(serviceEntry);
                    await _context.SaveChangesAsync();

                    lastInsertedId = serviceEntry.IdServiceLeave;
                }
                else if (leaveEntitlementDto.RegType == "Newjoin")
                {
                    var newJoinEntry = new HrmLeaveEntitlementReg
                    {
                        LeaveEntitlementId = leaveEntitlementDto.HeadProrataId,
                        Year = leaveEntitlementDto.Year,
                        Month = leaveEntitlementDto.Month,
                        Count = leaveEntitlementDto.LeaveCount,
                        Newjoin = 1
                    };

                    _context.HrmLeaveEntitlementRegs.Add(newJoinEntry);
                    await _context.SaveChangesAsync();

                    lastInsertedId = newJoinEntry.LeaveentitlementregId;
                }
            }
            if (leaveEntitlementDto.RegType == "Entitlement")
            {
                var existingHead = await _context.HrmLeaveEntitlementHeads.FirstOrDefaultAsync(h => h.SettingsId == leaveEntitlementDto.BasicSettingsId);
                if (existingHead == null)
                {
                    var newHead = new HrmLeaveEntitlementHead
                    {
                        SettingsId = leaveEntitlementDto.BasicSettingsId,
                        CreatedBy = leaveEntitlementDto.CreatedBy,
                        CreatedDate = DateTime.UtcNow
                    };

                    MapEntitlementHeadFields(newHead, leaveEntitlementDto);
                    _context.HrmLeaveEntitlementHeads.Add(newHead);
                    await _context.SaveChangesAsync();
                    lastInsertedId = newHead.LeaveEntitlementId;
                }
                else
                {
                    MapEntitlementHeadFields(existingHead, leaveEntitlementDto);
                    await _context.SaveChangesAsync();
                    lastInsertedId = existingHead.LeaveEntitlementId;
                }


            }
            await _context.HrmLeaveEntitlementRegs.Where(r => r.LeaveEntitlementId == lastInsertedId && r.Newjoin == 0).ExecuteDeleteAsync();
            await _context.HrmLeaveEntitlementRegs.Where(r => r.LeaveentitlementregId == lastInsertedId && r.Newjoin == 1).ExecuteDeleteAsync();
            await _context.HrmLeaveServicedbasedleaves.Where(r => r.LeaveEntitlementId == lastInsertedId).ExecuteDeleteAsync();
            await _context.HrmLeavePartialPayments.Where(p => p.SettingsDetailsId == lastInsertedId).ExecuteDeleteAsync();
            var exists = await _context.HrmLeaveBasicsettingsDetails.AnyAsync(b => b.SettingsId == leaveEntitlementDto.BasicSettingsId);
            if (!exists)
            {
                var detail = MapBasicSettingsDetail(leaveEntitlementDto);
                _context.HrmLeaveBasicsettingsDetails.Add(detail);
                await _context.SaveChangesAsync();
                lastInsertedId = detail.SettingsDetailsId;
                var basicSetting = await _context.HrmLeaveBasicSettings.FirstOrDefaultAsync(s => s.SettingsId == leaveEntitlementDto.BasicSettingsId);

                if (basicSetting != null)
                {
                    basicSetting.RejoinWarningShow = leaveEntitlementDto.RejoinWarningShow;
                    basicSetting.RejoinWarningShowDaysMax = leaveEntitlementDto.RejoinWarningShowDaysMax;

                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                var recordsToUpdate = await _context.HrmLeaveBasicsettingsDetails.FirstOrDefaultAsync(b => b.SettingsId == leaveEntitlementDto.BasicSettingsId);
                recordsToUpdate.SettingsId = leaveEntitlementDto.BasicSettingsId;
                recordsToUpdate.Lopcheck = leaveEntitlementDto.Lopcheck;
                recordsToUpdate.Gender = leaveEntitlementDto.Gender;
                recordsToUpdate.MaritalStatus = leaveEntitlementDto.MaritalStatus;
                recordsToUpdate.Carryforward = leaveEntitlementDto.JoinDate;
                recordsToUpdate.RolloverCount = leaveEntitlementDto.Rollover;
                recordsToUpdate.CreatedBy = leaveEntitlementDto.CreatedBy;
                recordsToUpdate.OndemandLeaveGrand = leaveEntitlementDto.Leaveondemand;
                recordsToUpdate.EligibilityPeriod = leaveEntitlementDto.Eligibility;
                recordsToUpdate.PredatedApplication = leaveEntitlementDto.Predatedapp;
                recordsToUpdate.Maternity = leaveEntitlementDto.Maternity;
                recordsToUpdate.Compensatory = leaveEntitlementDto.Compensatory;
                recordsToUpdate.MinServiceDays = leaveEntitlementDto.MinServiceDays;
                recordsToUpdate.CsectionMaxLeave = leaveEntitlementDto.CsectionMaxLeave;
                recordsToUpdate.EligibleCount = leaveEntitlementDto.EligibleCount;
                recordsToUpdate.LeaveType = leaveEntitlementDto.LeaveType;
                recordsToUpdate.CompCaryfrwrd = leaveEntitlementDto.CompCaryfrwrd;
                recordsToUpdate.Carryforwardtype = leaveEntitlementDto.CarryForwardType;
                recordsToUpdate.Defaultreturndate = leaveEntitlementDto.Defaultreturndate;
                recordsToUpdate.Attachment = leaveEntitlementDto.Attachment;
                recordsToUpdate.Salaryadvance = leaveEntitlementDto.Salaryadvance;
                recordsToUpdate.Roledeligation = leaveEntitlementDto.Roledeligation;
                recordsToUpdate.CreatedDate = DateTime.UtcNow;
                recordsToUpdate.Weeklyleaveday = leaveEntitlementDto.Weeklyleaveday;
                recordsToUpdate.Ishalfday = leaveEntitlementDto.Ishalfday;
                recordsToUpdate.LeaveInclude = leaveEntitlementDto.LeaveInclude;
                recordsToUpdate.LeavedaysSalaryadvance = leaveEntitlementDto.LeavedaysSalaryadvance;
                recordsToUpdate.SalaryadvanceApplybeforedays = leaveEntitlementDto.SalaryadvanceApplybeforedays;
                recordsToUpdate.Returnrequest = leaveEntitlementDto.Returnrequest;
                recordsToUpdate.Attachmentmandatory = leaveEntitlementDto.Attachmentmandatory;
                recordsToUpdate.Applywithoutbalance = leaveEntitlementDto.Applywithoutbalance;
                recordsToUpdate.Casualholiday = leaveEntitlementDto.Casualholiday;
                recordsToUpdate.PassageeligibilityEnable = leaveEntitlementDto.PassageeligibilityEnable;
                recordsToUpdate.Passageeligibilitydays = leaveEntitlementDto.Passageeligibilitydays;
                recordsToUpdate.PassportRequest = leaveEntitlementDto.PassportRequest;
                recordsToUpdate.EnableLeaveGrander = leaveEntitlementDto.EnableLeaveGrander;
                recordsToUpdate.Autocarryforward = leaveEntitlementDto.Autocarryforward;
                recordsToUpdate.Yearlylimit = leaveEntitlementDto.Yearlylimit;
                recordsToUpdate.Yearlylimitcount = leaveEntitlementDto.Yearlylimitcount;
                recordsToUpdate.ApplicableOnnotice = leaveEntitlementDto.ApplicableOnnotice;
                recordsToUpdate.Blockpreviouslap = leaveEntitlementDto.Blockpreviouslap;
                recordsToUpdate.LeaveEncashment = leaveEntitlementDto.LeaveEncashment;
                recordsToUpdate.Disableyearlylimit = leaveEntitlementDto.Disableyearlylimit;
                recordsToUpdate.Allowleavecancel = leaveEntitlementDto.Allowleavecancel;
                recordsToUpdate.Passportrequireddays = leaveEntitlementDto.Passportrequireddays;
                recordsToUpdate.LeaveAccrual = leaveEntitlementDto.LeaveAccrual;
                recordsToUpdate.ShowcurrentmonthWeekoff = leaveEntitlementDto.ShowcurrentmonthWeekoff;
                recordsToUpdate.Leavebalanceroundoption = leaveEntitlementDto.Leavebalanceroundoption;
                recordsToUpdate.LeaveEncashmentMnthly = leaveEntitlementDto.LeaveEncashmentMnthly;
                recordsToUpdate.LeaveReductionForLateIn = leaveEntitlementDto.LeaveReductionForLateIn;
                await _context.SaveChangesAsync();

            }


            return lastInsertedId;
        }
        private void MapEntitlementHeadFields(HrmLeaveEntitlementHead head, LeaveEntitlementDto dto)
        {
            head.EmployeeType = dto.EmpType;
            head.AllemployeeLeaveCount = dto.AllEmployeeLeave;
            head.DateofJoiningCheck = dto.NewJoin;
            head.JoinedDate = dto.JoinDate;
            head.LeaveCount = dto.NewJoinLeaveCount;
            head.LeaveGrantType = dto.GrantType;
            head.Experiance = dto.Experience;
            head.Monthwise = dto.Monthwise;
            head.NewjoinGranttype = dto.NJNGrantType;
            head.NewjoinLeavecount = dto.NJNLeaveCount;
            head.NewjoinMonthwise = dto.NJNMonthwise;
            head.Laps = dto.Laps;
            head.StartDate = dto.GrantingStartDate;
            head.Eligibility = dto.Eligibility;
            head.EligibilityGrant = dto.EligibilityGrant;
            head.Carryforward = dto.CarryForward;
            head.Cfbasedon = dto.CarryForwardType;
            head.Rollovercount = dto.Rollover;
            head.CarryforwardNj = dto.CarryForwardNJ;
            head.CfbasedonNj = dto.CFBasedOnNJ;
            head.RollovercountNj = dto.RolloverCountNJ;
            head.Firstmonthleavecount = dto.FirstMonthLeaveCount;
            head.Credetedon = dto.CreditedOn;
            head.Yearcount = dto.ProbationML;
            head.JoinmonthdayaftrNyear = dto.NewJoinML;
            head.JoinmonthleaveaftrNyear = dto.OtherML;
            head.Beginningcarryfrwrd = dto.BeginningCarryForward;
            head.Vacationaccrualtype = dto.VacationType;
            head.Previousexperiance = dto.PreviousExperience;
            head.GrantfullleaveforAll = dto.GrantFullLeave;
            head.FullleaveProRata = dto.FullLeaveProRata;
            head.Settingspaymode = dto.SettingsPayMode;
            head.PartialpaymentBalancedays = dto.PartialPaymentBalance;
            head.PartialpaymentNextcount = dto.PartialPaymentNextCount;
            head.LeaveHours = dto.LeaveHours;
            head.LeaveCriteria = dto.LeaveCriteria;
            head.CalculateOnFirst = dto.CalculateOnFirst;
            head.CalculateOnSecond = dto.CalculateOnSecond;
            head.LeaveHoursNewjoin = dto.LeaveHoursNj;
            head.LeaveCriteriaNewjoin = dto.LeaveCriteriaNj;
            head.CalculateOnFirstNewjoin = dto.CalculateOnFirstNj;
            head.CalculateOnSecondNewjoin = dto.CalculateOnSecondNj;
            head.NyearBasedOnJoinDate = dto.NYearAfterJoinDate;
            head.ConsiderProbationDate = dto.ConsiderProbationDate;
            head.IsShowPartialPaymentDays = dto.ShowInLeaveBalance;
            head.ExtraLeaveCountProxy = dto.ExtraLeaveCountProxy;
        }

        private HrmLeaveBasicsettingsDetail MapBasicSettingsDetail(LeaveEntitlementDto leaveEntitlementDto)
        {
            return new HrmLeaveBasicsettingsDetail
            {
                SettingsId = leaveEntitlementDto.BasicSettingsId,
                Lopcheck = leaveEntitlementDto.Lopcheck,
                Gender = leaveEntitlementDto.Gender,
                MaritalStatus = leaveEntitlementDto.MaritalStatus,
                Carryforward = leaveEntitlementDto.JoinDate,
                RolloverCount = leaveEntitlementDto.Rollover,
                CreatedBy = leaveEntitlementDto.CreatedBy,
                OndemandLeaveGrand = leaveEntitlementDto.Leaveondemand,
                EligibilityPeriod = leaveEntitlementDto.Eligibility,
                PredatedApplication = leaveEntitlementDto.Predatedapp,
                Maternity = leaveEntitlementDto.Maternity,
                Compensatory = leaveEntitlementDto.Compensatory,
                MinServiceDays = leaveEntitlementDto.MinServiceDays,
                CsectionMaxLeave = leaveEntitlementDto.CsectionMaxLeave,
                EligibleCount = leaveEntitlementDto.EligibleCount,
                LeaveType = leaveEntitlementDto.LeaveType,
                CompCaryfrwrd = leaveEntitlementDto.CompCaryfrwrd,
                Carryforwardtype = leaveEntitlementDto.CarryForwardType,
                Defaultreturndate = leaveEntitlementDto.Defaultreturndate,
                Attachment = leaveEntitlementDto.Attachment,
                Salaryadvance = leaveEntitlementDto.Salaryadvance,
                Roledeligation = leaveEntitlementDto.Roledeligation,
                CreatedDate = DateTime.UtcNow,
                Weeklyleaveday = leaveEntitlementDto.Weeklyleaveday,
                Ishalfday = leaveEntitlementDto.Ishalfday,
                LeaveInclude = leaveEntitlementDto.LeaveInclude,
                LeavedaysSalaryadvance = leaveEntitlementDto.LeavedaysSalaryadvance,
                SalaryadvanceApplybeforedays = leaveEntitlementDto.SalaryadvanceApplybeforedays,
                Returnrequest = leaveEntitlementDto.Returnrequest,
                Attachmentmandatory = leaveEntitlementDto.Attachmentmandatory,
                Applywithoutbalance = leaveEntitlementDto.Applywithoutbalance,
                Casualholiday = leaveEntitlementDto.Casualholiday,
                PassageeligibilityEnable = leaveEntitlementDto.PassageeligibilityEnable,
                Passageeligibilitydays = leaveEntitlementDto.Passageeligibilitydays,
                PassportRequest = leaveEntitlementDto.PassportRequest,
                EnableLeaveGrander = leaveEntitlementDto.EnableLeaveGrander,
                Autocarryforward = leaveEntitlementDto.Autocarryforward,
                Yearlylimit = leaveEntitlementDto.Yearlylimit,
                Yearlylimitcount = leaveEntitlementDto.Yearlylimitcount,
                ApplicableOnnotice = leaveEntitlementDto.ApplicableOnnotice,
                Blockpreviouslap = leaveEntitlementDto.Blockpreviouslap,
                LeaveEncashment = leaveEntitlementDto.LeaveEncashment,
                Disableyearlylimit = leaveEntitlementDto.Disableyearlylimit,
                Allowleavecancel = leaveEntitlementDto.Allowleavecancel,
                Passportrequireddays = leaveEntitlementDto.Passportrequireddays,
                LeaveAccrual = leaveEntitlementDto.LeaveAccrual,
                ShowcurrentmonthWeekoff = leaveEntitlementDto.ShowcurrentmonthWeekoff,
                Leavebalanceroundoption = leaveEntitlementDto.Leavebalanceroundoption,
                LeaveEncashmentMnthly = leaveEntitlementDto.LeaveEncashmentMnthly,
                LeaveReductionForLateIn = leaveEntitlementDto.LeaveReductionForLateIn

            };
        }





    }

}

