using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class DeleteEmpDetailRequestDto
    {
        public string TransactionType { get; set; }
        public int DetailId { get; set; }
        public int EmpId { get; set; }
    }

}
