using System;
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
		public void create_a_new_capitalcallmanual_without_valid_fundid_throws_error() {
			Assert.IsFalse(IsPropertyValid("FundID"));
		}


        [Test]
        public void create_a_new_capitalcallmanual_without_valid_capitalamount_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalAmountCalled"));
        }

        [Test]
        public void create_a_new_capitalcallmanual_without_valid_capitalcalldate_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalCallDate"));
        }


		[Test]
		public void create_a_new_capitalcallmanual_without_valid_capitalcallduedate_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalCallDueDate"));
		}
    }
}