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
    public class FileTypeInvalidDataTest :FileTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultFileType, false);
			this.ServiceErrors = DefaultFileType.Save();
        }

		[Test]
		public void create_a_new_filetype_without_filetypename_throws_error() {
			Assert.IsFalse(IsPropertyValid("FileTypeName"));
		}

		[Test]
		public void create_a_new_filetype_without_too_long_filetypename_throws_error() {
			Assert.IsFalse(IsPropertyValid("FileTypeName"));
		}

    }
}