using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditReportingTypeModel{

		public int ReportingTypeId { get; set; }
		
		[Required(ErrorMessage = "Reporting Type is required.")]
		[StringLength(100, ErrorMessage = "Reporting Type must be under 100 characters.")]
		[DisplayName("Reporting Type:")]
		public string Reporting { get; set; }

		[DisplayName("Enable:")]
		public bool Enabled { get; set; }
 
	}
}