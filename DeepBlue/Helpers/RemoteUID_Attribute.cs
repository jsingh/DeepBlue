using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Helpers {
	public sealed class RemoteUID_Attribute : ValidationAttribute {
		
		public RemoteUID_Attribute(){
			Action = string.Empty;
			Controller = string.Empty;
			ValidateParameterName = string.Empty;
			Params = new string[] { };
			RouteName = string.Empty;
		}
		
		public string Action { get; set; }
		public string Controller { get; set; }
		public string ValidateParameterName { get; set; }
		public string[] Params { get; set; }
		public string RouteName { get; set; }

		public override bool IsValid(object value) {
			return true;
		}
	} 
}