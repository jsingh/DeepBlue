using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IUnderlyingDirectLastPriceService {
		void SaveUnderlyingDirectLastPrice(UnderlyingDirectLastPrice underlyingDirectLastPrice);
	}
	public class UnderlyingDirectLastPriceService : IUnderlyingDirectLastPriceService {

		#region IUnderlyingDirectLastPriceService Members

		public void SaveUnderlyingDirectLastPrice(UnderlyingDirectLastPrice underlyingDirectLastPrice) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (underlyingDirectLastPrice.UnderlyingDirectLastPriceID == 0) {
					context.UnderlyingDirectLastPrices.AddObject(underlyingDirectLastPrice);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("UnderlyingDirectLastPrices", underlyingDirectLastPrice);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, underlyingDirectLastPrice);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}