namespace HRMS.EmployeeInformation.Repository.Common.RepositoryB
{
    public interface IRepositoryB
    {
        Task<List<object>> QualificationDocumentsDetails(int QualificationId);

    }
}
