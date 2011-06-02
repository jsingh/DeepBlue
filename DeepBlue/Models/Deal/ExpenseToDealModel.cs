using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Deal {
	public class ExpenseToDealModel {

		public int DealId { get; set; }

		public string DealName { get; set; }

		public int DealNo { get; set; }

		public decimal Amount { get; set; }
	}
}