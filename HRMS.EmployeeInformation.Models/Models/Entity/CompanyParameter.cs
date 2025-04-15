using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class CompanyParameter
{
    public long? Id { get; set; }

    public int? CompanyId { get; set; }

    public string? ParameterCode { get; set; }

    public string? Description { get; set; }

    public int? Value { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? EntityLevel { get; set; }

    public string? Type { get; set; }

    public int? ControlType { get; set; }

    public string? Data { get; set; }

    public string? FileName { get; set; }

    public string? Text { get; set; }

    public string? MultipleValues { get; set; }
}
