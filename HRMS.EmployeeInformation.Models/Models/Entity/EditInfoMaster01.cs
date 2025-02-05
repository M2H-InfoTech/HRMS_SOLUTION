using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class EditInfoMaster01
{
    public int Info01Id { get; set; }

    public int? InfoId { get; set; }

    public string? InfoCode { get; set; }

    public string? Description { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? TableColumn { get; set; }
}
