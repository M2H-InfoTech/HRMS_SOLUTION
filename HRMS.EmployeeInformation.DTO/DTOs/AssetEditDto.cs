using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public  class AssetEditDto
    {

        public int? varEmpID { get; set; }
        public string? AssetGroup { get; set; }
        public string? varAssestName { get; set; }
        public string? AssetNo { get; set; }
        public string? AssetModel { get; set; }
        public string? Monitor { get; set; }
        public DateTime InWarranty { get; set; }
        public DateTime OutOfWarranty { get; set; }
        public DateTime ReceiveDate { get; set; }
        public int? EntryBy { get; set; }
        public DateTime ExpiryDate { get; set; }
      
        public DateTime ReturnDate { get; set; }
        public string? Remarks { get; set; }
        public string? varAssignAsetStatus { get; set; }
        public int? varAssestID { get; set; }

        
    }
}


