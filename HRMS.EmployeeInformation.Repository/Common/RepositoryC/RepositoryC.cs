using AutoMapper;
using EMPLOYEE_INFORMATION.Data;
using EMPLOYEE_INFORMATION.Models;
using EMPLOYEE_INFORMATION.Models.Entity;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace HRMS.EmployeeInformation.Repository.Common.RepositoryC
{
    public class RepositoryC : IRepositoryC
    {
        private readonly EmployeeDBContext _context;
        //private IStringLocalizer _stringLocalizer;
        private readonly IMemoryCache _memoryCache;
        private readonly EmployeeSettings _employeeSettings;
        private int paramDynVal;

        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public RepositoryC(EmployeeDBContext dbContext, EmployeeSettings employeeSettings, IMemoryCache memoryCache, IMapper mapper, IWebHostEnvironment env)
        {
            _context = dbContext;
            _employeeSettings = employeeSettings;
            _memoryCache = memoryCache;
            _mapper = mapper;
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public async Task<FillTravelTypeDto> FillTravelType()
        {
            var result = new FillTravelTypeDto
            {

                Traveltype = await _context.TravelTypes
                    .Select(t => new TraveltypeDto
                    {
                        TravelType_Id = t.TravelTypeId,
                        TravelType = t.TravelType1,
                        value = t.Value
                    })
                    .ToListAsync(),

                // Fetching only dependents where Self != 1
                DependentMaster1 = await _context.DependentMasters
                    .Where(d => d.Self != 1)
                    .Select(d => new DependentMaster1Dto
                    {
                        DependentId = d.DependentId,
                        Description = d.Description
                    })
                    .ToListAsync(),

                // Fetching all dependents
                AllDependents = await _context.DependentMasters
                    .Select(d => new DependentMaster1Dto
                    {
                        DependentId = d.DependentId,
                        Description = d.Description
                    })
                    .ToListAsync()
            };

            return result;
        }


        public static IEnumerable<string> SplitStrings_XML(string list, char delimiter = ',')
        {
            if (string.IsNullOrWhiteSpace(list))
                return Enumerable.Empty<string>();

            // Split the input string by the delimiter and return as IEnumerable
            return list.Split(delimiter)
                       .Select(item => item.Trim()) // Trim whitespace from each item
                       .Where(item => !string.IsNullOrEmpty(item)); // Exclude empty items
        }
        public string GetEmployeeOffSet(int employeeId)
        {
            if (employeeId == 0)
            {
                return "+05:30";
            }

            string offsetValue = _context.CompanyParameters02s
                .Where(a => a.EmpId == employeeId)
                .Join(_context.CompanyParameters, a => a.ParamId, b => b.Id, (a, b) => new { a, b })
                .Join(_context.TimeOffSets, ab => ab.a.Value, c => c.TimeOffSetId, (ab, c) => new { ab, c })
                .Where(x => x.ab.b.ParameterCode == "OFFSET" && x.ab.b.Type == "COM")
                .Select(x => x.c.OffsetValue)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(offsetValue))
            {
                return offsetValue;
            }

            // Fetch employee entity details in a single DB call
            var employee = _context.EmployeeDetails
                .Where(e => e.EmpId == employeeId)
                .Select(e => new { e.EmpEntity, e.EmpFirstEntity })
                .FirstOrDefault();

            if (employee != null && !string.IsNullOrEmpty(employee.EmpEntity))
            {
                var entityList = SplitStrings_XML(employee.EmpEntity, ',').ToList(); // Ensure evaluation in memory

                offsetValue = _context.CompanyParameters01s
                .Where(a => a.LevelId != 1 && entityList.Contains(a.LinkId.ToString()))
                    .Join(_context.CompanyParameters, a => a.ParamId, b => b.Id, (a, b) => new { a, b })
                    .Join(_context.TimeOffSets, ab => ab.a.Value, c => c.TimeOffSetId, (ab, c) => new { ab, c })
                    .Where(x => x.ab.b.ParameterCode == "OFFSET" && x.ab.b.Type == "COM")
                    .OrderByDescending(x => x.ab.a.LevelId)
                    .Select(x => x.c.OffsetValue)
                    .FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(offsetValue))
            {
                return offsetValue;
            }

            if (employee != null && !string.IsNullOrEmpty(employee.EmpFirstEntity))
            {
                var firstEntityList = SplitStrings_XML(employee.EmpFirstEntity, ',').ToList();

                offsetValue = _context.CompanyParameters01s
                    .Where(a => a.LevelId == 1 && firstEntityList.Contains(a.LinkId.ToString()))
                    .Join(_context.CompanyParameters, a => a.ParamId, b => b.Id, (a, b) => new { a, b })
                    .Join(_context.TimeOffSets, ab => ab.a.Value, c => c.TimeOffSetId, (ab, c) => new { ab, c })
                    .Where(x => x.ab.b.ParameterCode == "OFFSET" && x.ab.b.Type == "COM")
                    .OrderByDescending(x => x.ab.a.LevelId)
                    .Select(x => x.c.OffsetValue)
                    .FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(offsetValue))
            {
                return offsetValue;
            }

            // Check company level if all other levels return null
            return _context.CompanyParameters
                .Where(a => a.ParameterCode == "OFFSET" && a.Type == "COM")
                .Join(_context.TimeOffSets, a => a.Value, c => c.TimeOffSetId, (a, c) => c.OffsetValue)
                .FirstOrDefault();
        }



        public async Task<List<FillEmployeesBasedOnRoleDto>> FillEmployeesBasedOnRole(int firstEntityId, int secondEntityId, string transactionType)
        {
            string offsetApprove = GetEmployeeOffSet(secondEntityId);
            var parts = offsetApprove.Split(':');
            int offsetHours = int.Parse(parts[0]);
            int offsetMinutes = int.Parse(parts[1]);
            DateTime offsetDateApp = DateTime.UtcNow.AddHours(offsetHours).AddMinutes(offsetMinutes);

            int transactionsid = _context.TransactionMasters
                .Where(t => t.TransactionType == transactionType)
                .Select(t => t.TransactionId)
                .FirstOrDefault();

            var RoleTemp = (from a in _context.HrEmployeeUserRelations
                            join b in _context.AdmUserRoleMasters on a.UserId equals b.UserId
                            select new { a.UserId, a.EmpId, b.RoleId }).ToList();

            var BranchTemp = _context.BranchDetails
                .Select(b => new { b.LinkId, b.Branch })
                .ToList();

            bool hasAccess = _context.TabAccessRights.Any(t => _context.TabMasters
                .Any(tab => tab.Code == "SelfAssgnShft" && tab.TabId == t.TabId) && t.RoleId == firstEntityId);

            int lnklev = _context.SpecialAccessRights
                .Where(s => s.RoleId == firstEntityId)
                .Select(s => s.LinkLevel)
                .FirstOrDefault() ?? 0;

            var entityAccessRights = _context.EntityAccessRights02s
                .Where(s => s.RoleId == firstEntityId && s.LinkLevel == 15)
                .AsEnumerable()
                .SelectMany(s => SplitStrings_XML(s.LinkId))
                .ToList();

            if (entityAccessRights.Any())
            {
                var employees = _context.HrEmpMasters
                    .Where(a => a.SeperationStatus == 0)
                    .AsEnumerable()
                    .Join(BranchTemp,
                          a => a.BranchId,
                          b => b.LinkId,
                          (a, b) => new FillEmployeesBasedOnRoleDto
                          {
                              EmpId = a.EmpId,
                              EmpCode = a.EmpCode,
                              Employee = string.Join(" || ",
                                  new[] { (a.FirstName + " " + a.MiddleName).Trim(), a.EmpCode, b.Branch }
                                  .Where(x => !string.IsNullOrEmpty(x)))
                          })
                    .ToList();

                return await Task.FromResult(employees); // ✅ Ensure it returns a list
            }
            else
            {
                bool hasDelegation = (from a in _context.RoleDelegation00s
                                      join b in _context.Roledelegationtransactions on a.RoleDelegationId equals b.RoleDelegationId
                                      where a.AssignedEmpId == secondEntityId
                                          && offsetDateApp >= a.FromDate
                                          && offsetDateApp <= a.ToDate
                                          && a.ApprovalStatus == "A"
                                          && a.Revoke != "Y"
                                          && transactionsid == int.Parse(b.TransactionId)
                                      select a).Any();

                if (hasDelegation)
                {
                    var RoleDeligateTemp = (from a in _context.RoleDelegation00s
                                            join b in _context.Roledelegationtransactions on a.RoleDelegationId equals b.RoleDelegationId
                                            join c in RoleTemp on a.EmpId equals c.EmpId into roleJoin
                                            from c in roleJoin.DefaultIfEmpty()
                                            where a.AssignedEmpId == secondEntityId
                                                  && offsetDateApp >= a.FromDate
                                                  && offsetDateApp <= a.ToDate
                                                  && a.ApprovalStatus == "A"
                                                  && a.Revoke != "Y"
                                                  && transactionsid == int.Parse(b.TransactionId)
                                            select new { c.RoleId, a.EmpId }).ToList();

                    var employees = FetchEmployees(BranchTemp, secondEntityId);
                    return await Task.FromResult(employees); // ✅ Ensure it returns a list
                }
                else
                {
                    var employees = FetchEmployees(BranchTemp, secondEntityId);
                    return await Task.FromResult(employees); // ✅ Ensure it returns a list
                }
            }
        }


        public List<FillEmployeesBasedOnRoleDto> FetchEmployees(IEnumerable<object> branchTemp, int secondEntityId)
        {
            // Step 1: Convert branchTemp to a strongly-typed list
            var typedBranchTemp = branchTemp.Select(b => new
            {
                LinkId = (int)b.GetType().GetProperty("LinkId").GetValue(b),
                Branch = (string)b.GetType().GetProperty("Branch").GetValue(b)
            }).ToList();

            // Step 2: Fetch Employees from the database first
            var employees = _context.HrEmpMasters
                .Where(d => d.SeperationStatus == 0
                            && (d.IsSave == null || d.IsSave == 0)
                            && (d.IsDelete == null || d.IsDelete == false)
                            && (d.EmpId == secondEntityId
                                || _context.HrEmpReportings
                                    .Where(r => r.ReprotToWhome == secondEntityId)
                                    .Select(r => r.EmpId)
                                    .Contains(d.EmpId)))
                .AsEnumerable() // Switch to client-side evaluation
                .Join(typedBranchTemp, // Now join in memory
                      d => d.BranchId,
                      e => e.LinkId,
                      (d, e) => new FillEmployeesBasedOnRoleDto
                      {
                          EmpId = d.EmpId,
                          EmpCode = d.EmpCode,
                          Employee = string.Join(" || ",
                              new[] { $"{d.FirstName ?? ""} {d.MiddleName ?? ""}".Trim(), d.EmpCode, e.Branch }
                              .Where(x => !string.IsNullOrEmpty(x)))
                      })
                .OrderBy(d => d.EmpCode)
                .ToList();

            return employees;
        }

        public async Task<GetDependentDetailsDto> GetDependentDetails(int employeeId)
        {
            var result = _context.EmployeeDetails
                        .Where(a => a.EmpId == employeeId)
                        .Select(a => new GetDependentDetailsDto
                        {
                            EmpId = a.EmpId,
                            EmpCode = a.EmpCode,
                            Name = a.Name,
                            Gender = a.Gender,
                            DateOfBirth = a.DateOfBirth.HasValue ? a.DateOfBirth.Value.ToString("dd/MM/yyyy") : null
                        }).FirstOrDefault();

            return result;
        }
        public string GetSequence(int employeeId, int mainMasterId, string entity = "", int firstEntity = 0)
        {
            string sequence = null;
            int? codeId = null;
            bool isLevel17 = (from ls in _context.LevelSettingsAccess00s
                              join tm in _context.TransactionMasters
                              on ls.TransactionId equals tm.TransactionId
                              where tm.TransactionType == "Seq_Gen" && ls.Levels == "17"
                              select ls).Any();

            if (isLevel17)
            {
                if (!string.IsNullOrEmpty(entity))
                {

                    var entityQuery = (from a in _context.EntityApplicable00s
                                       join c in _context.AdmCodegenerationmasters
                                       on a.MasterId equals c.CodeId
                                       join tm in _context.TransactionMasters
                                       on a.TransactionId equals tm.TransactionId
                                       where tm.TransactionType == "Seq_Gen" &&
                                             a.MainMasterId == mainMasterId &&
                                             a.LinkLevel != 1 &&
                                             SplitStrings_XML(entity, ',').Contains(a.LinkId.ToString())
                                       orderby a.LinkLevel
                                       select new { a, c })
                  .FirstOrDefault();

                    if (entityQuery != null)
                    {
                        //sequence = entityQuery.a.ToString;
                        codeId = entityQuery.c.CodeId;
                    }

                    if (sequence == null && firstEntity != 0)
                    {
                        var firstEntityQuery = (from a in _context.EntityApplicable00s
                                                join c in _context.AdmCodegenerationmasters
                                                on a.MasterId equals c.CodeId
                                                join tm in _context.TransactionMasters
                                                on a.TransactionId equals tm.TransactionId
                                                where tm.TransactionType == "Seq_Gen" &&
                                                      a.MainMasterId == mainMasterId &&
                                                      a.LinkLevel == 1 &&
                                                      a.LinkId == firstEntity
                                                orderby a.LinkLevel
                                                select new { a, c })
                        .FirstOrDefault();

                        if (firstEntityQuery != null)
                        {
                            //sequence = firstEntityQuery.a.LastSequence;
                            codeId = firstEntityQuery.c.CodeId;
                        }
                    }

                    if (sequence == null)
                    {
                        var defaultQuery = (from a in _context.EntityApplicable00s
                                            join c in _context.AdmCodegenerationmasters
                                            on a.MasterId equals c.CodeId
                                            join tm in _context.TransactionMasters
                                            on a.TransactionId equals tm.TransactionId
                                            where tm.TransactionType == "Seq_Gen" &&
                                                  a.MainMasterId == mainMasterId &&
                                                  a.LinkLevel == 15
                                            select new { a, c })
                       .FirstOrDefault();

                        if (defaultQuery != null)
                        {
                            //sequence = defaultQuery.a.LastSequence;
                            codeId = defaultQuery.c.CodeId;
                        }
                    }
                }
                else
                {
                    // Entity is empty, handle alternate logic
                    var empQuery = (from emp in _context.HrEmpMasters
                                    where emp.EmpId == employeeId
                                    from e in SplitStrings_XML(emp.EmpEntity, ',')
                                    join ea in _context.EntityApplicable00s
                                    on e equals ea.LinkId.ToString()
                                    join adm in _context.AdmCodegenerationmasters
                                    on ea.MasterId equals adm.CodeId
                                    join tm in _context.TransactionMasters
                                    on ea.TransactionId equals tm.TransactionId
                                    where tm.TransactionType == "Seq_Gen" &&
                                          ea.MainMasterId == mainMasterId &&
                                          ea.LinkLevel != 1
                                    orderby ea.LinkLevel
                                    select new { emp, ea, adm })
                  .FirstOrDefault();

                    if (empQuery != null)
                    {
                        //sequence = empQuery.ea.LastSequence;
                        codeId = empQuery.adm.CodeId;
                    }

                    if (sequence == null)
                    {
                        var fallbackQuery = (from emp in _context.HrEmpMasters
                                             where emp.EmpId == employeeId
                                             join ea in _context.EntityApplicable00s
                                             on Convert.ToInt64(emp.EmpFirstEntity) equals ea.LinkId
                                             join adm in _context.AdmCodegenerationmasters
                                             on ea.MasterId equals adm.CodeId
                                             join tm in _context.TransactionMasters
                                             on ea.TransactionId equals tm.TransactionId
                                             where tm.TransactionType == "Seq_Gen" &&
                                                   ea.MainMasterId == mainMasterId &&
                                                   ea.LinkLevel == 1
                                             orderby ea.LinkLevel
                                             select new { ea, adm })
                    .FirstOrDefault();

                        if (fallbackQuery != null)
                        {
                            //sequence = fallbackQuery.ea.LastSequence;
                            codeId = fallbackQuery.adm.CodeId;
                        }
                    }
                }
            }
            else
            {
                // Handle 'level == 16' and other cases
                // Add similar LINQ logic here based on the SQL conditions
            }

            return codeId?.ToString();
        }



        public async Task<int> SaveDependentEmp(SaveDependentEmpDto SaveDependentEmp)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            int ErrorID = 0; // Default error value

            try
            {
                bool doesNotExist = !_context.Dependent00s.Any(d => d.DepId == SaveDependentEmp.SchemeId);

                if (doesNotExist)
                {
                    int? documentID = (int?)await _context.HrmsDocument00s
                                    .Where(a => a.DocName == "Dependent")
                                    .Select(a => (int?)a.DocId)
                                    .FirstOrDefaultAsync();


                    int transactionID = await _context.TransactionMasters
                        .Where(a => a.TransactionType == "Document")
                        .Select(a => a.TransactionId)
                        .FirstOrDefaultAsync();

                    var sequenceid = GetSequence(SaveDependentEmp.EmpId, transactionID, "", 0);
                    var codeID = GetSequence(SaveDependentEmp.EmpId, transactionID, "", 0);

                    if (sequenceid == null || codeID == null)
                    {
                        ErrorID = 5;
                        return ErrorID; // Return error code
                    }

                    var requestSequence = await _context.AdmCodegenerationmasters
                        .Where(a => a.CodeId == Convert.ToInt32(sequenceid))
                        .Select(a => a.LastSequence)
                        .FirstOrDefaultAsync();

                    // Update Code Generation Master
                    var codeGenMaster = await _context.AdmCodegenerationmasters
                        .FirstOrDefaultAsync(c => c.CodeId == Convert.ToInt32(codeID));

                    if (codeGenMaster != null)
                    {
                        codeGenMaster.CurrentCodeValue = (await _context.AdmCodegenerationmasters
                            .Where(c => c.CodeId == Convert.ToInt32(codeID))
                            .MaxAsync(c => (int?)c.CurrentCodeValue) ?? 0) + 1;

                        await _context.SaveChangesAsync();

                        int length = codeGenMaster.NumberFormat.Length - codeGenMaster.CurrentCodeValue.ToString().Length;
                        string seq = codeGenMaster.NumberFormat.Substring(0, length);
                        string finalValue = $"{codeGenMaster.Code}{seq}{codeGenMaster.CurrentCodeValue}";

                        codeGenMaster.LastSequence = finalValue;
                        await _context.SaveChangesAsync();
                    }

                    // Insert into HrmsEmpDocuments00
                    var empDocument = new HrmsEmpdocuments00
                    {
                        DocId = documentID.Value,
                        EmpId = SaveDependentEmp.EmpId,
                        RequestId = requestSequence,
                        TransactionType = "Document",
                        Status = "A",
                        FlowStatus = "E",
                        ProxyId = 0,
                        EntryBy = SaveDependentEmp.EntryBy,
                        EntryDate = DateTime.UtcNow
                    };

                    _context.HrmsEmpdocuments00s.Add(empDocument);
                    await _context.SaveChangesAsync();

                    int detailId = empDocument.DetailId;

                    var empDocumentApproved = new HrmsEmpdocumentsApproved00
                    {
                        DetailId = detailId,
                        DocId = empDocument.DocId,
                        EmpId = empDocument.EmpId,
                        RequestId = empDocument.RequestId,
                        TransactionType = empDocument.TransactionType,
                        Status = empDocument.Status,
                        ProxyId = empDocument.ProxyId,
                        FlowStatus = empDocument.FlowStatus,
                        DateFrom = DateTime.UtcNow,
                        EntryBy = empDocument.EntryBy,
                        EntryDate = empDocument.EntryDate
                    };

                    _context.HrmsEmpdocumentsApproved00s.Add(empDocumentApproved);
                    await _context.SaveChangesAsync();

                    // Update ADM_CODEGENERATIONMASTER for sequenceid
                    var codeGenMasterSeq = await _context.AdmCodegenerationmasters
                        .FirstOrDefaultAsync(c => c.CodeId == Convert.ToInt32(sequenceid));

                    if (codeGenMasterSeq != null)
                    {
                        codeGenMasterSeq.CurrentCodeValue = (await _context.AdmCodegenerationmasters
                            .Where(c => c.CodeId == Convert.ToInt32(sequenceid))
                            .MaxAsync(c => (int?)c.CurrentCodeValue) ?? 0) + 1;

                        await _context.SaveChangesAsync();

                        int lengthSeq = codeGenMasterSeq.NumberFormat.Length - codeGenMasterSeq.CurrentCodeValue.ToString().Length;
                        string seqSeq = codeGenMasterSeq.NumberFormat.Substring(0, lengthSeq);
                        string finalValueSeq = $"{codeGenMasterSeq.Code}{seqSeq}{codeGenMasterSeq.CurrentCodeValue}";

                        codeGenMasterSeq.LastSequence = finalValueSeq;
                        await _context.SaveChangesAsync();
                    }

                    // Insert into Dependent00
                    var dependent = new Dependent00
                    {
                        Name = SaveDependentEmp.Name,
                        Gender = SaveDependentEmp.Gender,
                        DateOfBirth = string.IsNullOrEmpty(SaveDependentEmp.DateOfBirth)
                        ? (DateTime?)null : DateTime.Parse(SaveDependentEmp.DateOfBirth),
                        RelationshipId = SaveDependentEmp.RelationshipId,
                        Description = SaveDependentEmp.Description,
                        EntryBy = SaveDependentEmp.EntryBy,
                        EntryDate = DateTime.UtcNow,
                        DepEmpId = SaveDependentEmp.EmpId,
                        InterEmpId = SaveDependentEmp.DependentID,
                        Type = SaveDependentEmp.Type,
                        TicketEligible = SaveDependentEmp.TicketEligible,
                        DocumentId = detailId,
                        IdentificationNo = SaveDependentEmp.IdentificationNo
                    };

                    _context.Dependent00s.Add(dependent);
                    await _context.SaveChangesAsync();

                    int educationDepId = dependent.DepId;
                    ErrorID = 1; // Success

                    // Insert into Dependent01 if FileName is provided
                    if (!string.IsNullOrEmpty(SaveDependentEmp.FileName))
                    {
                        var dependentDoc = new Dependent01
                        {
                            DepId = educationDepId,
                            DepEmpid = SaveDependentEmp.EmpId,
                            DocFileType = SaveDependentEmp.FileType,
                            DocFileName = SaveDependentEmp.FileName,
                            FileData = SaveDependentEmp.Bytedepndent
                        };

                        _context.Dependent01s.Add(dependentDoc);
                        await _context.SaveChangesAsync();
                    }

                    // Insert into DependentEducation if applicable
                    if (SaveDependentEmp.IsEducation)
                    {
                        var dependentEducation = new DependentEducation
                        {
                            EduId = SaveDependentEmp.EducationID,
                            CourseId = SaveDependentEmp.CourseID,
                            SpecialId = SaveDependentEmp.SpecialID,
                            DepId = educationDepId,
                            EmpId = SaveDependentEmp.EmpId,
                            EntryBy = SaveDependentEmp.EntryBy,
                            EntryDate = DateTime.UtcNow,
                            UpdatedBy = SaveDependentEmp.EntryBy,
                            UpdatedDate = DateTime.UtcNow,
                            Year = SaveDependentEmp.Year,
                            CourseType = SaveDependentEmp.CourseType,
                            UniversityEduId = SaveDependentEmp.EdUniversity
                        };

                        _context.DependentEducations.Add(dependentEducation);
                        await _context.SaveChangesAsync();
                    }

                    ErrorID = 1;
                }
                else
                {

                    var dependent = await _context.Dependent00s
                        .FirstOrDefaultAsync(d => d.DepId == SaveDependentEmp.SchemeId);

                    if (dependent != null)
                    {
                        dependent.Name = SaveDependentEmp.Name;
                        dependent.Gender = SaveDependentEmp.Gender;
                        dependent.DateOfBirth = string.IsNullOrEmpty(SaveDependentEmp.DateOfBirth)
                            ? (DateTime?)null
                            : DateTime.Parse(SaveDependentEmp.DateOfBirth);
                        dependent.RelationshipId = SaveDependentEmp.RelationshipId;
                        dependent.Description = SaveDependentEmp.Description;
                        dependent.ModifiedBy = SaveDependentEmp.EntryBy;
                        dependent.ModifiedDate = DateTime.UtcNow;
                        dependent.InterEmpId = SaveDependentEmp.DependentID;
                        dependent.Type = SaveDependentEmp.Type;
                        dependent.TicketEligible = SaveDependentEmp.TicketEligible;
                        dependent.IdentificationNo = SaveDependentEmp.IdentificationNo;

                        await _context.SaveChangesAsync();
                    }

                    // **Update or Insert Dependent01 (File Info)**
                    if (!string.IsNullOrEmpty(SaveDependentEmp.FileType) && !string.IsNullOrEmpty(SaveDependentEmp.FileName))
                    {
                        var dependentDoc = await _context.Dependent01s
                            .FirstOrDefaultAsync(d => d.DepId == SaveDependentEmp.SchemeId && d.DepEmpid == SaveDependentEmp.EmpId);

                        if (dependentDoc != null)
                        {
                            // **Update existing record**
                            dependentDoc.DocFileType = SaveDependentEmp.FileType;
                            dependentDoc.DocFileName = SaveDependentEmp.FileName;
                            dependentDoc.FileData = SaveDependentEmp.Bytedepndent;
                        }
                        else
                        {
                            // **Insert new record**
                            _context.Dependent01s.Add(new Dependent01
                            {
                                DepId = SaveDependentEmp.SchemeId,
                                DepEmpid = SaveDependentEmp.EmpId,
                                DocFileType = SaveDependentEmp.FileType,
                                DocFileName = SaveDependentEmp.FileName,
                                FileData = SaveDependentEmp.Bytedepndent
                            });
                        }

                        await _context.SaveChangesAsync();
                    }

                    // **Update or Insert DependentEducation**
                    if (SaveDependentEmp.IsEducation)
                    {
                        var dependentEducation = await _context.DependentEducations
                            .FirstOrDefaultAsync(d => d.DepId == SaveDependentEmp.SchemeId && d.EmpId == SaveDependentEmp.EmpId);

                        if (dependentEducation == null)
                        {
                            // **Insert new education record**
                            _context.DependentEducations.Add(new DependentEducation
                            {
                                EduId = SaveDependentEmp.EducationID,
                                CourseId = SaveDependentEmp.CourseID,
                                SpecialId = SaveDependentEmp.SpecialID,
                                DepId = SaveDependentEmp.SchemeId,
                                EmpId = SaveDependentEmp.EmpId,
                                EntryBy = SaveDependentEmp.EntryBy,
                                EntryDate = DateTime.UtcNow,
                                UpdatedBy = SaveDependentEmp.EntryBy,
                                UpdatedDate = DateTime.UtcNow,
                                Year = SaveDependentEmp.Year,
                                CourseType = SaveDependentEmp.CourseType,
                                UniversityEduId = SaveDependentEmp.EdUniversity
                            });
                        }
                        else
                        {
                            // **Update existing education record**
                            dependentEducation.EduId = SaveDependentEmp.EducationID;
                            dependentEducation.CourseId = SaveDependentEmp.CourseID;
                            dependentEducation.SpecialId = SaveDependentEmp.SpecialID;
                            dependentEducation.UniversityEduId = SaveDependentEmp.EdUniversity;
                            dependentEducation.CourseType = SaveDependentEmp.CourseType;
                            dependentEducation.Year = SaveDependentEmp.Year;
                            dependentEducation.UpdatedBy = SaveDependentEmp.EntryBy;
                            dependentEducation.UpdatedDate = DateTime.UtcNow;
                        }

                        await _context.SaveChangesAsync();
                    }

                    ErrorID = 2; // Success for updates
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ErrorID = 0; // Indicate failure
            }

            return ErrorID;
        }

        public async Task<object> RetrieveEducation()
        {
            var result = await _context.EducationMasters
                        .Select(a => new
                        {
                            EducId = a.EducId,
                            EduDescription = a.EduDescription
                        }
                        ).ToListAsync();
            return result;
        }
        public async Task<object> RetrieveCourse()
        {
            var result = await _context.EdCourseMasters
                        .Select(a => new
                        {
                            CourseId = a.CourseId,
                            CourseDesc = a.CourseDesc
                        }
                        ).ToListAsync();
            return result;
        }
        public async Task<object> RetrieveSpecial()
        {
            var result = await _context.EdSpecializationMasters
                        .Select(a => new
                        {
                            EdSpecId = a.EdSpecId,
                            SpecializationDesc = a.SpecializationDesc
                        }
                        ).ToListAsync();
            return result;
            }
        public async Task<object> RetrieveUniversity()
            {
            var result = await _context.UniversityMasters
                        .Select(a => new
                            {
                            UniId = a.UniId,
                            UniversityDescription = a.UniversityDescription
                            }
                        ).ToListAsync ( );
            return result;
            }
        public async Task<List<EditDependentEmpResultDto>> EditDependentEmpNew (int Schemeid, int EmpId)
            {
            var DependentData = await (from a in _context.Dependent00s
                                join b in _context.Dependent01s on a.DepId equals b.DepId into bGroup
                                from b in bGroup.DefaultIfEmpty ( )
                                join c in _context.DependentMasters on a.RelationshipId equals c.DependentId into cGroup
                                from c in cGroup.DefaultIfEmpty ( )
                                where a.DepId == Schemeid
                                select new EditDependentEmpDto
                                    {
                                    depid = a.DepId,
                                    Name = a.Name,
                                    Gender = a.Gender,
                                    DateOfBirth = a.DateOfBirth.HasValue ? a.DateOfBirth.Value.ToString ("dd/MM/yyyy") : null,
                                    RelationshipId = a.RelationshipId,
                                    RelationShip = c != null ? c.Description : null,
                                    Description = a.Description,
                                    DepEmpId = a.DepEmpId,
                                    DocFileType = b != null ? b.DocFileType : null,
                                    DocFileName = b != null ? b.DocFileName : null,
                                    FileData = b != null ? b.FileData : null,
                                    InterEmpID = a.InterEmpId,
                                    Type = a.Type,
                                    TicketEligible = a.TicketEligible,
                                    DocumentID = a.DocumentId,
                                    IdentificationNo = a.IdentificationNo ?? ""
                                    }).ToListAsync ( );
            var DependentEducation = await (from a in _context.DependentEducations
                                            join b in _context.EducationMasters on a.EduId equals b.EducId
                                            join c in _context.EdCourseMasters on a.CourseId equals c.EducId into cGroup
                                            from c in cGroup.DefaultIfEmpty ( )
                                            join d in _context.EdSpecializationMasters on a.SpecialId equals d.EdCourId into dGroup
                                            from d in dGroup.DefaultIfEmpty ( )
                                            join e in _context.UniversityMasters on a.UniversityEduId equals e.UniId into eGroup
                                            from e in eGroup.DefaultIfEmpty ( )
                                            where a.DepId == Schemeid && a.EmpId == EmpId
                                            select new EditDependentEducationDto
                                                {
                                                UniversityEdu = a.UniversityEdu,
                                                Year = a.Year,
                                                CourseType = a.CourseType,
                                                EducId = a.EduId,
                                                CourseId = a.CourseId,
                                                EdSpecId = a.SpecialId,
                                                UniId = a.UniversityEduId
                                                }).ToListAsync ( );

            var result = DependentData.Select (dependent => new EditDependentEmpResultDto
                {
                EditDependentEmp = DependentData,
                EditDependentEducation = DependentEducation
                }).ToList ( );

            return result;
            }


        public List<WorkFlowAvailabilityDto> GetWorkFlowSequenceAvailability (int employeeId = 0, string transactionType = "", int masterId = 0)
            {
            var result = new List<WorkFlowAvailabilityDto> ( );

            int? workFlow = null;
            int? codeId = null;
            int? transactionId = _context.TransactionMasters
                .Where (t => t.TransactionType == transactionType)
                .Select (t => (int?)t.TransactionId)
                .FirstOrDefault ( );

            var employee = _context.HrEmpMasters.FirstOrDefault (e => e.EmpId == employeeId);
            int? firstEntity = int.TryParse (employee?.EmpFirstEntity, out int parsedEntity) ? parsedEntity : (int?)null;
            string empEntities = employee?.EmpEntity;

            string workFlowNeed = (from a in _context.CompanyParameters
                                   join b in _context.HrmValueTypes on a.Value equals b.Value
                                   where b.Type == _employeeSettings.EmployeeReportingType &&
                                         a.ParameterCode == _employeeSettings.ParameterCode03
                                   select b.Code).FirstOrDefault ( );






            codeId = int.TryParse (GetSequence (employeeId, transactionId ?? 0, "", 0), out int parsedCodeId) ? parsedCodeId : (int?)null;


            if (workFlow == null && masterId > 0)
                {
                workFlow = (from a in _context.SpecialWorkFlows
                            join b in _context.TransactionMasters on a.TransactionId equals b.TransactionId
                            where b.TransactionType == transactionType
                                  && masterId == (transactionType == _employeeSettings.TransactionType02 ? a.GrievanceTypeId
                                                : transactionType == _employeeSettings.TransactionType03 ? a.LeaveType
                                                : transactionType == _employeeSettings.TransactionType04 ? a.SurveyTopic
                                                : (int?)null)
                            select (int?)1)
             .FirstOrDefault ( );

                }

            if (workFlow == null)
                {
                workFlow = _context.ParamWorkFlow02s
                    .Where (pw => pw.TransactionId == transactionId && pw.LinkEmpId == employeeId)
                    .Select (pw => (int?)1)
                    .FirstOrDefault ( );
                }

            if (workFlow == null)
                {
                List<string> empEntityList = empEntities?.Split (',').ToList ( ) ?? new List<string> ( );

                workFlow = _context.ParamWorkFlow01s
                    .Where (pw => pw.TransactionId == transactionId &&
                                 ((pw.LinkLevel == 1 && pw.LinkId == firstEntity) ||
                                  (pw.LinkLevel > 1 && empEntityList.Contains (pw.LinkId.ToString ( )))))
                    .Select (pw => (int?)1)
                    .FirstOrDefault ( );

                }

            if (workFlow == null)
                {
                workFlow = _context.ParamWorkFlow00s
                    .Where (pw => pw.TransactionId == transactionId)
                    .Select (pw => (int?)1)
                    .FirstOrDefault ( );
                }

            if (workFlowNeed == _employeeSettings.No && new[] { _employeeSettings.TransactionType05, _employeeSettings.TransactionType06, _employeeSettings.TransactionType07, _employeeSettings.TransactionType08, _employeeSettings.TransactionType09, _employeeSettings.TransactionType }.Contains (transactionType))
                {
                result.Add (new WorkFlowAvailabilityDto { WorkFlowAvailable = _employeeSettings.LetterS, SequenceAvailable = _employeeSettings.LetterS });
                }
            else
                {
                if (codeId == null && workFlow == null)
                    result.Add (new WorkFlowAvailabilityDto { WorkFlowAvailable = _employeeSettings.LetterN, SequenceAvailable = _employeeSettings.LetterN });
                if (codeId == null && workFlow > 0)
                    result.Add (new WorkFlowAvailabilityDto { WorkFlowAvailable = _employeeSettings.LetterY, SequenceAvailable = _employeeSettings.LetterN });
                if (codeId > 0 && workFlow == null)
                    result.Add (new WorkFlowAvailabilityDto { WorkFlowAvailable = _employeeSettings.LetterN, SequenceAvailable = _employeeSettings.LetterY });
                if (codeId > 0 && workFlow > 0)
                    result.Add (new WorkFlowAvailabilityDto { WorkFlowAvailable = _employeeSettings.LetterY, SequenceAvailable = _employeeSettings.LetterY });
                }

            return result;
            }

        public async Task<List<WorkFlowAvailabilityDto>> WorkFlowAvailability (int Emp_Id, string Transactiontype, int ParameterID)
            {
            List<WorkFlowAvailabilityDto> result;

            if (Transactiontype == _employeeSettings.TransactiontypeSHIFT)
                {
                bool shiftApprovalExists = await _context.CompanyParameters
                    .AnyAsync (cp => cp.Type == _employeeSettings.Code && cp.ParameterCode == _employeeSettings.ParameterCodeShift && cp.Value == 1);

                if (shiftApprovalExists)
                    {
                    var workflowList = GetWorkFlowSequenceAvailability (Emp_Id, Transactiontype, ParameterID);

                    result = workflowList.Select (wf => new WorkFlowAvailabilityDto
                        {
                        WorkFlowAvailable = wf.WorkFlowAvailable,
                        SequenceAvailable = wf.SequenceAvailable
                        }).ToList ( );
                    }
                else
                    {
                    var workflowList = GetWorkFlowSequenceAvailability (Emp_Id, Transactiontype, ParameterID);

                    result = workflowList.Select (wf => new WorkFlowAvailabilityDto
                        {
                        WorkFlowAvailable = _employeeSettings.LetterY,
                        SequenceAvailable = wf.SequenceAvailable
                        }).ToList ( );
                    }
                }
            else
                {
                var workflowList = GetWorkFlowSequenceAvailability (Emp_Id, Transactiontype, ParameterID);

                result = workflowList.Select (wf => new WorkFlowAvailabilityDto
                    {
                    WorkFlowAvailable = wf.WorkFlowAvailable,
                    SequenceAvailable = wf.SequenceAvailable
                    }).ToList ( );
                }

            return result;
            }

        public async Task<string> InsertDepFields (List<TmpDocFileUpDto> InsertDepFields)
            {
            using var transaction = await _context.Database.BeginTransactionAsync ( );
            try
                {
                var workFlowNeedValue = await (from a in _context.CompanyParameters
                                               join b in _context.HrmValueTypes on a.Value equals b.Value
                                               where b.Type == _employeeSettings.EmployeeReportingType && a.ParameterCode == _employeeSettings.ParameterCode02 && a.Type == _employeeSettings.CompanyParameterType
                                               select b.Code).FirstOrDefaultAsync ( );

                var tmpDocFileUpList = InsertDepFields;

                bool recordsExist = (from a in _context.HrmsEmpdocuments01s.ToList ( )
                                     join b in tmpDocFileUpList
                                     on a.DetailId equals b.DetailID
                                     select a).Any ( );

                if (recordsExist)
                    {
                    var tmpDocFileUpListConverted = tmpDocFileUpList
                        .Select (b => new
                            {
                            DetailID = b.DetailID ?? 0,
                            DocFieldID = int.TryParse (b.DocFieldID, out int parsedId) ? parsedId : 0,
                            DocFieldText = b.DocFieldText
                            })
                        .ToList ( );

                    var updateRecords = from a in _context.HrmsEmpdocuments01s
                                        join b in tmpDocFileUpListConverted
                                        on new { DetailId = a.DetailId ?? 0, DocFields = a.DocFields ?? 0 }
                                        equals new { DetailId = b.DetailID, DocFields = b.DocFieldID }
                                        select new { a, b };

                    foreach (var record in updateRecords)
                        {
                        record.a.DocValues = record.b.DocFieldText;
                        }

                    await _context.SaveChangesAsync ( );

                    var insertRecords = from b in tmpDocFileUpListConverted
                                        join a in _context.HrmsEmpdocuments01s
                                            on new { DetailID = b.DetailID, DocFields = b.DocFieldID }
                                            equals new { DetailID = a.DetailId ?? 0, DocFields = a.DocFields ?? 0 } into joined
                                        from a in joined.DefaultIfEmpty ( )
                                        where a == null
                                        select new HrmsEmpdocuments01
                                            {
                                            DetailId = b.DetailID,
                                            DocFields = b.DocFieldID,
                                            DocValues = b.DocFieldText
                                            };

                    await _context.HrmsEmpdocuments01s.AddRangeAsync (insertRecords);
                    await _context.SaveChangesAsync ( );
                    }
                else
                    {
                    var insertRecords = tmpDocFileUpList
                        .Select (b => new HrmsEmpdocuments01
                            {
                            DetailId = b.DetailID,
                            DocFields = int.TryParse (b.DocFieldID, out int parsedId) ? parsedId : 0,
                            DocValues = b.DocFieldText
                            })
                        .ToList ( );


                    await _context.HrmsEmpdocuments01s.AddRangeAsync (insertRecords);
                    await _context.SaveChangesAsync ( );
                    }

                var detailIds = tmpDocFileUpList.Select (b => b.DetailID).ToList ( );

                bool recordsExistApproved = _context.HrmsEmpdocumentsApproved01s
                    .Any (a => detailIds.Contains (a.DetailId));

                if (recordsExistApproved)
                    {

                    var tmpDocFileUpListConverted01 = tmpDocFileUpList
                        .Select (b => new
                            {
                            DetailID = b.DetailID ?? 0,
                            DocFieldID = string.IsNullOrEmpty (b.DocFieldID) ? 0 : int.Parse (b.DocFieldID),
                            DocFieldText = b.DocFieldText
                            })
                        .ToList ( );
                    var updateRecords = from a in _context.HrmsEmpdocumentsApproved01s
                                        join b in tmpDocFileUpListConverted01
                                        on new { DetailId = a.DetailId ?? 0, DocFields = a.DocFields ?? 0 }
                                        equals new { DetailId = b.DetailID, DocFields = b.DocFieldID }
                                        select new { a, b };


                    foreach (var record in updateRecords)
                        {
                        record.a.DocValues = record.b.DocFieldText;
                        }
                    await _context.SaveChangesAsync ( );

                    var newRecords = (from b in tmpDocFileUpList
                                      join a in _context.HrmsEmpdocumentsApproved01s
                                      on new { DetailId = b.DetailID ?? 0, DocFields = int.TryParse (b.DocFieldID, out int parsedId) ? parsedId : 0 }
                                      equals new { DetailId = a.DetailId ?? 0, DocFields = a.DocFields ?? 0 }
                                      into joined
                                      from a in joined.DefaultIfEmpty ( )
                                      where a == null
                                      select new HrmsEmpdocumentsApproved01
                                          {
                                          DetailId = b.DetailID,
                                          DocFields = int.TryParse (b.DocFieldID, out int parsedId) ? parsedId : 0,
                                          DocValues = b.DocFieldText
                                          }).ToList ( );

                    await _context.HrmsEmpdocumentsApproved01s.AddRangeAsync (newRecords);
                    await _context.SaveChangesAsync ( );
                    }
                else
                    {
                    if (workFlowNeedValue == _employeeSettings.No)
                        {

                        var newEntries = tmpDocFileUpList.Select (b => new HrmsEmpdocumentsApproved01
                            {
                            DetailId = b.DetailID,
                            DocFields = int.TryParse (b.DocFieldID, out int parsedId) ? parsedId : 0,
                            DocValues = b.DocFieldText
                            }).ToList ( );

                        await _context.HrmsEmpdocumentsApproved01s.AddRangeAsync (newEntries);
                        await _context.SaveChangesAsync ( );
                        }
                    }

                await transaction.CommitAsync ( );
                return _employeeSettings.DataInsertSuccessStatus;
                }
            catch (Exception ex)
                {
                await transaction.RollbackAsync ( );
                return $"Error: {ex.Message}";
                }
            }
        public async Task<List<FillDocumentTypeDto>> GetDocumentTypeEdit ()
            {
            return await (from b in _context.HrmsDocument00s
                          join c in _context.HrmsDocument00s on b.DocId equals c.DocId into bcGroup
                          from c in bcGroup.DefaultIfEmpty ( )
                          join d in _context.HrmsDocTypeMasters on c.DocType equals Convert.ToInt32 (d.DocTypeId) into cdGroup
                          from d in cdGroup.DefaultIfEmpty ( )
                          where b.Active == true
                                && d.DocType != _employeeSettings.Documents01
                                && d.DocType != _employeeSettings.Documents02
                          select new FillDocumentTypeDto
                              {
                              DocID = b.DocId,
                              DocName = b.DocName
                              })
                  .AsNoTracking ( )
                  .ToListAsync ( );


            }
        public async Task<List<DocumentFieldCheckBankDto>> DocumentFieldOfCheckBank (int DocumentID)
            {
            var bankDetailsDocTypeId = await _context.HrmsDocTypeMasters
    .Where (dt => dt.DocType == _employeeSettings.Documents02)
    .Select (dt => dt.DocTypeId)
    .FirstOrDefaultAsync ( );

            var result = await (from a in _context.HrmsDocumentField00s
                                join b in _context.HrmsDatatypes on a.DataTypeId equals b.DataTypeId into datatypeGroup
                                from b in datatypeGroup.DefaultIfEmpty ( )
                                join c in _context.HrmsEmpdocuments00s on a.DocId equals c.DocId into doc00
                                from c in doc00.DefaultIfEmpty ( )
                                join d in _context.HrmsEmpdocuments01s
                                on new { DetailID = (int?)c.DetailId, DocFieldID = (int?)a.DocFieldId }
                                equals new { DetailID = d.DetailId, DocFieldID = d.DocFields } into doc01
                                from d in doc01.DefaultIfEmpty ( )
                                join e in _context.HrmsDocument00s on (long?)a.DocId equals e.DocId
                                join f in _context.HrmsDocTypeMasters on (long?)e.DocType equals f.DocTypeId
                                join g in _context.AdmCountryMasters on d.DocValues equals g.CountryName into countryGroup
                                from g in countryGroup.DefaultIfEmpty ( )
                                join h in _context.ReasonMasters on d.DocValues equals h.Description into reasonGroup
                                from h in reasonGroup.DefaultIfEmpty ( )
                                where c.DetailId == DocumentID && e.DocType == bankDetailsDocTypeId && (a.DocDescription != _employeeSettings.Active || a.DocDescription == null)
                                select new DocumentFieldCheckBankDto
                                    {
                                    DocFieldID = a.DocFieldId,
                                    DocDescription = a.DocDescription,
                                    DocID = a.DocId,
                                    DataTypeId = a.DataTypeId,
                                    IsDate = b.IsDate,
                                    IsGeneralCategory = b.IsGeneralCategory,
                                    DataType = b.DataType,
                                    DetailID = c.DetailId,
                                    TransactionType = c.TransactionType,
                                    Status = c.Status,
                                    DocFieldID01 = d.DocFieldId,
                                    DocValues = d.DocValues,
                                    CountryId = g.CountryId,
                                    GeneralCategoryId = h.ReasonId,
                                    FolderName = e.FolderName
                                    }).AsNoTracking ( ).ToListAsync ( );

            return result;
            }

        public async Task<List<DocumentFieldGetEditDocFieldsDto>> DocumentFieldOfGetEditDocFields (int DocumentID, string Status)
            {
            if (Status == "Pending")
                {
                var result = await (
                      from a in _context.HrmsDocumentField00s
                      join f in _context.HrmsDocument00s on (long?)a.DocId equals f.DocId
                      join g in _context.HrmsDocTypeMasters on (long?)f.DocType equals g.DocTypeId
                      join b in _context.HrmsDatatypes on a.DataTypeId equals b.DataTypeId into bGroup
                      from b in bGroup.DefaultIfEmpty ( )
                      join c in _context.HrmsEmpdocuments00s on a.DocId equals c.DocId into cGroup
                      from c in cGroup.DefaultIfEmpty ( )
                      join d in _context.HrmsEmpdocuments01s on c.DetailId equals d.DetailId into dGroup
                      from d in dGroup.DefaultIfEmpty ( )
                      where c.DetailId == DocumentID && g.DocType != _employeeSettings.Documents01 && a.DocDescription != _employeeSettings.Active && a.DocFieldId == d.DocFields

                      select new DocumentFieldGetEditDocFieldsDto
                          {
                          DocFieldID = a.DocFieldId,
                          DocDescription = a.DocDescription,
                          DocID = a.DocId,
                          DataTypeId = a.DataTypeId,
                          IsDate = b.IsDate,
                          IsGeneralCategory = b.IsGeneralCategory,
                          DataType = b.DataType,
                          DetailID = c.DetailId,
                          TransactionType = c.TransactionType,
                          Status = c.Status,
                          DocFieldID01 = d.DocFieldId,
                          DocValues = d.DocValues ?? "",
                          FolderName = f.FolderName,
                          EmpID = c.EmpId ?? 0,
                          IsMandatory = a.IsMandatory,
                          IsAllowMultiple = f.IsAllowMultiple
                          }).AsNoTracking ( ).ToListAsync ( );
                return result;
                }
            else
                {
                var result = await (
                 from a in _context.HrmsDocumentField00s
                 join f in _context.HrmsDocument00s on (long?)a.DocId equals f.DocId
                 join g in _context.HrmsDocTypeMasters on (long?)f.DocType equals g.DocTypeId
                 join b in _context.HrmsDatatypes on a.DataTypeId equals b.DataTypeId into bGroup
                 from b in bGroup.DefaultIfEmpty ( )
                 join c in _context.HrmsEmpdocumentsApproved00s on a.DocId equals c.DocId into cGroup
                 from c in cGroup.DefaultIfEmpty ( )
                 join d in _context.HrmsEmpdocumentsApproved01s on c.DetailId equals d.DetailId into dGroup
                 from d in dGroup.DefaultIfEmpty ( )
                 where c.DetailId == DocumentID && g.DocType != _employeeSettings.Documents01 && a.DocDescription != _employeeSettings.Active && a.DocFieldId == d.DocFields

                 select new DocumentFieldGetEditDocFieldsDto
                     {
                     DocFieldID = a.DocFieldId,
                     DocDescription = a.DocDescription,
                     DocID = a.DocId,
                     DataTypeId = a.DataTypeId,
                     IsDate = b.IsDate,
                     IsGeneralCategory = b.IsGeneralCategory,
                     DataType = b.DataType,
                     DetailID = c.DetailId ?? 0,
                     TransactionType = c.TransactionType,
                     Status = c.Status,
                     DocFieldID01 = d.DocFieldId,
                     DocValues = d.DocValues ?? "",
                     FolderName = f.FolderName,
                     EmpID = c.EmpId ?? 0,
                     IsMandatory = a.IsMandatory,
                     IsAllowMultiple = f.IsAllowMultiple
                     }).AsNoTracking ( ).ToListAsync ( );
                return result;
                }


            }
        public async Task<List<GetCountryNameDto>> DocumentFieldOfGetCountryName ( )
            {
            return await (from a in _context.AdmCountryMasters

                          select new GetCountryNameDto
                              {
                              Country_ID = a.CountryId,
                              Country_Name = a.CountryName
                              }).AsNoTracking ( ).ToListAsync ( );

            }
        public async Task<object> DocumentFieldOfGetBankTypeEdit ( )
            {
            var result = await (from b in _context.HrmsDocument00s
                                join c in _context.HrmsDocTypeMasters
                                    on (long?)b.DocType equals c.DocTypeId into docTypeGroup
                                from c in docTypeGroup.DefaultIfEmpty ( )
                                where b.Active == true && c.DocType == _employeeSettings.Documents02
                                select new
                                    {
                                    DocId = b.DocId,
                                    DocName = b.DocName
                                    }).ToListAsync ( );

            return result;
            }

        public async Task<List<DocumentGetFolderNameDto>> DocumentOfGetFolderName (int DocumentID)
            {
            var result = await (from a in _context.HrmsDocument00s

                                where a.DocId == DocumentID

                                select new DocumentGetFolderNameDto
                                    {
                                    DocID = a.DocId,
                                    DocName = a.DocName,
                                    FolderName = a.FolderName
                                    }).AsNoTracking ( ).ToListAsync ( );
            return result;

            }
        public async Task<string> UpdateEmpDocumentDetails (object documentDetails, int DetailID, string Status, int EntryBy)
            {
            if (documentDetails == null || string.IsNullOrEmpty (Status) || DetailID <= 0)
                {
                return "Invalid input data";
                }

            try
                {
                var utcNow = DateTime.UtcNow;

                if (Status == "Pending")
                    {
                    var document = await _context.HrmsEmpdocuments00s
                        .FirstOrDefaultAsync (d => d.DetailId == DetailID);

                    if (document != null)
                        {
                        document.EntryDate = utcNow;
                        document.UpdatedBy = EntryBy;
                        document.UpdatedDate = utcNow;

                        await _context.SaveChangesAsync ( );
                        }
                    }
                else if (Status == "Approved")
                    {
                    var document = await _context.HrmsEmpdocumentsApproved00s
                        .FirstOrDefaultAsync (d => d.DetailId == DetailID);

                    if (document != null)
                        {
                        document.EntryDate = utcNow;
                        document.UpdatedBy = EntryBy;
                        document.UpdatedDate = utcNow;

                        await _context.SaveChangesAsync ( );
                        }
                    }

                return "Success";
                }
            catch (Exception ex)
                {
                return $"Error: {ex.Message}";
                }
            }



        }
    }
