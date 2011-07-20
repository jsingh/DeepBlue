using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace DeepBlue.Helpers {

	public class MvcTab : IDisposable {
		private bool _disposed;
		private HttpResponseBase _httpResponse;

		public delegate void DisposeEventHandler(HttpResponseBase httpResponse);
		public event DisposeEventHandler OnDispose;

		public MvcTab(HttpResponseBase httpResponse) {
			_httpResponse = httpResponse;
		}

		[SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")]
		public void Dispose() {
			Dispose(true /* disposing */);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing) {
			if (!_disposed) {
				_disposed = true;
				if (this.OnDispose != null) {
					this.OnDispose(_httpResponse);
				}
				_httpResponse.Write("</div><div class=right></div></div>");
			}
		}

		public void EndTab() {
			Dispose(true);
		}
	}
}
