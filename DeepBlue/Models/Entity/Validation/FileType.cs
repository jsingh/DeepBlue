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

		public FileType(IFileTypeService fileService)
			: this() {
			this.FileTypeService = fileService;
		}

		public FileType() {
		}

		private IFileTypeService _fileService;
		public IFileTypeService FileTypeService {
			get {
				if (_fileService == null) {
					_fileService = new FileTypeService();
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
			FileTypeService.SaveFileType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(FileType fileEntityType) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(fileEntityType);
			return errors;
		}
	}
}