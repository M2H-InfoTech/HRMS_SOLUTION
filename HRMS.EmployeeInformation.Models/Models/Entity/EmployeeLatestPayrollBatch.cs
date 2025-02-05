using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class EmployeeLatestPayrollBatch
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public int? PayrollBatchId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}
