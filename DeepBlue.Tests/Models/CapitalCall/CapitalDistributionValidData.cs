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
	public class CapitalDistributionValidData : CapitalDistributionTest {
		#region Valid Test Methods
		[SetUp]
		public override void Setup() {
			base.Setup();
			Create_Data(DefaultCapitalDistribution, true);
			this.ServiceErrors = DefaultCapitalDistribution.Save();
		}


		[Test]
		public void create_a_new_capitaldistribution_with_valid_capitaldistributiondate_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalDistributionDate"));
		}

		[Test]
		public void create_a_new_capitaldistribution_with_valid_capitaldistributionduedate_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalDistributionDueDate"));
		}

		[Test]
		public void create_a_new_capitaldistribution_with_valid_fundid_passes() {
			Assert.IsTrue(IsPropertyValid("FundID"));
		}

		[Test]
		public void create_a_new_capitaldistribution_with_valid_distributionamount_passes() {
			Assert.IsTrue(IsPropertyValid("DistributionAmount"));
		}

		[Test]
		public void create_a_new_capitaldistribution_with_valid_distributionnumber_passes() {
			Assert.IsTrue(IsPropertyValid("DistributionNumber"));
		}

		[Test]
		public void create_a_new_capitaldistribution_with_valid_createddate_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_capitaldistribution_with_valid_createdby_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_capitaldistribution_with_valid_lastupdateddate_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedDate"));
		}

		[Test]
		public void create_a_new_capitaldistribution_with_valid_lastupdatedby_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedBy"));
		}
		#endregion
    }
}