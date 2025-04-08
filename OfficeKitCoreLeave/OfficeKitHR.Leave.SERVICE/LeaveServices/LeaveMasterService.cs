using AutoMapper;
using EMPLOYEE_INFORMATION.Data;
using EMPLOYEE_INFORMATION.Models;
using EMPLOYEE_INFORMATION.Models.Entity;
using Microsoft.EntityFrameworkCore;
using MPLOYEE_INFORMATION.DTO.DTOs;
using OFFICEKITCORELEAVE.OfficeKit.Leave.Data;
using OFFICEKITCORELEAVE.OfficeKit.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKit.Leave.MODELS;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.Interface.LeaveInterfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using HRMS.EmployeeInformation.Models;

namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.SERVICE.LeaveServices
{
    public class LeaveMasterService : ILeaveMasterService
    {
        private readonly IMapper _mapper;
        private readonly LeaveDBContext _dbContext;
        private readonly EmployeeDBContext _employeedbContext;
        public LeaveMasterService (IMapper mapper_, LeaveDBContext dbContext_, EmployeeDBContext employeedbContext_)
        {
            _mapper = mapper_;
            _dbContext = dbContext_;
            _employeedbContext = employeedbContext_;
        }

        public async Task<int> SaveLeaveMaster (HrmLeaveMasterDTO dto)
        {
            var leaveMaster = _mapper.Map<HrmLeaveMaster> (dto);

            if (dto.LeaveMasterId != 0)
            {
                _dbContext.HrmLeaveMasters.Update (leaveMaster);
            }


            else
            {
                await _dbContext.HrmLeaveMasters.AddAsync (leaveMaster);
            }

            await _dbContext.SaveChangesAsync ( );
            return leaveMaster.LeaveMasterId;
        }

        public async Task<HrmLeaveMasterDTO> GetLeaveMasterById (int leaveMasterId)
        {
            try
            {
                var leaveMaster = await _dbContext.HrmLeaveMasters.FindAsync (leaveMasterId);
                if (leaveMaster == null)
                {
                    return null;
                }

                return _mapper.Map<HrmLeaveMasterDTO> (leaveMaster);
            }
            catch (Exception ex)
            {
                throw new Exception ("Failed to retrieve leave master", ex);
            }
        }

        public async Task<bool> DeleteLeaveMaster (int leaveMasterId)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync ( );
            try
            {
                var settingsLink = await _dbContext.HrmLeaveMasterandsettingsLinks
                    .FirstOrDefaultAsync (x => x.LeaveMasterId == leaveMasterId);

                if (settingsLink != null)
                {
                    int? settingsId = settingsLink.SettingsId;

                    // Get SettingsDetailsId (exception)
                    var settingsDetail = await _dbContext.HrmLeaveBasicsettingsDetails
                        .FirstOrDefaultAsync (x => x.SettingsId == settingsId);
                    int? settingsDetailsId = settingsDetail?.SettingsDetailsId;

                    // Get LeaveEntitlementId (prorata)
                    var entitlement = await _dbContext.HrmLeaveEntitlementHeads
                        .FirstOrDefaultAsync (x => x.SettingsId == settingsId);
                    int? entitlementId = entitlement?.LeaveEntitlementId;

                    // Delete from HRM_LEAVE_ENTITLEMENT_REG
                    if (entitlementId.HasValue)
                    {
                        var entitlementRegs = _dbContext.HrmLeaveEntitlementRegs
                            .Where (x => x.LeaveEntitlementId == entitlementId.Value);
                        _dbContext.HrmLeaveEntitlementRegs.RemoveRange (entitlementRegs);
                    }

                    // Delete from HRM_LEAVE_ENTITLEMENT_HEAD
                    if (entitlement != null)
                        _dbContext.HrmLeaveEntitlementHeads.Remove (entitlement);

                    //// Delete from HRM_LEAVE_EXCEPTIONAL_ELIGIBILITY
                    //if (settingsDetailsId.HasValue)
                    //{
                    //    var exceptionalEligibilities = _dbContext.HrmLeaveExceptionalEligibilities
                    //        .Where (x => x.SettingsDetailsHeadId == settingsDetailsId.Value);
                    //    _dbContext.HrmLeaveExceptionalEligibilities.RemoveRange (exceptionalEligibilities);
                    //}

                    // Delete from HRM_LEAVE_BASICSETTINGS_DETAILS
                    if (settingsDetail != null)
                        _dbContext.HrmLeaveBasicsettingsDetails.Remove (settingsDetail);

                    // Delete from HRM_LEAVE_BASIC_SETTINGS
                    var basicSettings = await _dbContext.HrmLeaveBasicSettings
                        .FirstOrDefaultAsync (x => x.SettingsId == settingsId);
                    if (basicSettings != null)
                        _dbContext.HrmLeaveBasicSettings.Remove (basicSettings);

                    // Delete from HRM_LEAVE_MASTERANDSETTINGS_LINK
                    _dbContext.HrmLeaveMasterandsettingsLinks.Remove (settingsLink);
                }

                // Delete from HRM_LEAVE_MASTER
                var leaveMaster = await _dbContext.HrmLeaveMasters
                    .FirstOrDefaultAsync (x => x.LeaveMasterId == leaveMasterId);
                if (leaveMaster != null)
                    _dbContext.HrmLeaveMasters.Remove (leaveMaster);

                // Save all changes
                await _dbContext.SaveChangesAsync ( );
                await transaction.CommitAsync ( );

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync ( );
                return false;
            }
        }


        public async Task<bool> Checkexistance (string leaveCode)
        {
            var checkDuplication = await _dbContext.HrmLeaveMasters.FirstOrDefaultAsync (x => x.LeaveCode.Replace (" ", "") == leaveCode.Replace (" ", ""));

            return checkDuplication != null;
        }
        private static IEnumerable<string> SplitStrings_XML (string list, char delimiter = ',')
        {
            if (string.IsNullOrWhiteSpace (list))
                return Enumerable.Empty<string> ( );

            // Split the input string by the delimiter and return as IEnumerable
            return list.Split (delimiter)
                   .Select (item => item.Trim ( )) // Trim whitespace from each item
                   .Where (item => !string.IsNullOrEmpty (item)); // Exclude empty items
        }
        public async Task<List<HrmLeaveMasterViewDto>> GetAllLeaveMasters (HrmLeaveMasterSearchDto sortDto)
        {
            var transid = await _employeedbContext.TransactionMasters
               .Where (t => t.TransactionType == "Leave")
               .Select (t => t.TransactionId)
               .FirstOrDefaultAsync ( );

            int? lnklev = await _employeedbContext.SpecialAccessRights
                .Where (s => s.RoleId == sortDto.RoleId)
                .Select (s => s.LinkLevel)
                .FirstOrDefaultAsync ( );

            bool hasAccess = await _employeedbContext.EntityAccessRights02s
                .AnyAsync (s => s.RoleId == sortDto.RoleId && s.LinkLevel == 15);

            if (hasAccess)
            {
                var leaveMasters = _dbContext.HrmLeaveMasters.ToList ( );

                // Query employeemaster from _employeedbContext
                var adm_User_Masters = _employeedbContext.AdmUserMasters.ToList ( );

                var leavemastersdata = (from leaveMaster in leaveMasters
                                        join ADM_User_Master in adm_User_Masters
                                        on leaveMaster.CreatedBy equals ADM_User_Master.UserId
                                        select new HrmLeaveMasterViewDto
                                        {
                                            Username = ADM_User_Master.UserName,
                                            LeavemasterId = leaveMaster.LeaveMasterId,
                                            LeaveCode = leaveMaster.LeaveCode,
                                            Description = leaveMaster.Description,
                                            PayType = leaveMaster.PayType,
                                            LeaveUnit = leaveMaster.LeaveUnit,
                                            Active = leaveMaster.Active,
                                        }).ToList ( );
                return leavemastersdata;
            }

            // **Step 1: Compute `ctnew`**
            var empEntity = await _employeedbContext.HrEmpMasters
                .Where (h => h.EmpId == sortDto.employeeId)
                .Select (h => h.EmpEntity)
                .FirstOrDefaultAsync ( );

            var ctnew = SplitStrings_XML (empEntity, ',')
                .Select ((item, index) => new LinkItemDto { Item = item, LinkLevel = index + 2 })
                .Where (c => !string.IsNullOrEmpty (c.Item))
                .ToList ( );

            // **Step 2: Compute `applicableFinal`**
            var applicableFinal = _employeedbContext.EntityAccessRights02s
                .Where (s => s.RoleId == sortDto.RoleId).ToList ( )
                .SelectMany (s => SplitStrings_XML (s.LinkId, default),
                    (s, item) => new LinkItemDto { Item = item, LinkLevel = s.LinkLevel })
                ;

            var applicablefinall = applicableFinal.ToList ( );

            if (lnklev > 0)
            {
                applicablefinall.AddRange (
                    ctnew.Where (c => c.LinkLevel >= lnklev)
                         .Select (c => new LinkItemDto { Item = c.Item, LinkLevel = c.LinkLevel })
                );
            }

            // Convert `applicableFinal` to HashSet for fast lookup
            //var applicableFinalSet = applicableFinal.Select(a => a.Item).ToHashSet();
            var applicableFinalSetLong = applicableFinal.Select (a => (long?)Convert.ToInt64 (a.Item)).ToHashSet ( );

            // **Step 3: Fetch `EntityApplicable00Final`**
            var entityApplicable00Final = await _employeedbContext.EntityApplicable00s
                .Where (e => e.TransactionId == transid)
                .Select (e => new { e.LinkId, e.LinkLevel, e.MasterId })
                .ToListAsync ( );

            // **Step 4: Compute `applicableFinal02`**
            var applicableFinal02 = applicableFinal.ToList ( ); // Already computed

            // **Step 5: Compute `applicableFinal02Emp`**
            var employeedetailsen = await _employeedbContext.EmployeeDetails.ToListAsync ( );
            var access2 = await _employeedbContext.EntityApplicable01s.ToListAsync ( );
            var high = await _employeedbContext.HighLevelViewTables.ToListAsync ( );


            var applicableFinal02Emp = (
            from emp in employeedetailsen
            join ea in access2 on emp.EmpId equals ea.EmpId
            join hlv in high on emp.LastEntity equals hlv.LastEntityId
            join af2 in applicableFinal02.ToList ( )  // Convert to List before join
            on hlv.LevelOneId.ToString ( ) equals af2.Item into af2LevelOne
            from af2L1 in af2LevelOne.DefaultIfEmpty ( )
            where ea.TransactionId == transid
            select ea.MasterId).Distinct ( ).ToList ( );



            // **Step 6: Compute `newhigh`**
            var newhigh = entityApplicable00Final
                .Where (e => applicableFinalSetLong.Contains (e.LinkId) || e.LinkLevel == 15)
                .Select (e => e.MasterId)
                .Union (applicableFinal02Emp)
                .Distinct ( )
                .ToList ( );

            var hrmleavemasters = await _dbContext.HrmLeaveMasters.ToListAsync ( );
            var admusermasters = await _employeedbContext.AdmUserMasters.ToListAsync ( );

            return (from leaveMaster in hrmleavemasters
                    where newhigh.Contains (leaveMaster.LeaveMasterId)
                    join ADM_User_Master in admusermasters
                    on leaveMaster.CreatedBy equals ADM_User_Master.UserId
                    select new HrmLeaveMasterViewDto
                    {
                        Username = ADM_User_Master.UserName,
                        LeavemasterId = leaveMaster.LeaveMasterId,
                        LeaveCode = leaveMaster.LeaveCode,
                        Description = leaveMaster.Description,
                        PayType = leaveMaster.PayType,
                        LeaveUnit = leaveMaster.LeaveUnit,
                        Active = leaveMaster.Active
                    }).ToList ( );
        }

        //public async Task<int> SaveApplicableSettings (SaveApplicableParameters  dto)
        //{
        //    dto.TranId1 = _employeedbContext.TransactionMasters.Where (x => x.TransactionType == dto.TransactionType).FirstOrDefault ( ).TransactionId;
        //    if(_employeedbContext.EntityApplicable00s.Any(x => x.TransactionId == dto.TranId1 && x.MasterId == dto.MasterId))
        //    {
        //        var entityapplicable00list = _employeedbContext.EntityApplicable00s.Where(x => x.TransactionId == dto.TranId1 && x.MasterId == dto.MasterId);
        //        var entityapplicable01list = _employeedbContext.EntityApplicable01s.Where(x => x.TransactionId == dto.TranId1 && x.MasterId == dto.MasterId);
        //        foreach ( var item in entityapplicable00list)
        //        {
        //            _employeedbContext.EntityApplicable00s.Remove(item);
        //        }
        //        foreach ( var item in entityapplicable01list)
        //        {
        //            _employeedbContext.EntityApplicable01s.Remove(item);
        //        }
        //        _employeedbContext.SaveChangesAsync ( );

        //    }
        //    return 0;
        //}

        public async Task<int> EntityApplicableSave (SaveApplicableParameters dto)
        {
            var transaction = await _employeedbContext.TransactionMasters
                .FirstOrDefaultAsync (x => x.TransactionType == dto.TransactionType);

            if (transaction == null)
            {
                return -1;
            }

            dto.TranId1 = transaction.TransactionId;

            if (dto.FirstEntityId == 15)
            {
                if (!await _employeedbContext.EntityApplicable00s
                .AnyAsync (x => x.TransactionId == dto.TranId1 && x.MasterId == dto.MasterId))
                {
                    return 0;
                }


                var entityApplicable00List = await _employeedbContext.EntityApplicable00s
                    .Where (x => x.TransactionId == dto.TranId1 && x.MasterId == dto.MasterId)
                    .ToListAsync ( );

                var entityApplicable01List = await _employeedbContext.EntityApplicable01s
                    .Where (x => x.TransactionId == dto.TranId1 && x.MasterId == dto.MasterId)
                    .ToListAsync ( );

                //_employeedbContext.EntityApplicable00s.RemoveRange (entityApplicable00List);
                //_employeedbContext.EntityApplicable01s.RemoveRange (entityApplicable01List);

                var entityapplicabledata = new EntityApplicable00
                {
                    TransactionId = dto.TranId1,
                    LinkLevel = dto.FirstEntityId,
                    LinkId = 0,
                    MasterId = dto.MasterId,
                    MainMasterId = dto.SecondEntityId,
                    EntryBy = dto.EntryBy,
                    EntryDate = dto.CreateDate,
                };

                //_employeedbContext.EntityApplicable00s.Add (entityapplicabledata);

                await _employeedbContext.SaveChangesAsync ( );
            }
            else
            {
                var rolemaster = _employeedbContext.AdmRoleMasters
                    .Where (x => x.Type == "A" && x.RoleId == dto.EntryBy);

                var entityaccessrights = _employeedbContext.EntityAccessRights02s
                    .Where (x => x.RoleId == dto.EntryBy && x.LinkLevel == 15);

                bool roleEntityCheck = !_employeedbContext.AdmRoleMasters.Any (r => r.Type == "A" && r.RoleId == dto.EntryBy) && !_employeedbContext.EntityAccessRights02s.Any (e => e.RoleId == dto.EntryBy && e.LinkLevel == 15);

                if (roleEntityCheck)
                {
                    // Materialize applicableFinal into memory
                    var applicableFinal = _employeedbContext.EntityAccessRights02s
                        .Where (s => s.RoleId == dto.EntryBy)
                        .ToList ( )  // Fetch from DB
                        .SelectMany (s => SplitStrings_XML (s.LinkId, default),
                            (s, item) => new LinkItemDto { Item = item, LinkLevel = s.LinkLevel })
                        .ToList ( );  // Ensure it’s fully in memory

                    var entityApplicable00Final = _employeedbContext.EntityApplicable00s
                        .Where (e => e.TransactionId == dto.TranId1 && e.MasterId == dto.MasterId)
                        .Select (e => new { e.LinkId, e.LinkLevel, e.MasterId, e.ApplicableId })
                        .ToList ( );  // Materialize for reference if needed

                    // Client-side evaluation for applicableIdsToDelete
                    var applicableIdsToDelete = (from c in entityApplicable00Final
                                                 from a in _employeedbContext.HighLevelViewTables
                                                 select new { c, a })  // Server-side cross join
                        .AsEnumerable ( )  // Switch to client-side
                        .SelectMany (x => applicableFinal, (x, b) => new { x.c, x.a, b })
                        .Where (x => (x.a.LevelOneId == x.c.LinkId && x.c.LinkLevel == 1) ||
                                    (x.a.LevelTwoId == x.c.LinkId && x.c.LinkLevel == 2) ||
                                    (x.a.LevelThreeId == x.c.LinkId && x.c.LinkLevel == 3) ||
                                    (x.a.LevelFourId == x.c.LinkId && x.c.LinkLevel == 4) ||
                                    (x.a.LevelFiveId == x.c.LinkId && x.c.LinkLevel == 5) ||
                                    (x.a.LevelSixId == x.c.LinkId && x.c.LinkLevel == 6) ||
                                    (x.a.LevelSevenId == x.c.LinkId && x.c.LinkLevel == 7) ||
                                    (x.a.LevelEightId == x.c.LinkId && x.c.LinkLevel == 8) ||
                                    (x.a.LevelNineId == x.c.LinkId && x.c.LinkLevel == 9) ||
                                    (x.a.LevelTenId == x.c.LinkId && x.c.LinkLevel == 10) ||
                                    (x.a.LevelElevenId == x.c.LinkId && x.c.LinkLevel == 11) ||
                                    (x.a.LevelTwelveId == x.c.LinkId && x.c.LinkLevel == 12)
                                    && (
                                        (x.a.LevelOneId == int.Parse (x.b.Item) && x.b.LinkLevel == 1) ||
                                        (x.a.LevelTwoId == int.Parse (x.b.Item) && x.b.LinkLevel == 2) ||
                                        (x.a.LevelThreeId == int.Parse (x.b.Item) && x.b.LinkLevel == 3) ||
                                        (x.a.LevelFourId == int.Parse (x.b.Item) && x.b.LinkLevel == 4) ||
                                        (x.a.LevelFiveId == int.Parse (x.b.Item) && x.b.LinkLevel == 5) ||
                                        (x.a.LevelSixId == int.Parse (x.b.Item) && x.b.LinkLevel == 6) ||
                                        (x.a.LevelSevenId == int.Parse (x.b.Item) && x.b.LinkLevel == 7) ||
                                        (x.a.LevelEightId == int.Parse (x.b.Item) && x.b.LinkLevel == 8) ||
                                        (x.a.LevelNineId == int.Parse (x.b.Item) && x.b.LinkLevel == 9) ||
                                        (x.a.LevelTenId == int.Parse (x.b.Item) && x.b.LinkLevel == 10) ||
                                        (x.a.LevelElevenId == int.Parse (x.b.Item) && x.b.LinkLevel == 11) ||
                                        (x.a.LevelTwelveId == int.Parse (x.b.Item) && x.b.LinkLevel == 12) ||
                                        (x.b.LinkLevel == 15)
                                    ))
                        .Select (x => x.c.ApplicableId)
                        .ToList ( );

                    // Delete operation
                    var recordsToDelete = _employeedbContext.EntityApplicable00s
                        .Where (e => applicableIdsToDelete.Contains (e.ApplicableId))
                        .ToList ( );
                    //_employeedbContext.EntityApplicable00s.RemoveRange (recordsToDelete);
                    _employeedbContext.SaveChanges ( );

                    // Client-side evaluation for applicableFinal02Emp
                    var applicableFinal02Emp = (from d in _employeedbContext.EmployeeDetails
                                                join e in _employeedbContext.EntityApplicable01s
                                                on d.EmpId equals e.EmpId
                                                where e.TransactionId == dto.TranId1 && e.MasterId == dto.MasterId
                                                join a in _employeedbContext.HighLevelViewTables
                                                on d.LastEntity equals a.LastEntityId
                                                select new { d, e, a })  // Server-side joins
                        .AsEnumerable ( )  // Switch to client-side
                        .SelectMany (x => applicableFinal, (x, b) => new { x.d, x.e, x.a, b })
                        .Where (x => (x.a.LevelOneId == int.Parse (x.b.Item) && x.b.LinkLevel == 1) ||
                                    (x.a.LevelTwoId == int.Parse (x.b.Item) && x.b.LinkLevel == 2) ||
                                    (x.a.LevelThreeId == int.Parse (x.b.Item) && x.b.LinkLevel == 3) ||
                                    (x.a.LevelFourId == int.Parse (x.b.Item) && x.b.LinkLevel == 4) ||
                                    (x.a.LevelFiveId == int.Parse (x.b.Item) && x.b.LinkLevel == 5) ||
                                    (x.a.LevelSixId == int.Parse (x.b.Item) && x.b.LinkLevel == 6) ||
                                    (x.a.LevelSevenId == int.Parse (x.b.Item) && x.b.LinkLevel == 7) ||
                                    (x.a.LevelEightId == int.Parse (x.b.Item) && x.b.LinkLevel == 8) ||
                                    (x.a.LevelNineId == int.Parse (x.b.Item) && x.b.LinkLevel == 9) ||
                                    (x.a.LevelTenId == int.Parse (x.b.Item) && x.b.LinkLevel == 10) ||
                                    (x.a.LevelElevenId == int.Parse (x.b.Item) && x.b.LinkLevel == 11) ||
                                    (x.a.LevelTwelveId == int.Parse (x.b.Item) && x.b.LinkLevel == 12) ||
                                    (x.b.LinkLevel == 15))
                        .Select (x => x.e.ApplicableId)
                        .ToList ( );

                    var deleteRecords = _employeedbContext.EntityApplicable01s
                        .Where (x => applicableFinal02Emp.Contains (x.ApplicableId))
                        .ToList ( );
                    //_employeedbContext.EntityApplicable01s.RemoveRange (deleteRecords);
                    await _employeedbContext.SaveChangesAsync ( );  // Use async consistently
                }
            }
            if (dto.LinkIds == null)
            {
                var levelOneSelectionIds = dto.EntityList;
                var levelTwoSelectionIds = string.Empty;
                var levelThreeSelectionIds = string.Empty;
                var levelFourSelectionIds = string.Empty;
                var levelFiveSelectionIds = string.Empty;
                var levelSixSelectionIds = string.Empty;
                var levelSevenSelectionIds = string.Empty;
                var levelEightSelectionIds = string.Empty;
                var levelNineSelectionIds = string.Empty;
                var levelTenSelectionIds = string.Empty;
                var levelElevenSelectionIds = string.Empty;
                var levelTwelveSelectionIds = string.Empty;
                var linkIdSegments = dto.LinkIds.Split ('+', StringSplitOptions.RemoveEmptyEntries);

                foreach (var segment in linkIdSegments)
                {
                    var linkIds = segment.Split (',', StringSplitOptions.RemoveEmptyEntries)
                        .Select (id => int.TryParse (id.Trim ( ), out int result) ? result : -1) // Handle invalid IDs
                        .Where (id => id != -1)
                        .ToList ( );

                    if (!linkIds.Any ( ))
                    {
                        continue; // Skip empty or invalid segments
                    }

                    // Get LinkLevel for the first valid LinkID in the segment
                    var linkLevel = _employeedbContext.SubCategoryLinksNews
                        .Where (s => linkIds.Contains (s.LinkId))
                        .Select (s => s.LinkLevel)
                        .FirstOrDefault ( );

                    var newEntities = _employeedbContext.SubCategoryLinksNews
                        .Where (s => linkIds.Contains (s.LinkId))
                        .Select (s => new EntityApplicable00
                        {
                            TransactionId = dto.TranId1 ?? 0,
                            LinkLevel = linkLevel,
                            LinkId = s.LinkId,
                            MasterId = dto.MasterId ?? 0,
                            EntryBy = dto.EntryBy ?? 0,
                            EntryDate = DateTime.UtcNow
                        })
                        .ToList ( );
                    _employeedbContext.EntityApplicable00s.AddRange (newEntities);
                    var subCategoryData = _employeedbContext.SubCategoryLinksNews.ToList ( );
                }
                _dbContext.SaveChanges ( );
            }
            if (dto.EmployeeIds != "0" && dto.FirstEntityId == 0)
            {
                var employeeIds = dto.EmployeeIds.Split (',', StringSplitOptions.RemoveEmptyEntries).Select (id => int.TryParse (id.Trim ( ), out int result) ? result : -1).Where (id => id != -1).ToList ( );

                if (!employeeIds.Any ( ))
                {
                    return 0; // No valid employee IDs to process
                }
                var newEntities = _employeedbContext.HrEmpMasters
                    .Where (emp => employeeIds.Contains (emp.EmpId))
                    .Select (emp => new EntityApplicable01
                    {
                        TransactionId = dto.TranId1 ?? 0,
                        LinkLevel = 13,
                        EmpId = emp.EmpId,
                        MasterId = dto.MasterId ?? 0,
                        MainMasterId = dto.SecondEntityId ?? 0,
                        EntryBy = dto.EntryBy ?? 0,
                        EntryDate = DateTime.UtcNow
                    })
                    .ToList ( );
                if (newEntities.Any ( ))
                {
                    _employeedbContext.EntityApplicable01s.AddRange (newEntities);
                    await _employeedbContext.SaveChangesAsync ( );
                }
            }
            if (dto.EntityList != null && dto.FirstEntityId == 0)
            {
                var entityIds = dto.EntityList.Split (',', StringSplitOptions.RemoveEmptyEntries).Select (id => int.TryParse (id.Trim ( ), out int result) ? result : -1).Where (id => id != -1).ToList ( );

                if (!entityIds.Any ( ))
                {
                    return 0; // No valid entity IDs to process
                }
                var newEntities = _dbContext.EntityLevelOnes
                    .Where (entity => entityIds.Contains (entity.LevelOneId))
                    .Select (entity => new EntityApplicable00
                    {
                        TransactionId = dto.TranId1 ?? 0,
                        LinkLevel = 1,
                        LinkId = entity.LevelOneId,
                        MasterId = dto.MasterId ?? 0,
                        MainMasterId = dto.SecondEntityId ?? 0,
                        EntryBy = dto.EntryBy ?? 0,
                        EntryDate = DateTime.UtcNow
                    })
                    .ToList ( );
                if (newEntities.Any ( ))
                {
                    _employeedbContext.EntityApplicable00s.AddRange (newEntities);
                    await _employeedbContext.SaveChangesAsync ( );
                }

                var Student = new EntityApplicable00
                {
                    LinkLevel = "1"
                };
            }
            return 0;
        }
    }
}