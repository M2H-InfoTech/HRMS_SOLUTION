using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class SaveGeoLocationRequestDTO
    {
        public int? EmpID { get; set; }
        public int GeoSpacingType { get; set; }
        public int LiveTracking { get; set; }
        public int EntryBy { get; set; }
        public List<GeoSpacingItemDTO> GeoSpacings { get; set; }
    }
}
