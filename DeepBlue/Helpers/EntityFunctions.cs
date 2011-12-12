using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;

namespace DeepBlue.Helpers {
	public class EntityFunctions2 {

		[EdmFunction("DeepBlueEntities.String", "CovertString")]
		public static string ConvertString(int value) {
			throw new NotSupportedException();
		}
	}
}