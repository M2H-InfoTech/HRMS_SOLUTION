using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class CoordinateDto
    {
        public int GeoLocationId { get; set; }
        public int? GeoBatchId01 { get; set; }
        public string Location { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Radius { get; set; }
        public int? Status01 { get; set; }
        public DateTime? EntryDate { get; set; }
        public int ?EntryBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }

        // From Geolocation00
        public int GeoBatchId00 { get; set; }
        public string GeoBatchDescription { get; set; }
        public int? Status00 { get; set; }

    }
}
