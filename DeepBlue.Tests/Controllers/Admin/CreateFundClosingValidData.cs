using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Admin;

namespace DeepBlue.Tests.Controllers.Admin {
    public class CreateFundClosingValidData :EditFundClosing {
        private EditFundClosingModel   Model {
            get {
				return base.ViewResult.ViewData.Model as EditFundClosingModel;
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
            MockAdminRepository.Setup(x => x.SaveFundClosing(It.IsAny<DeepBlue.Models.Entity.FundClosing>())).Returns(new List<Helpers.ErrorInfo>());
        }

        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(GetValidformCollection());
            base.ActionResult = base.DefaultController.UpdateFundClosing(GetValidformCollection());
        }

        #region Tests after model state is invalid
        

        [Test]
        public void returns_back_to_new_view_if_saving_fun_failed() {
            SetFormCollection();
            Assert.IsNull(Model);
        }

        #endregion
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
			formCollection.Add("FundID", "1");
			formCollection.Add("FundClosingDate","2011-02-26");
			formCollection.Add("Name","n/a");
            return formCollection;
        }
    }
}
