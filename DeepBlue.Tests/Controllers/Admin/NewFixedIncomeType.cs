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
	public class NewFixedIncomeType : FixedIncomeTypeBase {

		private EditFixedIncomeTypeModel  Model {
            get {
				return base.ViewResult.ViewData.Model as EditFixedIncomeTypeModel;
            }
        }
        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
			base.ActionResult = base.DefaultController.EditFixedIncomeType(1);
        }
 
		[Test]
		public void create_a_new_fixedincometype() {
			Assert.IsInstanceOfType<ActionResult>(base.ActionResult);
		}
		
    }
}
