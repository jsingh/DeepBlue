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
    public class ShareClassTypeValidDataTest : ShareClassTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultShareClassType, true);
			this.ServiceErrors = DefaultShareClassType.Save();
        }

		[Test]
		public void create_a_new_shareclasstype_with_shareclasstype_passes() {
			Assert.IsTrue(IsPropertyValid("ShareClass"));
		}

    }
}