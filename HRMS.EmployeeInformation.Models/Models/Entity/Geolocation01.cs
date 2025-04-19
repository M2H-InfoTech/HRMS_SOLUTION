using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class Geolocation01
{
    public int GeoLocationId { get; set; }

    public int? GeoBatchId { get; set; }

    public string? Location { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public string? Radius { get; set; }

    public int? Status { get; set; }
}
