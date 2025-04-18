using Microsoft.AspNetCore.Http;

namespace HRMS.EmployeeInformation.Repository.Common.DocUpload
{
    public interface IDocUploadRepository
    {
        //Task<string> UploadAndInsertEmployeeDocumentAsync(IFormFile file, int detailId, int entryBy);
        Task<string> UploadAndInsertEmployeeDocumentAsync(IFormFile file, int detailId, string folderPath);

    }
}
