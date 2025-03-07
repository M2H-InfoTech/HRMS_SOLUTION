using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMPLOYEE_INFORMATION.Models;

public partial class ReasonMaster
{
    
   [Key]
    public int ReasonId { get; set; }

    public string? Type { get; set; }

    public string? Description { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? EntryBy { get; set; }

    public int? Value { get; set; }

    public int? Others { get; set; }

    public bool? IsMultiAsset { get; set; }

    public int? AssetRoleId { get; set; }

    public string? Status { get; set; }

    public int? DivMasterId { get; set; }

    public int? SubCatLinkId { get; set; }

    public int? AssetSpecEmpId { get; set; }

    public int? DisableInEss { get; set; }

    public int? SeperationType { get; set; }

    public int? WpsSubFormat { get; set; }
}
