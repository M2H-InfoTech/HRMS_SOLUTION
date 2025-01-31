using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class HrEmpPersonal
{
    public int InstId { get; set; }

    public int PerId { get; set; }

    public int? EmpId { get; set; }

    public DateTime? Dob { get; set; }

    public string? Gender { get; set; }

    public int? Nationality { get; set; }

    public int? Religion { get; set; }

    public int? Country { get; set; }

    public string? BirthPlace { get; set; }

    public string? MaritalStatus { get; set; }

    public string? IdentMark { get; set; }

    public string? BloodGrp { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDt { get; set; }

    public string? Height { get; set; }

    public string? Weight { get; set; }

    public int? CountryOfBirth { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public DateTime? WeddingDate { get; set; }

    public string? EmployeeType { get; set; }
}
