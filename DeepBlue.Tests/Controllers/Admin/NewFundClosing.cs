using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Admin;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Admin;

namespace DeepBlue.Tests.Controllers.Admin {
	public class NewFundClosing : FundClosingBase  {

		private EditFundClosingModel  Model {
            get {
				return base.ViewResult.ViewData.Model as EditFundClosingModel;
            }
        }
        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
			base.ActionResult = base.DefaultController.EditFundClosing(1);
        }
 
		[Test]
		public void create_a_new_fundclosing() {
			Assert.IsInstanceOfType<ActionResult>(base.ActionResult);
		}
    }
}
