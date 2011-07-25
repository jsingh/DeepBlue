using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Report;

namespace DeepBlue.Tests.Controllers.Report {
    public class CreateCapitalCallSummaryValidData : CreateCapitalCallSummary {
        private ModelStateDictionary ModelState {
            get {
                return base.ViewResult.ViewData.ModelState;
            }
        }

        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
        }

		[Test]
		public void valid_capitalcallsummarylist_json_result_error() {
			Assert.IsTrue((DefaultController.CapitalCallSummaryList(1, 1) != null));
		}
    }
}
