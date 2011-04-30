using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Investor;

namespace DeepBlue.Tests.Controllers.Investor
{
    public class FindinvestorData : Create
    {
        private ModelStateDictionary ModelState
        {
            get
            {
                return base.ViewResult.ViewData.ModelState;
            }
        }

		[SetUp]
		public override void Setup() {
			// Arrange
			base.Setup();
		}


        private void SetFormCollection()
        {
            base.ActionResult = base.DefaultController.FindInvestor(1);
        }

        #region Tests after model state is invalid

		[Test]
		public void returns_back_to_new_view_if_findinvestor_failed() 
		{	
			SetFormCollection();
			Assert.IsNotNull(Model);
		}

        #endregion
    }
}
