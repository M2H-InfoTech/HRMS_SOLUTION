using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO
{
    public class DocumentsDownoaldDto
    {
        public int DocID { get; set; }
        public int DetailID { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileData { get; set; }
        public string FolderName { get; set; }

    }
}
