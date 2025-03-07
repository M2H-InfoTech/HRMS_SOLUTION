using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.EmployeeInformation.Models;

public partial class ReasonMasterFieldValue
{
    
    [Key]
    public int ReasonFieldId { get; set; }

    public int? ReasonId { get; set; }

    public int? CategoryFieldId { get; set; }

    public string? FieldValues { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }
}
