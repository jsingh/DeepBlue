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
			// Test if the SaveFund call fails
			MockReportRepository.Setup(x => x.CapitalCallLineItems(1,1)).Returns(new List<CapitalCallItem>());
        }

        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(GetValidformCollection());
            base.ActionResult = base.DefaultController.CapitalCallSummary();
        }

		#region Tests where form collection doesnt have the required values. Tests for DataAnnotations
		private bool test_posted_value(string parameterName) {
			SetFormCollection();
			return IsValid(parameterName);
		}

		/// <summary>
		/// After the TryUpdateModel tries to update the model, see if the error counts is the same as the DataAnnotations ValidationAttribute count
		/// on the property
		/// </summary>
		/// <param name="parameterName"></param>
		/// <param name="errorCount"></param>
		/// <returns></returns>
		private bool test_error_count(string parameterName, int errorCount) {
			SetFormCollection();
			int errors = 0;
			IsValid(parameterName, out errors);
			return errorCount == errors;
		}

		[Test]
		public void valid_Report_capitalcallsummary_fundid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("FundId"));
		}

		[Test]
		public void valid_Report_capitalcallsummary_fundid_sets_1_error() {
			Assert.IsTrue(test_error_count("FundId", 0));
		}

		[Test]
		public void valid_Report_capitalcallsummary_id_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("CapitalCallId"));
		}

		[Test]
		public void valid_Report_capitalcallsummary_id_sets_1_error() {
			Assert.IsTrue(test_error_count("CapitalCallId", 0));
		}

        #endregion
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
			formCollection.Add("FundId", "1");
			formCollection.Add("CapitalCallId", "1");
            return formCollection;
        }
    }
}
