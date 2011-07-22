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
    public class UnderlyingFundCashDistributionValidDataTest : UnderlyingFundCashDistributionTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultUnderlyingFundCashDistribution, true);
			this.ServiceErrors = DefaultUnderlyingFundCashDistribution.Save();
        }

		[Test]
		public void create_a_new_underlyingfundcashdistribution_with_fundid_passes() {
			Assert.IsTrue(IsPropertyValid("FundID"));
		}

		[Test]
		public void create_a_new_underlyingfundcashdistribution_with_createdby_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_underlyingfundcashdistribution_with_createddate_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_underlyingfundcashdistribution_with_lastupdatedby_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedBy"));
		}

		[Test]
		public void create_a_new_underlyingfundcashdistribution_with_lastupdateddate_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedDate"));
		}

		[Test]
		public void create_a_new_underlyingfundcashdistribution_with_amount_passes() {
			Assert.IsTrue(IsPropertyValid("Amount"));
		}

		[Test]
		public void create_a_new_underlyingfundcashdistribution_with_noticedate_passes() {
			Assert.IsTrue(IsPropertyValid("NoticeDate"));
		}

		[Test]
		public void create_a_new_underlyingfundcashdistribution_with_receiveddate_passes() {
			Assert.IsTrue(IsPropertyValid("ReceivedDate"));
		}
    }
}