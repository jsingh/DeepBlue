using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(FileTypeMD))]
	public partial class FileType {
		public class FileTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "File Type Name is required")]
			[StringLength(50, ErrorMessage = "File Type Name must be under 50 characters.")]
			public global::System.String FileTypeName {
				get;
				set;
			}

			[Required(ErrorMessage = "File Extension is required")]
			[StringLength(20, ErrorMessage = "File Extension must be under 20 characters.")]
			public global::System.String FileExtension {
				get;
				set;
			}
			
			[StringLength(100, ErrorMessage = "Description must be under 100 characters.")]
			public global::System.String Description {
				get;
				set;
			}
			#endregion
		}

		public FileType(IFileTypeService fileTypeService)
			: this() {
			this.FileTypeService = fileTypeService;
		}

		public FileType() {
		}

		private IFileTypeService _fileTypeService;
		public IFileTypeService FileTypeService {
			get {
				if (_fileTypeService == null) {
					_fileTypeService = new FileTypeService();
				}
				return _fileTypeService;
			}
			set {
				_fileTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var fileType = this;
			IEnumerable<ErrorInfo> errors = Validate(fileType);
			if (errors.Any()) {
				return errors;
			}
			FileTypeService.SaveFileType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(FileType fileType) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(fileType);
			return errors;
		}
	}
}