using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class EmpPersonal
{
    public int PersonalId { get; set; }

    public string? EmployeeCode { get; set; }

    public string? Gender { get; set; }

    public string? Nationality { get; set; }

    public string? Country { get; set; }

    public string? Religion { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? MaritalStatus { get; set; }

    public string? BirthPlace { get; set; }

    public string? IdentityMark { get; set; }

    public string? BloodGroup { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}
