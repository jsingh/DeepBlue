﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.CapitalCall {
    public class CapitalCallManualInvalidDataTest : CapitalCallManualTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultCapitalCallManual, false);
			this.ServiceErrors = DefaultCapitalCallManual.Save();
        }

		[Test]
		public void create_a_new_capitalcallmanual_without_fundid_throws_error() {
			Assert.IsFalse(IsPropertyValid("FundID"));
		}

		[Test]
		public void create_a_new_capitalcallmanual_without_capitalcallnumber_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalCallNumber"));
		}

		[Test]
		public void create_a_new_capitalcallmanual_without_capitalcalltypeid_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalCallTypeID"));
		}

        [Test]
        public void create_a_new_capitalcallmanual_without_capitalcalldate_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalCallDate"));
        }

		[Test]
		public void create_a_new_capitalcallmanual_without_capitalcallduedate_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalCallDueDate"));
		}

		[Test]
		public void create_a_new_capitalcallmanual_without_capitalamount_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalAmountCalled"));
		}
    }
}