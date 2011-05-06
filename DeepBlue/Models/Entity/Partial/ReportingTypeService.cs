using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IReportingTypeService {
		void SaveReportingType(ReportingType reportingType);
	}
	public class ReportingTypeService : IReportingTypeService {

		#region IReportingTypeService Members

		public void SaveReportingType(ReportingType ReportingType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (ReportingType.ReportingTypeID == 0) {
					context.ReportingTypes.AddObject(ReportingType);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("ReportingTypes", ReportingType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, ReportingType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}