using System.ComponentModel.DataAnnotations;

namespace HRMS.EmployeeInformation.Models;

public partial class PersonalDetailsHistory
{
    [Key]
    public int? EmployeeId { get; set; }

    public string? GuardiansName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? PersonalMail { get; set; }

    public int? Country { get; set; }

    public int? Nationality { get; set; }

    public int? CountryOfBirth { get; set; }

    public string? BloodGroup { get; set; }

    public int? Religion { get; set; }

    public string? IdentificationMark { get; set; }

    public string? Height { get; set; }

    public string? Weight { get; set; }

    public string? MaritalStatus { get; set; }

    public string? Gender { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? WeddingDate { get; set; }
}
