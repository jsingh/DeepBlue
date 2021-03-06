﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.CapitalCall;

namespace DeepBlue.Tests.Controllers.CapitalCall {
	public class CreateCapitalCallDistribution : CapitalCallDistributionBase {

		protected CreateDistributionModel  Model {
			get {
				return base.ViewResult.ViewData.Model as CreateDistributionModel;
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
