using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {

	public class EquityDocumentModel {

		public int EquityDocumentTypeId { get; set; }

		public DateTime EquityDocumentDate { get; set; }

		public int EquityUploadTypeId { get; set; }

		public string EquityFilePath { get; set; }

	}
}