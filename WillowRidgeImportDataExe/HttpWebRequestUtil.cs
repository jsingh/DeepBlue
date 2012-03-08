using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;

namespace DeepBlue.ImportData {
	public class HttpWebRequestUtil {
		public static string LoginPostUrl = "Account/LogOn";
		public static string HttpGet(string URI) {
			System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
			//req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
			System.Net.WebResponse resp = req.GetResponse();
			System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
			return sr.ReadToEnd().Trim();
		}
		public static string JsonContentType = "application/json; charset=utf-8";

		public static string GetUrl(string relativeUrl) {
			if (relativeUrl.StartsWith("/")) {
				relativeUrl = relativeUrl.Substring(1, relativeUrl.Length - 1);
			}
			return Globals.BaseUrl + "/" + relativeUrl;
		}

		public static string HttpPost(string URI, string Parameters) {
			System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
			//req.Proxy = new System.Net.WebProxy(ProxyString, true);
			//Add these, as we're doing a POST
			req.ContentType = "application/x-www-form-urlencoded";
			req.Method = "POST";
			//We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
			byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
			req.ContentLength = bytes.Length;
			System.IO.Stream os = req.GetRequestStream();
			os.Write(bytes, 0, bytes.Length); //Push it out there
			os.Close();
			System.Net.WebResponse resp = req.GetResponse();
			if (resp == null) return null;
			System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
			return sr.ReadToEnd().Trim();
		}

		public static bool Login(string Username, string Password, string EntityCode, CookieCollection cookies, bool allowAutoRedirect = false) {
			string url = Globals.BaseUrl + "/" + LoginPostUrl;
			string poststring = string.Format("UserName={0}&Password={1}&EntityCode={2}",
											Username, Password, EntityCode);
			byte[] postdata = Encoding.UTF8.GetBytes(poststring);

			HttpWebResponse webResponse = SendRequest(url, postdata, true, cookies);
			return webResponse.StatusCode == HttpStatusCode.OK;
		}

		public static bool LoginPortal(string Username, string Password, string EntityCode, CookieCollection cookies, bool allowAutoRedirect = false) {
			string url = Globals.BaseUrl + "/" + LoginPostUrl;
			string poststring = string.Format("UserName={0}&Password={1}&EntityCode={2}",
										Username, Password, EntityCode);

			byte[] postdata = Encoding.UTF8.GetBytes(poststring);

			HttpWebResponse webResponse = SendRequest(url, postdata, true, cookies, allowAutoRedirect);
			return webResponse.StatusCode == HttpStatusCode.OK;
		}


		public static HttpWebResponse FollowUrl(CookieCollection cookies, string urlToFollow) {
			string url = Globals.BaseUrl + "/" + urlToFollow;
			return SendRequest(url, null, false, cookies);
		}

		public static HttpWebResponse SendRequest(string url, byte[] postData, bool isPost, CookieCollection cookies, bool allowAutoRedirect = false, string contentType = null) {
			string requestMethod = isPost ? "POST" : "GET";

			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
			// AllowAutoRedirect = false means HttpWebRequest will not automatically follow 302 response
			webRequest.AllowAutoRedirect = allowAutoRedirect;
			webRequest.CookieContainer = new CookieContainer();
			foreach (Cookie cookie in cookies) {
				webRequest.CookieContainer.Add(cookie);
			}
			webRequest.Method = requestMethod;
			webRequest.Referer = url;
			webRequest.Headers.Add("origin", Globals.BaseUrl);
			webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727; InfoPath.2; .NET CLR 3.5.21022;";

			HttpWebResponse webResponse = null;
			if (isPost) {
				if (string.IsNullOrEmpty(contentType)) {
					webRequest.ContentType = "application/x-www-form-urlencoded";
				}
				else {
					webRequest.ContentType = contentType;
				}
				webRequest.ContentLength = postData.Length;
				using (Stream writer = webRequest.GetRequestStream()) {
					writer.Write(postData, 0, postData.Length);
				}
				webResponse = (HttpWebResponse)webRequest.GetResponse();
			}
			else {
				if (!string.IsNullOrEmpty(contentType)) {
					webRequest.ContentType = contentType;
				}
				webRequest.ContentLength = 0;
				//req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
				webResponse = (HttpWebResponse)webRequest.GetResponse();
			}

			string returnedCookie = string.Empty;
			foreach (Cookie c in webResponse.Cookies) {
				returnedCookie += string.Format("  CookieName:{0}, Value:{1} ", c.Name, c.Value);
			}
			//We need to add any response cookies to our cookie container
			cookies.Add(webResponse.Cookies);

			////Only for debug
			//using (var stream = new StreamReader(webResponse.GetResponseStream())) {
			//    System.Diagnostics.Debug.WriteLine(stream.ReadToEnd());
			//}

			if (webResponse.StatusCode == HttpStatusCode.Found) {
				string locationtoRedirect = Globals.BaseUrl;
				if (webResponse.Headers["Location"].StartsWith("/") == false) {
					locationtoRedirect += "/";
				}
				locationtoRedirect += webResponse.Headers["Location"];
				webResponse = SendRequest(locationtoRedirect, null, false, cookies);
			}

			return webResponse;

		}

		public static NameValueCollection SetUpForm<T>(T obj, string keyPrefix, string valuePrefix, string[] excludedProperties = null) {
			try {
				if (excludedProperties == null) {
					excludedProperties = new string[0];
				}
				NameValueCollection collection = new NameValueCollection();
				Type type = obj.GetType();
				PropertyInfo[] properties = type.GetProperties();
				foreach (PropertyInfo ppty in properties) {
					if (!excludedProperties.Contains(ppty.Name)) {
						Type propertyType = ppty.PropertyType;
						object val = ppty.GetValue(obj, null);
						collection.Add(keyPrefix + "" + ppty.Name, valuePrefix + (val == null ? string.Empty : val.ToString()));
					}
				}
				return collection;
			}
			catch (Exception ex) {
				Util.Log("SetUpForm -> " + ex.Message);
				return null;
			}
		}

		public static byte[] GetPostData<T>(T obj, string keyPrefix, string valuePrefix) {
			NameValueCollection collection = SetUpForm(obj, keyPrefix, valuePrefix);
			return System.Text.Encoding.ASCII.GetBytes(ToFormValue(collection));
		}

		public static string ToFormValue(NameValueCollection collection) {
			StringBuilder builder = new StringBuilder();
			foreach (string key in collection.Keys) {
				// Encode the name value pair before you assemble it, in case either the name of the pair contains any special characters, like &
				builder.Append(string.Format("{0}={1}", System.Web.HttpUtility.UrlEncode(key), System.Web.HttpUtility.UrlEncode(collection[key])));
				builder.Append("&");
			}
			string retVal = builder.ToString();
			return retVal.Length > 0 ? retVal.Substring(0, retVal.Length - 1) : retVal;
		}

		public static int? GetNewKeyFromResponse(string resp) {
			int? pk = null;
			if (!string.IsNullOrEmpty(resp)) {
				if (resp.ToLower().StartsWith("true")) {
					string[] parts = resp.Split(new string[] { "||" }, StringSplitOptions.None);
					if (parts.Length >= 1) {
						int id = 0;
						if (Int32.TryParse(parts[1], out id)) {
							pk = id;
						}
					}
				}
			}
			return pk;
		}
	}
}