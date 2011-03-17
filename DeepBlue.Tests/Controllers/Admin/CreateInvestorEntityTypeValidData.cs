using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Admin;

namespace DeepBlue.Tests.Controllers.Admin {
    public class CreateInvestorEntityTypeValidData :EditInvestorEntityType {
        private EditInvestorEntityTypeModel Model {
            get {
				return base.ViewResult.ViewData.Model as EditInvestorEntityTypeModel;
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
            MockRepository.Setup(x => x.SaveInvestorEntityType(It.IsAny<DeepBlue.Models.Entity.InvestorEntityType>())).Returns(new List<Helpers.ErrorInfo>());
        }

        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(GetValidformCollection());
            base.ActionResult = base.DefaultController.UpdateInvestorEntityType(GetValidformCollection());
        }

        #region Tests after model state is invalid
        

        [Test]
        public void returns_back_to_new_view_if_saving_investorentitytype_failed() {
            SetFormCollection();
            Assert.IsNotNull(Model);
        }

        #endregion
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
			formCollection.Add("InvestorEntityTypeName", "n/a");
            return formCollection;
        }
    }
}
