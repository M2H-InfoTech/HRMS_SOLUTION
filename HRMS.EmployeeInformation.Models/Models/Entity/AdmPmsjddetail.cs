using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class AdmPmsjddetail
{
    public long Pmsjdid { get; set; }

    public string? JdrequestId { get; set; }

    public int? EmpId { get; set; }

    public int? DesignId { get; set; }

    public string? Designation { get; set; }

    public int? PmsempId { get; set; }

    public int? AttachmentCount { get; set; }

    public string? RequesterRemark { get; set; }

    public string? ApproverRemark { get; set; }

    public string? Jdtype { get; set; }

    public string? Jdstatus { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public bool? Active { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? Level1Id { get; set; }

    public int? Level2Id { get; set; }

    public string? GroupCompany { get; set; }

    public string? BusiVertical { get; set; }
}
