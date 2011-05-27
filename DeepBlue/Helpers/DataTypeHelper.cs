﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {
	public class DataTypeHelper {

		public static decimal ToDecimal(string value) {
			return (string.IsNullOrEmpty(value) ? 0 : Convert.ToDecimal(value));
		}

		public static Int32 ToInt32(string value) {
			return (string.IsNullOrEmpty(value) ? 0 : Convert.ToInt32(value));
		}
	}
}