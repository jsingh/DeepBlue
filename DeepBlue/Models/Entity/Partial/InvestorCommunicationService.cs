using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity.Partial {
	public interface IInvestorCommunicationService {
		void SaveInvestorCommunication(InvestorCommunication investorCommunication);
	}

	public class InvestorCommunicationService : IInvestorCommunicationService {
		public void SaveInvestorCommunication(InvestorCommunication investorCommunication) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (investorCommunication.InvestorCommunicationID == 0) {
					context.InvestorCommunications.AddObject(investorCommunication);
				}
				else {
					EntityKey key;
					object originalItem;
					key = default(EntityKey);
					originalItem = null;
					key = context.CreateEntityKey("InvestorCommunications", investorCommunication);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, investorCommunication);
					}
					key = context.CreateEntityKey("Communications", investorCommunication.Communication);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, investorCommunication.Communication);
					}
				}
				context.SaveChanges();
			}
		}
	}
}