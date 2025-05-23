using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class TempdependentDto
    {
        public string DependentName { get; set; }
        public string Gender { get; set; }
        [JsonProperty("DateOfBirth(MM/DD/YYYY)")]
        public DateTime DateOfBirth { get; set; }
        public string Relationship { get; set; }
        public string Description { get; set; }
        public string EmployeeCode { get; set; }
        public string IdentificationNo { get; set; }
    }

}
