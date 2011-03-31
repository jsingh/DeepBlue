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
    public class CapitalCallReceiveValidData : CapitalCallReceiveTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultCapitalCallReceive, true);
			this.ServiceErrors = DefaultCapitalCallReceive.Save();
        }

		[Test]
		public void create_a_new_capitalcallreceive_with_valid_fundid_passes() {
			Assert.IsTrue(IsPropertyValid("FundID"));
		}

        [Test]
        public void create_a_new_capitalcallreceive_with_valid_capitalamount_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalAmountCalled"));
        }

        [Test]
        public void create_a_new_capitalcallreceive_with_valid_capitalcalldate_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalCallDate"));
        }

		[Test]
		public void create_a_new_capitalcallreceive_with_valid_capitalcallduedate_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalCallDueDate"));
		}

    }
}