using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class CategoryGroup
{
    public int CatId { get; set; }

    public int? GroupId { get; set; }

    public string? DescriptionGrp { get; set; }
}
