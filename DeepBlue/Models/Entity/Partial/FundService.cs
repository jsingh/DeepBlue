using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IFundService {
		void SaveFund(Fund fund);
	}

	public class FundService : IFundService {
		public void SaveFund(Fund fund) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (fund.FundID == 0) {
					context.Funds.AddObject(fund);
				} else {
					//Update fund,fund account values
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key; 
					object originalItem;
					foreach (var fundAccount in fund.FundAccounts) {
						originalItem = null;
						key = default(EntityKey);
						key = context.CreateEntityKey("FundAccounts", fundAccount);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, fundAccount);
						}
					}
					foreach (var fundClosing in fund.FundClosings) {
						originalItem = null;
						key = default(EntityKey);
						key = context.CreateEntityKey("FundClosings", fundClosing);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, fundClosing);
						}
					}
					originalItem = null;
					key = default(EntityKey);
					key = context.CreateEntityKey("Funds", fund);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, fund);
					}
				}
				context.SaveChanges();
			}
		}
	}
}