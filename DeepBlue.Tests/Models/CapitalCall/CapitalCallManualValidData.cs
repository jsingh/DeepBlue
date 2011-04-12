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
    public class CapitalCallManualValidData : CapitalCallManualTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultCapitalCallManual, true);
			this.ServiceErrors = DefaultCapitalCallManual.Save();
        }

		[Test]
		public void create_a_new_capitalcallreqular_with_fundid_passes() {
			Assert.IsTrue(IsPropertyValid("FundID"));
		}

		[Test]
		public void create_a_new_capitalcallmanual_with_capitalcallnumber_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalCallNumber"));
		}

		[Test]
		public void create_a_new_capitalcallmanual_with_capitalcalltypeid_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalCallTypeID"));
		}

		[Test]
		public void create_a_new_capitalcallmanual_with_capitalcalldate_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalCallDate"));
		}

		[Test]
		public void create_a_new_capitalcallmanual_with_capitalcallduedate_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalCallDueDate"));
		}

        [Test]
        public void create_a_new_capitalcallmanual_with_capitalamount_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalAmountCalled"));
        }
		
		[Test]
		public void create_a_new_capitalcallmanual_with_capitalcallnewinvestmentamount_passes() {
			Assert.IsTrue(IsPropertyValid("NewInvestmentAmount"));
		}

    }
}