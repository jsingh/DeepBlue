﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Deal;

namespace DeepBlue.Tests.Controllers.Deal {
	public class CreateUnderlyingFundPostRecordCapitalCall : UnderlyingFundPostRecordCapitalCallBase  {

		protected UnderlyingFundPostRecordCapitalCallModel Model {
			get {
				return base.ViewResult.ViewData.Model as UnderlyingFundPostRecordCapitalCallModel;
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
