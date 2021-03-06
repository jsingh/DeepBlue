﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(DealUnderlyingFundMD))]
	public partial class DealUnderlyingFund {
		public class DealUnderlyingFundMD {
			#region Primitive Properties
			[Required(ErrorMessage = "UnderlyingFundID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "UnderlyingFundID is required")]
			public global::System.Int32 UnderlyingFundID {
				get;
				set;
			}

			[Required(ErrorMessage = "Deal is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
			public global::System.Int32 DealID {
				get;
				set;
			}

			[DateRange(ErrorMessage = "RecordDate is required")]
			public Nullable<global::System.DateTime> RecordDate {
				get;
				set;
			}

            [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "CommittedAmount is required")]
			public global::System.Decimal CommittedAmount {
				get;
				set;
			}

			#endregion

		}

		public   int? TraceID {
			get {
				return this.DealUnderlyingtFundID;
			}
		}
		public   decimal? Amount {
			get {
				return this.CommittedAmount;
			}
		}
		public   int? AttributedTo {
			get {
				return this.UnderlyingFundID;
			}
		}
		public   string AttributedToName {
			get {
				UnderlyingFund uf = this.UnderlyingFund;
				if (uf == null) {
					DeepBlueEntities context = new DeepBlueEntities();
					uf = context.UnderlyingFunds.Where(x => x.UnderlyingtFundID == this.UnderlyingFundID).FirstOrDefault();
				}
				return uf.FundName;
			}
		}
		public   string AttributedToType {
			get {
				return "UnderlyingFund";
			}
		}

		public DealUnderlyingFund(IDealUnderlyingFundService dealUnderlyingFundService)
			: this() {
			this.DealUnderlyingFundService = dealUnderlyingFundService;
		}

		public DealUnderlyingFund() {
		}

		private IDealUnderlyingFundService _DealUnderlyingFundService;
		public IDealUnderlyingFundService DealUnderlyingFundService {
			get {
				if (_DealUnderlyingFundService == null) {
					_DealUnderlyingFundService = new DealUnderlyingFundService();
				}
				return _DealUnderlyingFundService;
			}
			set {
				_DealUnderlyingFundService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			DealUnderlyingFundService.SaveDealUnderlyingFund(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DealUnderlyingFund dealUnderlyingFund) {
			return ValidationHelper.Validate(dealUnderlyingFund);
		}
	}
}