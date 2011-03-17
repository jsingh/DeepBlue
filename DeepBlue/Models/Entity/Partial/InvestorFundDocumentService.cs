using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IInvestorFundDocumentService {
		void SaveInvestorFundDocument(InvestorFundDocument investor);
	}

	public class InvestorFundDocumentService : IInvestorFundDocumentService {
		public void SaveInvestorFundDocument(InvestorFundDocument investorFundDocument) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (investorFundDocument.InvestorFundDocumentID == 0) {
					context.InvestorFundDocuments.AddObject(investorFundDocument);
				} else {
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("Files", investorFundDocument.File);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, investorFundDocument.File);
					}
					key = default(EntityKey);
					originalItem = null;
					key = context.CreateEntityKey("InvestorFundDocuments", investorFundDocument);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, investorFundDocument);
					}
				}
				context.SaveChanges();
			}
		}
	}
}