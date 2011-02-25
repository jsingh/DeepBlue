using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Investor;

namespace DeepBlue.Tests.Controllers.Investor {
	public class Create : InvestorBase {

        protected CreateModel Model {
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
            base.Setup();
        }


        protected void Create_Invalid_Data(CreateModel model) {
            model.InvestorName = model.Alias = model.Phone = model.Email = model.Address1 = model.City = model.Zip = string.Empty;
        }
    }
}
