using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CommunicationGroupingMD))]
	public partial class CommunicationGrouping {
		public class CommunicationGroupingMD {
			#region Primitive Properties
			[Required(ErrorMessage = "Communication Grouping Name is required")]
			[StringLength(20, ErrorMessage = "Communication Grouping Name must be under 20 characters.")]
			public global::System.String CommunicationGroupingName {
				get;
				set;
			}
			#endregion
		}

		public CommunicationGrouping(ICommunicationGroupingService communicationGroupingService)
			: this() {
			this.CommunicationGroupingService = communicationGroupingService;
		}

		public CommunicationGrouping() {
		}

		private ICommunicationGroupingService _CommunicationGroupingService;
		public ICommunicationGroupingService CommunicationGroupingService {
			get {
				if (_CommunicationGroupingService == null) {
					_CommunicationGroupingService = new CommunicationGroupingService();
				}
				return _CommunicationGroupingService;
			}
			set {
				_CommunicationGroupingService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			CommunicationGroupingService.SaveCommunicationGrouping(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CommunicationGrouping communicationGrouping) {
			return ValidationHelper.Validate(communicationGrouping);
		}
	}
}