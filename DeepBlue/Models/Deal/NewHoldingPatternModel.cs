using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Deal {
	public class NewHoldingPatternModel {

		public int FundId { get; set; }

		public string FundName { get; set; }

		public int OldNoOfShares { get; set; }

		public int SplitFactor { get; set; }

		public int NewNoOfShares {
			get {
				return (OldNoOfShares * SplitFactor);
			}
		}
	}
}