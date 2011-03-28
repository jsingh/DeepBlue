using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.CapitalCall;

namespace DeepBlue.Tests.Controllers.CapitalCall {
    public class CreateCapitalCallReqularValidData : CreateCapitalCallReqular {
        private CreateReqularModel  Model {
            get {
				return base.ViewResult.ViewData.Model as CreateReqularModel;
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
			MockCapiticalCallRepository.Setup(x => x.SaveCapitalCall(It.IsAny<DeepBlue.Models.Entity.CapitalCall>())).Returns(new List<Helpers.ErrorInfo>());
        }

        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(GetValidformCollection());
            base.ActionResult = base.DefaultController.Create(GetValidformCollection());
        }
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
			formCollection.Add("CapitalCallAmount", "10000.00");
			formCollection.Add("CapitalCallDate", "1/1/1999");
			formCollection.Add("CapitalCallDueDate", "1/1/1999");
			formCollection.Add("NewInvestmentAmount", "20000.00");         
            return formCollection;
        }
    }
}
