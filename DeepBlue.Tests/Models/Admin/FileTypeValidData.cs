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
    public class FileTypeValidDataTest : FileTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultFileType, true);
			this.ServiceErrors = DefaultFileType.Save();
        }

		[Test]
		public void create_a_new_filetype_with_filetype_passes() {
			Assert.IsTrue(IsPropertyValid("FileTypeName"));
		}

    }
}