using Microsoft.AspNetCore.Http;

namespace HRMS.EmployeeInformation.Repository.Common
{
    public interface IRepository<T> where T : class
    {
        Task<(bool Success, string Message, List<string>? FileNames)> UploadOrUpdateDocuments(
        List<IFormFile> files,
        string subFolderPath,
        Func<IFormFile, string, T> mapToEntity,
        Func<T, object> getId);
    }
}
