using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class EmployeeUploadResultDto
    {
        public string EmployeeCode { get; set; }
        public string IdentificationNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string GuardianName { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string DateOfBirth { get; set; }
        public string JoinDate { get; set; }
        public string JobType { get; set; }
        public string PersonalEmail { get; set; }
        public string OfficialEmail { get; set; }
        public string LevelOneDescription { get; set; }
        public string LevelTwoDescription { get; set; }
        public string LevelThreeDescription { get; set; }
        public string LevelFourDescription { get; set; }
        public string LevelFiveDescription { get; set; }
        public string LevelSixDescription { get; set; }
        public string LevelSevenDescription { get; set; }
        public string LevelEightDescription { get; set; }
        public string LevelNineDescription { get; set; }
        public string LevelTenDescription { get; set; }
        public string LevelElevenDescription { get; set; }
        public string LevelTwelveDescription { get; set; }
        public string ProbationEndDate { get; set; }
        public string EntryBy { get; set; }
        public string EntryDate { get; set; }
        public int? ErrorId { get; set; }
        public string ErrorDescription { get; set; }
    }

}
