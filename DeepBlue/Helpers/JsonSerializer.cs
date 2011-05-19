using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DeepBlue.Helpers {
	public class JsonSerializer {
		public static string ToJsonObject(object obj) {
			JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
			return jsonSerialize.Serialize(obj);
		}
	}
}