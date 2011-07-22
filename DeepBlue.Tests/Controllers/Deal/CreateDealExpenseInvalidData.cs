using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Deal ;

namespace DeepBlue.Tests.Controllers.Deal {
    public class CreateDealExpenseInvalidData : CreateDealExpense {
		private ResultModel ResultModel {
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
        }

        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(new FormCollection());
			base.ActionResult = base.DefaultController.CreateDealExpense(GetInvalidformCollection());
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
		public void invalid_Deal_dealclosingcosttypeid_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("DealClosingCostTypeId"));
		}

		[Test]
		public void invalid_Deal_dealid_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("DealId"));
		}

		[Test]
		public void invalid_Deal_amount_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("Amount"));
		}


		[Test]
		public void invalid_Deal_date_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("Date"));
		}

		[Test]
		public void invalid_Deal_dealclosingcosttypeid_sets_1_error() {
			Assert.IsTrue(test_error_count("DealClosingCostTypeId", 1));
		}

		[Test]
		public void invalid_Deal_dealid_sets_1_error() {
			Assert.IsTrue(test_error_count("DealId", 1));
		}

		[Test]
		public void invalid_Deal_amount_sets_1_error() {
			Assert.IsTrue(test_error_count("Amount", 1));
		}


		[Test]
		public void invalid_Deal_date_sets_1_error() {
			Assert.IsTrue(test_error_count("Date", 1));
		}


        [Test]
        public void invalid_Fund_results_in_invalid_modelstate() {
            SetFormCollection();
            Assert.IsFalse(base.DefaultController.ModelState.IsValid);
        }
      
        #endregion

        #region Tests after model state is invalid
        private void SetModelInvalid() {
            base.DefaultController.ModelState.AddModelError(string.Empty, string.Empty);
            SetFormCollection();
        }

		[Test]
		public void model_state_invalid_redirects_to_result_view() {
			SetModelInvalid();
			Assert.IsNotNull(ResultModel);
		}
       
		#endregion
   
        private FormCollection GetInvalidformCollection() {
            FormCollection formCollection = new FormCollection();
			formCollection.Add("DealClosingCostTypeId", string.Empty);
			formCollection.Add("DealId", string.Empty);
			formCollection.Add("Amount", string.Empty);
			formCollection.Add("Date", string.Empty);
            return formCollection;
        }
    }
}
