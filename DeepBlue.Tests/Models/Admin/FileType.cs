using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Admin {
    public class FileTypeTest : Base {
		public DeepBlue.Models.Entity.FileType DefaultFileType { get; set; }

        public Mock<IFileTypeService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IFileTypeService>();

			DefaultFileType = new DeepBlue.Models.Entity.FileType(MockService.Object);
            MockService.Setup(x => x.SaveFileType(It.IsAny<DeepBlue.Models.Entity.FileType>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.FileType filetype, bool ifValid) {
			RequiredFieldDataMissing(filetype, ifValid);
			StringLengthInvalidData(filetype, ifValid);			
		}

		#region FileType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.FileType filetype, bool ifValidData) {
			if (ifValidData) {
				filetype.FileTypeName = "FileTypeName";
			}
			else{
				filetype.FileTypeName = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.FileType filetype, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			filetype.FileTypeName = GetString(50 + delta);
		}
		#endregion
    }
}