using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Admin.Enums;
using DeepBlue.Models.Entity;

namespace DeepBlue.Models.Admin {
	public class ListModel {
		public ListModel(){
			EntityTypes = new List<InvestorEntityType>();
		}
		public ControllerType ControllerType { get; set; }
		
		public List<InvestorEntityType> EntityTypes { get; set; }
	}
}