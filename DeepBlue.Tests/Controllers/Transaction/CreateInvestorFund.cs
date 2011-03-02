using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Transaction;

namespace DeepBlue.Tests.Controllers.Transaction {
	public class CreateInvestorFund : InvestorFundBase {

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
	 
	}
}
