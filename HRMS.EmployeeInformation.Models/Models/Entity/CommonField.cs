using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class CommonField
{
    public long ComFieldId { get; set; }

    public string? ComDescription { get; set; }

    public int? ComDataTypeId { get; set; }

    public bool? IsMandatoryField { get; set; }

    public int? ComId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
