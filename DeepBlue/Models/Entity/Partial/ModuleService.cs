using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Entity {
	public interface IModuleService {
		void SaveModule(MODULE module);
	}
	public class ModuleService : IModuleService {

		#region IModuleService Members

		public void SaveModule(MODULE module) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (module.ModuleID == 0) {
					context.MODULEs.AddObject(module);
				} else {
					context.MODULEsTable.SingleOrDefault(entityType => entityType.ModuleID == module.ModuleID);
					context.MODULEs.ApplyCurrentValues(module);
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}