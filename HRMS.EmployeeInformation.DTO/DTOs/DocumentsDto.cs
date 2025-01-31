using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class DocumentsDto
    {

        public IEnumerable<DocumentsTableDto>? DocumentsTable { get; set; }
        public IEnumerable<DocumentsTable1Dto>? DocumentsTable1 { get; set; }
        public IEnumerable<DocumentsTable2Dto>? DocumentsTable2 { get; set; }
        public IEnumerable<DocumentsTable3Dto>? DocumentsTable3 { get; set; }
        public IEnumerable<DocumentsTable4Dto>? DocumentsTable4 { get; set; }
    }
 
        public class DocumentsTableDto
        {
			public int? DetailID { get; set; }
			public int? DocID { get; set; }
			public int? EmpID { get; set; }
			public int? DocFieldID { get; set; }
			public string? DocDescription { get; set; }
			public string? DocValues { get; set; }
			public int? IsGeneralCategory { get; set; }
			public string? DataType { get; set; }
			public	string? DocName {  get; set; }
			public int? IsDate {  get; set; }
			public string? FieldValues { get; set; }
			public string? FieldDescription {  get; set; }
			public int? repeatrank {  get; set; }


        }

		public class DocumentsTable1Dto
		{
            public int? DetailID { get; set; }
            public int? DocID { get; set; }
            public int? EmpID { get; set; }
            public int? DocFieldID { get; set; }
            public string? DocDescription { get; set; }
            public string? DocValues { get; set; }
            public int? IsGeneralCategory { get; set; }
            public string? DataType { get; set; }
            public string? DocName { get; set; }
            public int? IsDate { get; set; }
            public string? FieldValues { get; set; }
            public string? FieldDescription { get; set; }
            public int? repeatrank { get; set; }


        }

    public class DocumentsTable2Dto 
    { 
        public int? DocID { get; set; }
        public string? DocName { get; set; }
        public string? DocDescription { get; set; }
    }

    public class DocumentsTable3Dto
    {
        public int DocID { get; set; }
        public int? DetailID { get; set; }
        public string? FileName { get; set; }
        public string? Status { get; set; }
    }
    public class DocumentsTable4Dto
    {
        public int DocID { get; set; }
        public int? DetailID { get; set; }
        public string? FileName { get; set; }
        public string? Status { get; set; }
    }
}
 
	