using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(USERMD))]
	public partial class USER {
		public class USERMD {
			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}


			[Required(ErrorMessage = "Login is required")]
			[StringLength(50, ErrorMessage = "Login must be under 50 characters.")]
			public global::System.String Login {
				get;
				set;
			}


			[Required(ErrorMessage = "Email is required")]
			[StringLength(50, ErrorMessage = "Email must be under 50 characters.")]
			public global::System.String Email {
				get;
				set;
			}


			[StringLength(50, ErrorMessage = "PasswordHash must be under 50 characters.")]
			public global::System.String PasswordHash {
				get;
				set;
			}


			[Required(ErrorMessage = "PasswordSalt is required")]
			[StringLength(50, ErrorMessage = "PasswordSalt must be under 50 characters.")]
			public global::System.String PasswordSalt {
				get;
				set;
			}


			[Required(ErrorMessage = "LastName is required")]
			[StringLength(30, ErrorMessage = "LastName must be under 30 characters.")]
			public global::System.String LastName {
				get;
				set;
			}


			[StringLength(30, ErrorMessage = "MiddleName must be under 30 characters.")]
			public global::System.String MiddleName {
				get;
				set;
			}


			[Required(ErrorMessage = "FirstName is required")]
			[StringLength(30, ErrorMessage = "FirstName must be under 30 characters.")]
			public global::System.String FirstName {
				get;
				set;
			}


			[Required(ErrorMessage = "Enabled is required")]
			public global::System.Boolean Enabled {
				get;
				set;
			}


			[StringLength(25, ErrorMessage = "PhoneNumber must be under 25 characters.")]
			public global::System.String PhoneNumber {
				get;
				set;
			}


			[Required(ErrorMessage = "IsAdmin is required")]
			public global::System.Boolean IsAdmin {
				get;
				set;
			}

			[DateRange(ErrorMessage = "CreatedDate is required")]
			public Nullable<global::System.DateTime> CreatedDate {
				get;
				set;
			}

			[DateRange(ErrorMessage = "LastUpdatedDate is required")]
			public Nullable<global::System.DateTime> LastUpdatedDate {
				get;
				set;
			}

			#endregion
		}

		public USER(IUserService userService)
			: this() {
			this.UserService = UserService;
		}

		public USER() {
		}

		private IUserService _UserService;
		public IUserService UserService {
			get {
				if (_UserService == null) {
					_UserService = new UserService();
				}
				return _UserService;
			}
			set {
				_UserService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			UserService.SaveUser(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(USER user) {
			return ValidationHelper.Validate(user);
		}
	}
}