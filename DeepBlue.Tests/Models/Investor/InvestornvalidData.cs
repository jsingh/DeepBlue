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
    public class InvestorInvalidDataTest : InvestorTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
            Create_Data(DefaultInvestor, false);
            this.ServiceErrors = DefaultInvestor.Save();
        }

        [Test]
        public void create_a_new_investor_without_valid_entityid_throws_error() {
            Assert.IsFalse(IsPropertyValid("EntityID"));
        }

        [Test]
        public void create_a_new_investor_without_investor_name_throws_error() {
            Assert.IsFalse(IsPropertyValid("InvestorName"));
        }

        [Test]
        public void create_a_new_investor_with_too_long_investor_name_throws_error() {
            Assert.IsFalse(IsPropertyValid("InvestorName"));
        }

        [Test]
        public void create_a_new_investor_without_valid_investorentitytypeid_throws_error() {
            Assert.IsFalse(IsPropertyValid("InvestorEntityTypeID"));
        }

        [Test]
        public void create_a_new_investor_with_too_long_alias_name_throws_error() {
            Assert.IsFalse(IsPropertyValid("Alias"));
        }

        [Test]
        public void create_a_new_investor_without_valid_previnvestorid_throws_error() {
            Assert.IsFalse(IsPropertyValid("PrevInvestorID"));
        }

        [Test]
        public void create_a_new_investor_with_too_long_managername_throws_error() {
            Assert.IsFalse(IsPropertyValid("ManagerName"));
        }

        [Test]
        public void create_a_new_investor_with_too_long_lastname_throws_error() {
            Assert.IsFalse(IsPropertyValid("LastName"));
        }

        [Test]
        public void create_a_new_investor_with_too_long_firstname_throws_error() {
            Assert.IsFalse(IsPropertyValid("FirstName"));
        }

        [Test]
        public void create_a_new_investor_with_too_long_middlename_throws_error() {
            Assert.IsFalse(IsPropertyValid("MiddleName"));
        }

        [Test]
        public void create_a_new_investor_without_valid_residencystate_throws_error() {
            Assert.IsFalse(IsPropertyValid("ResidencyState"));
        }

        [Test]
        public void create_a_new_investor_with_too_long_notes_throws_error() {
            Assert.IsFalse(IsPropertyValid("Notes"));
        }

        [Test]
        public void create_a_new_investor_without_valid_createddate_throws_error() {
            Assert.IsFalse(IsPropertyValid("CreatedDate"));
        }

        [Test]
        public void create_a_new_investor_without_valid_createdby_throws_error() {
            Assert.IsFalse(IsPropertyValid("CreatedBy"));
        }
    }
}