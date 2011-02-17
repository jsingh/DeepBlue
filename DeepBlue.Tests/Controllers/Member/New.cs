using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Member;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Member;

namespace DeepBlue.Tests.Controllers.Member {
    public class New : MemberBase {

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
		public void create_a_new_member() {
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
