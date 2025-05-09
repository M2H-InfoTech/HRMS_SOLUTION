using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;
public partial class LeavePolicyHolidayInclude
{
    public int HolidayIncludeId { get; set; }

    public int? LeavePolicyInstanceLimitId { get; set; }

    public int? Holidaystatus { get; set; }

    public decimal? Fromdays { get; set; }

    public decimal? Todays { get; set; }

    public int? Leavetype { get; set; }

    public int? Createdby { get; set; }

    public DateTime CreatedDate { get; set; }

    public decimal? LeaveDays { get; set; }

    public int? OffdaysIncExc { get; set; }
}
