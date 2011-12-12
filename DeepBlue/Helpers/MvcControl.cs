using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics.CodeAnalysis;

namespace DeepBlue.Helpers {
	public class MvcControl : IDisposable {

		private string _tagName;
		private bool _disposed;
		private HttpResponseBase _httpResponse;

		public delegate void DisposeEventHandler(HttpResponseBase httpResponse);
		public event DisposeEventHandler OnDispose;

		public MvcControl(HttpResponseBase httpResponse, string tagName) {
			_httpResponse = httpResponse;
			_tagName = tagName;
		}

		[SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")]
		public void Dispose() {
			Dispose(true /* disposing */);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing) {
			if (!_disposed) {
				_disposed = true;
                if (this.OnDispose != null)
                {
                    this.OnDispose(_httpResponse);
                }
				_httpResponse.Write("</" + _tagName + ">");
			}
		}

	 
	}
}