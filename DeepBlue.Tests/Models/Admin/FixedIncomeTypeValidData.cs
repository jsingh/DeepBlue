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
    public class FixedIncomeTypeValidDataTest : FixedIncomeTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultFixedIncomeType, true);
			this.ServiceErrors = DefaultFixedIncomeType.Save();
        }

		[Test]
		public void create_a_new_fixedincometype_with_fixedincometype_passes() {
			Assert.IsTrue(IsPropertyValid("FixedIncomeType1"));
		}

    }
}