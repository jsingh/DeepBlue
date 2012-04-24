using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace DeepBlue.Helpers {

	public class FakeView : IView {
		#region IView Members

		public void Render(ViewContext viewContext, TextWriter writer) {
			throw new NotImplementedException();
		}

		#endregion
	}
}