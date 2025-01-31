using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class EmpDocumentAccess
{
    public int DocAccessId { get; set; }

    public int? InstId { get; set; }

    public int? EmpId { get; set; }

    public int? DocId { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public int? IsCompanyLevel { get; set; }

    public int? Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }
}
