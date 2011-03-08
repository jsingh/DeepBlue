using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DeepBlue.Helpers {

	public class FlexigridRow {
		public List<string> cell = new List<string>();
	}

	public class FlexigridData {
		public int page;
		public int total;
		public List<FlexigridRow> rows = new List<FlexigridRow>();
	}

	public class JsonSerializer {
		public static string ToJsonObject(FlexigridData flexgridData) {
			 JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
			 return jsonSerialize.Serialize(flexgridData);
		}
	}
}