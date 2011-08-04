using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.CapitalCall;

namespace DeepBlue.Tests.Controllers.CapitalCall {
    public class CreateCapitalCallDistributionValidData : CreateCapitalCallDistribution {

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
            base.ActionResult = base.DefaultController.CreateDistribution(GetValidformCollection());
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
		public void valid_capitalcalldistribution_fundid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("FundId"));
		}

		[Test]
		public void valid_capitalcalldistribution_fundid_sets_1_error() {
			Assert.IsTrue(test_error_count("FundId", 0));
		}

		[Test]
		public void valid_capitalcalldistribution_distributionamount_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("DistributionAmount"));
		}

		[Test]
		public void valid_capitalcalldistribution_distributionamount_sets_1_error() {
			Assert.IsTrue(test_error_count("DistributionAmount", 0));
		}

		[Test]
		public void valid_capitalcalldistribution_capitaldistributiondate_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("CapitalDistributionDate"));
		}

		[Test]
		public void valid_capitalcalldistribution_capitaldistributiondate_sets_1_error() {
			Assert.IsTrue(test_error_count("CapitalDistributionDate", 0));
		}

		[Test]
		public void valid_capitalcalldistribution_capitaldistributionduedate_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("CapitalDistributionDueDate"));
		}

		[Test]
		public void valid_capitalcalldistribution_capitaldistributionduedate_sets_1_error() {
			Assert.IsTrue(test_error_count("CapitalDistributionDueDate", 0));
		}

		[Test]
		public void valid_capitalcalldistribution_capitalcallnumber_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("DistributionNumber"));
		}

		[Test]
		public void valid_capitalcalldistribution_capitalcallnumber_sets_1_error() {
			Assert.IsTrue(test_error_count("DistributionNumber", 0));
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
			formCollection.Add("DistributionAmount", "10000.00");
			formCollection.Add("CapitalDistributionDate", "1/1/1999");
			formCollection.Add("CapitalDistributionDueDate", "1/1/1999");
			formCollection.Add("DistributionNumber", "1");     
            return formCollection;
        }
    }
}
