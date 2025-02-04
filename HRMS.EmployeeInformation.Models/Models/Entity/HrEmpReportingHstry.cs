using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrEmpReportingHstry
{
    public int? InstId { get; set; }

    public int ReportHistId { get; set; }

    public int? ReportId { get; set; }

    public int EmpId { get; set; }

    public int? ReportingTo { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? ResignationId { get; set; }

    public int? NewReportingTo { get; set; }
}
