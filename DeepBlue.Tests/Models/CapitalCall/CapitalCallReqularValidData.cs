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
    public class CapitalCallReqularValidData : CapitalCallReqularTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultCapitalCallReqular, true);
			this.ServiceErrors = DefaultCapitalCallReqular.Save();
        }


		[Test]
		public void create_a_new_capitalcallreqular_with_valid_fundid_passes() {
			Assert.IsTrue(IsPropertyValid("FundId"));
		}

        [Test]
        public void create_a_new_capitalcallreqular_with_valid_capitalamount_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalCallAmount"));
        }

        [Test]
        public void create_a_new_capitalcallreqular_with_valid_capitalcalldate_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalCallDate"));
        }

		[Test]
		public void create_a_new_capitalcallreqular_with_valid_capitalcallduedate_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalCallDueDate"));
		}
    }
}