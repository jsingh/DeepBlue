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
    public class FundClosingInvalidDataTest : FundClosingTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultFundClosing, false);
			this.ServiceErrors = DefaultFundClosing.Save();
        }

		[Test]
		public void create_a_new_fundclosing_without_fundclosing_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("Name"));
		}

		[Test]
		public void create_a_new_fundclosing_without_too_long_fundclosing_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("Name"));
		}

		[Test]
		public void create_a_new_fundclosing_without_fundid_throws_error() {
			Assert.IsFalse(IsPropertyValid("FundID"));
		}

		[Test]
		public void create_a_new_fundclosing_without_fundclosingdate_throws_error() {
			Assert.IsFalse(IsPropertyValid("FundClosingDate"));
		}

    }
}