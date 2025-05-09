using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class LeaveApplication02
{
    public int LeaveApp02Id { get; set; }

    public int? LeaveApplicationId { get; set; }

    public int? LeaveId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? LeaveDate { get; set; }

    public decimal? Dayscount { get; set; }

    public int? Timemode { get; set; }

    public string? Cancelstatus { get; set; }

    public string? LeaveStatus { get; set; }

    public decimal? Canceldays { get; set; }

    public int? RemainingTimemodeaftercancel { get; set; }

    public int? ForLeaveancelId { get; set; }

    public long? Processpayrollid { get; set; }

    public int? ReconsiderinPayroll { get; set; }

    public int? ReconsiderPayperiod { get; set; }

    public int? ReconsiderPayscale { get; set; }

    public int? ProcesspayrollIdleaveIncluded { get; set; }
}
