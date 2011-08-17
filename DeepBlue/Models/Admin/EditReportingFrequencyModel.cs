using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditReportingFrequencyModel{

		public int ReportingFrequencyId { get; set; }

		[Required(ErrorMessage = "Reporting Frequency is required.")]
		[StringLength(100, ErrorMessage = "Reporting Frequency must be under 100 characters.")]
		[DisplayName("Reporting Frequency:")]
		public string ReportingFrequency { get; set; }

		[DisplayName("Enable:")]
		public bool Enabled { get; set; }
 
	}
}