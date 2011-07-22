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

	public class EquitySplitInvalidDataTest : EquitySplitTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultEquitySplit, false);
			this.ServiceErrors = DefaultEquitySplit.Save();
        }

		[Test]
		public void create_a_new_equitysplit_without_equityid_passes() {
			Assert.IsFalse(IsPropertyValid("EquityID"));
		}

		[Test]
		public void create_a_new_equitysplit_without_splitfactor_passes() {
			Assert.IsFalse(IsPropertyValid("SplitFactor"));
		}

		[Test]
		public void create_a_new_equitysplit_without_splitdate_passes() {
			Assert.IsFalse(IsPropertyValid("SplitDate"));
		}

		[Test]
		public void create_a_new_equitysplit_without_createdby_passes() {
			Assert.IsFalse(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_equitysplit_without_createddate_passes() {
			Assert.IsFalse(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_equitysplit_without_lastupdatedby_passes() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedBy"));
		}

		[Test]
		public void create_a_new_equitysplit_without_lastupdateddate_passes() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedDate"));
		}
 
    }
}