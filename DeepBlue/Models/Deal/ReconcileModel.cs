using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {

	public class ReconcileModel {

		[Required(ErrorMessage = "ReconcileTypeId is required.")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "ReconcileTypeId is required.")]
		public int ReconcileTypeId { get; set; }

		[Required(ErrorMessage = "PaidOn is required.")]
		[DateRange(ErrorMessage = "PaidOn is required.")]
		public DateTime PaidOn { get; set; }

		[Required(ErrorMessage = "Payment Date is required.")]
		[DateRange(ErrorMessage = "Payment Date is required.")]
		public DateTime PaymentDate { get; set; }

		public bool IsReconciled { get; set; }

		public string ChequeNumber { get; set; }

		[Required(ErrorMessage = "Id is required.")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Id is required.")]
		public int Id { get; set; }

	}
}