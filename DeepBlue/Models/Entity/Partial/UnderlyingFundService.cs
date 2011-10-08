using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity.Partial {
	public interface IUnderlyingFundService {
		void SaveUnderlyingFund(UnderlyingFund underlyingFund);
	}

	public class UnderlyingFundService : IUnderlyingFundService {
		public void SaveUnderlyingFund(UnderlyingFund underlyingFund) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (underlyingFund.UnderlyingtFundID == 0) {
					context.UnderlyingFunds.AddObject(underlyingFund);
				}
				else {
					UnderlyingFund updateUnderlyingFund = context.UnderlyingFunds.SingleOrDefault(deepblueUnderlyingFund => deepblueUnderlyingFund.UnderlyingtFundID == underlyingFund.UnderlyingtFundID);
					//Update underlyingFund,underlyingFund account values
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key;
					object originalItem;

					if (underlyingFund.Account != null) {
						/* Account */
						originalItem = null;
						key = default(EntityKey);
						key = context.CreateEntityKey("Accounts", underlyingFund.Account);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, underlyingFund.Account);
						}
						else {
							updateUnderlyingFund.Account = new Account {
								Account1 = underlyingFund.Account.Account1,
								AccountNumberCash = underlyingFund.Account.AccountNumberCash,
								AccountOf = underlyingFund.Account.AccountOf,
								Attention = underlyingFund.Account.Attention,
								BankName = underlyingFund.Account.BankName,
								CreatedBy = underlyingFund.Account.CreatedBy,
								CreatedDate = underlyingFund.Account.CreatedDate,
								EntityID = underlyingFund.Account.EntityID,
								Fax = underlyingFund.Account.Fax,
								FFCNumber = underlyingFund.Account.FFCNumber,
								IBAN = underlyingFund.Account.IBAN,
								IsPrimary = underlyingFund.Account.IsPrimary,
								LastUpdatedBy = underlyingFund.Account.LastUpdatedBy,
								LastUpdatedDate = underlyingFund.Account.LastUpdatedDate,
								Phone = underlyingFund.Account.Phone,
								Reference = underlyingFund.Account.Reference,
								Routing = underlyingFund.Account.Routing,
								SWIFT = underlyingFund.Account.SWIFT,
								Comments = underlyingFund.Account.Comments,
								FFC = underlyingFund.Account.FFC,
								ByOrderOf = underlyingFund.Account.ByOrderOf
							};
						}
						/* End Account */
					}
					originalItem = null;
					key = default(EntityKey);
					key = context.CreateEntityKey("UnderlyingFunds", underlyingFund);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, underlyingFund);
					}
				}
				context.SaveChanges();
			}
		}
	}
}