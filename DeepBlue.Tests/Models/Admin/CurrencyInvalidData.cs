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
    public class CurrencyInvalidDataTest : CurrencyTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultCurrency,false);
            this.ServiceErrors = DefaultCurrency.Save();
        }

		[Test]
		public void create_a_new_currency_without_currency_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("Currency"));
		}

		[Test]
		public void create_a_new_currency_without_too_long_currency_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("Currency"));
		}
    }
}