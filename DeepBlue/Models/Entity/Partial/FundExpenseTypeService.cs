﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IFundExpenseTypeService {
		void SaveFundExpenseType(FundExpenseType fundExpenseType);
	}
	public class FundExpenseTypeService : IFundExpenseTypeService {

		#region IFundExpenseTypeService Members

		public void SaveFundExpenseType(FundExpenseType fundExpenseType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (fundExpenseType.FundExpenseTypeID == 0) {
					context.FundExpenseTypes.AddObject(fundExpenseType);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("FundExpenseTypes", fundExpenseType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, fundExpenseType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}