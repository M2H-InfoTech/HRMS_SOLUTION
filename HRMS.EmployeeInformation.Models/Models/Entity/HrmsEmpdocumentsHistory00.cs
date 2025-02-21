using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.Models.Models.Entity;
public partial class HrmsEmpdocumentsHistory00
{
    public int DocHistId { get; set; }

    public int DocApprovedId { get; set; }

    public int? DetailId { get; set; }

    public int? DocId { get; set; }

    public int? EmpId { get; set; }

    public string? Status { get; set; }

    public string? DocStatus { get; set; }

    public string? RequestId { get; set; }

    public DateTime? DateFrom { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}

