using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace DeepBlue.Models.Deal {
	
	public class DealFundDocumentList {

		public int DealFundDocumentId { get; set; }

		public string DocumentType { get; set; }

		public DateTime? DocumentDate { get; set; }

		public string FilePath { get; set; }

		public string FileName { get; set; }

		public string FileTypeName { get; set; }

		public string FundName { get; set; }

		public string InvestorName { get; set; }

		public string FullPath {
			get {
				return Path.Combine(this.FilePath, this.FileName);
			}
		}
	}
}