using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class CurrencyMaster
{
    public int CurrencyId { get; set; }

    public string? Currency { get; set; }

    public string? CurrencyCode { get; set; }

    public int? CountryId { get; set; }

    public DateTime EntryDate { get; set; }

    public int EntryBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public string IsActive { get; set; } = null!;

    public int? DecimalValue { get; set; }

    public string? Symbol { get; set; }
}
