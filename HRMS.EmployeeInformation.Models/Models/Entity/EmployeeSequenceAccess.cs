using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class EmployeeSequenceAccess
{
    public long SequenceEmployeeId { get; set; }

    public int? EmployeeId { get; set; }

    public int? SequenceId { get; set; }

    public int? SequenceType { get; set; }

    public int? LinkLevel { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ValidTo { get; set; }
}
