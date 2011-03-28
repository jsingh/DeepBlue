using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Investor;

namespace DeepBlue.Tests.Controllers.Investor {
    public class CreateValidData : Create {
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
            // Test if the SaveInvestor call fails
            MockRepository.Setup(x => x.SaveInvestor(It.IsAny<DeepBlue.Models.Entity.Investor>())).Returns(new List<Helpers.ErrorInfo>());
        }

        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(GetValidformCollection());
            base.ActionResult = base.DefaultController.Create(GetValidformCollection());
        }
        #region Tests after model state is invalid
        

        [Test]
		//public void returns_back_to_new_view_if_saving_investor_failed() {
		//    SetFormCollection();
		//    Assert.IsNotNull(Model);
		//}

        #endregion
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
            formCollection.Add("InvestorName", "n/a");
            formCollection.Add("Alias", "n/a");
            formCollection.Add("Phone", "2547331111");
            formCollection.Add("Email", "test@email.com");
            formCollection.Add("Address1", "123 Main street");
            formCollection.Add("City", "New York");
            formCollection.Add("StateOfResidency", "1");
            formCollection.Add("EntityType", "1");
            formCollection.Add("SocialSecurityTaxId", "111-11-1111");
            formCollection.Add("Zip", "11005");
            return formCollection;
        }
    }
}
