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
    public class EquityTypeInvalidDataTest :EquityTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultEquityType, false);
			this.ServiceErrors = DefaultEquityType.Save();
        }

		[Test]
		public void create_a_new_equitytype_without_equity_throws_error() {
			Assert.IsFalse(IsPropertyValid("Equity"));
		}

		[Test]
		public void create_a_new_equitytype_without_too_long_equity_throws_error() {
			Assert.IsFalse(IsPropertyValid("Equity"));
		}

    }
}