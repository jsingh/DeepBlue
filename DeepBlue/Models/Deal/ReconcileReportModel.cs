using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Deal {
	public class ReconcileReportModel {

		public string CounterParty { get; set; }

		public string FundName { get; set; }

		public string Type { get; set; }

		public int ReconcileTypeId { get; set; }

		public decimal? Amount { get; set; }

		public DateTime? PaymentDate { get; set; }

		public DateTime? PaidOn { get; set; }

		public bool? IsReconciled { get; set; }

		public string ChequeNumber { get; set; }

		public int id { get; set; }

		public int ParentId { get; set; }

	}
}