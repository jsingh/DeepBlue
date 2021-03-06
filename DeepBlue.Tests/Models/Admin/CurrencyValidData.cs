﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Admin {
    public class CurrencyValidDataTest : CurrencyTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultCurrency, true);
            this.ServiceErrors = DefaultCurrency.Save();
        }

		[Test]
		public void create_a_new_currency_with_currency_name_passes() {
			Assert.IsTrue(IsPropertyValid("Currency"));
		}
    }
}