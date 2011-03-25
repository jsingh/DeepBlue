using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Fund;

namespace DeepBlue.Tests.Controllers.FundSetup {
	public class Create : FundBase {

		protected FundDetail  Model {
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
			base.Setup();
		}
		
	}
}
