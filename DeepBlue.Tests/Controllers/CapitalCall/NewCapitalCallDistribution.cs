using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.CapitalCall;
using DeepBlue.Helpers;
using DeepBlue.Controllers.CapitalCall;

namespace DeepBlue.Tests.Controllers.CapitalCall {
	public class NewCapitalCallDistribution : CapitalCallDistributionBase {

        private CreateDistributionModel  Model {
            get {
				return base.ViewResult.ViewData.Model as CreateDistributionModel;
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
		
    }
}
