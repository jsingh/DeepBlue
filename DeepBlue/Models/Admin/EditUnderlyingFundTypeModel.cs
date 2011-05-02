﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditUnderlyingFundTypeModel{
		public EditUnderlyingFundTypeModel() {
		}

		public int UnderlyingFundTypeID { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		[StringLength(50, ErrorMessage = "Name must be under 50 characters.")]
		[RemoteUID_(Action = "UnderlyingFundTextAvailable", Controller = "Admin", ValidateParameterName = "Name", Params = new string[] { "UnderlyingFundTypeID" })]
		[DisplayName("Name:")]
		public string Name { get; set; }
	}
}