using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class AssignedAsset
{
    public long AssignId { get; set; }

    public string? AssestRequestId { get; set; }

    public int? EmpId { get; set; }

    public string? AssestTypeId { get; set; }

    public string? AssestId { get; set; }

    public string? AssestType { get; set; }

    public string? Employee { get; set; }

    public string? Assest { get; set; }

    public string? IssuedDate { get; set; }

    public DateTime? ReturnedDate { get; set; }

    public int? ReturnedTo { get; set; }

    public string? RequestRemark { get; set; }

    public string? ProxyRemark { get; set; }

    public string? Status { get; set; }

    public bool? Active { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? IsUpload { get; set; }
}
