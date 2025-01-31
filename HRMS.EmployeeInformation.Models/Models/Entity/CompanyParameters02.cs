using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class CompanyParameters02
{
    public int Id { get; set; }

    public int? ParamId { get; set; }

    public int? EmpId { get; set; }

    public int? Value { get; set; }

    public string? Data { get; set; }

    public string? FileName { get; set; }

    public string? Text { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? MultipleValues { get; set; }
}
