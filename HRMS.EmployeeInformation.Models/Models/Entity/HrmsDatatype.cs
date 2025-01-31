using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrmsDatatype
{
    public int TypeId { get; set; }

    public int? DataTypeId { get; set; }

    public string? DataType { get; set; }

    public int? IsDate { get; set; }

    public int? IsGeneralCategory { get; set; }

    public int? IsDropdown { get; set; }
}
