using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    public class CareerHistoryTransferModelDto
        {
        public int? TransferId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Level1 { get; set; }
        public string Level2 { get; set; }
        public string Level3 { get; set; }
        public string Level4 { get; set; }
        public string Level5 { get; set; }
        public string Level6 { get; set; }
        public string Level7 { get; set; }
        public string Level8 { get; set; }
        public string Level9 { get; set; }
        public string Level10 { get; set; }
        public string Level11 { get; set; }
        }
    }
