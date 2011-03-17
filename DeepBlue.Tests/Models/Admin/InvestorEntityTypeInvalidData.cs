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
    public class InvestorEntityTypeInvalidDataTest : InvestorEntityTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultInvestorEntityType, false);
			this.ServiceErrors = DefaultInvestorEntityType.Save();
        }

		[Test]
		public void create_a_new_investorentitytype_without_investorentitytype_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("InvestorEntityTypeName"));
		}
    }
}