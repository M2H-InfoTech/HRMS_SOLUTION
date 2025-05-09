using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class Leavecancel00
{
    public int LeavecancelId { get; set; }

    public int? LeaveApplicationId { get; set; }

    public DateTime? Lcfromdate { get; set; }

    public DateTime? Lctodate { get; set; }

    public decimal? Lcdays { get; set; }

    public int? Lcdaytype { get; set; }

    public int? Lcfirsthalf { get; set; }

    public int? Lclasthalf { get; set; }

    public int? LcfullLeave { get; set; }

    public string? Lcstatus { get; set; }

    public string? Lcremark { get; set; }

    public int? LcproxyId { get; set; }

    public string? RequestCode { get; set; }

    public string? Cancelflowstatus { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? EntryFrom { get; set; }

    public string? UpdatedFrom { get; set; }

    public int? EmployeeId { get; set; }

    public int? IsDirestPosting { get; set; }

    public DateTime? FinalApprovalDate { get; set; }
}
