using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.CapitalCall;
using DeepBlue.Models.Admin;

namespace DeepBlue.Tests.Controllers.CapitalCall {
    public class CreateCapitalCallManualValidData : CreateCapitalCallManual {

		protected ResultModel ResultModel {
			get {
				return base.ViewResult.ViewData.Model as ResultModel;
			}
		}

        private ModelStateDictionary ModelState {
            get {
                return base.ViewResult.ViewData.ModelState;
            }
        }

        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
            // Test if the SaveCapitalCall call fails
			MockCapiticalCallRepository.Setup(x => x.SaveCapitalCall(It.IsAny<DeepBlue.Models.Entity.CapitalCall>())).Returns(new List<Helpers.ErrorInfo>());
        }

        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(GetValidformCollection());
            base.ActionResult = base.DefaultController.CreateManualCapitalCall(GetValidformCollection());
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

		#region Tests where form collection doesnt have the required values. Tests for DataAnnotations

		private bool test_posted_value(string parameterName) {
			SetFormCollection();
			return IsValid(parameterName);
		}

		[Test]
		public void valid_capitalcallmanual_fundid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("FundId"));
		}

		[Test]
		public void valid_capitalcallmanual_fundid_sets_1_error() {
			Assert.IsTrue(test_error_count("FundId", 0));
		}

		[Test]
		public void valid_capitalcallmanual_capitalcallamount_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("CapitalAmountCalled"));
		}

		[Test]
		public void valid_capitalcallmanual_capitalcallamount_sets_1_error() {
			Assert.IsTrue(test_error_count("CapitalAmountCalled", 0));
		}

		[Test]
		public void valid_capitalcallmanual_capitalcalldate_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("CapitalCallDate"));
		}

		[Test]
		public void valid_capitalcallmanual_capitalcalldate_sets_1_error() {
			Assert.IsTrue(test_error_count("CapitalCallDate", 0));
		}

		[Test]
		public void valid_capitalcallmanual_capitalcallduedate_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("CapitalCallDueDate"));
		}

		[Test]
		public void valid_capitalcallmanual_capitalcallduedate_sets_1_error() {
			Assert.IsTrue(test_error_count("CapitalCallDueDate", 0));
		}
		#endregion

		#region Tests after model state is valid

		[Test]
		public void returns_back_to_new_view_if_saving_capitalcall_failed() {
			SetFormCollection();
			Assert.IsNotNull(ResultModel);
		}

		#endregion

		private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
			formCollection.Add("FundId", "1");
			formCollection.Add("CapitalAmountCalled", "10000.00");
			formCollection.Add("CapitalCallDate", "1/1/1999");
			formCollection.Add("CapitalCallDueDate", "1/1/1999");       
            return formCollection;
        }
    }
}
