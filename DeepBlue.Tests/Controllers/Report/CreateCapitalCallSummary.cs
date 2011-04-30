using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Report;

namespace DeepBlue.Tests.Controllers.Report{
	public class CreateCapitalCallSummary : CapitalCallSummaryBase {

		protected CapitalCallSummaryModel   Model {
			get {
				return base.ViewResult.ViewData.Model as CapitalCallSummaryModel;
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
