using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IFundExpenseService {
		void SaveFundExpense(FundExpense fundExpense);
	}
	public class FundExpenseService : IFundExpenseService {

		#region IFundExpenseService Members

		public void SaveFundExpense(FundExpense fundExpense) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (fundExpense.FundExpenseID == 0) {
					context.FundExpenses.AddObject(fundExpense);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("FundExpenses", fundExpense);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, fundExpense);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}