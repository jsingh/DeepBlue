using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Investor;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Investor;

namespace DeepBlue.Tests.Controllers.Investor {
	public class New : InvestorBase {

        private CreateModel Model {
            get {
                return base.ViewResult.ViewData.Model as CreateModel;
            }
        }
        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
			base.ActionResult = base.DefaultController.New();
        }

		[Test]
		public void create_a_new_investor() {
			Assert.IsInstanceOfType<ActionResult>(base.ActionResult);
		}

        [Test]
        public void get_list_of_countries_equal_239() {
            Assert.AreEqual(Model.SelectList.Countries.Count, 239);
        }

        [Test]
        public void get_list_of_states_equal_52() {
            Assert.AreEqual(Model.SelectList.States.Count, 52);
        }
		
    }
}
