using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class CompanyParameters01
{
    public int Id { get; set; }

    public int? ParamId { get; set; }

    public int? Value { get; set; }

    public long? LinkId { get; set; }

    public long? LevelId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Data { get; set; }

    public string? FileName { get; set; }

    public string? Text { get; set; }

    public string? MultipleValues { get; set; }
}
