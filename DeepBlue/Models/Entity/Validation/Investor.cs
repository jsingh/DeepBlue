using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(InvestorMD))]
	public partial class Investor {
		public class InvestorMD {
			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Social is required")]
			[StringLength(25, ErrorMessage = "Social must be under 25 characters.")]
			public global::System.String Social {
				get;
				set;
			}

			[Required(ErrorMessage = "Investor Name is required")]
			[StringLength(100, ErrorMessage = "Investor Name must be under 100 characters.")]
			public global::System.String InvestorName {
				get;
				set;
			}

			[Required(ErrorMessage = "Investor Entity Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Investor Entity Type is required")]
			public global::System.Int32 InvestorEntityTypeID {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "Alias must be under 50 characters.")]
			public global::System.String Alias {
				get;
				set;
			}

			[Range(0, int.MaxValue, ErrorMessage = "Prev Investor is required")]
			public Nullable<global::System.Int32> PrevInvestorID {
				get;
				set;
			}

			[StringLength(40, ErrorMessage = "Manager Name must be under 40 characters.")]
			public global::System.String ManagerName {
				get;
				set;
			}

			[Required]
			[StringLength(30, ErrorMessage = "Last Name must be under 30 characters.")]
			public global::System.String LastName {
				get;
				set;
			}

			[StringLength(30, ErrorMessage = "First Name must be under 30 characters.")]
			public global::System.String FirstName {
				get;
				set;
			}

			[StringLength(30, ErrorMessage = "Middle Name must be under 30 characters.")]
			public global::System.String MiddleName {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Residency State is required")]
			public Nullable<global::System.Int32> ResidencyState {
				get;
				set;
			}

			[StringLength(500, ErrorMessage = "Notes must be under 500 characters.")]
			public global::System.String Notes {
				get;
				set;
			}


			[Required(ErrorMessage = "Created Date is required")]
			[DateRange(ErrorMessage = "Created Date is required")]
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
			#endregion
		}

		public Investor(IInvestorService investorService)
			: this() {
			this.InvestorService = investorService;
		}

		public Investor() {
		}
		private IInvestorService _investorService;
		public IInvestorService InvestorService {
			get {
				if (_investorService == null) {
					_investorService = new InvestorService();
				}
				return _investorService;
			}
			set {
				_investorService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var investor = this;
			IEnumerable<ErrorInfo> errors = Validate(investor);
			if (errors.Any()) {
				return errors;
			}
			InvestorService.SaveInvestor(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(Investor investor) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(investor);
			foreach (InvestorAddress address in investor.InvestorAddresses) {
				errors = errors.Union(ValidationHelper.Validate(address.Address));
			}
			foreach (InvestorCommunication comm in investor.InvestorCommunications) {
				errors = errors.Union(ValidationHelper.Validate(comm.Communication));
			}
			foreach (InvestorAccount account in investor.InvestorAccounts) {
				errors = errors.Union(ValidationHelper.Validate(account));
			}
			foreach (InvestorContact investorContact in investor.InvestorContacts) {
				Contact contact = investorContact.Contact;
				errors = errors.Union(ValidationHelper.Validate(contact));
				foreach (ContactAddress contactAddr in contact.ContactAddresses) {
					errors = errors.Union(ValidationHelper.Validate(contactAddr.Address));
				}
				foreach (ContactCommunication comm in contact.ContactCommunications) {
					errors = errors.Union(ValidationHelper.Validate(comm.Communication));
				}
			}
			return errors;
		}
	}

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

			[StringLength(100, ErrorMessage = "Contact Name must be under 100 characters.")]
			public global::System.String ContactName {
				get;
				set;
			}

			[StringLength(20, ErrorMessage = "Contact Type must be under 20 characters.")]
			public global::System.String ContactType {
				get;
				set;
			}

			[StringLength(30, ErrorMessage = "Last Name must be under 30 characters."), Required]
			public global::System.String LastName {
				get;
				set;
			}

			[StringLength(30, ErrorMessage = "First Name must be under 30 characters.")]
			public global::System.String FirstName {
				get;
				set;
			}

			[StringLength(30, ErrorMessage = "Middle Name must be under 30 characters.")]
			public global::System.String MiddleName {
				get;
				set;
			}

			[Required(ErrorMessage = "Created Date is required")]
			[DateRange(ErrorMessage = "Created Date is required")]
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
			#endregion
		}
	}

	[MetadataType(typeof(AddressMD))]
	public partial class Address {
		public class AddressMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Address Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Address Type is required")]
			public global::System.Int32 AddressTypeID {
				get;
				set;
			}

			[StringLength(40, ErrorMessage = "Address1 must be under 40 characters.")]
			public global::System.String Address1 {
				get;
				set;
			}

			[StringLength(40, ErrorMessage = "Address2 must be under 40 characters.")]
			public global::System.String Address2 {
				get;
				set;
			}

			[StringLength(40, ErrorMessage = "Address3 must be under 40 characters.")]
			public global::System.String Address3 {
				get;
				set;
			}

			[StringLength(30, ErrorMessage = "City must be under 30 characters.")]
			public global::System.String City {
				get;
				set;
			}

			[StringLength(125, ErrorMessage = "StProvince must be under 125 characters.")]
			public global::System.String StProvince {
				get;
				set;
			}

			/// <summary>
			/// DB wise it is not required. but currently all the entities will
			/// be USA based, so we will be mandating this
			/// </summary>
			[Required(ErrorMessage = "State is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "State is required")]
			public Nullable<global::System.Int32> State {
				get;
				set;
			}

			[StringLength(10, ErrorMessage = "Postal Code must be under 10 characters.")]
			public global::System.String PostalCode {
				get;
				set;
			}

			[Required(ErrorMessage = "Country is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Country is required")]
			public global::System.Int32 Country {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "County must be under 50 characters.")]
			public global::System.String County {
				get;
				set;
			}

			[Required(ErrorMessage = "Created Date is required")]
			[DateRange(ErrorMessage = "Created Date is required")]
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

			[StringLength(50, ErrorMessage = "Phone must be under 50 characters.")]
			public global::System.String Phone {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "Fax must be under 50 characters.")]
			public global::System.String Fax {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "Email must be under 50 characters.")]
			public global::System.String Email {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "Web Address must be under 50 characters.")]
			public global::System.String WebAddress {
				get;
				set;
			}
			#endregion
		}
	}

	[MetadataType(typeof(CommunicationMD))]
	public partial class Communication {
		public class CommunicationMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Communication Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Communication Type is required")]
			public global::System.Int32 CommunicationTypeID {
				get;
				set;
			}

			[StringLength(200, ErrorMessage = "Communication Value must be under 50 characters.")]
			public global::System.String CommunicationValue {
				get;
				set;
			}

			[StringLength(4, ErrorMessage = "Last Four Phone Value must be under 50 characters.")]
			public global::System.String LastFourPhone {
				get;
				set;
			}

			[StringLength(200, ErrorMessage = "Communication Comment Value must be under 50 characters.")]
			public global::System.String CommunicationComment {
				get;
				set;
			}

			[Required(ErrorMessage = "Created Date is required")]
			[DateRange(ErrorMessage = "Created Date is required")]
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
			#endregion
		}
	}

	[MetadataType(typeof(InvestorAccountMD))]
	public partial class InvestorAccount {
		public class InvestorAccountMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Range((int)0, int.MaxValue, ErrorMessage = "Routing is required")]
			public global::System.Int32 Routing {
				get;
				set;
			}

			[StringLength(40, ErrorMessage = "Account must be under 40 characters.")]
			public global::System.String Account {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "SWIFT must be under 50 characters.")]
			public global::System.String SWIFT {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "AccountOf must be under 50 characters.")]
			public global::System.String AccountOf {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "FFC must be under 50 characters.")]
			public global::System.String FFC {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "FFCNumber must be under 50 characters.")]
			public global::System.String FFCNumber {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "IBAN must be under 50 characters.")]
			public global::System.String IBAN {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "ByOrderOf must be under 50 characters.")]
			public global::System.String ByOrderOf {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "Reference must be under 50 characters.")]
			public global::System.String Reference {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "Attention must be under 50 characters.")]
			public global::System.String Attention {
				get;
				set;
			}

			[StringLength(105, ErrorMessage = "Comments must be under 105 characters.")]
			public global::System.String Comments {
				get;
				set;
			}

			[Required(ErrorMessage = "Created Date is required")]
			[DateRange(ErrorMessage = "Created Date is required")]
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

			[StringLength(50, ErrorMessage = "BankName must be under 50 characters.")]
			public global::System.String BankName {
				get;
				set;
			}
			#endregion
		}
	}
}