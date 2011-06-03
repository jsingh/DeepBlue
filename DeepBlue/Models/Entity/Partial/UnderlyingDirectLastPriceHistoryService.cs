using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IUnderlyingDirectLastPriceHistoryService {
		void SaveUnderlyingDirectLastPriceHistory(UnderlyingDirectLastPriceHistory underlyingDirectLastPriceHistory);
	}
	public class UnderlyingDirectLastPriceHistoryService : IUnderlyingDirectLastPriceHistoryService {

		#region IUnderlyingDirectLastPriceHistoryService Members

		public void SaveUnderlyingDirectLastPriceHistory(UnderlyingDirectLastPriceHistory underlyingDirectLastPriceHistory) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (underlyingDirectLastPriceHistory.UnderlyingDirectLastPriceHistoryID == 0) {
					context.UnderlyingDirectLastPriceHistories.AddObject(underlyingDirectLastPriceHistory);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("UnderlyingDirectLastPriceHistories", underlyingDirectLastPriceHistory);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, underlyingDirectLastPriceHistory);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}