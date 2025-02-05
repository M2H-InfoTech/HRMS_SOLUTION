using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class Geotagging00A
{
    public int GeoCompAid { get; set; }

    public int? GeoCompId { get; set; }

    public int? GeoCriteria { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public string? Radius { get; set; }

    public int? LocationId { get; set; }
}
