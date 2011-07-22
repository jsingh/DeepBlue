using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Fund;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Fund;

namespace DeepBlue.Tests.Controllers.Fund {
	public class New :FundBase {

        private FundDetail  Model {
            get {
				return base.ViewResult.ViewData.Model as FundDetail;
            }
        }
        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
			//base.ActionResult = base.DefaultController.New();
        }
 
		[Test]
		public void create_a_new_fund() {
			Assert.IsInstanceOfType<ActionResult>(base.ActionResult);
		}
    }
}
