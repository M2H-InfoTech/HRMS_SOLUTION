using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class GeoSpacingDto
    {
        public int? GeoEmpId { get; set; }
        public int? GeoEmpAid { get; set; }
        public int? EmpId { get; set; }
        public int? LevelId { get; set; }
        public int? Geotype { get; set; }
        public string? GeotypeCode { get; set; }
        public string? GeotypeDescription { get; set; }
        public int? GeoCriteria { get; set; }
        public string? GeoCriteriaCode { get; set; }
        public string? GeoCriteriaDescription { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Radius { get; set; }
        public int? LiveTracking { get; set; }
        public int? LocationId { get; set; }
        public string? GeoCoordinates { get; set; }
    }
}
