using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class Dependent00
{
    public int DepId { get; set; }

    public string? Name { get; set; }

    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int? RelationshipId { get; set; }

    public string? Description { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? ModifiedBy { get; set; }

    public int? DepEmpId { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? InterEmpId { get; set; }

    public string? Type { get; set; }

    public bool? TicketEligible { get; set; }

    public int? DocumentId { get; set; }

    public string? IdentificationNo { get; set; }
}
