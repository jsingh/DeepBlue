using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity.Partial {
	public interface IInvestorAddressService {
		void SaveInvestorAddress(InvestorAddress investorAddress);
	}

	public class InvestorAddressService : IInvestorAddressService {
		public void SaveInvestorAddress(InvestorAddress investorAddress) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (investorAddress.InvestorAddressID == 0) {
					context.InvestorAddresses.AddObject(investorAddress);
				}
				else {
					EntityKey key;
					object originalItem;
					key = default(EntityKey);
					originalItem = null;
					key = context.CreateEntityKey("InvestorAddresses", investorAddress);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, investorAddress);
					}
					key = context.CreateEntityKey("Addresses", investorAddress.Address);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, investorAddress.Address);
					}
				}
				context.SaveChanges();
			}

		}
	}
}