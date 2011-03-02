using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Transaction;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Transaction;

namespace DeepBlue.Tests.Controllers.Transaction {
	public class NewInvestorFund : InvestorFundBase {

        private CreateModel Model {
            get {
                return base.ViewResult.ViewData.Model as CreateModel;
            }
        }
        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
			base.ActionResult = base.DefaultController.New(0);
        }

		[Test]
		public void create_a_new_transaction() {
			Assert.IsInstanceOfType<ActionResult>(base.ActionResult);
		}
 
    }
}
