using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class GeneralCategoryField
{
    public int CategoryFieldId { get; set; }

    public string? FieldDescription { get; set; }

    public int? GeneralCategoryId { get; set; }

    public int? DataTypeId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? IsMandatory { get; set; }
}
