using AutoMapper;
using EMPLOYEE_INFORMATION.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using MPLOYEE_INFORMATION.DTO.DTOs;
using HRMS.EmployeeInformation.Models.Models.Entity;
using System.Reflection.Metadata;
using EMPLOYEE_INFORMATION.Models;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace HRMS.EmployeeInformation.Repository.Common.RepositoryB
{
    public class RepositoryB : IRepositoryB
    {
        private readonly EmployeeDBContext _context;
        //private IStringLocalizer _stringLocalizer;
        private readonly IMemoryCache _memoryCache;
        private readonly EmployeeSettings _employeeSettings;
        private int paramDynVal;

        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public RepositoryB(EmployeeDBContext dbContext, EmployeeSettings employeeSettings, IMemoryCache memoryCache, IMapper mapper, IWebHostEnvironment env)
        {
            _context = dbContext;
            _employeeSettings = employeeSettings;
            _memoryCache = memoryCache;
            _mapper = mapper;
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public async Task<List<FillDocumentTypeDto>> FillDocumentType(int EmpID)
        {
            return await (from a in _context.HrmsDocument00s
                          join b in _context.HrmsDocTypeMasters on a.DocType equals Convert.ToInt32(b.DocTypeId) 
                          join c in _context.EmpDocumentAccesses on Convert.ToInt32(a.DocId) equals c.DocId
                          where c.EmpId == EmpID
                                && !_context.HrmsEmpdocuments00s.Any(d => d.DocId == a.DocId && d.EmpId == EmpID && d.Status != "R")
                                && a.Active == true
                                && b.DocType != "Statutory"
                                && b.DocType != "BANK DETAILS"
                                || (_context.HrmsDocument00s
                                    .Join(_context.HrmsDocTypeMasters, x => (long)x.DocType, y => y.DocTypeId, (x, y) => new { x.DocId, x.IsAllowMultiple, y.DocType, y.Code })
                                    .Any(d => d.DocId == a.DocId && d.IsAllowMultiple == 1 && d.DocType != "Statutory" && d.Code != "BNK"))
                          select new FillDocumentTypeDto
                          {
                              DocID = a.DocId,
                              DocName = a.DocName

                          }).AsNoTracking().ToListAsync();


        }
        public async Task<List<DocumentFieldDto>> DocumentField(int DocumentID)
        {
            return await (from a in _context.HrmsDocumentField00s
                          join b in _context.HrmsDatatypes on a.DataTypeId equals b.DataTypeId into datatypeGroup
                          from b in datatypeGroup.DefaultIfEmpty()
                          join c in _context.HrmsDocument00s on (long)a.DocId equals c.DocId
                          where a.DocId == DocumentID

                          select new DocumentFieldDto
                          {
                              DocFieldID = a.DocFieldId,
                              DocDescription = a.DocDescription,
                              DataTypeId = a.DataTypeId,
                              DocID = a.DocId,
                              CreatedBy = a.CreatedBy,
                              ModifiedBy = a.ModifiedBy,
                              ModifiedDate = a.ModifiedDate,
                              IsMandatory = a.IsMandatory,
                              TypeId = b.TypeId,
                              DataType = b.DataType,
                              IsDate = b.IsDate,
                              IsGeneralCategory = b.IsGeneralCategory,
                              IsDropdown = b.IsDropdown,
                              DocName = c.DocName,
                              DocType = c.DocType,
                              Active = c.Active,
                              IsExpiry = c.IsExpiry,
                              NotificationCountDays = c.NotificationCountDays,
                              FolderName = c.FolderName,
                              IsAllowMultiple = c.IsAllowMultiple,
                              IsESI = c.IsEsi,
                              IsPF = c.IsPf,
                              ShowInRecruitment = c.ShowInRecruitment
                          }).AsNoTracking().ToListAsync();

        }
        public async Task<List<DocumentGetGeneralSubCategoryListDto>> DocumentGetGeneralSubCategoryList(string Remarks)
        {
            return await (from a in _context.ReasonMasters
                          join b in _context.GeneralCategories on a.Type equals b.Description
                          where b.Description == Remarks && a.Status == "A"


                          select new DocumentGetGeneralSubCategoryListDto
                          {
                              Reason_Id = a.ReasonId,
                              Description = a.Description
                          }).AsNoTracking().ToListAsync();

        }

        public async Task<string> InsertDocumentsFieldDetails(List<TmpDocFileUpDto> DocumentBankField, int DocumentID, int In_EntryBy)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var workFlowNeedValue = await (from a in _context.CompanyParameters
                                               join b in _context.HrmValueTypes on a.Value equals b.Value
                                               where b.Type == "EmployeeReporting" && a.ParameterCode == "ENDOCAPPRL" && a.Type == "COM"
                                               select b.Code).FirstOrDefaultAsync();

                var tmpDocFileUpList = DocumentBankField;

                bool recordsExist = (from a in _context.HrmsEmpdocuments01s.ToList()
                                     join b in tmpDocFileUpList
                                     on a.DetailId equals b.DetailID
                                     select a).Any();

                if (recordsExist)
                {
                    var tmpDocFileUpListConverted = tmpDocFileUpList
                        .Select(b => new
                        {
                            DetailID = b.DetailID ?? 0,
                            DocFieldID = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0,
                            DocFieldText = b.DocFieldText
                        })
                        .ToList();

                    var updateRecords = from a in _context.HrmsEmpdocuments01s
                                        join b in tmpDocFileUpListConverted
                                        on new { DetailId = a.DetailId ?? 0, DocFields = a.DocFields ?? 0 }
                                        equals new { DetailId = b.DetailID, DocFields = b.DocFieldID }
                                        select new { a, b };

                    foreach (var record in updateRecords)
                    {
                        record.a.DocValues = record.b.DocFieldText;
                    }

                    await _context.SaveChangesAsync();

                    var insertRecords = from b in tmpDocFileUpListConverted
                                        join a in _context.HrmsEmpdocuments01s
                                            on new { DetailID = b.DetailID, DocFields = b.DocFieldID }
                                            equals new { DetailID = a.DetailId ?? 0, DocFields = a.DocFields ?? 0 } into joined
                                        from a in joined.DefaultIfEmpty()
                                        where a == null
                                        select new HrmsEmpdocuments01
                                        {
                                            DetailId = b.DetailID,
                                            DocFields = b.DocFieldID,
                                            DocValues = b.DocFieldText
                                        };

                    await _context.HrmsEmpdocuments01s.AddRangeAsync(insertRecords);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var insertRecords = tmpDocFileUpList
                        .Select(b => new HrmsEmpdocuments01
                        {
                            DetailId = b.DetailID,
                            DocFields = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0,
                            DocValues = b.DocFieldText
                        })
                        .ToList();


                    await _context.HrmsEmpdocuments01s.AddRangeAsync(insertRecords);
                    await _context.SaveChangesAsync();
                }

                var detailIds = tmpDocFileUpList.Select(b => b.DetailID).ToList();

                bool recordsExistApproved = _context.HrmsEmpdocumentsApproved01s
                    .Any(a => detailIds.Contains(a.DetailId));

                if (recordsExistApproved)
                {

                    var documentHistory = _context.HrmsEmpdocumentsApproved00s
                        .Where(a => a.DetailId == DocumentID)
                        .Select(a => new HrmsEmpdocumentsHistory00
                        {
                            DetailId = a.DetailId,
                            DocApprovedId = a.DocApprovedId,
                            DocId = a.DocId,
                            EmpId = a.EmpId,
                            Status = "U",
                            RequestId = a.RequestId,
                            DateFrom = DateTime.UtcNow,
                            EntryBy = In_EntryBy,
                            EntryDate = DateTime.UtcNow
                        }).ToList();

                    await _context.HrmsEmpdocumentsHistory00s.AddRangeAsync(documentHistory);
                    await _context.SaveChangesAsync();

                    int docHisId = documentHistory.FirstOrDefault()?.DocApprovedId ?? 0;
                    var tmpDocFileUpListConverted = tmpDocFileUpList
                        .Select(b => new
                        {
                            DetailID = b.DetailID,
                            DocFieldID = string.IsNullOrEmpty(b.DocFieldID) ? (int?)null : int.Parse(b.DocFieldID),
                            DocFieldText = b.DocFieldText
                        })
                        .ToList();


                    var historyRecords = (from a in _context.HrmsEmpdocumentsApproved01s
                                          join b in tmpDocFileUpListConverted
                                          on new { a.DetailId, a.DocFields }
                                          equals new { DetailId = b.DetailID, DocFields = b.DocFieldID }
                                          where a.DocValues != b.DocFieldText
                                          select new HrmsEmpdocumentsHistory01
                                          {
                                              DocHisId = docHisId,
                                              DetailId = b.DetailID,
                                              DocFields = b.DocFieldID,
                                              DocValues = b.DocFieldText,
                                              OldDocValues = a.DocValues
                                          }).ToList();


                    await _context.HrmsEmpdocumentsHistory01s.AddRangeAsync(historyRecords);
                    await _context.SaveChangesAsync();

                    var missingHistoryRecords = (from b in tmpDocFileUpList
                                                 join a in _context.HrmsEmpdocumentsApproved01s
                                                 on new { DetailId = b.DetailID ?? 0, DocFields = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0 }
                                                 equals new { DetailId = a.DetailId ?? 0, DocFields = a.DocFields ?? 0 }
                                                 into joined
                                                 from a in joined.DefaultIfEmpty()
                                                 where a == null
                                                 select new HrmsEmpdocumentsHistory01
                                                 {
                                                     DocHisId = docHisId,
                                                     DetailId = b.DetailID,
                                                     DocFields = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0,
                                                     DocValues = b.DocFieldText,
                                                     OldDocValues = null
                                                 }).ToList();

                    await _context.HrmsEmpdocumentsHistory01s.AddRangeAsync(missingHistoryRecords);
                    await _context.SaveChangesAsync();

                    var tmpDocFileUpListConverted01 = tmpDocFileUpList
                        .Select(b => new
                        {
                            DetailID = b.DetailID ?? 0,
                            DocFieldID = string.IsNullOrEmpty(b.DocFieldID) ? 0 : int.Parse(b.DocFieldID),
                            DocFieldText = b.DocFieldText
                        })
                        .ToList();
                    var updateRecords = from a in _context.HrmsEmpdocumentsApproved01s
                                        join b in tmpDocFileUpListConverted01
                                        on new { DetailId = a.DetailId ?? 0, DocFields = a.DocFields ?? 0 }
                                        equals new { DetailId = b.DetailID, DocFields = b.DocFieldID }
                                        select new { a, b };


                    foreach (var record in updateRecords)
                    {
                        record.a.DocValues = record.b.DocFieldText;
                    }
                    await _context.SaveChangesAsync();

                    var newRecords = (from b in tmpDocFileUpList
                                      join a in _context.HrmsEmpdocumentsApproved01s
                                      on new { DetailId = b.DetailID ?? 0, DocFields = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0 }
                                      equals new { DetailId = a.DetailId ?? 0, DocFields = a.DocFields ?? 0 }
                                      into joined
                                      from a in joined.DefaultIfEmpty()
                                      where a == null
                                      select new HrmsEmpdocumentsApproved01
                                      {
                                          DetailId = b.DetailID,
                                          DocFields = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0,
                                          DocValues = b.DocFieldText
                                      }).ToList();

                    await _context.HrmsEmpdocumentsApproved01s.AddRangeAsync(newRecords);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (workFlowNeedValue == "No")
                    {

                        var documentHistory = _context.HrmsEmpdocumentsApproved00s
                            .Where(a => a.DetailId == DocumentID)
                            .Select(a => new HrmsEmpdocumentsHistory00
                            {
                                DetailId = a.DetailId,
                                DocApprovedId = a.DocApprovedId,
                                DocId = a.DocId,
                                EmpId = a.EmpId,
                                Status = "I",
                                RequestId = a.RequestId,
                                DateFrom = DateTime.UtcNow,
                                EntryBy = In_EntryBy,
                                EntryDate = DateTime.UtcNow
                            }).ToList();

                        await _context.HrmsEmpdocumentsHistory00s.AddRangeAsync(documentHistory);
                        await _context.SaveChangesAsync();

                        int docHisId = documentHistory.FirstOrDefault()?.DocApprovedId ?? 0;


                        var historyInsert = tmpDocFileUpList.Select(b => new HrmsEmpdocumentsHistory01
                        {
                            DetailId = b.DetailID,
                            DocFields = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0,
                            DocValues = b.DocFieldText,
                            OldDocValues = null
                        }).ToList();

                        await _context.HrmsEmpdocumentsHistory01s.AddRangeAsync(historyInsert);
                        await _context.SaveChangesAsync();

                        var newEntries = tmpDocFileUpList.Select(b => new HrmsEmpdocumentsApproved01
                        {
                            DetailId = b.DetailID,
                            DocFields = int.TryParse(b.DocFieldID, out int parsedId) ? parsedId : 0,
                            DocValues = b.DocFieldText
                        }).ToList();

                        await _context.HrmsEmpdocumentsApproved01s.AddRangeAsync(newEntries);
                        await _context.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();
                return "Successfully Saved";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> SetEmpDocuments(TmpFileUpDto DocumentBankField, int DetailID, string Status, int In_EntryBy)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var workFlowNeedValue = await (from a in _context.CompanyParameters
                                               join b in _context.HrmValueTypes on a.Value equals b.Value
                                               where b.Type == "EmployeeReporting" && a.ParameterCode == "ENDOCAPPRL" && a.Type == "COM"
                                               select b.Code).FirstOrDefaultAsync();


                if ((Status == "Approved" || Status == "A") || workFlowNeedValue == "No")
                {
                    int? docHisId = 0;

                    bool documentExists = await _context.HrmsEmpdocumentsApproved02s
                        .AnyAsync(d => d.DetailId == DetailID);

                    if (documentExists)
                    {
                        docHisId = await _context.HrmsEmpdocumentsHistory00s
                            .Where(h => h.DetailId == DetailID)
                            .OrderByDescending(h => h.DocHistId)
                            .Select(h => (int?)h.DocHistId)
                            .FirstOrDefaultAsync();

                        var historyEntry = await (from a in _context.HrmsEmpdocuments02s
                                                  where a.DetailId == DetailID
                                                  select new HrmsEmpdocumentsHistory02
                                                  {
                                                      DocHisId = docHisId ?? 0,
                                                      DetailId = a.DetailId,
                                                      FileName = DocumentBankField.FileName,
                                                      FileType = DocumentBankField.FileType,
                                                      FileData = DocumentBankField.FileData,
                                                      OldFileName = a.FileName
                                                  }).ToListAsync();

                        await _context.HrmsEmpdocumentsHistory02s.AddRangeAsync(historyEntry);
                    }
                    else
                    {
                        docHisId = await _context.HrmsEmpdocumentsHistory00s
                            .Where(h => h.DetailId == DetailID)
                            .OrderByDescending(h => h.DocHistId)
                            .Select(h => (int?)h.DocHistId)
                            .FirstOrDefaultAsync();

                        var historyEntry = new HrmsEmpdocumentsHistory02
                        {
                            DocHisId = docHisId ?? 0,
                            DetailId = DocumentBankField.DetailID,
                            FileName = DocumentBankField.FileName,
                            FileType = DocumentBankField.FileType,
                            FileData = DocumentBankField.FileData,
                            OldFileName = null
                        };

                        await _context.HrmsEmpdocumentsHistory02s.AddAsync(historyEntry);

                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                if (Status == "Pending" || Status == "P")
                {
                    var existingDocs = _context.HrmsEmpdocuments02s.Where(d => d.DetailId == DetailID);
                    _context.HrmsEmpdocuments02s.RemoveRange(existingDocs);

                    var newDocument = new HrmsEmpdocuments02
                    {
                        DetailId = DocumentBankField.DetailID,
                        FileName = DocumentBankField.FileName,
                        FileType = DocumentBankField.FileType,
                        FileData = DocumentBankField.FileData
                    };

                    await _context.HrmsEmpdocuments02s.AddAsync(newDocument);

                    if (workFlowNeedValue == "No")
                    {
                        var existingApprovedDocs = _context.HrmsEmpdocumentsApproved02s.Where(d => d.DetailId == DetailID);
                        _context.HrmsEmpdocumentsApproved02s.RemoveRange(existingApprovedDocs);
                        await _context.SaveChangesAsync();

                        var newApprovedDocument = new HrmsEmpdocumentsApproved02
                        {
                            DetailId = DocumentBankField.DetailID,
                            FileName = DocumentBankField.FileName,
                            FileType = DocumentBankField.FileType,
                            FileData = DocumentBankField.FileData
                        };

                        await _context.HrmsEmpdocumentsApproved02s.AddAsync(newApprovedDocument);
                        await _context.SaveChangesAsync();
                    }
                }
                else if (Status == "Approved" || Status == "A")
                {
                    var existingDocs01 = _context.HrmsEmpdocuments02s.Where(d => d.DetailId == DetailID);
                    _context.HrmsEmpdocuments02s.RemoveRange(existingDocs01);

                    var newDocument = new HrmsEmpdocuments02
                    {
                        DetailId = DocumentBankField.DetailID,
                        FileName = DocumentBankField.FileName,
                        FileType = DocumentBankField.FileType,
                        FileData = DocumentBankField.FileData
                    };

                    await _context.HrmsEmpdocuments02s.AddAsync(newDocument);

                    var existingApprovedDocs = _context.HrmsEmpdocumentsApproved02s.Where(d => d.DetailId == DetailID);
                    _context.HrmsEmpdocumentsApproved02s.RemoveRange(existingApprovedDocs);


                    var newApprovedDocument = new HrmsEmpdocumentsApproved02
                    {
                        DetailId = DocumentBankField.DetailID,
                        FileName = DocumentBankField.FileName,
                        FileType = DocumentBankField.FileType,
                        FileData = DocumentBankField.FileData
                    };

                    await _context.HrmsEmpdocumentsApproved02s.AddAsync(newApprovedDocument);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                return "Successfully Saved";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return $"Error: {ex.Message}";
            }
        }

    }
}
