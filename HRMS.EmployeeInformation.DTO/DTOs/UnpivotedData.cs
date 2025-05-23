using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class UnpivotedData
    {
        public string EmployeeCode {  get; set; }
        public int DocNewId { get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
        public int DocID { get; set; }
    }

}
