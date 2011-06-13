using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(DealMD))]
	public partial class Deal {
		public class DealMD : CreatedByFields {
			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "FundID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "FundID is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required(ErrorMessage = "DealNumber is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "DealNumber is required")]
			public global::System.Int32 DealNumber {
				get;
				set;
			}

			[Required(ErrorMessage = "PurchaseTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "PurchaseTypeID is required")]
			public global::System.Int32 PurchaseTypeID {
				get;
				set;
			}
 

			[Required(ErrorMessage = "Deal Name is required")]
			[StringLength(50, ErrorMessage = "Deal Name must be under 50 characters")]
			public global::System.String DealName {
				get;
				set;
			}
 
			#endregion
		}

		public Deal(IDealService dealService)
			: this() {
			this.DealService = dealService;
		}

		public Deal() {
		}

		private IDealService _dealService;
		public IDealService DealService {
			get {
				if (_dealService == null) {
					_dealService = new DealService();
				}
				return _dealService;
			}
			set {
				_dealService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var deal = this;
			IEnumerable<ErrorInfo> errors = Validate(deal);
			if (errors.Any()) {
				return errors;
			}
			DealService.SaveDeal(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(Deal deal) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(deal);
			if (deal.Partner != null) {
				errors = errors.Union(ValidationHelper.Validate(deal.Partner));
			}
			if (deal.Contact != null) {
				errors = errors.Union(ValidationHelper.Validate(deal.Contact));
				foreach (ContactCommunication comm in deal.Contact.ContactCommunications) {
					errors = errors.Union(ValidationHelper.Validate(comm.Communication));
				}
			}
			if (deal.Contact1 != null) {
				errors = errors.Union(ValidationHelper.Validate(deal.Contact1));
				foreach (ContactCommunication comm in deal.Contact1.ContactCommunications) {
					errors = errors.Union(ValidationHelper.Validate(comm.Communication));
				}
			}
			return errors;
		}
	}
}