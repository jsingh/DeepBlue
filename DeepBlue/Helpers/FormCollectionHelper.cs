using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace DeepBlue.Helpers {
	public class FormCollectionHelper {

		public static FormCollection GetFormCollection(FormCollection collection, int index) {
			FormCollection rowCollection = new FormCollection();
			string[] values; string value;
			foreach (string key in collection.Keys) {
				values = collection[key].Split((",").ToCharArray());
				if (values.Length > index)
					value = values[index];
				else
					value = string.Empty;
				if (value == "on") value = "true";
				rowCollection.Add(key, value);
			}
			return rowCollection;
		}

		public static FormCollection GetFormCollection(FormCollection collection, int rowIndex, Type type) {
			FormCollection rowCollection = new FormCollection();
			PropertyInfo[] propertyInfos;
			propertyInfos = type.GetProperties();
			string value;
			foreach (PropertyInfo propertyInfo in propertyInfos) {
				value = collection[rowIndex.ToString() + "_" + propertyInfo.Name];
				if (value != null) {
					if (value.Contains("true,"))
						value = "true";
					if (value.Contains("false,"))
						value = "false";
				}
				rowCollection.Add(propertyInfo.Name, value);
			}
			return rowCollection;
		}
	}
}