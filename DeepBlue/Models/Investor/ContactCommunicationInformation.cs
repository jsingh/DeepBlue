using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Investor {
	public class ContactCommunicationInformation {

		public int? ContactCommunicationId { get; set; }

		public int CommunicationTypeId { get; set; }

		public string CommunicationValue { get; set; }

	}
}