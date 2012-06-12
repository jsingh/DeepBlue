using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(BrokerMD))]
	public partial class Broker {
		public class BrokerMD : CreatedByFields {

			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}


			[Required(ErrorMessage = "BrokerName is required")]
			[StringLength(100, ErrorMessage = "BrokerName must be under 100 characters.")]
			public global::System.String BrokerName {
				get;
				set;
			}


			[StringLength(100, ErrorMessage = "ContactPerson must be under 100 characters.")]
			public global::System.String ContactPerson {
				get;
				set;
			}


			[StringLength(100, ErrorMessage = "ContactNumber must be under 100 characters.")]
			public global::System.String ContactNumber {
				get;
				set;
			}


			[StringLength(50, ErrorMessage = "Email must be under 50 characters.")]
			public global::System.String Email {
				get;
				set;
			}


			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "AddressID is required")]
			public Nullable<global::System.Int32> AddressID {
				get;
				set;
			}
			
			#endregion
		}
		public Broker(IBrokerService brokerService)
			: this() {
			this.BrokerService = brokerService;
		}

		public Broker() {
		}

		private IBrokerService _BrokerService;
		public IBrokerService BrokerService {
			get {
				if (_BrokerService == null) {
					_BrokerService = new BrokerService();
				}
				return _BrokerService;
			}
			set {
				_BrokerService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			BrokerService.SaveBroker(this);
			return errors;
		}

		private IEnumerable<ErrorInfo> Validate(Broker broker) {
			return ValidationHelper.Validate(broker);
		}
	}
}
