using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {

	public interface IAccountingEntryService {
		void SaveAccountingEntry(AccountingEntry accountingEntry);
	}

	public class AccountingEntryService : IAccountingEntryService {

		#region IAccountingEntryService Members

		public void SaveAccountingEntry(AccountingEntry accountingEntry) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (accountingEntry.AccountingEntryID == 0) {
					context.AccountingEntries.AddObject(accountingEntry);
				} else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("AccountingEntries", accountingEntry);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, accountingEntry);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
 
	 
}