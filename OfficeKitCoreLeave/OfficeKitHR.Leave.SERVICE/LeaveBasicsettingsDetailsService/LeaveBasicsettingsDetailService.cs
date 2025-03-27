using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OFFICEKITCORELEAVE.OfficeKit.Leave.Data;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.Interface.HrmLeaveBasicsettingsDetailInterface;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.MODELS;

namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.SERVICE.HrmLeaveBasicsettingsDetailService
{
    public class LeaveBasicsettingsDetailService : ILeaveBasicSettingsDetails
    {
        private readonly LeaveDBContext _leaveDBContext;
        private readonly IMapper _mapper;
        public LeaveBasicsettingsDetailService (LeaveDBContext leaveDBContext, IMapper mapper_)
        {
            _leaveDBContext = leaveDBContext;
            _mapper = mapper_;
        }

        public Task<int> SaveLeaveBasicSettingsDetails (HrmLeaveBasicsettingsDetailDto LeaveBasicSettingsDetails)
        {
            if (LeaveBasicSettingsDetails.masterId != 0)
            {
                var CheckExistance = _leaveDBContext.HrmLeaveMasterandsettingsLinks.Any (x => x.SettingsId == LeaveBasicSettingsDetails.SettingsId);
                if (CheckExistance == null)
                {
                    var Data = new HrmLeaveMasterandsettingsLinkDto
                    {
                        LeaveMasterId = LeaveBasicSettingsDetails.LeaveMasterId,
                        SettingsId = LeaveBasicSettingsDetails.SettingsId,
                        CreatedBy = LeaveBasicSettingsDetails.CreatedBy,
                        CreatedDate = LeaveBasicSettingsDetails.CreatedDate,
                    };

                    var HrmLeaveMasterandsettingsLinksEntity = _mapper.Map<HrmLeaveMasterandsettingsLink> (Data);

                    var Result = _leaveDBContext.HrmLeaveMasterandsettingsLinks.Add (HrmLeaveMasterandsettingsLinksEntity);
                    _leaveDBContext.SaveChanges ( );
                    var Id = Data.IdMasterandSettingsLink;
                    return Task.FromResult (Id);
                }
                else
                {
                    var LinkDatatoUpdate = _leaveDBContext.HrmLeaveMasterandsettingsLinks.FirstOrDefault (x => x.SettingsId == LeaveBasicSettingsDetails.SettingsId);
                    LinkDatatoUpdate.LeaveMasterId = LeaveBasicSettingsDetails.LeaveMasterId;
                    _leaveDBContext.HrmLeaveMasterandsettingsLinks.Update (LinkDatatoUpdate);
                    _leaveDBContext.SaveChanges ( );
                    return Task.FromResult (LinkDatatoUpdate.IdMasterandSettingsLink);
                }
            }
            else if (LeaveBasicSettingsDetails.headproraid != 0)
            {
                if (LeaveBasicSettingsDetails.regtype == "Prorata")
                {
                    var hrmLeaveEntitlementReg = new HrmLeaveEntitlementRegDto
                    {
                        LeaveEntitlementId = LeaveBasicSettingsDetails.LeaveEntitlementId,
                        Year = LeaveBasicSettingsDetails.Year,
                        Month = LeaveBasicSettingsDetails.Month,
                        Count = LeaveBasicSettingsDetails.Count,
                        Newjoin = 0
                    };
                    var hrmLeaveEntitlementRegEntity = _mapper.Map<HrmLeaveEntitlementReg> (hrmLeaveEntitlementReg);
                    _leaveDBContext.Add (hrmLeaveEntitlementRegEntity);
                    _leaveDBContext.SaveChanges ( );
                    var Id = hrmLeaveEntitlementRegEntity.LeaveentitlementregId;
                }
                else if (LeaveBasicSettingsDetails.regtype == "Service")
                {
                    var hrmLeaveServiceBasedLeave = new HrmLeaveServicedbasedleaveDto
                    {
                        LeaveEntitlementId = LeaveBasicSettingsDetails.LeaveEntitlementId,
                        FromYear = LeaveBasicSettingsDetails.Year,
                        ToYear = LeaveBasicSettingsDetails.Month,
                        LeaveCount = LeaveBasicSettingsDetails.LeaveCount,
                        ExperiancebasedGrant = LeaveBasicSettingsDetails.ExperiancebasedGrant,
                        Experiancebasedrollover = LeaveBasicSettingsDetails.Experiancebasedrollover,
                        Checkcase = LeaveBasicSettingsDetails.Checkcase,
                        ExperiancebasedVacation = LeaveBasicSettingsDetails.ExperiancebasedVacation,
                    };
                    var hrmLeaveServiceBasedLeaveEntity = _mapper.Map<HrmLeaveServicedbasedleave> (hrmLeaveServiceBasedLeave);
                    _leaveDBContext.HrmLeaveServicedbasedleaves.Add (hrmLeaveServiceBasedLeaveEntity);
                    _leaveDBContext.SaveChanges ( );
                    var Id = hrmLeaveServiceBasedLeaveEntity.IdServiceLeave;
                }
                else if (LeaveBasicSettingsDetails.regtype == "Newjoin")
                {
                    var hrmLeaveEntitlementReg = new HrmLeaveEntitlementRegDto
                    {
                        LeaveEntitlementId = LeaveBasicSettingsDetails.LeaveEntitlementId,
                        Year = LeaveBasicSettingsDetails.Year,
                        Month = LeaveBasicSettingsDetails.Month,
                        Count = LeaveBasicSettingsDetails.Count,
                        Newjoin = 1
                    };
                    var hrmLeaveEntitlementRegEntity = _mapper.Map<HrmLeaveEntitlementReg> (hrmLeaveEntitlementReg);
                    _leaveDBContext.Add (hrmLeaveEntitlementRegEntity);
                    _leaveDBContext.SaveChanges ( );
                    var Id = hrmLeaveEntitlementRegEntity.LeaveentitlementregId;
                }
            }
            else if (LeaveBasicSettingsDetails.regtype == "Entitlement")
            {
                int entitlehead = 0;
                var checkEntitleExistance = _leaveDBContext.HrmLeaveEntitlementHeads
                    .Any (x => x.SettingsId == LeaveBasicSettingsDetails.SettingsId);

                var hrmLeaveEntitlementHead = new HrmLeaveEntitlementHeadDto
                {
                    SettingsId = LeaveBasicSettingsDetails.SettingsId,
                    EmployeeType = LeaveBasicSettingsDetails.EmployeeType,
                    AllemployeeLeaveCount = LeaveBasicSettingsDetails.AllemployeeLeaveCount,
                    DateofJoiningCheck = LeaveBasicSettingsDetails.DateofJoiningCheck,
                    JoinedDate = LeaveBasicSettingsDetails.JoinedDate,
                    LeaveCount = LeaveBasicSettingsDetails.LeaveCount,
                    CreatedBy = LeaveBasicSettingsDetails.CreatedBy,
                    LeaveGrantType = LeaveBasicSettingsDetails.LeaveGrantType,
                    Experiance = LeaveBasicSettingsDetails.Experiance,
                    Monthwise = LeaveBasicSettingsDetails.Monthwise,
                    NewjoinGranttype = LeaveBasicSettingsDetails.NewjoinGranttype,
                    NewjoinLeavecount = LeaveBasicSettingsDetails.NewjoinLeavecount,
                    NewjoinMonthwise = LeaveBasicSettingsDetails.NewjoinMonthwise,
                    Laps = LeaveBasicSettingsDetails.Laps,
                    StartDate = LeaveBasicSettingsDetails.StartDate,
                    Eligibility = LeaveBasicSettingsDetails.Eligibility,
                    EligibilityGrant = LeaveBasicSettingsDetails.EligibilityGrant,
                    Carryforward = LeaveBasicSettingsDetails.Carryforward,
                    Cfbasedon = LeaveBasicSettingsDetails.Cfbasedon,
                    Rollovercount = LeaveBasicSettingsDetails.Rollovercount,
                    CarryforwardNj = LeaveBasicSettingsDetails.CarryforwardNj,
                    CfbasedonNj = LeaveBasicSettingsDetails.CfbasedonNj,
                    RollovercountNj = LeaveBasicSettingsDetails.RollovercountNj,
                    CreatedDate = DateTime.UtcNow,
                    Firstmonthleavecount = LeaveBasicSettingsDetails.Firstmonthleavecount,
                    Credetedon = LeaveBasicSettingsDetails.Credetedon,
                    Yearcount = LeaveBasicSettingsDetails.Yearcount,
                    JoinmonthdayaftrNyear = LeaveBasicSettingsDetails.JoinmonthdayaftrNyear,
                    JoinmonthleaveaftrNyear = LeaveBasicSettingsDetails.JoinmonthleaveaftrNyear,
                    Beginningcarryfrwrd = LeaveBasicSettingsDetails.Beginningcarryfrwrd,
                    Vacationaccrualtype = LeaveBasicSettingsDetails.Vacationaccrualtype,
                    Previousexperiance = LeaveBasicSettingsDetails.Previousexperiance,
                    GrantfullleaveforAll = LeaveBasicSettingsDetails.GrantfullleaveforAll,
                    FullleaveProRata = LeaveBasicSettingsDetails.FullleaveProRata,
                    Settingspaymode = LeaveBasicSettingsDetails.Settingspaymode,
                    PartialpaymentBalancedays = LeaveBasicSettingsDetails.PartialpaymentBalancedays,
                    PartialpaymentNextcount = LeaveBasicSettingsDetails.PartialpaymentNextcount,
                    LeaveHours = LeaveBasicSettingsDetails.LeaveHours,
                    LeaveCriteria = LeaveBasicSettingsDetails.LeaveCriteria,
                    CalculateOnFirst = LeaveBasicSettingsDetails.CalculateOnFirst,
                    CalculateOnSecond = LeaveBasicSettingsDetails.CalculateOnSecond,
                    LeaveHoursNewjoin = LeaveBasicSettingsDetails.LeaveHoursNewjoin,
                    LeaveCriteriaNewjoin = LeaveBasicSettingsDetails.LeaveCriteriaNewjoin,
                    CalculateOnFirstNewjoin = LeaveBasicSettingsDetails.CalculateOnFirstNewjoin,
                    CalculateOnSecondNewjoin = LeaveBasicSettingsDetails.CalculateOnSecondNewjoin,
                    NyearBasedOnJoinDate = LeaveBasicSettingsDetails.NyearBasedOnJoinDate,
                    ConsiderProbationDate = LeaveBasicSettingsDetails.ConsiderProbationDate,
                    IsShowPartialPaymentDays = LeaveBasicSettingsDetails.IsShowPartialPaymentDays,
                    ExtraLeaveCountProxy = LeaveBasicSettingsDetails.ExtraLeaveCountProxy
                };

                var hrmLeaveEntitlementHeadEntity = _mapper.Map<HrmLeaveEntitlementHead> (hrmLeaveEntitlementHead);

                if (!checkEntitleExistance)
                {
                    _leaveDBContext.HrmLeaveEntitlementHeads.Add (hrmLeaveEntitlementHeadEntity);
                }
                else
                {
                    _leaveDBContext.HrmLeaveEntitlementHeads.Update (hrmLeaveEntitlementHeadEntity);
                }

                _leaveDBContext.SaveChanges ( );
                var id = hrmLeaveEntitlementHeadEntity.LeaveEntitlementId;
                //entitlehead = _leaveDBContext.HrmLeaveEntitlementHeads.ToListAsync().Result.Where(x => x.SettingsId == LeaveBasicSettingsDetails.SettingsId).FirstOrDefault().LeaveEntitlementId;

                entitlehead = _leaveDBContext.HrmLeaveEntitlementHeads.FirstOrDefaultAsync (x => x.SettingsId == LeaveBasicSettingsDetails.SettingsId).Result?.LeaveEntitlementId ?? 0;
                var hrmLeaveEntitlementRegExistance = _leaveDBContext.HrmLeaveEntitlementRegs.Any(x => x.LeaveEntitlementId == LeaveBasicSettingsDetails.LeaveEntitlementId && x.Newjoin == 0);
                if(hrmLeaveEntitlementRegExistance)
                {
                    var hrmLeaveEntitlementRegData = _leaveDBContext.HrmLeaveEntitlementRegs.Where (x => x.LeaveEntitlementId == LeaveBasicSettingsDetails.LeaveEntitlementId && x.Newjoin == 0);
                    var mappedData = _mapper.Map<HrmLeaveEntitlementReg> (hrmLeaveEntitlementRegData);
                    var result = _leaveDBContext.HrmLeaveEntitlementRegs.Remove (mappedData);
                }
                var hrmLeaveEntitlementRegExistanceNewJoin = _leaveDBContext.HrmLeaveEntitlementRegs.Any (x => x.LeaveEntitlementId == LeaveBasicSettingsDetails.LeaveEntitlementId && x.Newjoin == 1);
                if (hrmLeaveEntitlementRegExistanceNewJoin)
                {
                    var hrmLeaveEntitlementRegData = _leaveDBContext.HrmLeaveEntitlementRegs.Where (x => x.LeaveEntitlementId == LeaveBasicSettingsDetails.LeaveEntitlementId && x.Newjoin == 1);
                    var mappedData = _mapper.Map<HrmLeaveEntitlementReg> (hrmLeaveEntitlementRegData);
                    var result = _leaveDBContext.HrmLeaveEntitlementRegs.Remove (mappedData);
                }
            }
            return Task.FromResult (0);
        }
    }
}
