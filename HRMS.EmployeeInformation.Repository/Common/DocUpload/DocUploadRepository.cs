using EMPLOYEE_INFORMATION.Data;
using HRMS.EmployeeInformation.Models;
using HRMS.EmployeeInformation.Models.Models.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace HRMS.EmployeeInformation.Repository.Common.DocUpload
{
    public class DocUploadRepository : IDocUploadRepository
    {

        private readonly IWebHostEnvironment _env;
        private readonly EmployeeDBContext _context;
        private readonly EmployeeSettings _employeeSettings;

        public DocUploadRepository(EmployeeDBContext context, IWebHostEnvironment env, EmployeeSettings employeeSettings)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _env = env;
            _employeeSettings = employeeSettings;
        }
        public async Task<string> UploadAndInsertEmployeeDocumentAsync(IFormFile file, int detailId, string folderPath)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Upload file to dynamic folder path
                string webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string fullPath = Path.Combine(webRootPath, folderPath, detailId.ToString());

                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);

                string uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.UtcNow:yyyyMMddHHmmssfff}{Path.GetExtension(file.FileName)}";
                string fullFilePath = Path.Combine(fullPath, uniqueFileName);

                using (var stream = new FileStream(fullFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                string fileType = file.ContentType;

                // Remove existing Live and Approved entries
                _context.HrmsEmpdocuments02s.RemoveRange(_context.HrmsEmpdocuments02s.Where(d => d.DetailId == detailId));
                _context.HrmsEmpdocumentsApproved02s.RemoveRange(_context.HrmsEmpdocumentsApproved02s.Where(d => d.DetailId == detailId));

                // Insert Live document
                var liveDoc = new HrmsEmpdocuments02
                {
                    DetailId = detailId,
                    FileName = uniqueFileName,
                    FileType = fileType
                };
                await _context.HrmsEmpdocuments02s.AddAsync(liveDoc);

                // Insert Approved document
                var approvedDoc = new HrmsEmpdocumentsApproved02
                {
                    DetailId = detailId,
                    FileName = uniqueFileName,
                    FileType = fileType
                };
                await _context.HrmsEmpdocumentsApproved02s.AddAsync(approvedDoc);

                // Insert History
                int docHisId = await _context.HrmsEmpdocumentsHistory00s
                    .Where(h => h.DetailId == detailId)
                    .OrderByDescending(h => h.DocHistId)
                    .Select(h => (int?)h.DocHistId)
                    .FirstOrDefaultAsync() ?? 0;

                string? oldFileName = await _context.HrmsEmpdocuments02s
                    .Where(x => x.DetailId == detailId)
                    .Select(x => x.FileName)
                    .FirstOrDefaultAsync();

                var historyDoc = new HrmsEmpdocumentsHistory02
                {
                    DocHisId = docHisId,
                    DetailId = detailId,
                    FileName = uniqueFileName,
                    FileType = fileType,
                    OldFileName = oldFileName
                };
                await _context.HrmsEmpdocumentsHistory02s.AddAsync(historyDoc);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return _employeeSettings.DataInsertSuccessStatus;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return $"Error: {ex.Message}";
            }
        }
        //public async Task<string> UploadAndInsertEmployeeDocumentAsync(IFormFile file, int detailId, int entryBy)
        //{

        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        // Upload file
        //        string webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        //        string folderPath = Path.Combine(webRootPath, "EmployeeDocuments", detailId.ToString());

        //        if (!Directory.Exists(folderPath))
        //            Directory.CreateDirectory(folderPath);

        //        string uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.UtcNow:yyyyMMddHHmmssfff}{Path.GetExtension(file.FileName)}";
        //        string fullPath = Path.Combine(folderPath, uniqueFileName);

        //        using (var stream = new FileStream(fullPath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }

        //        string fileType = file.ContentType;

        //        // Remove existing Live and Approved entries
        //        _context.HrmsEmpdocuments02s.RemoveRange(_context.HrmsEmpdocuments02s.Where(d => d.DetailId == detailId));
        //        _context.HrmsEmpdocumentsApproved02s.RemoveRange(_context.HrmsEmpdocumentsApproved02s.Where(d => d.DetailId == detailId));

        //        // Insert Live
        //        var liveDoc = new HrmsEmpdocuments02
        //        {
        //            DetailId = detailId,
        //            FileName = uniqueFileName,
        //            FileType = fileType
        //        };
        //        await _context.HrmsEmpdocuments02s.AddAsync(liveDoc);

        //        // Insert Approved
        //        var approvedDoc = new HrmsEmpdocumentsApproved02
        //        {
        //            DetailId = detailId,
        //            FileName = uniqueFileName,
        //            FileType = fileType
        //        };
        //        await _context.HrmsEmpdocumentsApproved02s.AddAsync(approvedDoc);

        //        // Insert History
        //        int docHisId = await _context.HrmsEmpdocumentsHistory00s
        //        .Where(h => h.DetailId == detailId)
        //            .OrderByDescending(h => h.DocHistId)
        //            .Select(h => (int?)h.DocHistId)
        //            .FirstOrDefaultAsync() ?? 0;

        //        string? oldFileName = await _context.HrmsEmpdocuments02s
        //            .Where(x => x.DetailId == detailId)
        //            .Select(x => x.FileName)
        //            .FirstOrDefaultAsync();

        //        var historyDoc = new HrmsEmpdocumentsHistory02
        //        {
        //            DocHisId = docHisId,
        //            DetailId = detailId,
        //            FileName = uniqueFileName,
        //            FileType = fileType,
        //            OldFileName = oldFileName
        //        };
        //        await _context.HrmsEmpdocumentsHistory02s.AddAsync(historyDoc);

        //        await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        return _employeeSettings.DataInsertSuccessStatus;
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        return $"Error: {ex.Message}";
        //    }
        //}
    }
}
