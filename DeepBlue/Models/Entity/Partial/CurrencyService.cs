using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface ICurrencyService {
		void SaveCurrency(Currency currency);
	}
	public class CurrencyService : ICurrencyService {

		#region ICurrencyService Members

		public void SaveCurrency(Currency currency) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (currency.CurrencyID == 0) {
					context.Currencies.AddObject(currency);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("Currencies", currency);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, currency);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}