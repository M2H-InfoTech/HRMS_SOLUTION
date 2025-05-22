using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HolidaysMasterApproval
{
    public int Holidaysmasterid { get; set; }

    public int HolidayMasterId { get; set; }

    public DateTime? HolidayFromDate { get; set; }

    public DateTime? HolidayToDate { get; set; }

    public string? HolidayName { get; set; }

    public string? Location { get; set; }

    public int? CurYear { get; set; }

    public int? EmpId { get; set; }

    public string? FlowStatus { get; set; }

    public string? Status { get; set; }

    public string? Remarks { get; set; }

    public string? SequenceId { get; set; }

    public int? HolidayCount { get; set; }

    public string? EntryFrom { get; set; }

    public DateTime? EntryDate { get; set; }
}
