using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class GeoSpacingItemDTO
    {
        public int GeoCriteria { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Radius { get; set; }
        public int LocationId { get; set; }
        public string Coordinates { get; set; }
    }
}
