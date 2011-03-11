using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.Document {
	public class DocumentRepository : IDocumentRepository {

		#region IDocumentRepository Members

		public IEnumerable<ErrorInfo> SaveDocument(InvestorFundDocument investorFundDocument) {
			return investorFundDocument.Save();
		}
	 
		public List<DocumentType> GetAllDocumentTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from document in context.DocumentTypes
						orderby document.DocumentTypeName
						select document).ToList();
			}
		}

		#endregion
	}
}