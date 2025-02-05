using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class Geotagging01
{
    public int GeoEntityId { get; set; }

    public int? LinkId { get; set; }

    public int? LevelId { get; set; }

    public int? Geotype { get; set; }

    public int? LiveTracking { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
