using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {

	public interface ILoggingService {
		void SaveLog(Log log);
	}

	public class LoggingService : ILoggingService {

		#region ILoggingService Members

		public void SaveLog(Log log) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (log.LogID == 0) {
					context.Logs.AddObject(log);
					if (!string.IsNullOrEmpty(log.AdditionalDetail)) {
						context.LogDetails.AddObject(new LogDetail() { Detail = log.AdditionalDetail });
					}
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("Logs", log);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, log);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}
