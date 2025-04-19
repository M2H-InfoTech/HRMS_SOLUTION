using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public  class GeocoordinatesDto
    {
        public int GeoMasterId { get; set; }
        public int ?GeoCriteria { get; set; }
        public int ?GeoSpaceType { get; set; }
        public string ?CoordinateName { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Radius { get; set; }
        public int? LocationId { get; set; }
        public int? EntryBy { get; set; }
        public int? Status { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
