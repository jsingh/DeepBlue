using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Models.Document;

namespace DeepBlue.Controllers.Document {
	public interface IDocumentRepository {

		#region Document
		List<DocumentType> GetAllDocumentTypes();
		IEnumerable<ErrorInfo> SaveDocument(InvestorFundDocument investorFundDocument);
		List<DocumentDetail> FindDocuments(int pageIndex, int pageSize, string sortName, string sortOrder, DateTime fromDate, DateTime toDate, int investorId, int fundId, int documentTypeId, DocumentStatus documentStatus, ref int totalRows);
		#endregion

	}
}
