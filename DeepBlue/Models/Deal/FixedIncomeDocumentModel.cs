using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {

	public class FixedIncomeDocumentModel {

		public int FixedIncomeDocumentTypeId { get; set; }

		public DateTime FixedIncomeDocumentDate { get; set; }

		public int FixedIncomeUploadTypeId { get; set; }

		public string FixedIncomeFilePath { get; set; }

	}
}