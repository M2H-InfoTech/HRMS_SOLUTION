using ATTENDANCE.DTO.Request;
using ATTENDANCE.DTO.Response;
using Azure.Core;
using EMPLOYEE_INFORMATION.Data;
using HRMS.EmployeeInformation.Models.Models.Entity;
using Microsoft.EntityFrameworkCore;
using MPLOYEE_INFORMATION.DTO.DTOs;
using System.Collections.Generic;

namespace ATTENDANCE.Repository.AssignPolicy
{
    public class AssignPolicyRepository(EmployeeDBContext _context) : IAssignPolicyRepository
    {

        private static IEnumerable<string> SplitStrings_XML(string list)
        {
            if (string.IsNullOrWhiteSpace(list))
                return Enumerable.Empty<string>();

            // Split the input string by the delimiter and return as IEnumerable
            return list.Split(',')
            .Select(item => item.Trim()) // Trim whitespace from each item
            .Where(item => !string.IsNullOrEmpty(item)); // Exclude empty items
        }
        public async Task<int> DeleteEmployeeShiftpolicy(int shiftId)
        {
            var data = await _context.AttendancepolicyMasterAccesses
                .FirstOrDefaultAsync(e => e.AttendanceAccessId == shiftId);

            if (data != null)
            {
                _context.AttendancepolicyMasterAccesses.Remove(data);
                await _context.SaveChangesAsync();
                return 1;
            }

            return 0;
        }
        public async Task<List<ShiftPolicyDto>> GetAllShiftPolicy(int levelId, int empId, string linkId)
        {
            var transactionId = await _context.TransactionMasters
                .Where(t => t.TransactionType == "AtnPolicy")
                .Select(t => t.TransactionId)
                .FirstOrDefaultAsync();

            if (levelId == 1 && empId == 0)
            {
                var linkIdList = linkId.Split(',').Select(int.Parse).ToList();

                var applicableMasterIds = await _context.EntityApplicable00s
                    .Where(e =>
                        // Condition 1: LinkLevel = 1 AND LinkId is in provided list AND matching transaction
                        (e.LinkLevel == 1 && linkIdList.Contains((int)e.LinkId) && e.TransactionId == transactionId)

                        // Condition 2: LinkLevel = 15 AND matching transaction
                        || (e.LinkLevel == 15 && e.TransactionId == transactionId)
                    )
                    .Select(e => e.MasterId).ToListAsync();
                var policies = await _context.Attendancepolicy00s
                    .Where(p => applicableMasterIds.Contains(p.AttendancePolicyId))
                    .Select(p => new ShiftPolicyDto
                    {
                        AttendancePolicyId = p.AttendancePolicyId,
                        PolicyName = p.PolicyName
                    })
                    .ToListAsync();
                return policies;
            }
            else
            {
                long? count = 0;
                int? levelOneId = null;
                long? rotatingLinkId = null;
                string totalLinkId = "";
                if (empId == 0)
                {
                    //var rotatingLinkId = await _context.SubCategoryLinksNews
                    //    .Where(s => s.LinkId == int.Parse(linkId)).Select(s => s.Root).FirstOrDefaultAsync();
                    //if (rotatingLinkId == 0)
                    //{
                    //    var levelOneId = await _context.SubCategoryLinksNews
                    //        .Where(s => s.LinkId == int.Parse(linkId)).Select(s => s.LinkableSubcategory).FirstOrDefaultAsync();
                    //    var totalLinkId = linkId;
                    //}
                    //else
                    //{
                    //    string totalLinkId = $"{rotatingLinkId},{linkId}";
                    //}
                    //var firstcount = await _context.SubCategoryLinksNews
                    //    .Where(s => s.LinkId == int.Parse(linkId))
                    //    .Select(S => S.LinkLevel)
                    //    .FirstOrDefaultAsync();
                    //var count = firstcount - 2;

                    var linkIdInt = int.Parse(linkId);
                    var subCategoryData = await _context.SubCategoryLinksNews
                        .Where(s => s.LinkId == linkIdInt)
                        .Select(s => new
                        {
                            s.Root,
                            s.LinkableSubcategory,
                            s.LinkLevel
                        })
                        .FirstOrDefaultAsync();



                    rotatingLinkId = subCategoryData.Root;



                    if (rotatingLinkId == 0)
                    {
                        levelOneId = subCategoryData.LinkableSubcategory;
                        totalLinkId = linkId;
                    }
                    else
                    {
                        totalLinkId = $"{linkId},{rotatingLinkId},";
                    }

                    count = subCategoryData.LinkLevel - 2;


                }
                else
                {
                    var linkId1 = await _context.HrEmpMasters
                         .Where(s => s.EmpId == empId)
                         .Select(s => s.LastEntity)
                         .FirstOrDefaultAsync();
                    rotatingLinkId = await _context.SubCategoryLinksNews
                                           .Where(s => s.LinkId == linkId1).Select(s => s.Root).FirstOrDefaultAsync();
                    var ids = new[] { rotatingLinkId?.ToString(), linkId1?.ToString() }
                        .Where(id => !string.IsNullOrWhiteSpace(id)); // Filter null/empty/whitespace

                    totalLinkId = string.Join(",", ids) + ","; // Append trailing comma if needed

                    var firstcount = await _context.LicensedCompanyDetails
                        .Select(S => S.EntityLimit)
                        .FirstOrDefaultAsync();
                    count = firstcount - 2;
                }
                while (count != 0)
                {
                    if (count == 1)
                    {
                        // `rotatingLinkId` holds whatever value @rotatinglinkid has in SQL
                        levelOneId = await _context.SubCategoryLinksNews
                            .Where(s => s.Root == 0 && s.LinkId == rotatingLinkId)
                            .Select(s => s.LinkableSubcategory)          // grab the LinkableSubcategory column
                            .FirstOrDefaultAsync();                      // returns null/default if nothing matches


                    }
                    else
                    {
                        rotatingLinkId = await _context.SubCategoryLinksNews
                            .Where(s => s.LinkId == rotatingLinkId)
                            .Select(s => s.Root)
                            .FirstOrDefaultAsync();
                        if (count == 2)
                        {
                            totalLinkId += (rotatingLinkId?.ToString() ?? string.Empty);
                        }
                        else
                        {
                            totalLinkId += (rotatingLinkId?.ToString() ?? string.Empty) + ",";
                        }


                    }
                    count--;
                }
                if (empId == 0)
                {
                    var splitTotallinkid = totalLinkId
                        .Split(',', StringSplitOptions.RemoveEmptyEntries) // removes empty strings
                        .Select(s => int.Parse(s))
                        .ToList();

                    var AttendancePolicyCompare = await _context.EntityApplicable00s
                        .Where(e =>
                        (e.LinkLevel == 1 && e.LinkId == levelOneId && e.TransactionId == transactionId) ||
                        (e.LinkLevel == 15 && e.TransactionId == transactionId) ||
                        (e.LinkLevel != 1 && splitTotallinkid.Contains((int)e.LinkId) && e.TransactionId == transactionId)
                        ).Select(e => e.MasterId)
                        .ToListAsync();

                    var applicable01Ids = await _context.EntityApplicable01s
                        .Where(e => e.TransactionId == transactionId && e.EmpId == empId)
                        .Select(e => e.MasterId)
                        .ToListAsync();

                    var allApplicableIds = AttendancePolicyCompare.Union(applicable01Ids).ToList();

                    var policies = await _context.Attendancepolicy00s
                        .Where(p => allApplicableIds.Contains(p.AttendancePolicyId))
                        .Select(p => new ShiftPolicyDto
                        {
                            AttendancePolicyId = p.AttendancePolicyId,
                            PolicyName = p.PolicyName
                        })
                        .ToListAsync();
                    return policies;
                }
                else
                {
                    var splitTotallinkid = totalLinkId.Split(',').Select(int.Parse).ToList();

                    var applicablePolicyIds = await _context.EntityApplicable00s
                        .Where(e =>
                            ((e.LinkLevel == 1 && e.LinkId == levelOneId && e.TransactionId == transactionId) ||
                             (e.LinkLevel == 15 && e.TransactionId == transactionId) ||
                             (e.LinkLevel != 1 && splitTotallinkid.Contains((int)e.LinkId) && e.TransactionId == transactionId))
                            ||
                            _context.EntityApplicable01s
                                .Where(e1 => e1.TransactionId == transactionId && e1.EmpId == empId)
                                .Select(e1 => e1.MasterId)
                                .Contains(e.MasterId)
                        )
                        .Select(e => e.MasterId)
                        .ToListAsync();

                    var policies = await _context.Attendancepolicy00s
                        .Where(p => applicablePolicyIds.Contains(p.AttendancePolicyId))
                        .Select(p => new ShiftPolicyDto
                        {
                            AttendancePolicyId = p.AttendancePolicyId,
                            PolicyName = p.PolicyName
                        })
                        .ToListAsync();
                    return policies;
                }
            }
        }


        //public async Task AssignShiftPolicy(AssignShiftPolicyDto Request)
        //{
        //    var currentDate = DateTime.UtcNow;
        //    var splitShifts = Request.ShiftIDs.Split(',').Select(int.Parse).ToList();
        //    var splitEmployeeIDs = Request.EmployeeIDs.Split(',').Select(int.Parse).ToList();
        //    var policies = await _context.Attendancepolicy00s
        //            .Where(p => splitShifts.Contains(p.AttendancePolicyId))
        //            .ToListAsync();
        //    if (Request.AttendanceAccessID == 0)
        //    {


        //        var policyId = await _context.AttendancepolicyMasterAccesses
        //            .Where(e => splitShifts.Contains((int)e.PolicyId) && splitEmployeeIDs.Contains((int)e.EmployeeId) && Request.ValidDatefrom == e.ValidDatefrom)
        //            .ToListAsync();
        //        if (policyId.Any())
        //        {

        //            _context.AttendancepolicyMasterAccesses.RemoveRange(policyId);
        //            await _context.SaveChangesAsync();
        //        }
        //        var newValidDateTo = Request.ValidDatefrom.Date.AddDays(-1);

        //        // Fetch records to update
        //        var recordsToUpdate = await _context.AttendancepolicyMasterAccesses
        //            .Where(x => splitEmployeeIDs.Contains((int)x.EmployeeId) && x.ValidDateTo == null)
        //            .ToListAsync();

        //        // Update properties
        //        foreach (var record in recordsToUpdate)
        //        {
        //            record.Active = "Y";
        //            record.ValidDateTo = newValidDateTo;
        //        }

        //        await _context.SaveChangesAsync();

        //        var PolicyDeleteBasedOnEmpid = await _context.AttendancepolicyMasterAccesses
        //             .Where(e => splitEmployeeIDs.Contains((int)e.EmployeeId) && e.ValidDatefrom == Request.ValidDatefrom)
        //             .ToListAsync();
        //        if (PolicyDeleteBasedOnEmpid.Any())
        //        {
        //            _context.AttendancepolicyMasterAccesses.RemoveRange(PolicyDeleteBasedOnEmpid);
        //            await _context.SaveChangesAsync();
        //        }


        //        var recordsToInsert = (from empId in splitEmployeeIDs
        //                               from policy in policies
        //                               select new AttendancepolicyMasterAccess
        //                               {
        //                                   EmployeeId = empId,
        //                                   PolicyId = policy.AttendancePolicyId,
        //                                   IsCompanyLevel = Request.levelId,
        //                                   CreatedBy = Request.entryBy,
        //                                   CreatedDate = DateTime.UtcNow,
        //                                   ValidDatefrom = Request.ValidDatefrom,
        //                                   ValidDateTo = Request.validDateTo,
        //                                   Active = "Y"
        //                               }).ToList();
        //        await _context.AttendancepolicyMasterAccesses.AddRangeAsync(recordsToInsert);
        //        await _context.SaveChangesAsync();
        //        if (Request.ValidDatefrom.Date <= currentDate.Date)
        //        {
        //            if (Request.validDateTo == null || Request.validDateTo.Date >= currentDate.Date)
        //            {
        //                Request.validDateTo = currentDate;
        //            }
        //            var transactionId = await _context.TransactionMasters
        //                .Where(t => t.TransactionType == "AtnPolicy")
        //                .Select(t => t.TransactionId)
        //                .FirstOrDefaultAsync();

        //            var recordToUpdate = (from empId in splitEmployeeIDs
        //                                   from policy in policies
        //                                   select new AutoCalAttendance00
        //                                   {
        //                                       FromDate = DateOnly.FromDateTime(Request.ValidDatefrom),
        //                                       ToDate = Request.validDateTo != default ? DateOnly.FromDateTime(Request.validDateTo) : null,
        //                                       EmployeeId = empId.ToString(),
        //                                       Status = "N",
        //                                       EntryDate = currentDate,
        //                                       RequestFrom = "Attendance Policy Assign",
        //                                       RequestFromId = transactionId,
        //                                       RequestId = policy.AttendancePolicyId.ToString(),
        //                                   }).ToList();

        //            await _context.AutoCalAttendance00s.AddRangeAsync(recordToUpdate);

        //        }

        //    }
        //    else
        //    {
        //        var record = await _context.AttendancepolicyMasterAccesses
        //            .FirstOrDefaultAsync(x => x.AttendanceAccessId == Request.AttendanceAccessID);

        //        if (record != null)
        //        {
        //            record.Active = "Y";
        //            record.ValidDatefrom = Request.ValidDatefrom;
        //            record.PolicyId = policies;
        //            record.ValidDateTo = Request.validDateTo;

        //            await _context.SaveChangesAsync();
        //        }

        //    }
        //}
        public async Task<List<AttendancePolicyAccessDto>> ViewEmployeeShiftPolicyFiltered(int attendanceAccessId, string employeeIds)
        {
            var splitEMpIDs = employeeIds.Split(',').Select(int.Parse).ToList();
            var result = await (from a in _context.AttendancepolicyMasterAccesses
                                join b in _context.EmployeeDetails on a.EmployeeId equals b.EmpId
                                join c in _context.Attendancepolicy00s on a.PolicyId equals c.AttendancePolicyId
                                join d in _context.HrEmpMasters on b.EmpId equals d.EmpId
                                join e in _context.DesignationDetails on d.DesigId equals e.LinkId
                                where (attendanceAccessId == 0 || a.AttendanceAccessId == attendanceAccessId)
                                      && splitEMpIDs.Contains(b.EmpId)
                                select new AttendancePolicyAccessDto
                                {
                                    AttendanceAccessId = (int)a.AttendanceAccessId,
                                    EmpId = b.EmpId,
                                    EmployeeName = b.Name + "|| " + b.EmpCode,
                                    AttendancePolicyId = c.AttendancePolicyId,
                                    PolicyName = c.PolicyName,
                                    EmployeeId = a.EmployeeId,
                                    Designation = e.Designation,
                                    Datefrom = a.ValidDatefrom.HasValue
                                        ? a.ValidDatefrom.Value.ToString("dd/MMM/yyyy").Replace(" ", "/")
                                        : string.Empty,
                                    ValidDteFrm = a.ValidDatefrom.HasValue
                                        ? a.ValidDatefrom.Value.ToString("dd/MM/yyyy")
                                        : string.Empty,
                                    DateTo = a.ValidDateTo.HasValue
                                        ? a.ValidDateTo.Value.ToString("dd/MMM/yyyy").Replace(" ", "/")
                                        : string.Empty,
                                    ValidDteTo = a.ValidDateTo.HasValue
                                        ? a.ValidDateTo.Value.ToString("dd/MM/yyyy")
                                        : string.Empty,
                                    ValidDatefrom = a.ValidDatefrom.HasValue
                                        ? a.ValidDatefrom.Value.ToString("dd-MMM-yyyy").Replace(" ", "-")
                                        : string.Empty,
                                    ValidDateTo = a.ValidDateTo.HasValue
                                        ? a.ValidDateTo.Value.ToString("dd-MMM-yyyy").Replace(" ", "-")
                                        : string.Empty
                                }).ToListAsync();

            return result;
        }
        public async Task<int> BulkDeleteEmpPolicy(string ShiftIDs)
        {
            var splitEMpIDs = ShiftIDs.Split(',').Select(int.Parse).ToList();
            var recordsToDelete = await _context.AttendancepolicyMasterAccesses
                .Where(e => splitEMpIDs.Contains((int)e.AttendanceAccessId))
                .ToListAsync();
            if (recordsToDelete.Any())
            {
                _context.AttendancepolicyMasterAccesses.RemoveRange(recordsToDelete);
                await _context.SaveChangesAsync();
                return 1;
            }
            return 0; // No records found to delete, return 0 or handle as needed

        }

        public async Task<List<AttendancePolicyAccessDto>> ViewEmployeeShiftPolicy(ViewEmployeeShiftPolicyDto request)
        {
            var employeeIdList = request.EmployeeIDs.Split(',').Select(int.Parse).ToList();
            var newEmpId = await _context.HrEmployeeUserRelations
                .Where(x => x.UserId == request.entryBy)
                .Select(x => (int?)x.EmpId)
                .FirstOrDefaultAsync();
            var linkLevel = await _context.SpecialAccessRights
                .Where(x => x.RoleId == request.roleId)
                .Select(x => (int?)x.LinkLevel)
                .FirstOrDefaultAsync();

            var transId = await _context.TransactionMasters
                .Where(x => x.TransactionType == "AtnPolicy")
                .Select(x => (int?)x.TransactionId)
                .FirstOrDefaultAsync();

            var designationDetails = await _context.DesignationDetails.ToListAsync();


            if (string.IsNullOrWhiteSpace(request.EmployeeIDs))
            {
                request.EmployeeIDs = "0";
            }
            var ifLevel15Exist = _context.EntityAccessRights02s
                .AsEnumerable()
                .Where(s => s.RoleId == request.roleId && s.LinkLevel == 15)
                .SelectMany(s => SplitStrings_XML(s.LinkId))
                .Any();
            if (ifLevel15Exist)
            {
                // Parse comma-separated employee IDs
                var employeeIds = request.EmployeeIDs == "0"
                    ? null
                    : request.EmployeeIDs.Split(',').Select(int.Parse).ToList();

                // Load DesignationDetails separately (since it's like a temp table)
                var designationDetailsList = await _context.DesignationDetails.ToListAsync();

                // Step 1: Filter and Join AttendancepolicyMasterAccess with EmployeeDetails
                var accessWithEmployees = await (
                    from access in _context.AttendancepolicyMasterAccesses
                    join emp in _context.EmployeeDetails
                        on access.EmployeeId equals emp.EmpId
                    where request.AttendanceAccessID == 0 || access.AttendanceAccessId == request.AttendanceAccessID
                    where employeeIds == null || employeeIds.Contains((int)access.EmployeeId)
                    select new { access, emp }
                ).ToListAsync();

                // Step 2: Join with Attendancepolicy00
                var accessWithPolicy = (
                    from ae in accessWithEmployees
                    join policy in _context.Attendancepolicy00s
                        on ae.access.PolicyId equals policy.AttendancePolicyId
                    select new { ae.access, ae.emp, policy }
                ).ToList();

                // Step 3: Join with HrEmpMaster and filter SeperationStatus
                var accessWithHr = (
                    from ap in accessWithPolicy
                    join hr in _context.HrEmpMasters
                        on ap.emp.EmpId equals hr.EmpId
                    where hr.SeperationStatus == 0
                    select new { ap.access, ap.emp, ap.policy, hr }
                ).ToList();

                // Step 4: Client-side join with DesignationDetails (in-memory)
                var resultList = (
                    from full in accessWithHr
                    join desig in designationDetailsList
                        on full.hr.DesigId equals desig.LinkId
                    select new AttendancePolicyAccessDto
                    {
                        AttendanceAccessId = (int)full.access.AttendanceAccessId,
                        EmployeeId = full.emp.EmpId,
                        EmployeeCode = full.emp.EmpCode,
                        EmployeeName = full.emp.Name,
                        AttendancePolicyId = full.policy.AttendancePolicyId,
                        PolicyName = full.policy.PolicyName,
                        Designation = desig.Designation,
                        Datefrom = full.access.ValidDatefrom?.ToString("dd/MMM/yyyy").Replace(" ", "/") ?? "",
                        ValidDteFrm = full.access.ValidDatefrom?.ToString("dd/MM/yyyy") ?? "",
                        DateTo = full.access.ValidDateTo?.ToString("dd/MMM/yyyy").Replace(" ", "/") ?? "",
                        ValidDteTo = full.access.ValidDateTo?.ToString("dd/MM/yyyy") ?? "",
                        ValidDatefrom = full.access.ValidDatefrom?.ToString("dd-MMM-yyyy").Replace(" ", "-") ?? "",
                        ValidDateTo = full.access.ValidDateTo?.ToString("dd-MMM-yyyy").Replace(" ", "-") ?? ""
                    }).ToList();

                return resultList;

            }
            else
            {
                var empEntityStr = await _context.HrEmpMasters
                .Where(h => h.EmpId == newEmpId)
                .Select(h => h.EmpEntity)
                .FirstOrDefaultAsync();

                var empEntityLinks = SplitStrings_XML(empEntityStr)
                    .Select((item, index) => new LinkItemDto { Item = item, LinkLevel = index + 2 });

                var accessRights = await _context.EntityAccessRights02s
                    .Where(s => s.RoleId == request.roleId && !string.IsNullOrEmpty(s.LinkId))
                    .ToListAsync();

                var accessLinkItems = accessRights
                    .SelectMany(s => SplitStrings_XML(s.LinkId)
                        .Select(item => new LinkItemDto { Item = item, LinkLevel = s.LinkLevel }));

                var applicableLinks = accessLinkItems.ToList();

                if (linkLevel > 0)
                {
                    applicableLinks.AddRange(empEntityLinks.Where(c => c.LinkLevel >= linkLevel));
                }

                var applicableFinal = applicableLinks.ToList();



                // Step 1: Load DesignationDetails (used later for join)


                // Step 2: Load Attendance Access with Employee Details
                var accessWithEmployees = await (
                    from access in _context.AttendancepolicyMasterAccesses
                    join emp in _context.EmployeeDetails
                        on access.EmployeeId equals emp.EmpId
                    where request.AttendanceAccessID == 0 || access.AttendanceAccessId == request.AttendanceAccessID
                    where employeeIdList == null || employeeIdList.Contains((int)access.EmployeeId)
                    select new { access, emp }
                ).ToListAsync();

                // Step 3: Join with Attendance Policy
                var accessWithPolicy = (
                    from ae in accessWithEmployees
                    join policy in _context.Attendancepolicy00s
                        on ae.access.PolicyId equals policy.AttendancePolicyId
                    select new { ae.access, ae.emp, policy }
                ).ToList();

                // Step 4: Join with HrEmpMaster and filter SeperationStatus
                var accessWithHr = (
                    from ap in accessWithPolicy
                    join hr in _context.HrEmpMasters
                        on ap.emp.EmpId equals hr.EmpId
                    where hr.SeperationStatus == 0
                    select new { ap.access, ap.emp, ap.policy, hr }
                ).ToList();

                // Step 5: Fetch HighLevelViewTable for each employee (inefficient if many — can optimize)
                var result = (
                    from full in accessWithHr
                    let highLevel = _context.HighLevelViewTables
                        .FirstOrDefault(x => x.LastEntityId == full.emp.LastEntity)
                    where applicableFinal.Any(f =>
                        (highLevel.LevelOneId == int.Parse(f.Item) && f.LinkLevel == 1) ||
                        (highLevel.LevelTwoId == int.Parse(f.Item) && f.LinkLevel == 2) ||
                        (highLevel.LevelThreeId == int.Parse(f.Item) && f.LinkLevel == 3) ||
                        (highLevel.LevelFourId == int.Parse(f.Item) && f.LinkLevel == 4) ||
                        (highLevel.LevelFiveId == int.Parse(f.Item) && f.LinkLevel == 5) ||
                        (highLevel.LevelSixId == int.Parse(f.Item) && f.LinkLevel == 6) ||
                        (highLevel.LevelSevenId == int.Parse(f.Item) && f.LinkLevel == 7) ||
                        (highLevel.LevelEightId == int.Parse(f.Item) && f.LinkLevel == 8) ||
                        (highLevel.LevelNineId == int.Parse(f.Item) && f.LinkLevel == 9) ||
                        (highLevel.LevelTenId == int.Parse(f.Item) && f.LinkLevel == 10) ||
                        (highLevel.LevelElevenId == int.Parse(f.Item) && f.LinkLevel == 11) ||
                        (highLevel.LevelTwelveId == int.Parse(f.Item) && f.LinkLevel == 12))
                    join desig in designationDetails
                        on full.hr.DesigId equals desig.LinkId
                    select new AttendancePolicyAccessDto
                    {
                        AttendanceAccessId = (int)full.access.AttendanceAccessId,
                        EmpId = full.emp.EmpId,
                        EmployeeName = full.emp.Name + "|| " + full.emp.EmpCode,
                        AttendancePolicyId = full.policy.AttendancePolicyId,
                        PolicyName = full.policy.PolicyName,
                        EmployeeId = full.access.EmployeeId,
                        Designation = desig.Designation,
                        Datefrom = full.access.ValidDatefrom?.ToString("dd/MMM/yyyy").Replace(" ", "/") ?? "",
                        DateTo = full.access.ValidDateTo?.ToString("dd/MMM/yyyy").Replace(" ", "/") ?? "",
                        ValidDatefrom = full.access.ValidDatefrom?.ToString("dd-MMM-yyyy").Replace(" ", "-") ?? "",
                        ValidDateTo = full.access.ValidDateTo?.ToString("dd-MMM-yyyy").Replace(" ", "-") ?? "",
                        ValidDteFrm = full.access.ValidDatefrom?.ToString("dd/MM/yyyy") ?? "",
                        ValidDteTo = full.access.ValidDateTo?.ToString("dd/MM/yyyy") ?? ""
                    }
                ).ToList();

                return result;
            }

        }


        
    }
}
