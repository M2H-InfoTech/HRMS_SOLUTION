using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class TrainingDto
    {


        public int Emp_Id { get; set; }
        public int trMasterId { get; set; }
        public string? Emp_Code { get; set; }
        public string ?trName { get; set; }
        public string? FileUrl { get; set; }
        public int? FileUpdId { get; set; }
        public string? EmpName { get; set; }
        public bool? IsSurvey { get; set; }
        public string ?Survey { get; set; }

        public DateTime ?Join_Dt { get; set; }

        public string ?selectStatus { get; set; }
        public string? FileName { get; set; }
        public string ?AttDate { get; set; }

        public string? IsAttended { get; set; }



    }

}
