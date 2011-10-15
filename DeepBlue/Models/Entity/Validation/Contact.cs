using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(ContactMD))]
	public partial class Contact {
		public class ContactMD {

			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[StringLength(100, ErrorMessage = "ContactName must be under 100 characters.")]
			public global::System.String ContactName {
				get;
				set;
			}

			[StringLength(20, ErrorMessage = "Designation must be under 20 characters.")]
			public global::System.String Designation {
				get;
				set;
			}

			[Required(ErrorMessage = "LastName is required")]
			[StringLength(30, ErrorMessage = "LastName must be under 30 characters.")]
			public global::System.String LastName {
				get;
				set;
			}

			[StringLength(30, ErrorMessage = "FirstName must be under 30 characters.")]
			public global::System.String FirstName {
				get;
				set;
			}

			[StringLength(30, ErrorMessage = "MiddleName must be under 30 characters.")]
			public global::System.String MiddleName {
				get;
				set;
			}

			[Required(ErrorMessage = "ReceivesDistributionNotices is required")]
			public global::System.Boolean ReceivesDistributionNotices {
				get;
				set;
			}

			[Required(ErrorMessage = "ReceivesK1 is required")]
			public global::System.Boolean ReceivesK1 {
				get;
				set;
			}

			[Required(ErrorMessage = "ReceivesFinancials is required")]
			public global::System.Boolean ReceivesFinancials {
				get;
				set;
			}

			[Required(ErrorMessage = "ReceivesInvestorLetters is required")]
			public global::System.Boolean ReceivesInvestorLetters {
				get;
				set;
			}

			[Required(ErrorMessage = "CreatedDate is required")]
			[DateRange(ErrorMessage = "CreatedDate is required")]
			public global::System.DateTime CreatedDate {
				get;
				set;
			}

			[Required(ErrorMessage = "CreatedBy is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CreatedBy is required")]
			public global::System.Int32 CreatedBy {
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

			[StringLength(100, ErrorMessage = "ContactCompany must be under 100 characters.")]
			public global::System.String ContactCompany {
				get;
				set;
			}

			[StringLength(100, ErrorMessage = "Title must be under 100 characters.")]
			public global::System.String Title {
				get;
				set;
			}

			[StringLength(500, ErrorMessage = "Notes must be under 500 characters.")]
			public global::System.String Notes {
				get;
				set;
			}

			#endregion
		}
		public Contact(IContactService contactService)
			: this() {
			this.ContactService = contactService;
		}

		public Contact() {
		}

		private IContactService _ContactService;
		public IContactService ContactService {
			get {
				if (_ContactService == null) {
					_ContactService = new ContactService();
				}
				return _ContactService;
			}
			set {
				_ContactService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			ContactService.SaveContact(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(Contact contact) {
			return ValidationHelper.Validate(contact);
		}
	}
}