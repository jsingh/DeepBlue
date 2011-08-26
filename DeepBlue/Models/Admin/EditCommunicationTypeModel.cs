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
	public class EditCommunicationTypeModel {
		public EditCommunicationTypeModel() {
			CommunicationTypeId = 0;
			CommunicationTypeName = string.Empty;
			Enabled = false;
		}

		public int CommunicationTypeId { get; set; }

		[Required(ErrorMessage = "Communication Type is required")]
		[StringLength(20, ErrorMessage = "Communication Type must be under 20 characters.")]
		[DisplayName("Name")]
		public string CommunicationTypeName { get; set; }

		[DisplayName("Enable")]
		public bool Enabled { get; set; }

		[Required(ErrorMessage = "Communication Group is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Communication Group is required")]
		[DisplayName("Communication Group")]
		public int CommunicationGroupId { get; set; }

		public List<SelectListItem> CommunicationGroupings { get; set; }

	}
}