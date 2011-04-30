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
    public class SecurityTypeValidDataTest : SecurityTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultSecurityType, true);
			this.ServiceErrors = DefaultSecurityType.Save();
        }

		[Test]
		public void create_a_new_securitytype_with_purchasetype_name_passes() {
			Assert.IsTrue(IsPropertyValid("Name"));
		}

    }
}