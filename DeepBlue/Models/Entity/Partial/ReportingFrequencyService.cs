using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IReportingFrequencyService {
		void SaveReportingFrequency(ReportingFrequency reportingFrequency);
	}
	public class ReportingFrequencyService : IReportingFrequencyService {

		#region IReportingFrequencyService Members

		public void SaveReportingFrequency(ReportingFrequency ReportingFrequency) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (ReportingFrequency.ReportingFrequencyID == 0) {
					context.ReportingFrequencies.AddObject(ReportingFrequency);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("ReportingFrequencies", ReportingFrequency);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, ReportingFrequency);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}