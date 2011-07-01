using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace DeepBlue.Helpers {

	public class ProgressModule : IHttpModule {

		/// <summary>
		/// <para>Handle the request</para>
		/// </summary>
		void BeginRequest(object sender, EventArgs e) {
			var application = sender as HttpApplication;

			// check for a progressId in the query string
			if (!string.IsNullOrEmpty(application.Request.QueryString["progressId"])) {
				var progressId = application.Request.QueryString["progressId"];

				if (application.Request.Path == "/Progress/") {
					// stop caching of the progress by the browser
					application.Response.CacheControl = "no-cache";
					application.Response.Expires = 0;

					if (_requests != null
													&& _requests.ContainsKey(progressId)) {
						application.Response.Write(
							Result(_requests[progressId].Progress, _requests[progressId].UploadData, _requests[progressId].TotalData));
						application.Response.End();

						return;
					}
					// returns 100 percent if the progressId is not found
					application.Response.Write(Result(0, string.Empty, string.Empty));
					application.Response.End();
				}
				else {

					// process an incomming request
					var workerRequest = (HttpContext.Current as IServiceProvider)
									.GetService(typeof(HttpWorkerRequest)) as HttpWorkerRequest;

					var info = new RequestReader {
						Request = workerRequest,
						Length = application.Request.ContentLength
					};

					lock (lockObject) {
						// store statically so that it can be reached by progress requests
						Requests.Add(progressId, info);
					}

					try {

						info.ReadAll();
					}
					finally {
						if (Requests.ContainsKey(progressId)) {
							lock (lockObject) {
								// remove from static storage
								Requests.Remove(progressId);
							}
						}
					}
				}
			}
		}

		public string Result(int percentage, string uploadData, string totalData) {
			return JsonSerializer.ToJsonObject(new { percentage = percentage, uploadData = uploadData, totalData = totalData }).ToString();
		}

		#region IHttpModule Members

		public void Dispose() { }

		/// <summary>
		/// <para>Initialise, hook up BeginRequest</para>
		/// </summary>
		/// <param name="context"></param>
		public void Init(HttpApplication context) {
			context.BeginRequest += BeginRequest;
		}

		#endregion

		#region request

		private static Dictionary<string, RequestReader> _requests = null;
		private static readonly object lockObject = new object();

		/// <summary>
		/// <para>Static storage for Requests to that progress can be reported on</para>
		/// </summary>
		private static Dictionary<string, RequestReader> Requests {
			get {
				return _requests == null
								? _requests = new Dictionary<string, RequestReader>()
								: _requests;
			}
		}

		/// <summary>
		/// <para>Request Reader</para>
		/// </summary>
		private class RequestReader {

			public HttpWorkerRequest Request { get; set; }
			public int Position { get; set; }
			public int Length { get; set; }
			private const int chunkSize = 1024;
			private const byte maxEmptyReadCount = 50;

			private string _UploadData = string.Empty;
			public string UploadData { get { return _UploadData; } }

			private string _TotalData = string.Empty;
			public string TotalData { get { return _TotalData; } }

			/// <summary>
			/// <para>Get current progress 0-100</para>
			/// </summary>
			public byte Progress {
				get {
					return (byte)Math.Round(100D * Position / Length);
				}
			}

			/// <summary>
			/// <para>Read all data in the request and pass on as preloaded content</para>
			/// </summary>
			public void ReadAll() {

				// as the request can be quite long, store the file to disk to avoid out of memory problems
				var requestFileName = Path.GetTempFileName();
				//		var requestFile = File.Create(requestFileName);
				try {

					Position = Request.GetPreloadedEntityBodyLength();
					//if (Position > 0)
					//{
					//  requestFile.Write(Request.GetPreloadedEntityBody(), 0, Position);
					//}

					var emptyReadCount = 0;
					while (Position < Length
									&& Request.IsClientConnected()) {
						// check for last chunk
						var buffer = new byte[
										Position + chunkSize > Length ? Length - Position : chunkSize];

						int read = Request.ReadEntityBody(buffer, chunkSize);

						if (read == 0) {
							emptyReadCount++;
							if (emptyReadCount > maxEmptyReadCount) break;
						}
						else {
							emptyReadCount = 0;
							//requestFile.Write(buffer, 0, read);
							Position += read;
						}
						_UploadData = Math.Round(((Decimal)Position / 1024) / 1024, 2).ToString("#0.00") + " mb";
						_TotalData = Math.Round(((Decimal)Length / 1024) / 1024, 2).ToString("#0.00") + " mb";
					}

					// check request was completed
					if (Position == Length) {

						var type = Request.GetType();
						if (type != null) {
							// set content as preloaded so that the normal processing can be done
							var requestData = new byte[Length];
							//	requestFile.Seek(0, 0);
							//		requestFile.Read(requestData, 0, Length);

							type.GetField(
															"_preloadedContent",
															BindingFlags.Instance | BindingFlags.NonPublic)
											.SetValue(Request, requestData);
							type.GetField(
															"_contentLength",
															BindingFlags.Instance | BindingFlags.NonPublic)
											.SetValue(Request, Length);
						}
					}

					//requestFile.Close();
					//	requestFile.Dispose();
				}
				finally {
					//	File.Delete(requestFileName);
				}
			}
		}

		#endregion

	}
}