using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Fund;

namespace DeepBlue.Tests.Controllers.Fund {
    public class CreateValidData : Create {
        private FundDetail  Model {
            get {
				return base.ViewResult.ViewData.Model as FundDetail;
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
            // Test if the SaveInvestor call fails
            MockFundRepository.Setup(x => x.SaveFund(It.IsAny<DeepBlue.Models.Entity.Fund>())).Returns(new List<Helpers.ErrorInfo>());
        }

        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(GetValidformCollection());
            base.ActionResult = base.DefaultController.Create(GetValidformCollection());
        }
        #region Tests after model state is invalid
        

		//[Test]
		//public void returns_back_to_new_view_if_saving_fund_failed() {
		//    SetFormCollection();
		//    Assert.IsNotNull(Model);
		//}

        #endregion
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
            formCollection.Add("FundName", "n/a");
            formCollection.Add("TaxID", "1");
            formCollection.Add("InceptionDate", "1/1/1999");
			formCollection.Add("BankName","n/a");
			formCollection.Add("Account","n/a"); 
            return formCollection;
        }
    }
}
