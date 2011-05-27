using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IUnderlyingFundNAVHistoryService {
		void SaveUnderlyingFundNAVHistory(UnderlyingFundNAVHistory underlyingFundNAV);
	}
	public class UnderlyingFundNAVHistoryService : IUnderlyingFundNAVHistoryService {

		#region IUnderlyingFundNAVHistoryService Members

		public void SaveUnderlyingFundNAVHistory(UnderlyingFundNAVHistory underlyingFundNAV) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (underlyingFundNAV.UnderlyingFundNAVHistoryID == 0) {
					context.UnderlyingFundNAVHistories.AddObject(underlyingFundNAV);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("UnderlyingFundNAVHistories", underlyingFundNAV);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, underlyingFundNAV);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}