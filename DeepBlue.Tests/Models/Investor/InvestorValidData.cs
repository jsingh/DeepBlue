using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Investor {
    public class InvestorValidDataTest : InvestorTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
            Create_Data(DefaultInvestor, true);
            this.ServiceErrors = DefaultInvestor.Save();
        }


        [Test]
        public void create_a_new_investor_with_valid_entityid_passes() {
            Assert.IsTrue(IsPropertyValid("EntityID"));
        }

        [Test]
        public void create_a_new_investor_with_investor_name_passes() {
            Assert.IsTrue(IsPropertyValid("InvestorName"));
        }

		[Test]
		public void create_a_new_investor_with_valid_socialsecuritytaxid_passes() {
			Assert.IsTrue(IsPropertyValid("SocialSecurityTaxId"));
		}

		[Test]
		public void create_a_new_investor_with_valid_stateofresidency_passes() {
			Assert.IsTrue(IsPropertyValid("StateOfResidency"));
		}


		[Test]
		public void create_a_new_investor_with_valid_investorentitytypeid_passes() {
			Assert.IsTrue(IsPropertyValid("InvestorEntityTypeID"));
		}

		[Test]
		public void create_a_new_investor_with_alias_name_passes() {
			Assert.IsTrue(IsPropertyValid("Alias"));
		}

		[Test]
		public void create_a_new_investor_with_valid_previnvestorid_passes() {
			Assert.IsTrue(IsPropertyValid("PrevInvestorID"));
		}

		[Test]
		public void create_a_new_investor_with_managername_passes() {
			Assert.IsTrue(IsPropertyValid("ManagerName"));
		}

		[Test]
		public void create_a_new_investor_with_lastname_passes() {
			Assert.IsTrue(IsPropertyValid("LastName"));
		}

		[Test]
		public void create_a_new_investor_with_firstname_passes() {
			Assert.IsTrue(IsPropertyValid("FirstName"));
		}

		[Test]
		public void create_a_new_investor_with_middlename_passes() {
			Assert.IsTrue(IsPropertyValid("MiddleName"));
		}

		[Test]
		public void create_a_new_investor_with_valid_residencystate_passes() {
			Assert.IsTrue(IsPropertyValid("ResidencyState"));
		}

		[Test]
		public void create_a_new_investor_with_notes_passes() {
			Assert.IsTrue(IsPropertyValid("Notes"));
		}

		[Test]
		public void create_a_new_investor_with_valid_createddate_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_investor_with_valid_createdby_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedBy"));
		}
    }
}