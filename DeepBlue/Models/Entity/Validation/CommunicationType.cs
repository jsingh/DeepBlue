using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CommunicationTypeMD))]
	public partial class CommunicationType {
		public class CommunicationTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Communication Type Name is required")]
			[StringLength(20, ErrorMessage = "Communication Type Name must be under 20 characters.")]
			public global::System.String CommunicationTypeName {
				get;
				set;
			}

			[Required(ErrorMessage="Communication Grouping is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Communication Grouping is required")]
			public global::System.Int32 CommunicationGroupingID {
				get;
				set;
			}
			#endregion
		}

		public CommunicationType(ICommunicationTypeService communicationTypeService)
			: this() {
			this.CommunicationTypeService = communicationTypeService;
		}

		public CommunicationType() {
		}

		private ICommunicationTypeService _CommunicationTypeService;
		public ICommunicationTypeService CommunicationTypeService {
			get {
				if (_CommunicationTypeService == null) {
					_CommunicationTypeService = new CommunicationTypeService();
				}
				return _CommunicationTypeService;
			}
			set {
				_CommunicationTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			CommunicationTypeService.SaveCommunicationType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CommunicationType communicationType) {
			return ValidationHelper.Validate(communicationType);
		}
	}
}