using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {

	public static class EntityExtension {

		public static EntityPermission GetEntityPermission(this List<EntityPermission> permissions, Table table) {
			return permissions.Where(p => p.TableName == table).FirstOrDefault();
		}

	}
	
}