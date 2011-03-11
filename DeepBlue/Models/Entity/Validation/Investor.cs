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
            [Required]
            [Range((int)(int)ConfigUtil.EntityIDStartRange, int.MaxValue)]
            public global::System.Int32 EntityID {
                get;
                set;
            }
            [StringLength(100), Required]
            public global::System.String InvestorName {
                get;
                set;
            }

            [Required]
            [Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
            public global::System.Int32 InvestorEntityTypeID {
                get;
                set;
            }

            [StringLength(50)]
            public global::System.String Alias {
                get;
                set;
            }

            [Range(0, int.MaxValue)]
            public Nullable<global::System.Int32> PrevInvestorID {
                get;
                set;
            }

            [StringLength(40)]
            public global::System.String ManagerName {
                get;
                set;
            }

            [StringLength(30), Required]
            public global::System.String LastName {
                get;
                set;
            }

            [StringLength(30)]
            public global::System.String FirstName {
                get;
                set;
            }

            [StringLength(30)]
            public global::System.String MiddleName {
                get;
                set;
            }

            [Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
            public Nullable<global::System.Int32> ResidencyState {
                get;
                set;
            }

            [StringLength(500)]
            public global::System.String Notes {
                get;
                set;
            }


            [Required]
            [Range(typeof(DateTime), "1/1/1900", "1/1/9999")]
            public global::System.DateTime CreatedDate {
                get;
                set;
            }

            [Required]
            [Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
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

		private IEnumerable<ErrorInfo> Validate(Investor investor){
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(investor);
			foreach (InvestorAddress address in investor.InvestorAddresses) {
				errors.Union(ValidationHelper.Validate(address.Address));
			}
			foreach (InvestorCommunication comm in investor.InvestorCommunications) {
				errors.Union(ValidationHelper.Validate(comm.Communication));
			}
			foreach (InvestorAccount account in investor.InvestorAccounts) {
				errors.Union(ValidationHelper.Validate(account));
			}

			foreach (InvestorContact investorContact in investor.InvestorContacts) {
				Contact contact = investorContact.Contact;
				errors.Union(ValidationHelper.Validate(contact));
				foreach (ContactAddress contactAddr in contact.ContactAddresses) {
					errors.Union(ValidationHelper.Validate(contactAddr.Address));
				}
				//foreach (ContactCommunication comm in contact.ContactCommunications) {
				//    errors.Union(ValidationHelper.Validate(comm.Communication));
				//}
			}
			return errors;
		}
    }

    [MetadataType(typeof(ContactMD))]
    public partial class Contact {
        public class ContactMD {
            #region Primitive Properties
            [Required]
            [Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue)]
            public global::System.Int32 EntityID {
                get;
                set;
            }
            [StringLength(100)]
            public global::System.String ContactName {
                get;
                set;
            }

            [StringLength(20)]
            public global::System.String ContactType {
                get;
                set;
            }

            [StringLength(30), Required]
            public global::System.String LastName {
                get;
                set;
            }

            [StringLength(30)]
            public global::System.String FirstName {
                get;
                set;
            }

            [StringLength(30)]
            public global::System.String MiddleName {
                get;
                set;
            }

            [Required]
            [Range(typeof(DateTime), "1/1/1900", "1/1/9999")]
            public global::System.DateTime CreatedDate {
                get;
                set;
            }

            [Required]
            [Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
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
            [Required]
            [Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue)]
            public global::System.Int32 EntityID {
                get;
                set;
            }

            [Required]
            [Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
            public global::System.Int32 AddressTypeID {
                get;
                set;
            }

            [StringLength(40), Required]
            public global::System.String Address1 {
                get;
                set;
            }

            [StringLength(40)]
            public global::System.String Address2 {
                get;
                set;
            }

            [StringLength(40)]
            public global::System.String Address3 {
                get;
                set;
            }

            [StringLength(30)]
            public global::System.String City {
                get;
                set;
            }

            [StringLength(125)]
            public global::System.String StProvince {
                get;
                set;
            }

            /// <summary>
            /// DB wise it is not required. but currently all the entities will
            /// be USA based, so we will be mandating this
            /// </summary>
            [Required]
            [Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
            public Nullable<global::System.Int32> State {
                get;
                set;
            }

            [StringLength(10)]
            public global::System.String PostalCode {
                get;
                set;
            }

            [Required]
            public global::System.Int32 Country {
                get;
                set;
            }

            [StringLength(50)]
            public global::System.String County {
                get;
                set;
            }

            [Required]
            [Range(typeof(DateTime), "1/1/1900", "1/1/9999")]
            public global::System.DateTime CreatedDate {
                get;
                set;
            }

            [Required]
            [Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
            public global::System.Int32 CreatedBy {
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
            [Required]
            [Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue)]
            public global::System.Int32 EntityID {
                get;
                set;
            }

            [Required]
            [Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
            public global::System.Int32 CommunicationTypeID {
                get;
                set;
            }

            [StringLength(200), Required]
            public global::System.String CommunicationValue {
                get;
                set;
            }

            [StringLength(4)]
            public global::System.String LastFourPhone {
                get;
                set;
            }

            [StringLength(200)]
            public global::System.String CommunicationComment {
                get;
                set;
            }

            [Required]
            [Range(typeof(DateTime), "1/1/1900", "1/1/9999")]
            public global::System.DateTime CreatedDate {
                get;
                set;
            }

            [Required]
            [Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
            public global::System.Int32 CreatedBy {
                get;
                set;
            }
            #endregion
        }
    }
}