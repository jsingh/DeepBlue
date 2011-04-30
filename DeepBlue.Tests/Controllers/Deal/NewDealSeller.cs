using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Deal;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Deal ;

namespace DeepBlue.Tests.Controllers.Deal {
	public class NewDealSeller :DealSellerBase {

        private DealSellerDetailModel Model {
            get {
				return base.ViewResult.ViewData.Model as DealSellerDetailModel;
            }
        }
        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
			base.ActionResult = base.DefaultController.New();
        }
 
		[Test]
		public void create_a_new_Deal() {
			Assert.IsInstanceOfType<ActionResult>(base.ActionResult);
		}
    }
}
