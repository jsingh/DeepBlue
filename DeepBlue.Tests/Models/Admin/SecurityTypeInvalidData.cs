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
    public class SecurityTypeInvalidDataTest :SecurityTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultSecurityType, false);
			this.ServiceErrors = DefaultSecurityType.Save();
        }

		[Test]
		public void create_a_new_securitytype_without_securitytype_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("Name"));
		}

		[Test]
		public void create_a_new_securitytype_without_too_long_securitytype_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("Name"));
		}

    }
}