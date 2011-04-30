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
    public class PurchaseTypeInvalidDataTest :PurchaseTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultPurchaseType, false);
			this.ServiceErrors = DefaultPurchaseType.Save();
        }

		[Test]
		public void create_a_new_purchasetype_without_purchasetype_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("Name"));
		}

		[Test]
		public void create_a_new_purchasetype_without_too_long_purchasetype_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("Name"));
		}

    }
}