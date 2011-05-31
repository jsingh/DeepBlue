using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IFundActivityHistoryService {
		void SaveFundActivityHistory(FundActivityHistory fundActivityHistory);
	}
	public class FundActivityHistoryService : IFundActivityHistoryService {

		#region IFundActivityHistoryService Members

		public void SaveFundActivityHistory(FundActivityHistory fundActivityHistory) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (fundActivityHistory.FundActivityHistoryID == 0) {
					context.FundActivityHistories.AddObject(fundActivityHistory);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("FundActivityHistories", fundActivityHistory);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, fundActivityHistory);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}