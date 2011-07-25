using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Tests.Models.Deal;

namespace DeepBlue.Tests.Models.Deal {

	public class EquitySplitValidDataTest : EquitySplitTest {

		[SetUp]
		public override void Setup() {
			base.Setup();
			Create_Data(DefaultEquitySplit, true);
			this.ServiceErrors = DefaultEquitySplit.Save();
		}

		[Test]
		public void create_a_new_equitysplit_with_equityid_passes() {
			Assert.IsTrue(IsPropertyValid("EquityID"));
		}

		[Test]
		public void create_a_new_equitysplit_with_splitfactor_passes() {
			Assert.IsTrue(IsPropertyValid("SplitFactor"));
		}

		[Test]
		public void create_a_new_equitysplit_with_splitdate_passes() {
			Assert.IsTrue(IsPropertyValid("SplitDate"));
		}

		[Test]
		public void create_a_new_equitysplit_with_createdby_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_equitysplit_with_createddate_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_equitysplit_with_lastupdatedby_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedBy"));
		}

		[Test]
		public void create_a_new_equitysplit_with_lastupdateddate_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedDate"));
		}

	}
}