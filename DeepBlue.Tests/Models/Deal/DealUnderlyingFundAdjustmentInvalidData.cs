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
	public class DealUnderlyingFundAdjustmentInvalidDataTest : DealUnderlyingFundAdjustmentTest {

		[SetUp]
		public override void Setup() {
			base.Setup();
			Create_Data(DefaultDealUnderlyingFundAdjustment, false);
			this.ServiceErrors = DefaultDealUnderlyingFundAdjustment.Save();
		}



		[Test]
		public void create_a_new_dealunderlyingfundadjustment_without_dealunderlyingfundid_throws_error() {
			Assert.IsFalse(IsPropertyValid("DealUnderlyingFundID"));
		}

		[Test]
		public void create_a_new_dealunderlyingfundadjustment_without_commitmentamount_throws_error() {
			Assert.IsFalse(IsPropertyValid("CommitmentAmount"));
		}

		[Test]
		public void create_a_new_dealunderlyingfundadjustment_without_unfundedamount_throws_error() {
			Assert.IsFalse(IsPropertyValid("UnfundedAmount"));
		}

		[Test]
		public void create_a_new_dealunderlyingfundadjustment_without_createdby_throws_error() {
			Assert.IsFalse(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_dealunderlyingfundadjustment_without_createddate_throws_error() {
			Assert.IsFalse(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_dealunderlyingfundadjustment_without_lastupdateddate_throws_error() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedDate"));
		}

		[Test]
		public void create_a_new_dealunderlyingfundadjustment_without_lastupdatedby_throws_error() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedBy"));
		}

	}
}