using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class AdmCountryMaster
{
    public int InstId { get; set; }

    public int CountryId { get; set; }

    public string? CountryName { get; set; }

    public string? CountryLocName { get; set; }

    public string Nationality { get; set; } = null!;

    public string? NationaltyLocName { get; set; }

    public int EntryBy { get; set; }

    public DateTime EntryDate { get; set; }

    public int ModiBy { get; set; }

    public DateTime ModiDate { get; set; }
}
