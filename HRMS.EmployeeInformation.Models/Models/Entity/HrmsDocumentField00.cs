using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class HrmsDocumentField00
{
    public long DocFieldId { get; set; }

    public string? DocDescription { get; set; }

    public int? DataTypeId { get; set; }

    public int? DocId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? IsMandatory { get; set; }
}
