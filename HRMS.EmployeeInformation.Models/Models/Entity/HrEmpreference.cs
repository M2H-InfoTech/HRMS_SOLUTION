using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class HrEmpreference
{
    public int RefId { get; set; }

    public int? EmpId { get; set; }

    public string? RefType { get; set; }

    public string? RefMethod { get; set; }

    public int? RefEmpId { get; set; }

    public int? ConsultantId { get; set; }

    public string? RefName { get; set; }

    public string? PhoneNo { get; set; }

    public string? Address { get; set; }

    public string? Status { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
