using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class Odcancel00
{
    public int OdcancelId { get; set; }

    public int? OdApplicationId { get; set; }

    public DateTime? OdFromdate { get; set; }

    public DateTime? OdTodate { get; set; }

    public decimal? OdDays { get; set; }

    public int? OdDaytype { get; set; }

    public int? OdFirsthalf { get; set; }

    public int? OdLasthalf { get; set; }

    public int? OdFullLeave { get; set; }

    public string? OdStatus { get; set; }

    public string? OdRemark { get; set; }

    public int? OdproxyId { get; set; }

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
