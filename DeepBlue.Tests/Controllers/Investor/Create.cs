using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Investor;

namespace DeepBlue.Tests.Controllers.Investor {
	public class Create : InvestorBase {
		
        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
			CreateModel  model = new CreateModel();
			FormCollection collection  = new FormCollection();
			model.InvestorName = "test";
			model.Alias = "test alias";
			model.SocialSecurityTaxId = 123;
			model.Address1 = "test";
			model.Address2 = "test2";
			model.City = "test city";
			model.State = 2;
			model.StateOfResidency = 2;
			model.WebAddress = "test web address";
			model.Zip = "123";
			base.ActionResult =  base.DefaultController.Create(model,collection);
		}
        [Test]
        public void create_a_new_investor() {
            Assert.IsInstanceOfType<RedirectToRouteResult>(base.ActionResult);
        }
    }
}
