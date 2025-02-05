using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class EditInfoHistory
{
    public int InfoHistoryId { get; set; }

    public int? EmpId { get; set; }

    public int? InfoId { get; set; }

    public int? Info01Id { get; set; }

    public string? InfoCode { get; set; }

    public string? Value { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? OldValue { get; set; }
}
