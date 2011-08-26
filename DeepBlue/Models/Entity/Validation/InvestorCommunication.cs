using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue.Models.Entity.Partial;

namespace DeepBlue.Models.Entity {

	[MetadataType(typeof(InvestorCommunicationMD))]
	public partial class InvestorCommunication {
		public class InvestorCommunicationMD {

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


			[Required(ErrorMessage = "CommunicationID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CommunicationID is required")]
			public global::System.Int32 CommunicationID {
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
		public InvestorCommunication(IInvestorCommunicationService investorcommunicationService)
			: this() {
			this.InvestorCommunicationService = InvestorCommunicationService;
		}

		public InvestorCommunication() {
		}

		private IInvestorCommunicationService _InvestorCommunicationService;
		public IInvestorCommunicationService InvestorCommunicationService {
			get {
				if (_InvestorCommunicationService == null) {
					_InvestorCommunicationService = new InvestorCommunicationService();
				}
				return _InvestorCommunicationService;
			}
			set {
				_InvestorCommunicationService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			errors = errors.Union(ValidationHelper.Validate(this.Communication));
			if (errors.Any()) {
				return errors;
			}
			InvestorCommunicationService.SaveInvestorCommunication(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(InvestorCommunication investorcommunication) {
			return ValidationHelper.Validate(investorcommunication);
		}
	}
}