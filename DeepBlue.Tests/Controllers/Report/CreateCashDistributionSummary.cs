﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Report;

namespace DeepBlue.Tests.Controllers.Report{
	public class CreateCashDistributionSummary : CashDistributionSummaryBase {

		protected CashDistributionSummaryModel Model {
			get {
				return base.ViewResult.ViewData.Model as CashDistributionSummaryModel;
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
