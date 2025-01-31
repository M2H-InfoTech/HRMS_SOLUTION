using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class QualificationAttachment
{
    public int QualAttachId { get; set; }

    public int? QualificationId { get; set; }

    public string? QualFileName { get; set; }

    public string? FileType { get; set; }

    public string? FilePath { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public string? DocStatus { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? EmpId { get; set; }
}
