using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Report;

namespace DeepBlue.Tests.Controllers.Report {
	public class CreateCashDistributionSummaryInvalidData : CreateCashDistributionSummary {
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
		public void invalid_createcashdistributionsummarylist_json_result_error() {
			Assert.IsTrue((DefaultController.DistributionSummaryList(new FormCollection()) == null));
		}
    }
}
