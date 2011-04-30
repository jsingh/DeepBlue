using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Report;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Report;

namespace DeepBlue.Tests.Controllers.Report {
	public class NewCapitalCallSummary :CapitalCallSummaryBase {

        private CapitalCallSummaryModel   Model {
            get {
				return base.ViewResult.ViewData.Model as CapitalCallSummaryModel;
            }
        }
        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
			base.ActionResult = base.DefaultController.CapitalCallSummary() ;
        }
 
		[Test]
		public void create_a_new_fund() {
			Assert.IsInstanceOfType<ActionResult>(base.ActionResult);
		}
    }
}
