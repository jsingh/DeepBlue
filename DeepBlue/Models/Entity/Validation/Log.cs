using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(LogMD))]
	public partial class Log {

		public global::System.String AdditionalDetail {
			get;
			set;
		}

		public class LogMD {

			#region Primitive Properties
		  
			[Required(ErrorMessage = "LogTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "LogTypeID is required")]
			public global::System.Int32 LogTypeID {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "ApplicationID is required")]
			public Nullable<global::System.Int32> ApplicationID {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "Controller must be under 50 characters.")]
			public global::System.String Controller {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "Action must be under 50 characters.")]
			public global::System.String Action {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "View must be under 50 characters.")]
			public global::System.String View {
				get;
				set;
			}

			[StringLength(255, ErrorMessage = "QueryString must be under 255 characters.")]
			public global::System.String QueryString {
				get;
				set;
			}

			[StringLength(500, ErrorMessage = "LogText must be under 500 characters.")]
			public global::System.String LogText {
				get;
				set;
			}

			[StringLength(250, ErrorMessage = "UserAgent must be under 250 characters.")]
			public global::System.String UserAgent {
				get;
				set;
			}

			[StringLength(100, ErrorMessage = "MachineName must be under 100 characters.")]
			public global::System.String MachineName {
				get;
				set;
			}

			[StringLength(100, ErrorMessage = "ProcessID must be under 100 characters.")]
			public global::System.String ProcessID {
				get;
				set;
			}

			[StringLength(100, ErrorMessage = "ProcessName must be under 100 characters.")]
			public global::System.String ProcessName {
				get;
				set;
			}

			#endregion
		}
		public Log(ILoggingService logService)
			: this() {
			this.LoggingService = logService;
		}

		public Log() {
		}

		private ILoggingService _LoggingService;
		public ILoggingService LoggingService {
			get {
				if (_LoggingService == null) {
					_LoggingService = new LoggingService();
				}
				return _LoggingService;
			}
			set {
				_LoggingService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			LoggingService.SaveLog(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(Log log) {
			return ValidationHelper.Validate(log);
		}
	}
}