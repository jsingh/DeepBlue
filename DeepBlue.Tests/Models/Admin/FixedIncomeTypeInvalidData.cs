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
    public class FixedIncomeTypeInvalidDataTest :FixedIncomeTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultFixedIncomeType, false);
			this.ServiceErrors = DefaultFixedIncomeType.Save();
        }

		[Test]
		public void create_a_new_fixedincometype_without_fixedincometype_throws_error() {
			Assert.IsFalse(IsPropertyValid("FixedIncomeType1"));
		}

		[Test]
		public void create_a_new_fixedincometype_without_too_long_fixedincometype_throws_error() {
			Assert.IsFalse(IsPropertyValid("FixedIncomeType1"));
		}

    }
}