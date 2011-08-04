using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Deal {
    public class FundExpenseValidDataTest : FundExpenseTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultFundExpense, true);
			this.ServiceErrors = DefaultFundExpense.Save();
        }

		[Test]
		public void create_a_new_fundexpense_without_fundid_passes() {
			Assert.IsTrue(IsPropertyValid("FundID"));
		}

		[Test]
		public void create_a_new_fundexpense_without_fundexpensetypeid_passes() {
			Assert.IsTrue(IsPropertyValid("FundExpenseTypeID"));
		}

		[Test]
		public void create_a_new_fundexpense_without_createdby_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_fundexpense_without_createddate_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_fundexpense_without_lastupdatedby_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedBy"));
		}

		[Test]
		public void create_a_new_fundexpense_without_lastupdateddate_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedDate"));
		}

		[Test]
		public void create_a_new_fundexpense_without_amount_passes() {
			Assert.IsTrue(IsPropertyValid("Amount"));
		}
 
    }
}