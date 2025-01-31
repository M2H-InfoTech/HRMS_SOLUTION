using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class EmployeeCertification
{
    public int CertificationId { get; set; }

    public int? EmpId { get; set; }

    public int? CertificationName { get; set; }

    public int? CertificationField { get; set; }

    public int? YearofCompletion { get; set; }

    public int? IssuingAuthority { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Status { get; set; }
}
