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

		[Required(ErrorMessage = "Name is required")]
		[StringLength(20, ErrorMessage = "Name must be under 20 characters.")]
		[RemoteUID_(Action = "CommunicationGroupingNameAvailable", Controller = "Admin", ValidateParameterName = "CommunicationGroupingName", Params = new string[] { "CommunicationGroupingId" })]
		[DisplayName("Name:")]
		public string CommunicationGroupingName { get; set; }

	}
}