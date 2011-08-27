using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue.Models.Entity.Partial;

namespace DeepBlue.Models.Entity {

	[MetadataType(typeof(InvestorContactMD))]
	public partial class InvestorContact {
		public class InvestorContactMD {

			#region Primitive Properties

			[Required(ErrorMessage = "InvestorID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "InvestorID is required")]
			public global::System.Int32 InvestorID {
				get;
				set;
			}


			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}


			[DateRange(ErrorMessage = "CreatedDate is required")]
			public Nullable<global::System.DateTime> CreatedDate {
				get;
				set;
			}


			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CreatedBy is required")]
			public Nullable<global::System.Int32> CreatedBy {
				get;
				set;
			}


			[DateRange(ErrorMessage = "LastUpdatedDate is required")]
			public Nullable<global::System.DateTime> LastUpdatedDate {
				get;
				set;
			}


			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "LastUpdatedBy is required")]
			public Nullable<global::System.Int32> LastUpdatedBy {
				get;
				set;
			}

			#endregion

		}

		public InvestorContact(IInvestorContactService investoraddressService)
			: this() {
			this.InvestorContactService = InvestorContactService;
		}

		public InvestorContact() {
		}

		private IInvestorContactService _InvestorContactService;
		public IInvestorContactService InvestorContactService {
			get {
				if (_InvestorContactService == null) {
					_InvestorContactService = new InvestorContactService();
				}
				return _InvestorContactService;
			}
			set {
				_InvestorContactService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			errors = errors.Union(ValidationHelper.Validate(this.Contact));
			foreach (ContactAddress contactAddr in this.Contact.ContactAddresses) {
				errors = errors.Union(ValidationHelper.Validate(contactAddr.Address));
			}
			foreach (ContactCommunication comm in this.Contact.ContactCommunications) {
				errors = errors.Union(ValidationHelper.Validate(comm.Communication));
			}
			if (errors.Any()) {
				return errors;
			}
			InvestorContactService.SaveInvestorContact(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(InvestorContact investoraddress) {
			return ValidationHelper.Validate(investoraddress);
		}
	}
}