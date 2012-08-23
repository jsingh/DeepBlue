using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IAccountingEntryTemplateService {
		void SaveAccountingEntryTemplate(AccountingEntryTemplate accountingEntryTemplate);
	}

	public class AccountingEntryTemplateService : IAccountingEntryTemplateService {

		#region IAccountingEntryTemplateService Members

		public void SaveAccountingEntryTemplate(AccountingEntryTemplate accountingEntryTemplate) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (accountingEntryTemplate.AccountingEntryTemplateID == 0) {
					context.AccountingEntryTemplates.AddObject(accountingEntryTemplate);
				} else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("AccountingEntryTemplates", accountingEntryTemplate);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, accountingEntryTemplate);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}