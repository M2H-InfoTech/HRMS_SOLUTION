using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class GeneralCategory
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public int? DataTypeId { get; set; }

    public int? SubLinkId { get; set; }

    public int? GroupId { get; set; }

    public int? EmplinkId { get; set; }

    public int? GeneralAssetRole { get; set; }

    public int? GeneralIsActive { get; set; }

    public int? MasterTypeId { get; set; }
}
