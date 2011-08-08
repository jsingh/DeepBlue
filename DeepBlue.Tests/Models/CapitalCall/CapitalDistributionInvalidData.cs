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
	public class CapitalCallDistributionInvalidDataTest : CapitalDistributionTest {
		#region Invalid Test Methods
		[SetUp]
		public override void Setup() {
			base.Setup();
			Create_Data(DefaultCapitalDistribution, false);
			this.ServiceErrors = DefaultCapitalDistribution.Save();
		}



		[Test]
		public void create_a_new_capitaldistribution_without_capitaldistributiondate_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalDistributionDate"));
		}

		[Test]
		public void create_a_new_capitaldistribution_without_capitaldistributionduedate_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalDistributionDueDate"));
		}

		[Test]
		public void create_a_new_capitaldistribution_without_fundid_throws_error() {
			Assert.IsFalse(IsPropertyValid("FundID"));
		}

		[Test]
		public void create_a_new_capitaldistribution_without_distributionamount_throws_error() {
			Assert.IsFalse(IsPropertyValid("DistributionAmount"));
		}

		[Test]
		public void create_a_new_capitaldistribution_without_distributionnumber_throws_error() {
			Assert.IsFalse(IsPropertyValid("DistributionNumber"));
		}

		[Test]
		public void create_a_new_capitaldistribution_without_createddate_throws_error() {
			Assert.IsFalse(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_capitaldistribution_without_createdby_throws_error() {
			Assert.IsFalse(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_capitaldistribution_without_lastupdateddate_throws_error() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedDate"));
		}

		[Test]
		public void create_a_new_capitaldistribution_without_lastupdatedby_throws_error() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedBy"));
		}
		#endregion
    }
}