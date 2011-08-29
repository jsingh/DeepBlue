using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;
using System.Data.Objects.DataClasses;
using System.Data;

namespace DeepBlue.Models.Entity {
	
	public interface IInvestorService {
		void SaveInvestor(Investor investor);
	}

	public class InvestorService : IInvestorService {
		public void SaveInvestor(Investor investor) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (investor.InvestorID == 0) {
					context.Investors.AddObject(investor);
				}
				else {
					EntityKey key;
					object originalItem;
					key = default(EntityKey);
					originalItem = null;
					key = context.CreateEntityKey("Investors", investor);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, investor);
					}
				}
				context.SaveChanges();
			}

		}
	}
	 
}