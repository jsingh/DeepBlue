using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(FileMD))]
	public partial class File {
		public class FileMD : CreatedByFields {
			#region Primitive Properties
			
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "File Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "File Type is required")]
			public global::System.Int32 FileTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "File Path is required")]
			[StringLength(500, ErrorMessage = "File Path must be under 500 characters.")]
			public global::System.String FilePath {
				get;
				set;
			}

			[Required(ErrorMessage = "File Name is required")]
			[StringLength(200, ErrorMessage = "File Name must be under 200 characters.")]
			public global::System.String FileName {
				get;
				set;
			}
		
			#endregion
		}

		public File(IFileService fileService)
			: this() {
			this.FileService = fileService;
		}

		public File() {
		}

		private IFileService _fileService;
		public IFileService FileService {
			get {
				if (_fileService == null) {
					_fileService = new FileService();
				}
				return _fileService;
			}
			set {
				_fileService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var file = this;
			IEnumerable<ErrorInfo> errors = Validate(file);
			if (errors.Any()) {
				return errors;
			}
			FileService.SaveFile(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(File file) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(file);
			return errors;
		}
	}
}