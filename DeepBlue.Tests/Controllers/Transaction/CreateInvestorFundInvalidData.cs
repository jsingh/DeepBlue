using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Transaction;

namespace DeepBlue.Tests.Controllers.Transaction {
    public class CreateInvestorFundInvalidData : CreateInvestorFund {
        private CreateModel Model {
            get {
                return base.ViewResult.ViewData.Model as CreateModel;
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
            base.ActionResult = base.DefaultController.CreateInvestorFund(new FormCollection());
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
        public void invalid_Fund_Id_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("FundId"));
        }

        [Test]
		public void invalid_Fund_Id_sets_1_error() {
			Assert.IsTrue(test_error_count("FundId", 1));
        }

        [Test]
		public void invalid_Fund_Id_results_in_invalid_modelstate() {
            SetFormCollection();
            Assert.IsFalse(base.DefaultController.ModelState.IsValid);
        }

		[Test]
		public void invalid_Fund_Closing_Id_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("FundClosingId"));
		}

		[Test]
		public void invalid_Fund_Closing_Id_sets_1_error() {
			Assert.IsTrue(test_error_count("FundClosingId", 1));
		}

		[Test]
		public void invalid_Fund_Closing_Id_results_in_invalid_modelstate() {
			SetFormCollection();
			Assert.IsFalse(base.DefaultController.ModelState.IsValid);
		}

		[Test]
		public void invalid_TotalCommitment_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("FundClosingId"));
		}

		[Test]
		public void invalid_TotalCommitment_sets_1_error() {
			Assert.IsTrue(test_error_count("FundClosingId", 1));
		}

		[Test]
		public void invalid_TotalCommitment_results_in_invalid_modelstate() {
			SetFormCollection();
			Assert.IsFalse(base.DefaultController.ModelState.IsValid);
		}
				
		[Test]
		public void invalid_Fund_CommittedDate_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("CommittedDate"));
		}

		[Test]
		public void invalid_Fund_CommittedDate_sets_1_error() {
			Assert.IsTrue(test_error_count("CommittedDate", 1));
		}

		[Test]
		public void invalid_Fund_CommittedDate_results_in_invalid_modelstate() {
			SetFormCollection();
			Assert.IsFalse(base.DefaultController.ModelState.IsValid);
		}
      

      
        #endregion

 
        
    }
}
