using AutoMapper;
using EMPLOYEE_INFORMATION.Data;
using Microsoft.EntityFrameworkCore;
using MPLOYEE_INFORMATION.DTO.DTOs;
using OFFICEKITCORELEAVE.OfficeKit.Leave.Data;
using OFFICEKITCORELEAVE.OfficeKit.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKit.Leave.MODELS;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.Interface.LeaveInterfaces;

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
            var Data = _mapper.Map<HrmLeaveMaster> (await GetLeaveMasterById (leaveMasterId));
            if (Data == null)
            {
                return false;
            }
            _dbContext.HrmLeaveMasters.Remove (Data);
            _dbContext.SaveChanges ( );
            return true;
        }
        //public async Task<List<HrmLeaveMasterViewDto>> GetAllLeaveMasters (HrmLeaveMasterSearchDto sortDto)
        //{
        //    var transaction = await _employeedbContext.TransactionMasters.FirstOrDefaultAsync (x => x.TransactionType == "Leave");
        //    int transactionId = transaction?.TransactionId ?? 0;
        //    var specialright = _employeedbContext.SpecialAccessRights.Where (x => x.RoleId == sortDto.RoleId);
        //    var boolr = _employeedbContext.EntityAccessRights02s.AsEnumerable ( ).SelectMany (s => SplitStrings_XML (s.LinkId, default).Select (item => new { s.RoleId, s.LinkLevel, Item = item })).Any (x => x.RoleId == sortDto.RoleId && x.LinkLevel == 15);

        //    if (boolr)
        //    {
        //        // Query leaveMaster from _dbContext
        //        var leaveMasters = _dbContext.HrmLeaveMasters.ToList ( );

        //        // Query employeemaster from _employeedbContext
        //        var adm_User_Masters = _employeedbContext.AdmUserMasters.ToList ( );

        //        var resultdata = (from leaveMaster in leaveMasters
        //                          join ADM_User_Master in adm_User_Masters
        //                          on leaveMaster.CreatedBy equals ADM_User_Master.UserId
        //                          select new HrmLeaveMasterViewDto
        //                          {
        //                              Username = ADM_User_Master.UserName,
        //                              LeavemasterId = leaveMaster.LeaveMasterId,
        //                              LeaveCode = leaveMaster.LeaveCode,
        //                              Description = leaveMaster.Description,
        //                              PayType = leaveMaster.PayType,
        //                              LeaveUnit = leaveMaster.LeaveUnit,
        //                              Active = leaveMaster.Active,
        //                          }).ToList ( );
        //        return resultdata;
        //    }


        //    else
        //    {
        //        var lnklev = 0;
        //        var empEntities = (from emp in _employeedbContext.HrEmpMasters
        //                           where emp.EmpId == sortDto.employeeId
        //                           select emp.EmpEntity).FirstOrDefault ( )?.Split (',').Select ((item, index) => new
        //                           {
        //                               Item = item,
        //                               LinkLevelSelf = (int?)(index + 2)
        //                           }).ToList ( );
        //        var data = empEntities;
        //        var entityapplicable00 = await _employeedbContext.EntityApplicable00s.Where (x => x.TransactionId == transactionId).ToListAsync ( );
        //        var entityaccessright02 = await _employeedbContext.EntityAccessRights02s.Where (x => x.RoleId == sortDto.RoleId).ToListAsync ( );
        //        //var applicableFinal = (
        //        //    from s in _employeedbContext.EntityAccessRights02s
        //        //    from f in SplitStrings_XML (s.LinkId,default).Select (item => new { Item = item })
        //        //    where s.RoleId == sortDto.RoleId
        //        //    select new { Item = f.Item, LinkLevel = s.LinkLevel }
        //        //)
        //        //.Union (
        //        //    from ct in empEntities
        //        //    where lnklev > 0 && ct.LinkLevelSelf >= lnklev
        //        //    select new { Item = ct.Item, LinkLevel = ct.LinkLevelSelf }
        //        //)
        //        //.ToList ( );
        //        var applicableFinal = (from s in _employeedbContext.EntityAccessRights02s.AsEnumerable ( ) // Load data into memory
        //                               from f in SplitStrings_XML (s.LinkId, default).Select (item => new { Item = item }) // Perform split in-memory
        //                               where s.RoleId == sortDto.RoleId
        //                               select new { Item = f.Item, LinkLevel = s.LinkLevel })
        //                               .Union (
        //                               from ct in empEntities where lnklev > 0 && ct.LinkLevelSelf >= lnklev select new { Item = ct.Item, LinkLevel = ct.LinkLevelSelf }).ToList ( );
        //        var EntityApplicalble00Final = _employeedbContext.EntityApplicable00s.ToListAsync ( ).Result.Where (x => x.TransactionId == transactionId);

        //        var applicableFinal02EMP = (from d in _employeedbContext.EmployeeDetails
        //                                    join e in _employeedbContext.EntityApplicable01s on d.EmpId equals e.EmpId
        //                                    where e.TransactionId == transactionId
        //                                    join a in _employeedbContext.HighLevelViewTables on d.LastEntity equals a.LastEntityId
        //                                    join b in applicableFinal on true equals true // Join with ApplicableFinal
        //                                    where
        //                                        (b.LinkLevel == 1 && a.LevelOneId == TryParseToInt (b.Item)) ||
        //                                        (b.LinkLevel == 2 && a.LevelTwoId == TryParseToInt (b.Item)) ||
        //                                        (b.LinkLevel == 3 && a.LevelThreeId == TryParseToInt (b.Item)) ||
        //                                        (b.LinkLevel == 4 && a.LevelFourId == TryParseToInt (b.Item)) ||
        //                                        (b.LinkLevel == 5 && a.LevelFiveId == TryParseToInt (b.Item)) ||
        //                                        (b.LinkLevel == 6 && a.LevelSixId == TryParseToInt (b.Item)) ||
        //                                        (b.LinkLevel == 7 && a.LevelSevenId == TryParseToInt (b.Item)) ||
        //                                        (b.LinkLevel == 8 && a.LevelEightId == TryParseToInt (b.Item)) ||
        //                                        (b.LinkLevel == 9 && a.LevelNineId == TryParseToInt (b.Item)) ||
        //                                        (b.LinkLevel == 10 && a.LevelTenId == TryParseToInt (b.Item)) ||
        //                                        (b.LinkLevel == 11 && a.LevelElevenId == TryParseToInt (b.Item)) ||
        //                                        (b.LinkLevel == 12 && a.LevelTwelveId == TryParseToInt (b.Item))
        //                                    select e.MasterId).Distinct ( );

        //        var newHigh = (from c in _employeedbContext.EntityApplicable00s
        //                       join a in _employeedbContext.HighLevelViewTables on c.LinkLevel equals a.LastEntityId
        //                       where
        //                           (c.LinkLevel == 1 && a.LevelOneId == c.LinkId) ||
        //                           (c.LinkLevel == 2 && a.LevelTwoId == c.LinkId) ||
        //                           (c.LinkLevel == 3 && a.LevelThreeId == c.LinkId) ||
        //                           (c.LinkLevel == 4 && a.LevelFourId == c.LinkId) ||
        //                           (c.LinkLevel == 5 && a.LevelFiveId == c.LinkId) ||
        //                           (c.LinkLevel == 6 && a.LevelSixId == c.LinkId) ||
        //                           (c.LinkLevel == 7 && a.LevelSevenId == c.LinkId) ||
        //                           (c.LinkLevel == 8 && a.LevelEightId == c.LinkId) ||
        //                           (c.LinkLevel == 9 && a.LevelNineId == c.LinkId) ||
        //                           (c.LinkLevel == 10 && a.LevelTenId == c.LinkId) ||
        //                           (c.LinkLevel == 11 && a.LevelElevenId == c.LinkId) ||
        //                           (c.LinkLevel == 12 && a.LevelTwelveId == c.LinkId)
        //                       join b in applicableFinal on true equals true
        //                       where
        //                           (b.LinkLevel == 1 && a.LevelOneId == TryParseToInt (b.Item)) ||
        //                           (b.LinkLevel == 2 && a.LevelTwoId == TryParseToInt (b.Item)) ||
        //                           (b.LinkLevel == 3 && a.LevelThreeId == TryParseToInt (b.Item)) ||
        //                           (b.LinkLevel == 4 && a.LevelFourId == TryParseToInt (b.Item)) ||
        //                           (b.LinkLevel == 5 && a.LevelFiveId == TryParseToInt (b.Item)) ||
        //                           (b.LinkLevel == 6 && a.LevelSixId == TryParseToInt (b.Item)) ||
        //                           (b.LinkLevel == 7 && a.LevelSevenId == TryParseToInt (b.Item)) ||
        //                           (b.LinkLevel == 8 && a.LevelEightId == TryParseToInt (b.Item)) ||
        //                           (b.LinkLevel == 9 && a.LevelNineId == TryParseToInt (b.Item)) ||
        //                           (b.LinkLevel == 10 && a.LevelTenId == TryParseToInt (b.Item)) ||
        //                           (b.LinkLevel == 11 && a.LevelElevenId == TryParseToInt (b.Item)) ||
        //                           (b.LinkLevel == 12 && a.LevelTwelveId == TryParseToInt (b.Item))
        //                       select c.MasterId)
        //                      .Union (applicableFinal02EMP)
        //                      .Union (from e in _employeedbContext.EntityApplicable00s where e.LinkLevel == 15 select e.MasterId)
        //                      .Distinct ( )
        //                      .ToList ( );

        //        var finalResult = (from user in _employeedbContext.AdmUserMasters
        //                           join settings in _dbContext.HrmLeaveBasicSettings on user.UserId equals settings.CreatedBy into ps
        //                           from settings in ps.DefaultIfEmpty ( )
        //                           join high in newHigh on settings.SettingsId equals high
        //                           select new
        //                           {
        //                               user.UserName,
        //                               settings.SettingsId,
        //                               settings.SettingsName,
        //                               settings.SettingsDescription,
        //                               CreatedDate = settings.CreatedDate, // Format as DD/MM/YYYY
        //                           }).ToList ( );


        //        var result = applicableFinal.ToList ( );

        //        List<HrmLeaveMasterViewDto> hrmLeaveMasters1 = new List<HrmLeaveMasterViewDto> ( );
        //        return hrmLeaveMasters1;
        //    }
        //}
        private static int TryParseToInt (string input)
        {
            return int.TryParse (input, out var result) ? result : -1;
        }
        //public async Task<List<HrmLeaveMasterViewDto>> GetLeaveMasterDataAsync (HrmLeaveMasterSearchDto sortDto)
        //    {
        //    var transaction = await _employeedbContext.TransactionMasters
        //        .FirstOrDefaultAsync (x => x.TransactionType == "Leave");

        //    int transactionId = transaction?.TransactionId ?? 0; // Handle null case safely


        //    bool hasAccess = await _employeedbContext.EntityAccessRights02s
        //        .AnyAsync (x => x.RoleId == sortDto.RoleId && x.LinkLevel == 15);

        //    if (!hasAccess)
        //        {
        //        return new List<HrmLeaveMasterViewDto> ( ); // Return empty list if no access
        //        }

        //    // Fetch leave master data efficiently
        //    var resultData = await (from leaveMaster in _dbContext.HrmLeaveMasters
        //                            join employee in _employeedbContext.AdmUserMasters
        //                                on leaveMaster.CreatedBy equals employee.UserId
        //                            select new HrmLeaveMasterViewDto
        //                                {
        //                                Username = employee.UserName,
        //                                LeavemasterId = leaveMaster.LeaveMasterId,
        //                                LeaveCode = leaveMaster.LeaveCode,
        //                                Description = leaveMaster.Description,
        //                                PayType = leaveMaster.PayType,
        //                                LeaveUnit = leaveMaster.LeaveUnit,
        //                                Active = leaveMaster.Active
        //                                })
        //                            .ToListAsync ( ); // Execute query

        //    return resultData;
        //    }

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
            var applicableFinal = await _employeedbContext.EntityAccessRights02s
                .Where (s => s.RoleId == sortDto.RoleId)
                .SelectMany (s => SplitStrings_XML (s.LinkId, default),
                    (s, item) => new LinkItemDto { Item = item, LinkLevel = s.LinkLevel })
                .ToListAsync ( );

            if (lnklev > 0)
            {
                applicableFinal.AddRange (
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
            var applicableFinal02Emp = await (
                from emp in _employeedbContext.EmployeeDetails
                join ea in _employeedbContext.EntityApplicable01s on emp.EmpId equals ea.EmpId
                join hlv in _employeedbContext.HighLevelViewTables on emp.LastEntity equals hlv.LastEntityId
                join af2 in applicableFinal02 on hlv.LevelOneId.ToString ( ) equals af2.Item into af2LevelOne
                from af2L1 in af2LevelOne.DefaultIfEmpty ( )
                where ea.TransactionId == transid
                select ea.MasterId
            ).Distinct ( ).ToListAsync ( );

            // **Step 6: Compute `newhigh`**
            var newhigh = entityApplicable00Final
                .Where (e => applicableFinalSetLong.Contains (e.LinkId) || e.LinkLevel == 15)
                .Select (e => e.MasterId)
                .Union (applicableFinal02Emp)
                .Distinct ( )
                .ToList ( );


            return await (from leaveMaster in _dbContext.HrmLeaveMasters
                          where newhigh.Contains (leaveMaster.LeaveMasterId)
                          join ADM_User_Master in _employeedbContext.AdmUserMasters
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
                          }).ToListAsync ( );
        } 
    }
}
