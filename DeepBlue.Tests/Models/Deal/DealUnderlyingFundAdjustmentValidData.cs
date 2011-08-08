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
	public class DealUnderlyingFundAdjustmentValidDataTest : DealUnderlyingFundAdjustmentTest {

		[SetUp]
		public override void Setup() {
			base.Setup();
			Create_Data(DefaultDealUnderlyingFundAdjustment, true);
			this.ServiceErrors = DefaultDealUnderlyingFundAdjustment.Save();
		}

		[Test]
		public void create_a_new_dealunderlyingfundadjustment_with_valid_dealunderlyingfundid_passes() {
			Assert.IsTrue(IsPropertyValid("DealUnderlyingFundID"));
		}

		[Test]
		public void create_a_new_dealunderlyingfundadjustment_with_valid_commitmentamount_passes() {
			Assert.IsTrue(IsPropertyValid("CommitmentAmount"));
		}

		[Test]
		public void create_a_new_dealunderlyingfundadjustment_with_valid_unfundedamount_passes() {
			Assert.IsTrue(IsPropertyValid("UnfundedAmount"));
		}

		[Test]
		public void create_a_new_dealunderlyingfundadjustment_with_valid_createdby_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_dealunderlyingfundadjustment_with_valid_createddate_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_dealunderlyingfundadjustment_with_valid_lastupdateddate_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedDate"));
		}

		[Test]
		public void create_a_new_dealunderlyingfundadjustment_with_valid_lastupdatedby_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedBy"));
		}

	}
}