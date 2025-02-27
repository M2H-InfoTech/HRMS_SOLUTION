namespace HRMS.EmployeeInformation.Service.InterfaceB
{
    public interface IEmployeeInformationServiceB
    {
        Task<List<object>> QualificationDocumentsDetails(int QualificationId);

    }
}
