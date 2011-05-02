using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditShareClassTypeModel{
		public EditShareClassTypeModel() {
		}

		public int ShareClassTypeID { get; set; }

		//[Required(ErrorMessage = "Entity Id is required.")]
		//[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Entity Id is required")]
		//[DisplayName("EntityID:")]
		public int EntityID { get; set; }

		[Required(ErrorMessage = "ShareClass is required.")]
		[StringLength(100, ErrorMessage = "ShareClass must be under 100 characters.")]
		[RemoteUID_(Action = "ShareClassTextAvailable", Controller = "Admin", ValidateParameterName = "ShareClass", Params = new string[] { "ShareClassTypeID" })]
		[DisplayName("ShareClass:")]
		public string ShareClass { get; set; }

		[DisplayName("Enable:")]
		public bool Enabled { get; set; }

		//[Required(ErrorMessage = "Created Date is required.")]
		//[DisplayName("Created Date:")]
		//[DateRange()]
		public DateTime CreatedDate { get; set; }

		//[Required(ErrorMessage = "Created By is required.")]
		//[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Created By is required.")]
		//[DisplayName("Created By:")]
		public int CreatedBy { get; set; }


		//[Required(ErrorMessage = "Last Updated Date is required.")]
		//[DisplayName("Lasted Updated Date:")]
		//[DateRange()]
		public DateTime LastUpdatedDate { get; set; }

		//[Required(ErrorMessage = "Last Updated By is required.")]
		//[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Last Updated By is required.")]
		//[DisplayName("Last Updated By:")]
		public int LastUpdatedBy { get; set; }
	}
}