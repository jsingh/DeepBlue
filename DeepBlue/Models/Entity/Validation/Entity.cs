using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(EntityMD))]
	public partial class ENTITY {
		public class EntityMD {

			#region Primitive Properties
			[Required(ErrorMessage = "Entity Name is required")]
			[StringLength(60, ErrorMessage = "Entity Name must be under 60 characters.")]
			public global::System.String EntityName {
				get;
				set;
			}

			[Required(ErrorMessage = "Enabled is required")]
			public global::System.Boolean Enabled {
				get;
				set;
			}

			[DateRange(ErrorMessage = "CreatedDate is required")]
			public Nullable<global::System.DateTime> CreatedDate {
				get;
				set;
			}

			[Required(ErrorMessage = "Entity Code is required")]
			[StringLength(50, ErrorMessage = "EntityCode must be under 50 characters.")]
			public global::System.String EntityCode {
				get;
				set;
			}

			#endregion
		}
		public ENTITY(IEntityService entityService)
			: this() {
			this.EntityService = entityService;
		}

		public ENTITY() {
		}

		private IEntityService _EntityService;
		public IEntityService EntityService {
			get {
				if (_EntityService == null) {
					_EntityService = new EntityService();
				}
				return _EntityService;
			}
			set {
				_EntityService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			EntityService.SaveEntity(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(ENTITY entity) {
			return ValidationHelper.Validate(entity);
		}
	}
}
