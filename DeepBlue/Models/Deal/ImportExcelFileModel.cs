using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Deal {
	public class ImportExcelFileModel {

		public string FileName { get; set; }

		public string FilePath { get; set; }
		
		public HttpPostedFileBase UploadFile { get; set; }
	
	}
}