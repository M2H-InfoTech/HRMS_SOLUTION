using EMPLOYEE_INFORMATION.Data;
using HRMS.EmployeeInformation.DTO.DTOs.WorkFlow;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Models.Models.Entity;
using LEAVE.Dto;
using LEAVE.Helpers;
using Microsoft.EntityFrameworkCore;

namespace LEAVE.Repository.LeaveBalance
{
    public class LeaveBalanceRepository : ILeaveBalanceRepository
    {

        private readonly EmployeeDBContext _context;
        private readonly HttpClient _httpClient;
        private readonly ExternalApiService _externalApiService;
        public LeaveBalanceRepository(EmployeeDBContext dbContext, HttpClient httpClient, ExternalApiService externalApiServic)
        {
            _context = dbContext;
            _httpClient = httpClient;
            _externalApiService = externalApiServic;
        }


        public async Task<List<RetrieveBranchDetailsDto>> RetrieveBranchDetails(int instID, int branchID)
        {
            var topBranches = await (
                from mb in _context.MasterBranchDetails
                join hvt in _context.HighLevelViewTables on mb.SubBranchId equals hvt.LevelSevenId
                where mb.InstId == instID && (mb.BranchId == branchID || branchID == 0)
                group mb by mb.BranchName into g
                select g.OrderByDescending(x => x.BranchId).FirstOrDefault()
            ).ToListAsync();

            var result = topBranches.Select(topBranch => new RetrieveBranchDetailsDto
            {
                InstId = topBranch.InstId,
                BranchId = topBranch.BranchId,
                BranchCode = topBranch.BranchCode,
                SubBranchId = topBranch.SubBranchId,
                BranchName = topBranch.BranchName,
                Location = topBranch.Location,
                Address = topBranch.Address,
                DateOfBranchOpen = topBranch.DateOfBranchOpen,
                Currency = topBranch.Currency,
                Latitude = topBranch.Latitude,
                Longitude = topBranch.Longitude,
                Circumference = topBranch.Circumference,
                Email = topBranch.Email,
                ExtensionCode = topBranch.ExtensionCode,
                Phone = topBranch.Phone,
                Logo = topBranch.Logo,
                TimeOffSetId = topBranch.TimeOffSetId
            }).ToList();

            return result;
        }

        public async Task<List<LeaveApplicationDto>> GetLeaveApplicationsAsync(int employeeId, int leaveId, string approvalStatus, string? flowStatus, DateTime? leaveFrom, DateTime? leaveTo)
        {
            IQueryable<LeaveApplicationDto> query;
            IQueryable<LeaveApplicationDto> groupedQuery;
            var baseLeaveData =
                from a in _context.LeaveApplication00s
                join d in _context.LeaveApplication02s on a.LeaveApplicationId equals d.LeaveApplicationId into adGroup
                from d in adGroup.DefaultIfEmpty()
                join b in _context.HrmLeaveMasters on a.LeaveId equals b.LeaveMasterId into abGroup
                from b in abGroup.DefaultIfEmpty()
                join c in _context.EmployeeDetails on a.EmployeeId equals c.EmpId into acGroup
                from c in acGroup.DefaultIfEmpty()
                select new
                {
                    a.LeaveApplicationId,
                    a.LeaveFrom,
                    a.LeaveTo,
                    a.NoOfLeaveDays,
                    a.Reason,
                    a.TimeMode,
                    a.ReturnDate,
                    a.IdproxyLeave,
                    a.ApprovalStatus,
                    a.Cancelstatus,
                    a.EmployeeId,
                    a.LeaveId,
                    a.RequestId,
                    a.FlowStatus,
                    LeaveCode = b != null ? b.LeaveCode : null,
                    Description = b != null ? b.Description : null,
                    c.Name,
                    c.EmpId,
                    LeaveDate = d != null ? d.LeaveDate : (DateTime?)null
                };

            groupedQuery =
                           from bl in baseLeaveData
                           join lc in _context.Leavecancel00s on bl.LeaveApplicationId equals lc.LeaveApplicationId into lcGroup
                           from lc in lcGroup.DefaultIfEmpty()
                           where bl.EmployeeId == employeeId
                               && bl.LeaveId == leaveId
                               && (
                                   (approvalStatus == "C" || approvalStatus == "PC")
                                   && (bl.Cancelstatus == approvalStatus || (bl.Cancelstatus != null && !bl.Cancelstatus.Equals("C")))
                                   || (!new[] { "C", "PC" }.Contains(approvalStatus)
                                       && (bl.ApprovalStatus == approvalStatus || string.IsNullOrEmpty(approvalStatus))
                                       && (bl.Cancelstatus ?? "P").Equals("P"))
                               )
                               && !bl.ApprovalStatus.Equals("D")
                               && (string.IsNullOrEmpty(flowStatus) || bl.FlowStatus == flowStatus)
                               && (
                                   (bl.LeaveDate >= leaveFrom && bl.LeaveDate <= leaveTo)
                                   || (leaveFrom == null && leaveTo == null)
                               )
                           group new { bl, lc } by new
                           {
                               bl.LeaveCode,
                               bl.Description,
                               bl.LeaveApplicationId,
                               Lcfromdate = lc != null ? lc.Lcfromdate : (DateTime?)null,
                               Lctodate = lc != null ? lc.Lctodate : (DateTime?)null,
                               bl.LeaveFrom,
                               bl.LeaveTo,
                               bl.NoOfLeaveDays,
                               bl.Reason,
                               bl.ReturnDate,
                               bl.IdproxyLeave,
                               DayTypeVal = lc != null ? lc.Lcdaytype : bl.TimeMode,
                               bl.TimeMode,
                               bl.Name,
                               bl.EmpId,
                               bl.Cancelstatus,
                               bl.ApprovalStatus,
                               bl.RequestId,
                               Lcdays = lc != null ? lc.Lcdays : (decimal?)null,
                               LeaveCancelId = lc != null ? lc.LeavecancelId : bl.LeaveApplicationId
                           } into g
                           orderby g.Key.LeaveApplicationId descending
                           select new LeaveApplicationDto
                           {
                               CaseCode = "R",
                               LeaveCode = g.Key.LeaveCode,
                               Description = g.Key.Description,
                               LeaveApplicationId = g.Key.LeaveApplicationId,
                               LeaveFrom = g.Key.Lcfromdate ?? g.Key.LeaveFrom,
                               LeaveTo = g.Key.Lctodate ?? g.Key.LeaveTo,
                               NoOfLeaveDays = g.Key.NoOfLeaveDays ?? 0,
                               Reason = g.Key.Reason,
                               TimeMode = g.Key.DayTypeVal,
                               Daytype = g.Key.DayTypeVal == 1 ? "Full Day" :
                                         g.Key.DayTypeVal == 2 ? "Half Day" : "NA",
                               ReturnDate = g.Key.ReturnDate,
                               IDProxyLeave = g.Key.IdproxyLeave,
                               ApprovalStatus = g.Key.ApprovalStatus,
                               Name = g.Key.Name,
                               EmployeeId = g.Key.EmpId,
                               ApprovalStatusDesc =
                                   g.Key.Cancelstatus == "C" ? "Cancelled" :
                                   g.Key.Cancelstatus == "PC" ? "Partially Cancelled" :
                                   g.Key.ApprovalStatus == "P" ? "Pending" :
                                   g.Key.ApprovalStatus == "R" ? "Rejected" :
                                   g.Key.ApprovalStatus == "A" ? "Approved" :
                                   g.Key.ApprovalStatus == "D" ? "Deleted" : "NA",
                               RequestId = g.Key.RequestId,
                               FileName = "",
                               CancelledDays = g.Key.Lcdays ?? 0,
                               LeaveCancelId = g.Key.LeaveCancelId
                           };



            query = groupedQuery;

            return await query.ToListAsync();
        }
        private async Task<bool> IsLinkLevelExists(int? roleId)
        {
            var exists = await _context.EntityAccessRights02s
                .Where(s => s.RoleId == roleId && s.LinkLevel == 15)
                .Select(x => x.LinkLevel)
                .FirstOrDefaultAsync(); // Returns default value if no data is found

            return exists > byte.MinValue; // `exists` will be 0 if not found
        }
        public async Task<List<EmployeeDto>> GetLeaveAssignmentEligibleEmployeesAsync(int entryByUserId, int? roleId)
        {
            var lnkLevelExists = await IsLinkLevelExists (roleId);
            if (lnkLevelExists)
            {
                var result = await (
                    from a in _context.EmployeeDetails
                    join b in _context.HighLevelViews
                        on a.LastEntity equals b.LastEntityId into highJoin
                    from b in highJoin.DefaultIfEmpty ( )
                    where _context.HrmLeaveBasicsettingsaccesses.Any (x => x.EmployeeId == a.EmpId)
                    select new EmployeeDto
                    {
                        EmpId = a.EmpId,
                        EmpCode = a.EmpCode,
                        Name = a.Name,
                        LevelOne = b.LevelOneDescription,
                        LevelTwo = b.LevelTwoDescription,
                        LevelThree = b.LevelThreeDescription
                    }).ToListAsync ( );
                return result;
            }
            else
            {
                return new List<EmployeeDto> ( );
            }
        }
        public async Task<WorkFlowMainDto> EmployeeDetailsWorkFlow (EmployeeDetailWFDto? dto)
        {
            WorkFlowMainDto workFlowMainDto = new WorkFlowMainDto ( );
            if (dto.TransactionType == "Resignation")
            {
                var raw = await (
                    from e in _context.EmployeeDetails
                    join d in _context.DesignationDetails on e.DesigId equals d.LinkId
                    join b in _context.BranchDetails on e.BranchId equals b.LinkId
                    join g in _context.GradeDetails on e.GradeId equals g.LinkId into gradeJoin
                    from g in gradeJoin.DefaultIfEmpty ( )
                    join i in _context.HrEmpImages on e.EmpId equals i.EmpId into imageJoin
                    from i in imageJoin.DefaultIfEmpty ( )
                    join ad in _context.HrEmpAddresses on e.EmpId equals ad.EmpId into addressJoin
                    from ad in addressJoin.DefaultIfEmpty ( )
                    join mbd in _context.MasterBranchDetails on (e.BranchId ?? 0) equals mbd.SubBranchId into branchDetailsJoin
                    from mbd in branchDetailsJoin.DefaultIfEmpty ( )
                    where e.EmpId == dto.EmployeeID
                    select new
                    {
                        e.EmpId,
                        e.Name,
                        e.EmpCode,
                        Designation = d.Designation,
                        e.DateOfBirth,
                        e.JoinDt,
                        e.NoticePeriod,
                        Branch = b.Branch,
                        ImageUrl = i != null ? i.ImageUrl : null,
                        Grade = g != null ? g.Grade : null,
                        Address = ad != null ? ad.Add1 : "",
                        ExtensionCode = mbd != null ? mbd.ExtensionCode : "NA"
                    }
                ).FirstOrDefaultAsync();

                // STEP 2: Do formatting and async calls outside the query
                EmployeeResignationDto dtoResult = null;

                if (raw != null)
                {
                    dtoResult = new EmployeeResignationDto
                    {
                        Emp_Id = raw.EmpId,
                        Name = raw.Name.ToUpper ( ),
                        Emp_Code = raw.EmpCode,
                        Designation = raw.Designation.ToUpper ( ),
                        DateOfBirth = raw.DateOfBirth?.ToString ("dd-MM-yyyy") ?? "",
                        Join_Dt = raw.JoinDt?.ToString ("dd-MM-yyyy") ?? "",
                        ServiceLength = await GetEmployeeServiceLength (raw.EmpId),
                        BranchName = raw.Branch.ToUpper ( ),
                        Url = raw.ImageUrl,
                        Grade = raw.Grade,
                        Address = raw.Address,
                        LOS = raw.JoinDt.HasValue ? await GetServiceLength (raw.JoinDt.Value) : "",
                        ExtensionCode = raw.ExtensionCode,
                        NoticePeriod = raw.NoticePeriod ?? 0
                    };
                }
                workFlowMainDto.employeeResignationDto = dtoResult;
                return workFlowMainDto;

            }
            else
            {
                var rawData = await (
                    from e in _context.EmployeeDetails
                    join d in _context.DesignationDetails on e.DesigId equals d.LinkId
                    join b in _context.BranchDetails on e.BranchId equals b.LinkId
                    join g in _context.GradeDetails on e.GradeId equals g.LinkId into gradeJoin
                    from g in gradeJoin.DefaultIfEmpty ( )
                    join i in _context.HrEmpImages on e.EmpId equals i.EmpId into imageJoin
                    from i in imageJoin.DefaultIfEmpty ( )
                    join ad in _context.HrEmpAddresses on e.EmpId equals ad.EmpId into addressJoin
                    from ad in addressJoin.DefaultIfEmpty ( )
                    where e.EmpId == dto.EmployeeID
                    select new EmployeeBasicDto
                    {
                        EmpId = e.EmpId,
                        Name = e.Name,
                        EmpCode = e.EmpCode,
                        Designation = d.Designation,
                        DateOfBirth = e.DateOfBirth,
                        JoinDt = e.JoinDt,
                        Branch = b.Branch,
                        ImageUrl = i != null ? i.ImageUrl : null,
                        Grade = g != null ? g.Grade : null,
                        Address = ad != null ? ad.Add1 : ""
                    }
                ).FirstOrDefaultAsync ( );

                // STEP 2: Map and format outside the query
                EmployeeDetailDto employeeDto = new EmployeeDetailDto ( );

                if (rawData != null)
                {
                    employeeDto = new EmployeeDetailDto
                    {
                        Emp_Id = rawData.EmpId,
                        Name = rawData.Name.ToUpper ( ),
                        Emp_Code = rawData.EmpCode,
                        Designation = rawData.Designation.ToUpper ( ),
                        DateOfBirth = rawData.DateOfBirth?.ToString ("dd-MM-yyyy") ?? "",
                        Join_Dt = rawData.JoinDt?.ToString ("dd-MM-yyyy") ?? "",
                        Join_Date = rawData.JoinDt?.ToString ("dd/MM/yyyy") ?? "",
                        ServiceLength = await GetEmployeeServiceLength (rawData.EmpId),
                        BranchName = rawData.Branch.ToUpper ( ),
                        Url = rawData.ImageUrl,
                        Grade = rawData.Grade,
                        Address = rawData.Address,
                        LOS = rawData.JoinDt.HasValue ? await GetServiceLength (rawData.JoinDt.Value) : ""
                    };
                    workFlowMainDto.EmployeeDetailDto = employeeDto;
                }
                if (dto.TransactionType == "Leave_App")
                {
                    WorkFlowActivityFlowLeaveAppDto dt = new WorkFlowActivityFlowLeaveAppDto ( );
                    dt.SpecialWorkFlowID = dto.specialWorkFlowId;
                    dt.EmployeeID = dto.EmployeeID;
                    dt.ReturnWorkFlowTable = true;
                    dt.TransactionType = dto.TransactionType;
                    var result = await WorkFlowActivityFlowLeaveApp (dt);
                    workFlowMainDto.leaveWorkflowDtos = result;
                    return workFlowMainDto;
                    //return result;
                }
                else if (dto.TransactionType == "Probation")
                {
                    WorkFlowActivityProbationInputDto dt = new WorkFlowActivityProbationInputDto ( );
                    var result = WorkFlowActivityProbation (dt).Result;
                    workFlowMainDto.probationWorkflowDisplayDtos = result;
                    return workFlowMainDto;
                }
                else if (dto.TransactionType == "PayRoll" || dto.TransactionType == "LeaveSalaryAdvanceProcess" || dto.TransactionType == "Finalsettlement")
                {
                    // Check if ProcessPayRoll00 record exists
                    if (_context.ProcessPayRoll00s.Any (p => p.ProcessPayRollId == dto.requestId))
                    {
                        // Update employeeId based on ProcessPayRoll01
                        dto.EmployeeID = _context.ProcessPayRoll01s
                            .Where (p => p.ProcessPayRollId == dto.requestId)
                            .Select (p => p.EmployeeId)
                            .Take (1)
                            .FirstOrDefault ( );
                    }
                }
                else
                {
                    // Call WorkFlowActivityFlow stored procedure (default case)
                    //return _context.Set<int> ( )
                    //    .FromSqlRaw (
                    //        "EXEC WorkFlowActivityFlow @emp_id = {0}, @TransactionType = {1}, @ReturnWorkFlowTable = {2}, @SpecialWorkFlowID = {3}, @SpecialWorkFlowSubID = {4}, @RequestID = {5}, @CommonDate = {6}, @GrievanceWistleBlower = {7}",
                    // dto.EmployeeID, dto.TransactionType, 1, dto.specialWorkFlowId, dto.SpecialWorkFlowSubID, dto.requestId, dto.CommonDate, dto.GrievanceWistleBlower);
                }

            }

            return workFlowMainDto;
        }
        public Task<int?> GetEmployeeParametersettingsNew (int? employeeId, string code, string type)
        {


            int? value = (from a in _context.CompanyParameters02s
                          join b in _context.CompanyParameters on a.ParamId equals b.Id
                          where b.ParameterCode == code && b.Type == type && a.EmpId == employeeId
                          select (int?)a.Value).FirstOrDefault ( );


            if (!value.HasValue || value == 0)
            {
                var entity = _context.EmployeeDetails
                               .Where (a => a.EmpId == employeeId)
                               .Select (a => a.EmpEntity)
                               .FirstOrDefault ( );

                if (!string.IsNullOrEmpty (entity))
                {
                    var entityList = entity.Split (',').ToList ( );

                    value = (from a in _context.CompanyParameters01s
                             join b in _context.CompanyParameters on a.ParamId equals b.Id into paramJoin
                             from b in paramJoin.DefaultIfEmpty ( )
                             where entityList.Contains (a.LinkId.ToString ( )) && a.LevelId != 1 &&
                                   b.ParameterCode == code && b.Type == type
                             orderby a.LevelId descending
                             select (int?)a.Value).FirstOrDefault ( );
                }
            }


            if (!value.HasValue || value == 0)
            {
                var firstEntity = _context.EmployeeDetails
                                    .Where (a => a.EmpId == employeeId)
                                    .Select (a => a.EmpFirstEntity)
                                    .FirstOrDefault ( );

                if (!string.IsNullOrEmpty (firstEntity))
                {
                    var firstEntityList = firstEntity.Split (',').ToList ( );

                    value = (from a in _context.CompanyParameters01s
                             join b in _context.CompanyParameters on a.ParamId equals b.Id into paramJoin
                             from b in paramJoin.DefaultIfEmpty ( )
                             where firstEntityList.Contains (a.LinkId.ToString ( )) &&
                                   b.ParameterCode == code && b.Type == type
                             orderby a.LevelId descending
                             select (int?)a.Value).FirstOrDefault ( );
                }
            }

            // If still null or 0, check Company-level parameters
            if (!value.HasValue || value == 0)
            {
                value = _context.CompanyParameters
                           .Where (p => p.ParameterCode == code && p.Type == type)
                           .Select (p => (int?)p.Value)
                           .FirstOrDefault ( );
            }

            return Task.FromResult (value);

        }

        private async Task<List<LeaveWorkflowDto>> WorkFlowActivityFlowLeaveApp (WorkFlowActivityFlowLeaveAppDto dto)
        {
            dto.WorkflowType = 1;
            var tempLevel = new TempLevelDto ( );
            var workflowInsertList = new List<TempLeaveWorkFlowDto> ( );
            dto.TransactionId = await _externalApiService.GetTransactionIdByTransactionTypeAsync (dto.TransactionType);
            var query = await (
                                from emp in _context.HrEmpMasters
                                from level in _context.HighLevelViewTables
                                where emp.EmpId == dto.EmployeeID &&
                                      emp.DesigId ==
                                          (level.LevelSixId == 0 ? level.LevelFiveId :
                                           level.LevelSevenId == 0 ? level.LevelSixId :
                                           level.LevelEightId == 0 ? level.LevelSevenId :
                                           level.LevelNineId == 0 ? level.LevelEightId :
                                           level.LevelTenId == 0 ? level.LevelNineId :
                                           level.LevelElevenId == 0 ? level.LevelTenId :
                                           level.LevelTwelveId == 0 ? level.LevelElevenId :
                                           level.LevelTwelveId)
                                select new TempLevelDto
                                {
                                    LevelOneId = level.LevelOneId,
                                    LevelTwoId = level.LevelTwoId,
                                    LevelThreeId = level.LevelThreeId,
                                    LevelFourId = level.LevelFourId,
                                    LevelFiveId = level.LevelFiveId,
                                    LevelSixId = level.LevelSixId,
                                    LevelSevenId = level.LevelSevenId,
                                    LevelEightId = level.LevelEightId,
                                    LevelNineId = level.LevelNineId,
                                    LevelTenId = level.LevelTenId,
                                    LevelElevenId = level.LevelElevenId,
                                    LevelTwelveId = level.LevelTwelveId
                                }
                            ).FirstOrDefaultAsync ( );

            var linkToEntity = await _context.ParamWorkFlow00s
                                .Where (p => p.TransactionId == dto.TransactionId)
                                .Select (p => p.EntityLevel)
                                .FirstOrDefaultAsync ( );

            bool condition = dto.SpecialWorkFlowID == 0 ||
                 (dto.SpecialWorkFlowID > 0 &&
                  GetEmployeeParametersettingsNew (dto.EmployeeID, "DisableSpecialworkflow", "LEV").Result == 1);
            if (condition)
            {
                if (!string.IsNullOrEmpty (linkToEntity) && linkToEntity != "0" && linkToEntity != "15")
                {
                    var linkEntityList = linkToEntity.Split (',').Select (int.Parse).ToList ( );

                    var levelMap = new Dictionary<int, int?>
                    {
                         { 1, tempLevel.LevelOneId },
                         { 2, tempLevel.LevelTwoId },
                         { 3, tempLevel.LevelThreeId },
                         { 4, tempLevel.LevelFourId },
                         { 5, tempLevel.LevelFiveId },
                         { 6, tempLevel.LevelSixId },
                         { 7, tempLevel.LevelSevenId },
                         { 8, tempLevel.LevelEightId },
                         { 9, tempLevel.LevelNineId },
                         { 10, tempLevel.LevelTenId },
                         { 11, tempLevel.LevelElevenId },
                         { 12, tempLevel.LevelTwelveId },
                    };

                    foreach (var item in linkEntityList)
                    {
                        if (levelMap.ContainsKey (item) && levelMap[item].HasValue)
                        {
                            var linkId = levelMap[item].Value;

                            var workflow = await _context.ParamWorkFlow01s
                                .Where (p => p.LinkId == linkId && p.TransactionId == dto.TransactionId)
                                .Select (p => p.WorkFlowId)
                                .FirstOrDefaultAsync ( );

                            if (workflow > 0)
                            {
                                dto.WorkFlowID = (int)workflow;
                                break;
                            }
                        }
                        else if (item == 13)
                        {
                            var workflow = await _context.ParamWorkFlow02s
                                .Where (p => p.LinkEmpId == dto.EmployeeID && p.TransactionId == dto.TransactionId)
                                .Select (p => p.WorkFlowId)
                                .FirstOrDefaultAsync ( );

                            if (workflow > 0)
                            {
                                dto.WorkFlowID = (int)workflow;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (dto.TransactionType == "Leave_App" || dto.TransactionType == "Leave_Cancel")
                {
                    if (dto.SpecialWorkFlowID > 0 && GetEmployeeParametersettingsNew (dto.EmployeeID, "DisableSpecialworkflow", "LEV").Result != 1)
                    {
                        string? linkToSpecialEntity = await _context.SpecialWorkFlows
                            .Where (sw => sw.TransactionId == dto.TransactionId && sw.LeaveType == dto.SpecialWorkFlowID)
                            .Select (sw => sw.EntityLevel)
                            .FirstOrDefaultAsync ( );

                        var specialLevels = linkToSpecialEntity?.Split (',').Select (int.Parse).ToList ( ) ?? new List<int> ( );

                        if (specialLevels.Contains (13))
                        {
                            var specialWF02 = _context.SpecialWorkFlow02s
                                .FirstOrDefault (w => w.LeaveType == dto.SpecialWorkFlowID && w.LinkEmpId == dto.EmployeeID && w.TransactionId == dto.TransactionId);

                            if (specialWF02 != null)
                                dto.WorkFlowID = specialWF02.WorkFlowId;
                        }

                        if (dto.WorkFlowID == 0)
                        {
                            dto.WorkFlowID = await _context.SpecialWorkFlows
                             .Where (sw => sw.LeaveType == dto.SpecialWorkFlowID && sw.TransactionId == dto.TransactionId)
                             .Select (sw => sw.WorkFlowId)
                             .FirstOrDefaultAsync ( ) ?? 0; // Default to 0 if null, or handle differently
                        }
                    }
                }

                if (dto.WorkFlowID == 0 && !string.IsNullOrEmpty (linkToEntity) && linkToEntity != "0" && linkToEntity != "15")
                {
                    var linkEntityList = linkToEntity.Split (',').Select (int.Parse).ToList ( );

                    var levelMap = new Dictionary<int, int?>
             {
                 { 1, tempLevel.LevelOneId },
                 { 2, tempLevel.LevelTwoId },
                 { 3, tempLevel.LevelThreeId },
                 { 4, tempLevel.LevelFourId },
                 { 5, tempLevel.LevelFiveId },
                 { 6, tempLevel.LevelSixId },
                 { 7, tempLevel.LevelSevenId },
                 { 8, tempLevel.LevelEightId },
                 { 9, tempLevel.LevelNineId },
                 { 10, tempLevel.LevelTenId },
                 { 11, tempLevel.LevelElevenId },
                 { 12, tempLevel.LevelTwelveId },
             };

                    foreach (var level in linkEntityList)
                    {
                        if (levelMap.ContainsKey (level) && levelMap[level].HasValue)
                        {
                            int linkId = levelMap[level].Value;

                            var found = await _context.ParamWorkFlow01s
                                .Where (p => p.LinkId == linkId && p.TransactionId == dto.TransactionId)
                                .Select (p => p.WorkFlowId)
                                .FirstOrDefaultAsync ( );

                            if (found > 0)
                            {
                                dto.WorkFlowID = (int)found;
                                break;
                            }
                        }
                        else if (level == 13)
                        {
                            var fallback = await _context.ParamWorkFlow02s
                                .Where (p => p.LinkEmpId == dto.EmployeeID && p.TransactionId == dto.TransactionId)
                                .Select (p => p.WorkFlowId)
                                .FirstOrDefaultAsync ( );

                            if (fallback > 0)
                            {
                                dto.WorkFlowID = (int)fallback;
                                break;
                            }
                        }
                    }
                }
                if (dto.WorkflowType == 2)
                {
                    dto.WorkFlowID = await _context.ParamWorkFlow00s
                        .Where (p => p.TransactionId == dto.TransactionId)
                        .Select (p => p.SecondLevelWorkflowId)
                        .FirstOrDefaultAsync ( );
                }
                else if (dto.WorkFlowID == 0)
                {
                    dto.WorkFlowID = await _context.ParamWorkFlow00s
                        .Where (p => p.TransactionId == dto.TransactionId)
                        .Select (p => p.WorkFlowId)
                        .FirstOrDefaultAsync ( );
                }
                if (dto.WorkFlowID > 0)
                {
                    var wfDetails = await _context.WorkFlowDetails
                        .Where (w => w.WorkFlowId == dto.WorkFlowID && w.IsActive == true)
                        .Select (w => new
                        {
                            w.FinalRule,
                            w.HierarchyType,
                            ForwardNext = w.ForwardNext ?? 0
                        })
                        .FirstOrDefaultAsync ( );

                    if (wfDetails != null)
                    {
                        dto.FinalRule = wfDetails.FinalRule;
                        dto.HierarchyType = wfDetails.HierarchyType;
                        dto.ForwardNext = wfDetails.ForwardNext;

                        bool isNewWorkflow = _context.WorkFlowDetails
                            .Any (w => w.WorkFlowId == dto.WorkFlowID && w.OldType == 0);

                        if (isNewWorkflow)
                        {
                            workflowInsertList = await (from a in _context.WorkFlowDetails01s
                                                        join c in _context.WorkFlowDetails
                                                          on a.WorkFlowId equals c.WorkFlowId
                                                        where a.WorkFlowId == dto.WorkFlowID && c.OldType == 0
                                                        let approver = GetRoleBasedEmployee (dto.EmployeeID, a.ParemeterId).Result
                                                        where approver != 0
                                                        orderby a.Rules, a.RuleOrder
                                                        select new TempLeaveWorkFlowDto
                                                        {
                                                            RequestId = dto.RequestID,
                                                            ShowStatus = (c.HierarchyType == false || (c.HierarchyType == true && a.RuleOrder == 1)) ? 1 : 0,
                                                            ApprovalStatus = "P",
                                                            Rule = a.Rules,
                                                            RuleOrder = a.RuleOrder,
                                                            HierarchyType = c.HierarchyType,
                                                            Approver = approver,
                                                            ApprovalRemarks = dto.ApprovalRemarks,
                                                            EntryBy = dto.EntryBy,
                                                            EntryDate = DateTime.Now,
                                                            UpdatedBy = dto.EntryBy,
                                                            UpdatedDate = DateTime.Now,
                                                            Delegate = dto.Delegate,
                                                            WorkFlowID = c.WorkFlowId,
                                                            ForwardNext = dto.ForwardNext,
                                                            FlowRoleID = a.ParemeterId,
                                                            WorkflowType = dto.WorkflowType
                                                        }).ToListAsync ( );
                        }
                    }
                    if (!string.IsNullOrEmpty (dto.FinalRule))
                    {
                        //if (!string.IsNullOrEmpty (dto.FinalRule))
                        //{
                        int rule = 0;
                        int workflowRuleId = 0;
                        var ruleGroups = dto.FinalRule.Split ('+'); // Each group is something like "1,2,3"

                        foreach (var ruleGroup in ruleGroups)
                        {
                            int count = 0;
                            workflowRuleId++;

                            var values = ruleGroup.Split (',');
                            int ruleOrder = 0;

                            rule = (rule != 0) ? rule + 1 : count + 1;

                            foreach (var value1 in values)
                            {
                                count++;
                                var flowRoleID = value1;
                                int linkToEmp = 0;
                                int roleValue = 0;

                                if (string.IsNullOrEmpty (value1))
                                {
                                    Console.WriteLine ("value1 is null or empty");
                                }


                                if (!string.IsNullOrEmpty (value1))
                                {
                                    var checkReporting = await _context.Categorymasterparameters
                                        .Where (x => x.ParameterId == int.Parse (value1))
                                        .Select (x => x.Reporting)
                                        .FirstOrDefaultAsync ( );

                                    linkToEntity = await _context.ParamRole00s
                                        .Where (x => x.ParameterId == int.Parse (value1))
                                        .Select (x => x.EntityLevel)
                                        .FirstOrDefaultAsync ( );
                                }

                                if (!string.IsNullOrEmpty (linkToEntity) && linkToEntity != "0" && linkToEntity != "15")
                                {
                                    int workflowRuleValue1 = 0;
                                    //  dto.EmployeeID = 0;
                                    int? empId = 0;

                                    var entityLevels = linkToEntity.Split (',').Select (int.Parse);

                                    foreach (var level in entityLevels)
                                    {
                                        string? linkID = level switch
                                        {
                                            1 => tempLevel.LevelOneId.ToString ( ),
                                            2 => tempLevel.LevelTwoId.ToString ( ),
                                            3 => tempLevel.LevelThreeId.ToString ( ),
                                            4 => tempLevel.LevelFourId.ToString ( ),
                                            5 => tempLevel.LevelFiveId.ToString ( ),
                                            6 => tempLevel.LevelSixId.ToString ( ),
                                            7 => tempLevel.LevelSevenId.ToString ( ),
                                            8 => tempLevel.LevelEightId.ToString ( ),
                                            9 => tempLevel.LevelNineId.ToString ( ),
                                            10 => tempLevel.LevelTenId.ToString ( ),
                                            11 => tempLevel.LevelElevenId.ToString ( ),
                                            12 => tempLevel.LevelTwelveId.ToString ( ),
                                            _ => null
                                        };

                                        if (!string.IsNullOrEmpty (linkID))
                                        {
                                            workflowRuleValue1 = level;

                                            var empRecord = await _context.ParamRole01s
                                                .Where (x => x.LinkId == int.Parse (linkID) &&
                                                            x.LinkLevel == workflowRuleValue1 &&
                                                            x.ParameterId == int.Parse (value1))
                                                .Select (x => x.EmpId)
                                                .FirstOrDefaultAsync ( );

                                            if (empRecord != 0)
                                            {
                                                empId = empRecord;
                                                break;
                                            }
                                        }
                                    }

                                    if (empId == null)
                                    {
                                        empId = 0;
                                    }

                                    if (empId == 0 && entityLevels.Contains (13))
                                    {
                                        if (!string.IsNullOrEmpty (value1))
                                        {
                                            empId = await _context.ParamRole02s
                                               .Where (x => x.LinkEmpId == dto.EmployeeID && x.ParameterId == int.Parse (value1))
                                               .Select (x => x.EmpId)
                                               .FirstOrDefaultAsync ( );
                                        }
                                        workflowRuleValue1 = 13;
                                    }
                                    if (empId == 0)
                                    {
                                        if (dto.CheckReporting == 1)
                                        {
                                            empId = await _context.HrEmpReportings
                                                .Where (x => x.EmpId == dto.EmployeeID)
                                                .Select (x => x.ReprotToWhome)
                                                .FirstOrDefaultAsync ( );
                                        }
                                        else if (dto.CheckReporting == 2)
                                        {
                                            var firstLevel = await _context.HrEmpReportings
                                                .Where (x => x.EmpId == dto.EmployeeID)
                                                .Select (x => x.ReprotToWhome)
                                                .ToListAsync ( );

                                            empId = await _context.HrEmpReportings
                                                .Where (x => firstLevel.Contains (x.EmpId))
                                                .Select (x => x.ReprotToWhome)
                                                .FirstOrDefaultAsync ( );
                                        }
                                    }

                                    if (empId == 0 || empId == null)
                                    {
                                        if (!string.IsNullOrEmpty (value1))
                                        {
                                            empId = await _context.ParamRole00s
                                            .Where (x => x.ParameterId == int.Parse (value1))
                                            .Select (x => x.EmpId)
                                            .FirstOrDefaultAsync ( );
                                        }
                                    }

                                    if (empId != 0)
                                    {
                                        ruleOrder++;
                                    }

                                    int ShowStatus = 0;
                                    if (dto.HierarchyType == false)
                                    {
                                        ShowStatus = 1;
                                    }

                                    else if (dto.HierarchyType == true && ruleOrder == 1)
                                    {
                                        ShowStatus = 1;
                                    }

                                    bool alreadyExists = workflowInsertList.Any (x => x.Approver == dto.EmployeeID);

                                    if (alreadyExists)
                                    {
                                        ruleOrder--;
                                    }
                                    else
                                    {
                                        if (empId != 0)
                                        {
                                            workflowInsertList.Add (new TempLeaveWorkFlowDto
                                            {
                                                RequestId = dto.RequestID,
                                                ShowStatus = ShowStatus,
                                                ApprovalStatus = "P",
                                                Rule = rule,
                                                RuleOrder = ruleOrder,
                                                HierarchyType = dto.HierarchyType,
                                                Approver = empId,
                                                ApprovalRemarks = dto.ApprovalRemarks,
                                                EntryBy = dto.EntryBy,
                                                EntryDate = DateTime.Now,
                                                UpdatedBy = dto.EntryBy,
                                                UpdatedDate = DateTime.Now,
                                                Delegate = dto.Delegate,
                                                WorkFlowID = dto.WorkFlowID,
                                                ForwardNext = dto.ForwardNext,
                                                FlowRoleID = dto.FlowRoleID,
                                                WorkflowType = dto.WorkflowType
                                            });
                                        }
                                    }
                                }
                            }
                        }
                        //}
                    }

                }
                if (dto.ReturnWorkFlowTable == false)
                {
                    if (dto.TransactionType != "Leave_App")
                    {
                        // Insert into LeaveWorkFlowstatus
                        var leaveWorkflowRecords = workflowInsertList.Select (x => new LeaveWorkFlowstatus
                        {
                            RequestId = x.RequestId,
                            ShowStatus = Convert.ToBoolean (x.ShowStatus),
                            ApprovalStatus = x.ApprovalStatus,
                            Rule = x.Rule,
                            RuleOrder = x.RuleOrder,
                            HierarchyType = x.HierarchyType,
                            Approver = x.Approver,
                            ApprovalRemarks = x.ApprovalRemarks,
                            EntryBy = x.EntryBy,
                            EntryDt = x.EntryDate,
                            UpdatedBy = x.UpdatedBy,
                            UpdatedDt = x.UpdatedDate,
                            DelegateApprover = int.TryParse (x.Delegate, out int result) ? result : 0, // Or handle invalid cases as needed,
                            EntryFrom = dto.EntryFrom,
                            Workflowtype = x.WorkflowType
                        }).ToList ( );

                        await _context.LeaveWorkFlowstatuses.AddRangeAsync (leaveWorkflowRecords);


                        // Insert into EMAIL_NOTIFICATION
                        var emailNotifications = workflowInsertList
                            .Join (
                                _context.LeaveApplication00s,
                                temp => temp.RequestId,
                                leave => leave.LeaveApplicationId,
                                (temp, leave) => new EmailNotification
                                {
                                    InstdId = 1,
                                    RequestId = leave.LeaveApplicationId,
                                    RequestIdCode = leave.RequestId,
                                    RequesterEmpId = leave.EmployeeId,
                                    ReceiverEmpId = temp.Approver,
                                    TriggerDate = leave.EntryDate,
                                    TransactionId = dto.TransactionId,
                                    ShowStatus = temp.ShowStatus,
                                    RequesterDate = leave.EntryDate,
                                    NotificationMessage = "Submitted a Leave For Approval",
                                    MailType = "A",
                                    Workflowtype = temp.WorkflowType
                                }
                            ).ToList ( );

                        await _context.EmailNotifications.AddRangeAsync (emailNotifications);

                        await _context.SaveChangesAsync ( );
                    }

                }
                else
                {
                    var result = (from a in workflowInsertList
                                  join b in _context.HrEmpImages on a.Approver equals b.EmpId
                                  join c in _context.EmployeeDetails on b.EmpId equals c.EmpId
                                  join d in _context.DesignationDetails on c.DesigId equals d.LinkId
                                  join e in _context.BranchDetails on c.BranchId equals e.LinkId
                                  where a.HideFlow != 1
                                  orderby a.Rule, a.RuleOrder
                                  select new LeaveWorkflowDto
                                  {
                                      EmpName = c.Name,
                                      Name = c.Name,
                                      ImageUrl = b.ImageUrl,
                                      Designation = d.Designation,
                                      Url = b.ImageUrl,
                                      ApprovalStatus = "",
                                      Branch = e.Branch,
                                      Rule = a.Rule,
                                      RuleOrder = a.RuleOrder,
                                      ShowStatus = a.ShowStatus,
                                      FlowRoleID = a.FlowRoleID,
                                      ForwardNext = a.ForwardNext,
                                      WorkFlowID = a.WorkFlowID,
                                      WorkFlowStatus = "",
                                      Emp_Id = c.EmpId,
                                      Code = c.EmpCode
                                  }).ToList ( );
                    return result;

                }
            }
            var Result = new List<LeaveWorkflowDto> ( );
            return Result;
        }
        private async Task<List<ProbationWorkflowDisplayDto>> WorkFlowActivityProbation (WorkFlowActivityProbationInputDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync ( );

            try
            {
                dto.EmpId = 72;
                dto.TransactionType = "Probation";
                dto.ReturnWorkFlowTable = true;
                // In-memory collection for TempLevel
                var tempLevel = new List<TempLevelDto> ( );

                // Populate TempLevel (equivalent to SQL CTE)
                var levels = from emp in _context.HrEmpMasters
                             join high in _context.HighLevelViewTables
                             on emp.DesigId equals
                                high.LevelSixId == 0 ? high.LevelFiveId :
                                high.LevelSevenId == 0 ? high.LevelSixId :
                                high.LevelEightId == 0 ? high.LevelSevenId :
                                high.LevelNineId == 0 ? high.LevelEightId :
                                high.LevelTenId == 0 ? high.LevelNineId :
                                high.LevelElevenId == 0 ? high.LevelTenId :
                                high.LevelTwelveId == 0 ? high.LevelElevenId : high.LevelTwelveId
                             where emp.EmpId == dto.EmpId
                             select new TempLevelDto
                             {
                                 LevelOneId = high.LevelOneId,
                                 LevelTwoId = high.LevelTwoId,
                                 LevelThreeId = high.LevelThreeId,
                                 LevelFourId = high.LevelFourId,
                                 LevelFiveId = high.LevelFiveId,
                                 LevelSixId = high.LevelSixId,
                                 LevelSevenId = high.LevelSevenId,
                                 LevelEightId = high.LevelEightId,
                                 LevelNineId = high.LevelNineId,
                                 LevelTenId = high.LevelTenId,
                                 LevelElevenId = high.LevelElevenId,
                                 LevelTwelveId = high.LevelTwelveId
                             };

                tempLevel.AddRange (await levels.ToListAsync ( ));

                // Initialize variables
                int transactionID = 0;
                int workFlowID = 0;
                string finalRule = "";
                int workflowRuleId = 0;
                string linkToEntity = "";
                int linkID = 0;
                int count = 0;
                int roleValue = 0;
                int employeeId = 0;
                bool hierarchyType = false;
                int forwardNext = 0;
                int flowRoleID = 0;

                // Get TransactionID
                transactionID = await _context.TransactionMasters
                    .Where (t => t.TransactionType == dto.TransactionType)
                    .Select (t => t.TransactionId)
                    .FirstOrDefaultAsync ( );

                linkToEntity = await _context.ParamWorkFlow00s
                    .Where (p => p.TransactionId == transactionID)
                    .Select (p => p.EntityLevel)
                    .FirstOrDefaultAsync ( ) ?? "";

                // In-memory collection for TempLeaveWorkFlow
                var tempLeaveWorkFlow = new List<TempLeaveWorkFlowDto> ( );

                // Workflow logic
                if (dto.SpecialWorkFlowID == 0)
                {
                    if (!string.IsNullOrEmpty (linkToEntity) && linkToEntity != "0" && linkToEntity != "15")
                    {
                        var entityLevels = linkToEntity.Split (',').Select (int.Parse).ToList ( );

                        for (int level = 1; level <= 12; level++)
                        {
                            if (entityLevels.Contains (level))
                            {
                                linkID = level switch
                                {
                                    1 => tempLevel.FirstOrDefault ( )?.LevelOneId ?? 0,
                                    2 => tempLevel.FirstOrDefault ( )?.LevelTwoId ?? 0,
                                    3 => tempLevel.FirstOrDefault ( )?.LevelThreeId ?? 0,
                                    4 => tempLevel.FirstOrDefault ( )?.LevelFourId ?? 0,
                                    5 => tempLevel.FirstOrDefault ( )?.LevelFiveId ?? 0,
                                    6 => tempLevel.FirstOrDefault ( )?.LevelSixId ?? 0,
                                    7 => tempLevel.FirstOrDefault ( )?.LevelSevenId ?? 0,
                                    8 => tempLevel.FirstOrDefault ( )?.LevelEightId ?? 0,
                                    9 => tempLevel.FirstOrDefault ( )?.LevelNineId ?? 0,
                                    10 => tempLevel.FirstOrDefault ( )?.LevelTenId ?? 0,
                                    11 => tempLevel.FirstOrDefault ( )?.LevelElevenId ?? 0,
                                    12 => tempLevel.FirstOrDefault ( )?.LevelTwelveId ?? 0,
                                    _ => 0
                                };

                                var paramWorkFlow = await _context.ParamWorkFlow01s
                                    .FirstOrDefaultAsync (p => p.LinkId == linkID && p.TransactionId == transactionID);

                                if (paramWorkFlow != null)
                                {
                                    workFlowID = (int)paramWorkFlow.WorkFlowId;
                                }
                            }
                        }

                        if (entityLevels.Contains (13))
                        {
                            var paramWorkFlow02 = await _context.ParamWorkFlow02s
                                .FirstOrDefaultAsync (p => p.LinkEmpId == dto.EmpId && p.TransactionId == transactionID);

                            if (paramWorkFlow02 != null)
                            {
                                workFlowID = (int)paramWorkFlow02.WorkFlowId;
                            }
                        }
                    }
                }
                else
                {
                    if (workFlowID == 0 && !string.IsNullOrEmpty (linkToEntity) && linkToEntity != "0" && linkToEntity != "15")
                    {
                        var entityLevels = linkToEntity.Split (',').Select (int.Parse).ToList ( );

                        for (int level = 1; level <= 12; level++)
                        {
                            if (entityLevels.Contains (level))
                            {
                                linkID = level switch
                                {
                                    1 => tempLevel.FirstOrDefault ( )?.LevelOneId ?? 0,
                                    2 => tempLevel.FirstOrDefault ( )?.LevelTwoId ?? 0,
                                    3 => tempLevel.FirstOrDefault ( )?.LevelThreeId ?? 0,
                                    4 => tempLevel.FirstOrDefault ( )?.LevelFourId ?? 0,
                                    5 => tempLevel.FirstOrDefault ( )?.LevelFiveId ?? 0,
                                    6 => tempLevel.FirstOrDefault ( )?.LevelSixId ?? 0,
                                    7 => tempLevel.FirstOrDefault ( )?.LevelSevenId ?? 0,
                                    8 => tempLevel.FirstOrDefault ( )?.LevelEightId ?? 0,
                                    9 => tempLevel.FirstOrDefault ( )?.LevelNineId ?? 0,
                                    10 => tempLevel.FirstOrDefault ( )?.LevelTenId ?? 0,
                                    11 => tempLevel.FirstOrDefault ( )?.LevelElevenId ?? 0,
                                    12 => tempLevel.FirstOrDefault ( )?.LevelTwelveId ?? 0,
                                    _ => 0
                                };

                                var paramWorkFlow = await _context.ParamWorkFlow01s
                                    .FirstOrDefaultAsync (p => p.LinkId == linkID && p.TransactionId == transactionID);

                                if (paramWorkFlow != null)
                                {
                                    workFlowID = (int)paramWorkFlow.WorkFlowId;
                                }
                            }
                        }

                        if (entityLevels.Contains (13))
                        {
                            var paramWorkFlow02 = await _context.ParamWorkFlow02s
                                .FirstOrDefaultAsync (p => p.LinkEmpId == dto.EmpId && p.TransactionId == transactionID);

                            if (paramWorkFlow02 != null)
                            {
                                workFlowID = (int)paramWorkFlow02.WorkFlowId;
                            }
                        }
                    }
                }

                if (dto.WorkflowType == 2)
                {
                    workFlowID = (int)await _context.ParamWorkFlow00s
                        .Where (p => p.TransactionId == transactionID)
                        .Select (p => p.SecondLevelWorkflowId)
                        .FirstOrDefaultAsync ( );
                }
                else if (workFlowID == 0)
                {
                    workFlowID = (int)await _context.ParamWorkFlow00s
                        .Where (p => p.TransactionId == transactionID)
                        .Select (p => p.WorkFlowId)
                        .FirstOrDefaultAsync ( );
                }

                if (workFlowID > 0)
                {
                    var workFlowDetails = await _context.WorkFlowDetails
                        .FirstOrDefaultAsync (w => w.WorkFlowId == workFlowID && (bool)w.IsActive);

                    if (workFlowDetails != null)
                    {
                        finalRule = workFlowDetails.FinalRule ?? "";
                        hierarchyType = (bool)workFlowDetails.HierarchyType;
                        forwardNext = workFlowDetails.ForwardNext ?? 0;

                        bool isNewWorkflow = await _context.WorkFlowDetails
                            .AnyAsync (w => w.WorkFlowId == workFlowID && w.OldType == 0);

                        if (isNewWorkflow)
                        {
                            var workflowData = from wfd in _context.WorkFlowDetails01s
                                               join wd in _context.WorkFlowDetails
                                               on new { WFID = wfd.WorkFlowId, OldType = 0 } equals new { WFID = (int?)wd.WorkFlowId, OldType = (int)wd.OldType }
                                               join cmp in _context.Categorymasterparameters
                                               on wfd.ParemeterId equals cmp.ParameterId
                                               where wfd.WorkFlowId == workFlowID
                                               select new TempLeaveWorkFlowDto
                                               {
                                                   RequestId = dto.RequestId,
                                                   //ShowStatus = (int?)wd.HierarchyType ? (wfd.RuleOrder == 1 ? 1 : 0) : 1,
                                                   ApprovalStatus = "P",
                                                   Rule = wfd.Rules,
                                                   RuleOrder = wfd.RuleOrder,
                                                   HierarchyType = wd.HierarchyType,
                                                   Approver = cmp.Reporting == 5 ? dto.SelfEmpID : GetRoleBasedEmployee (dto.EmpId, wfd.ParemeterId).Result,
                                                   ApprovalRemarks = dto.ApprovalRemarks,
                                                   EntryBy = dto.EntryBy,
                                                   EntryDate = DateTime.Now,
                                                   UpdatedBy = dto.EntryBy,
                                                   UpdatedDate = DateTime.Now,
                                                   Delegate = dto.Deligate,
                                                   WorkFlowID = wfd.WorkFlowId,
                                                   ForwardNext = forwardNext,
                                                   FlowRoleID = wfd.ParemeterId,
                                                   WorkflowType = dto.WorkflowType,
                                                   IsSelf = cmp.Reporting == 5 ? 1 : 0
                                               };

                            tempLeaveWorkFlow.AddRange (await workflowData.Where (w => w.Approver != 0).OrderBy (w => w.Rule).ThenBy (w => w.RuleOrder).ToListAsync ( ));
                        }
                        else if (!string.IsNullOrEmpty (finalRule))
                        {
                            int rule = 0;
                            int ruleOrder = 0;

                            var rules = finalRule.Split ('+').Where (r => !string.IsNullOrEmpty (r)).ToList ( );

                            foreach (var ruleValue in rules)
                            {
                                workflowRuleId++;
                                rule = rule != 0 ? rule + 1 : 1;
                                ruleOrder = 0;

                                var parameters = ruleValue.Split (',').Where (p => !string.IsNullOrEmpty (p)).Select (int.Parse).ToList ( );

                                foreach (var paramId in parameters)
                                {
                                    count++;
                                    flowRoleID = paramId;

                                    var checkReporting = await _context.Categorymasterparameters
                                        .Where (c => c.ParameterId == paramId)
                                        .Select (c => c.Reporting)
                                        .FirstOrDefaultAsync ( );

                                    var entityLevel = await _context.ParamRole00s
                                        .Where (p => p.ParameterId == paramId)
                                        .Select (p => p.EntityLevel)
                                        .FirstOrDefaultAsync ( ) ?? "";

                                    employeeId = 0;

                                    if (!string.IsNullOrEmpty (entityLevel) && entityLevel != "0" && entityLevel != "15")
                                    {
                                        var entityLevels = entityLevel.Split (',').Select (int.Parse).ToList ( );

                                        for (int level = 1; level <= 12; level++)
                                        {
                                            if (entityLevels.Contains (level))
                                            {
                                                linkID = level switch
                                                {
                                                    1 => tempLevel.FirstOrDefault ( )?.LevelOneId ?? 0,
                                                    2 => tempLevel.FirstOrDefault ( )?.LevelTwoId ?? 0,
                                                    3 => tempLevel.FirstOrDefault ( )?.LevelThreeId ?? 0,
                                                    4 => tempLevel.FirstOrDefault ( )?.LevelFourId ?? 0,
                                                    5 => tempLevel.FirstOrDefault ( )?.LevelFiveId ?? 0,
                                                    6 => tempLevel.FirstOrDefault ( )?.LevelSixId ?? 0,
                                                    7 => tempLevel.FirstOrDefault ( )?.LevelSevenId ?? 0,
                                                    8 => tempLevel.FirstOrDefault ( )?.LevelEightId ?? 0,
                                                    9 => tempLevel.FirstOrDefault ( )?.LevelNineId ?? 0,
                                                    10 => tempLevel.FirstOrDefault ( )?.LevelTenId ?? 0,
                                                    11 => tempLevel.FirstOrDefault ( )?.LevelElevenId ?? 0,
                                                    12 => tempLevel.FirstOrDefault ( )?.LevelTwelveId ?? 0,
                                                    _ => 0
                                                };

                                                var paramRole = await _context.ParamRole01s
                                                    .FirstOrDefaultAsync (p => p.LinkId == linkID && p.LinkLevel == level && p.ParameterId == paramId);

                                                if (paramRole != null)
                                                {
                                                    employeeId = (int)paramRole.EmpId;
                                                }
                                            }
                                        }

                                        if (employeeId == 0 && entityLevels.Contains (13))
                                        {
                                            var empIdNullable = await _context.ParamRole02s
                                            .Where (p => p.LinkEmpId == dto.EmpId && p.ParameterId == paramId)
                                            .Select (p => p.EmpId)
                                            .FirstOrDefaultAsync ( );

                                            if (empIdNullable.HasValue)
                                                employeeId = empIdNullable.Value;
                                            else
                                                employeeId = 0;
                                        }

                                        if (employeeId == 0)
                                        {
                                            if (checkReporting == 1)
                                            {
                                                employeeId = (int)await _context.HrEmpReportings
                                                    .Where (r => r.EmpId == dto.EmpId)
                                                    .Select (r => r.ReprotToWhome)
                                                    .FirstOrDefaultAsync ( );
                                            }
                                            else if (checkReporting == 2)
                                            {
                                                employeeId = (int)await _context.HrEmpReportings
                                                    .Where (r => r.EmpId == (_context.HrEmpReportings
                                                        .Where (inner => inner.EmpId == dto.EmpId)
                                                        .Select (inner => inner.ReprotToWhome)
                                                        .FirstOrDefault ( )))
                                                    .Select (r => r.ReprotToWhome)
                                                    .FirstOrDefaultAsync ( );
                                            }
                                            else if (checkReporting == 5)
                                            {
                                                employeeId = dto.SelfEmpID;
                                            }
                                        }

                                        if (employeeId == 0)
                                        {
                                            employeeId = (int)await _context.ParamRole00s
                                                .Where (p => p.ParameterId == paramId)
                                                .Select (p => p.EmpId)
                                                .FirstOrDefaultAsync ( );
                                        }
                                    }
                                    else
                                    {
                                        if (checkReporting == 1)
                                        {
                                            employeeId = (int)await _context.HrEmpReportings
                                                .Where (r => r.EmpId == dto.EmpId)
                                                .Select (r => r.ReprotToWhome)
                                                .FirstOrDefaultAsync ( );
                                        }
                                        else if (checkReporting == 2)
                                        {
                                            employeeId = (int)await _context.HrEmpReportings
                                                .Where (r => r.EmpId == (_context.HrEmpReportings
                                                    .Where (inner => inner.EmpId == dto.EmpId)
                                                    .Select (inner => inner.ReprotToWhome)
                                                    .FirstOrDefault ( )))
                                                .Select (r => r.ReprotToWhome)
                                                .FirstOrDefaultAsync ( );
                                        }
                                        else if (checkReporting == 5)
                                        {
                                            employeeId = dto.SelfEmpID;
                                        }

                                        if (employeeId == 0)
                                        {
                                            employeeId = (int)await _context.ParamRole00s
                                                .Where (p => p.ParameterId == paramId && p.LinkLevel == 15)
                                                .Select (p => p.EmpId)
                                                .FirstOrDefaultAsync ( );
                                        }
                                    }

                                    if (employeeId != 0)
                                    {
                                        ruleOrder++;
                                        int showStatusLocal = hierarchyType ? (ruleOrder == 1 ? 1 : 0) : 1;

                                        tempLeaveWorkFlow.Add (new TempLeaveWorkFlowDto
                                        {
                                            RequestId = dto.RequestId,
                                            ShowStatus = showStatusLocal,
                                            ApprovalStatus = "P",
                                            Rule = rule,
                                            RuleOrder = ruleOrder,
                                            HierarchyType = hierarchyType,
                                            Approver = employeeId,
                                            ApprovalRemarks = dto.ApprovalRemarks,
                                            EntryBy = dto.EntryBy,
                                            EntryDate = DateTime.Now,
                                            UpdatedBy = dto.EntryBy,
                                            UpdatedDate = DateTime.Now,
                                            Delegate = dto.Deligate,
                                            WorkFlowID = workFlowID,
                                            ForwardNext = forwardNext,
                                            FlowRoleID = flowRoleID,
                                            WorkflowType = dto.WorkflowType,
                                            IsSelf = checkReporting == 5 ? 1 : 0
                                        });
                                    }
                                }
                            }
                        }
                    }
                }

                if (!dto.ReturnWorkFlowTable)
                {
                    if (dto.TransactionType == "Probation")
                    {
                        var probationWorkFlowStatus = tempLeaveWorkFlow.Select (t => new ProbationWorkFlowstatus
                        {
                            RequestId = t.RequestId ?? 0,
                            ShowStatus = t.ShowStatus == 1,
                            ApprovalStatus = t.ApprovalStatus,
                            Rule = t.Rule ?? 0,
                            RuleOrder = t.RuleOrder ?? 0,
                            HierarchyType = t.HierarchyType ?? false,
                            Approver = t.Approver ?? 0,
                            ApprovalRemarks = t.ApprovalRemarks,
                            EntryBy = t.EntryBy ?? 0,
                            EntryDt = t.EntryDate ?? DateTime.Now,
                            UpdatedBy = t.UpdatedBy ?? 0,
                            UpdatedDt = t.UpdatedDate ?? DateTime.Now,
                            Deligate = t.Delegate,
                            EntryFrom = dto.EntryFrom,
                            IsSelf = t.IsSelf
                        }).ToList ( );

                        await _context.ProbationWorkFlowstatuses.AddRangeAsync (probationWorkFlowStatus);

                        var emailNotifications = (from t in tempLeaveWorkFlow
                                                  join pr in _context.ProbationRating00s on t.RequestId equals pr.ProbRateId
                                                  join emp in _context.EmployeeDetails on pr.EmpId equals emp.EmpId
                                                  select new EmailNotification
                                                  {
                                                      InstdId = 1,
                                                      RequestId = pr.ProbRateId,
                                                      RequestIdCode = emp.EmpCode,
                                                      RequesterEmpId = pr.EmpId,
                                                      ReceiverEmpId = t.Approver ?? 0,
                                                      TriggerDate = pr.EntryDate,
                                                      TransactionId = transactionID,
                                                      ShowStatus = t.ShowStatus == 1 ? 1 : 0,
                                                      RequesterDate = pr.EntryDate,
                                                      NotificationMessage = t.IsSelf == 1 ? "Probation Review pending for Self" : $"Probation                           Review pending for {emp.Name}",
                                                      MailType = "A",
                                                      Workflowtype = t.WorkflowType
                                                  }).ToList ( );

                        await _context.EmailNotifications.AddRangeAsync (emailNotifications);
                    }

                    await _context.SaveChangesAsync ( );
                    await transaction.CommitAsync ( );
                    return null;
                }
                else
                {
                    var result = (from t in tempLeaveWorkFlow
                                  join img in _context.HrEmpImages on t.Approver equals img.EmpId
                                  join emp in _context.EmployeeDetails on img.EmpId equals emp.EmpId
                                  join des in _context.DesignationDetails on emp.DesigId equals des.LinkId
                                  join br in _context.BranchDetails on emp.BranchId equals br.LinkId
                                  where t.HideFlow != 1
                                  orderby t.Rule, t.RuleOrder
                                  select new ProbationWorkflowDisplayDto
                                  {
                                      EmpName = emp.Name,
                                      Name = emp.Name,
                                      Url = img.ImageUrl,
                                      Designation = des.Designation,
                                      Image_Url = img.ImageUrl,
                                      ApprovalStatus = "",
                                      Branch = br.Branch,
                                      Rule = t.Rule,
                                      RuleOrder = t.RuleOrder,
                                      ShowStatus = t.ShowStatus,
                                      FlowRoleID = t.FlowRoleID,
                                      ForwardNext = t.ForwardNext,
                                      WorkFlowID = t.WorkFlowID,
                                      WorkFlowStatus = "",
                                      Emp_Id = emp.EmpId,
                                      Code = emp.EmpCode,
                                      IsSelf = t.IsSelf
                                  }).ToList ( );

                    await _context.SaveChangesAsync ( );
                    await transaction.CommitAsync ( );
                    return result;
                }
            }
            catch
            {
                await transaction.RollbackAsync ( );
                throw;
            }
        }




        //string CamelCase (string name)
        //{
        //    if (string.IsNullOrWhiteSpace (name)) return name;
        //    return string.Join (" ", name.Split (' ').Select (word =>
        //        char.ToUpper (word[0]) + word.Substring (1).ToLower ( )));
        //}
        public async Task<int?> GetRoleBasedEmployee (int? empId, int? currRoleId)
        {

            int? linkId = 0, employeeId = 0, workFlowID;
            var tempLevel = await (from c in _context.HrEmpMasters
                                   join e in _context.HighLevelViews
                                   on c.DesigId equals
                                   (e.LevelSixId == 0 ? e.LevelFiveId :
                                   e.LevelSevenId == 0 ? e.LevelSixId :
                                   e.LevelEightId == 0 ? e.LevelSevenId :
                                   e.LevelNineId == 0 ? e.LevelEightId :
                                   e.LevelTenId == 0 ? e.LevelNineId :
                                   e.LevelElevenId == 0 ? e.LevelTenId :
                                   e.LevelTwelveId == 0 ? e.LevelElevenId :
                                   e.LevelTwelveId)
                                   where c.EmpId == empId
                                   select new TempLevelDto
                                   {
                                       LevelOneId = e.LevelOneId,
                                       LevelTwoId = e.LevelTwoId,
                                       LevelThreeId = e.LevelThreeId,
                                       LevelFourId = e.LevelFourId,
                                       LevelFiveId = e.LevelFiveId,
                                       LevelSixId = e.LevelSixId,
                                       LevelSevenId = e.LevelSevenId,
                                       LevelEightId = e.LevelEightId,
                                       LevelNineId = e.LevelNineId,
                                       LevelTenId = e.LevelTenId,
                                       LevelElevenId = e.LevelElevenId,
                                       LevelTwelveId = e.LevelTwelveId
                                   }).ToListAsync ( );

            var linkToEntity = await _context.ParamRole00s
                            .Where (a => a.ParameterId == currRoleId)
                            .Select (a => a.EntityLevel)
                            .FirstOrDefaultAsync ( );

            if (linkToEntity != "0" && linkToEntity != "15" && linkToEntity is not null)
            {
                var linkToEntityList = linkToEntity.Split (',').Select (int.Parse).ToList ( );

                if (linkToEntityList.Contains (1))
                {
                    linkId = tempLevel.Select (x => x.LevelOneId).FirstOrDefault ( );
                    employeeId = await GetEmployeeIDBasedOnLinkAsync (linkId, currRoleId);
                }
                if (linkToEntityList.Contains (2))
                {
                    linkId = tempLevel.Select (x => x.LevelTwoId).FirstOrDefault ( );
                    employeeId = await GetEmployeeIDBasedOnLinkAsync (linkId, currRoleId);
                }
                if (linkToEntityList.Contains (3))
                {
                    linkId = tempLevel.Select (x => x.LevelThreeId).FirstOrDefault ( );
                    employeeId = await GetEmployeeIDBasedOnLinkAsync (linkId, currRoleId);
                }
                if (linkToEntityList.Contains (4))
                {
                    linkId = tempLevel.Select (x => x.LevelFourId).FirstOrDefault ( );
                    employeeId = await GetEmployeeIDBasedOnLinkAsync (linkId, currRoleId);
                }
                if (linkToEntityList.Contains (5))
                {
                    linkId = tempLevel.Select (x => x.LevelFiveId).FirstOrDefault ( );
                    employeeId = await GetEmployeeIDBasedOnLinkAsync (linkId, currRoleId);
                }
                if (linkToEntityList.Contains (6))
                {
                    linkId = tempLevel.Select (x => x.LevelSixId).FirstOrDefault ( );
                    employeeId = await GetEmployeeIDBasedOnLinkAsync (linkId, currRoleId);
                }
                if (linkToEntityList.Contains (7))
                {
                    linkId = tempLevel.Select (x => x.LevelSevenId).FirstOrDefault ( );
                    employeeId = await GetEmployeeIDBasedOnLinkAsync (linkId, currRoleId);
                }
                if (linkToEntityList.Contains (8))
                {
                    linkId = tempLevel.Select (x => x.LevelEightId).FirstOrDefault ( );
                    employeeId = await GetEmployeeIDBasedOnLinkAsync (linkId, currRoleId);
                }
                if (linkToEntityList.Contains (9))
                {
                    linkId = tempLevel.Select (x => x.LevelNineId).FirstOrDefault ( );
                    employeeId = await GetEmployeeIDBasedOnLinkAsync (linkId, currRoleId);
                }
                if (linkToEntityList.Contains (10))
                {
                    linkId = tempLevel.Select (x => x.LevelTenId).FirstOrDefault ( );
                    employeeId = await GetEmployeeIDBasedOnLinkAsync (linkId, currRoleId);
                }
                if (linkToEntityList.Contains (11))
                {
                    linkId = tempLevel.Select (x => x.LevelElevenId).FirstOrDefault ( );
                    employeeId = await GetEmployeeIDBasedOnLinkAsync (linkId, currRoleId);
                }
                if (linkToEntityList.Contains (12))
                {
                    linkId = tempLevel.Select (x => x.LevelTwelveId).FirstOrDefault ( );
                    employeeId = await GetEmployeeIDBasedOnLinkAsync (linkId, currRoleId);
                }
                if (linkToEntityList.Contains (13))
                {
                    bool exists = await _context.ParamRole02s
                .AnyAsync (x => x.LinkEmpId == empId && x.ParameterId == currRoleId);

                    if (exists)
                    {
                        employeeId = await _context.ParamRole02s
                            .Where (a => a.LinkEmpId == empId && a.ParameterId == currRoleId)
                            .Select (a => a.EmpId)
                            .FirstOrDefaultAsync ( );
                    }
                }

            }

            else
            {
                employeeId = await _context.ParamRole00s
                           .Where (a => a.ParameterId == currRoleId)
                           .Select (a => a.EmpId)
                           .FirstOrDefaultAsync ( );
            }
            if (employeeId == 0)
            {
                employeeId = await _context.ParamRole00s
                          .Where (a => a.ParameterId == currRoleId)
                          .Select (a => a.EmpId)
                          .FirstOrDefaultAsync ( );

                bool exists = await _context.Categorymasterparameters
               .AnyAsync (x => x.ParameterId == currRoleId && x.Reporting == 1);

                bool exists1 = await _context.Categorymasterparameters
               .AnyAsync (x => x.ParameterId == currRoleId && x.Reporting == 2);

                if (exists)
                {
                    employeeId = await _context.HrEmpReportings
                          .Where (a => a.EmpId == empId)
                          .Select (a => a.ReprotToWhome)
                          .FirstOrDefaultAsync ( );
                }
                else if (exists1)
                {
                    var innerEmpIds = await _context.HrEmpReportings
                                    .Where (x => x.EmpId == empId)
                                    .Select (x => x.ReprotToWhome)
                                    .ToListAsync ( );

                    employeeId = await _context.HrEmpReportings
                               .Where (x => innerEmpIds.Contains (x.EmpId))
                               .Select (x => x.ReprotToWhome)
                               .FirstOrDefaultAsync ( );

                }
            }

            return employeeId;

        }

        private async Task<int?> GetEmployeeIDBasedOnLinkAsync (int? linkId, int? currRoleId)
        {
            bool exists = await _context.ParamRole01s
                .AnyAsync (x => x.LinkId == linkId && x.ParameterId == currRoleId);

            if (exists)
            {
                return await _context.ParamRole01s
                    .Where (a => a.LinkId == linkId && a.ParameterId == currRoleId)
                    .Select (a => a.EmpId)
                    .FirstOrDefaultAsync ( );
            }

            return null;
        }
        private async Task<string?> GetServiceLength (DateTime givenDate)
        {

            DateTime today = DateTime.Today;

            int years = today.Year - givenDate.Year;
            if (today.Month < givenDate.Month || (today.Month == givenDate.Month && today.Day < givenDate.Day))
            {
                years--;
            }

            DateTime tempDate = givenDate.AddYears (years);

            int months = today.Month - tempDate.Month;
            if (months < 0) months += 12;
            if (today.Day < givenDate.Day) months--;

            tempDate = tempDate.AddMonths (months);

            int days = (today - tempDate).Days;

            return $"{years}Y: {months}M: {days}D";
        }
        private async Task<string> GetEmployeeServiceLength (int empId)
        {
            // Get join date
            var joinDate = await _context.EmployeeDetails
                          .Where (e => e.EmpId == empId)
                          .Select (e => e.JoinDt)
                          .FirstOrDefaultAsync ( );

            if (joinDate == null)
                return "0Y: 0M: 0D"; // Employee not found

            // Determine last working date
            var lastDate = await _context.Resignations
            .Where (r => r.EmpId == empId
                    && (r.ApprovalStatus == "P" || r.ApprovalStatus == "A")
                    && r.RejoinStatus != "A"
                    && r.ApprovalStatus != "D"
                    && r.RelievingDate < DateTime.UtcNow)
            .OrderByDescending (r => r.RelievingDate)
            .Select (r => r.RelievingDate)
            .FirstOrDefaultAsync ( ) ?? DateTime.Now;

            // Future join date scenario
            if (joinDate > lastDate)
                return "0Y: 0M: 0D";

            // Calculate service length
            int years = lastDate.Year - joinDate.Value.Year;
            int months = lastDate.Month - joinDate.Value.Month;
            int days = lastDate.Day - joinDate.Value.Day;

            // Adjust for negative month or day
            if (days < 0)
            {
                months--;
                days += DateTime.DaysInMonth (lastDate.Year, lastDate.Month);
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            return $"{years}Y: {months}M: {days}D";
        }
        private async Task<TempLevelDto> GetHierarchyLevelsAsync (int empId)
        {
            var query = from emp in _context.HrEmpMasters
                        join level in _context.HighLevelViewTables
                            on emp.DesigId equals
                                (level.LevelSixId == 0 ? level.LevelFiveId :
                                 level.LevelSevenId == 0 ? level.LevelSixId :
                                 level.LevelEightId == 0 ? level.LevelSevenId :
                                 level.LevelNineId == 0 ? level.LevelEightId :
                                 level.LevelTenId == 0 ? level.LevelNineId :
                                 level.LevelElevenId == 0 ? level.LevelTenId :
                                 level.LevelTwelveId == 0 ? level.LevelElevenId :
                                 level.LevelTwelveId)
                        where emp.EmpId == empId
                        select new TempLevelDto
                        {
                            LevelOneId = level.LevelOneId,
                            LevelTwoId = level.LevelTwoId,
                            LevelThreeId = level.LevelThreeId,
                            LevelFourId = level.LevelFourId,
                            LevelFiveId = level.LevelFiveId,
                            LevelSixId = level.LevelSixId,
                            LevelSevenId = level.LevelSevenId,
                            LevelEightId = level.LevelEightId,
                            LevelNineId = level.LevelNineId,
                            LevelTenId = level.LevelTenId,
                            LevelElevenId = level.LevelElevenId,
                            LevelTwelveId = level.LevelTwelveId
                        };

            return await query.FirstOrDefaultAsync ( ) ?? new TempLevelDto ( );
        }


        }

    }