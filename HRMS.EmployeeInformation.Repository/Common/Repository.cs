using EMPLOYEE_INFORMATION.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace HRMS.EmployeeInformation.Repository.Common
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly EmployeeDBContext _context;
        private readonly IWebHostEnvironment _env;

        public Repository(EmployeeDBContext context, IWebHostEnvironment env)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _env = env;
        }

        public async Task<(bool Success, string Message, List<string>? FileNames)> UploadOrUpdateDocuments(
            List<IFormFile> files,
            string subFolderPath,
            Func<IFormFile, string, T> mapToEntity,
            Func<T, object> getId)
        {
            if (files == null || !files.Any())
            {
                return (false, "No files provided for upload.", null);
            }

            string webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string uploadPath = Path.Combine(webRootPath, subFolderPath);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            List<string> fileNames = new List<string>();
            List<string> errors = new List<string>();

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    continue;
                }

                string uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.UtcNow:yyyyMMddHHmmssFFF}{Path.GetExtension(file.FileName)}";
                string filePath = Path.Combine(uploadPath, uniqueFileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    fileNames.Add(uniqueFileName);

                    var newEntity = mapToEntity(file, filePath);
                    object entityId = getId(newEntity);

                    var existingEntity = await _context.Set<T>().FindAsync(entityId);

                    if (existingEntity != null)
                    {
                        _context.Entry(existingEntity).CurrentValues.SetValues(newEntity);
                    }
                    else
                    {
                        await _context.Set<T>().AddAsync(newEntity);
                    }
                }
                catch (Exception ex)
                {
                    errors.Add($"Error uploading file {file.FileName}: {ex.Message}");
                }
            }

            await _context.SaveChangesAsync();

            if (errors.Any())
            {
                return (false, $"Some files failed to upload: {string.Join("; ", errors)}", fileNames);
            }

            return (true, "Files uploaded and updated successfully.", fileNames);
        }
    }
}
