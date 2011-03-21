using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Admin;

namespace DeepBlue.Tests.Controllers.Admin {
    public class CreateCustomValidData : EditCustomField {
        private EditCustomFieldModel   Model {
            get {
				return base.ViewResult.ViewData.Model as EditCustomFieldModel;
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
            MockAdminRepository.Setup(x => x.SaveCustomField(It.IsAny<DeepBlue.Models.Entity.CustomField>())).Returns(new List<Helpers.ErrorInfo>());
        }

        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(GetValidformCollection());
            base.ActionResult = base.DefaultController.UpdateCustomField(GetValidformCollection());
        }

        #region Tests after model state is invalid
        

        [Test]
        public void returns_back_to_new_view_if_saving_customfield_failed() {
            SetFormCollection();
            Assert.IsNull(Model);
        }

        #endregion
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
			formCollection.Add("ModuleID", "1");
			formCollection.Add("DataTypeID","1");
			formCollection.Add("CustomFieldText","n/a");
            return formCollection;
        }
    }
}
