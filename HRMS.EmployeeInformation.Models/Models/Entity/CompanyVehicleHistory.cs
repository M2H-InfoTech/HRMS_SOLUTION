namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class CompanyVehicleHistory
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public bool? CompanyVehicle { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? Initial { get; set; }
}
