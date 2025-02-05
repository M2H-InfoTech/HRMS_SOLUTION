using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class Geotagging02A
{
    public int GeoEmpAid { get; set; }

    public int? GeoEmpId { get; set; }

    public int? GeoCriteria { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public string? Radius { get; set; }

    public int? LocationId { get; set; }

    public string? Coordinates { get; set; }
}
