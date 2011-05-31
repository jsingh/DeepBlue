using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(GeographyMD))]
	public partial class Geography {
		public class GeographyMD : CreatedByFields {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Geography is required")]
			[StringLength(100, ErrorMessage = "Geography Name must be under 100 characters.")]
			public global::System.String Geography1 {
				get;
				set;
			}

			#endregion
		}

		public Geography(IGeographyService geographyservice)
			: this() {
				this.geographyService = geographyservice;
		}

		public Geography() {
		}

		private IGeographyService _GeographyService;
		public IGeographyService geographyService {
			get {
				if (_GeographyService == null) {
					_GeographyService = new GeographyService();
				}
				return _GeographyService;
			}
			set {
				_GeographyService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var creategeography= this;
			IEnumerable<ErrorInfo> errors = Validate(creategeography);
			if (errors.Any()) {
				return errors;
			}
			geographyService.SaveGeography(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(Geography geography) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(geography);
			return errors;
		}
	}
}