using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.Document {
	public interface IDocumentRepository {

		#region Document
		List<DocumentType> GetAllDocumentTypes();
		IEnumerable<ErrorInfo> SaveDocument(InvestorFundDocument investorFundDocument);
		#endregion
 
	}
}
