using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class KpiSubcategory
{
    public int KpisubcategoryId { get; set; }

    public string? Goal { get; set; }

    public string? Subcategory { get; set; }

    public int? Entryby { get; set; }

    public DateTime? Entrydate { get; set; }

    public int? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public int? GoalTypeId { get; set; }

    public string? IsRatingEnable { get; set; }

    public int? RatingTypeId { get; set; }

    public int? Uniqueid { get; set; }

    public int? Masterid { get; set; }
}
