using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using DeepBlue.Models.Entity;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditCommunicationGroupingModel {
		public EditCommunicationGroupingModel() {
			CommunicationGroupingId = 0;
			CommunicationGroupingName = string.Empty;
		}

		public int CommunicationGroupingId { get; set; }

		[Required(ErrorMessage = "Communication Group is required")]
		[StringLength(20, ErrorMessage = "Communication Group must be under 20 characters.")]
		[DisplayName("Name")]
		public string CommunicationGroupingName { get; set; }

	}
}