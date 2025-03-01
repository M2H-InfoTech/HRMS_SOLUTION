using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class SaveCommunicationSDto
    {

        public int EmpID { get; set; }
        public string address1 { get; set; }
        public string PostboxNo { get; set; }
        public int countryID { get; set; }
        public string address2 { get; set; }
        public string PostboxNo2 { get; set; }
        public int countryID2 { get; set; }
        public string mobileNo { get; set; }
        public string ContactNo { get; set; }
        public string Status { get; set; }
        public string OfficeNo { get; set; }
        public string FlowStatus { get; set; }
        public int DetailID { get; set; }
        [Key]
        public int AddrID { get; set; }
        public int Entry_By { get; set; }
        public string UpdateType { get; set; }
        public string Extension { get; set; }
        public int InstId { get; set; }
        public DateTime EntryDt { get; set; }

        public string Emername { get; set; }
        public string RelationId { get; set; }
        public string FilterType { get; set; }
        public string EmailField { get; set; }
        public string PersonalEmail { get; set; }
    }
}

