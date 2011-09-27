using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace DeepBlue.Models.Deal {
	
	public class UnderlyingFundContactList {

		public int UnderlyingFundContactId { get; set; }
		
		public int? UnderlyingFundId { get; set; }

		public int ContactId { get; set; }

		public string ContactName { get; set; }

		public string WebAddress { get; set; }

		public string Address { get; set; }

		public string Phone { get; set; }

		public string Email { get; set; }

		public string ContactTitle { get; set; }

		public string ContactNotes { get; set; }

		public string WebUsername { get; set; }

		public string WebPassword { get; set; }

		public bool ChangeWebPassword { get; set; }
	}
}