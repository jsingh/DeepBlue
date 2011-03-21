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
    public class FundClosingValidDataTest :FundClosingTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultFundClosing, true);
			this.ServiceErrors = DefaultFundClosing.Save();
        }

		[Test]
		public void create_a_new_fundclosing_with_fund_name_passes() {
			Assert.IsTrue(IsPropertyValid("Name"));
		}

		[Test]
		public void create_a_new_fundclosing_with_valid_fundid_passes() {
			Assert.IsTrue(IsPropertyValid("FundID"));
		}

		[Test]
		public void create_a_new_fundclosing_with_valid_fundclosingdate_passes() {
			Assert.IsTrue(IsPropertyValid("FundClosingDate"));
		}

    }
}