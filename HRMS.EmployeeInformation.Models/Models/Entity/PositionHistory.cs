using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class PositionHistory
{
    public int PositionId { get; set; }

    public int? EmpId { get; set; }

    public int? DesigId { get; set; }

    public int? DepId { get; set; }

    public int? GradeId { get; set; }

    public int? BandId { get; set; }

    public int? BranchId { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? Status { get; set; }

    public int? EmpStatus { get; set; }

    public int? LastEntity { get; set; }

    public int? TransferId { get; set; }

    public int? TransferBatchId { get; set; }

    public string? OldEmpCode { get; set; }
}
