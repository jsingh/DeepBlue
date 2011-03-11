using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
					context.Files.SingleOrDefault(file => file.FileID == investorFundDocument.File.FileID);
					context.Files.ApplyCurrentValues(investorFundDocument.File);
					context.InvestorFundDocuments.SingleOrDefault(document => document.InvestorFundDocumentID == investorFundDocument.InvestorFundDocumentID);
					context.InvestorFundDocuments.ApplyCurrentValues(investorFundDocument);
				}
				context.SaveChanges();
			}
		}
	}
}