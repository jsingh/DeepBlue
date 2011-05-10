using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Issuer {
	public class IssuerListModel {

		public int IssuerId { get; set; }

		public string Name { get; set; }

		public string ParentName { get; set; }

		public string Country { get; set; }
	}
}