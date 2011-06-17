using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Models.Document;
using System.Data.Objects;

namespace DeepBlue.Controllers.Document {
	public class DocumentRepository : IDocumentRepository {

		#region IDocumentRepository Members

		public IEnumerable<ErrorInfo> SaveDocument(InvestorFundDocument investorFundDocument) {
			return investorFundDocument.Save();
		}

		public List<DocumentDetail> FindDocuments(int pageIndex, int pageSize, string sortName, string sortOrder, DateTime fromDate, DateTime toDate, int investorId, int fundId, int documentTypeId, DocumentStatus documentStatus, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<DocumentDetail> entityTypeQuery = (from document in context.InvestorFundDocuments
														  where document.DocumentDate >= EntityFunctions.TruncateTime(fromDate) && document.DocumentDate <= EntityFunctions.TruncateTime(toDate)
														  && (documentTypeId > 0 ? document.DocumentTypeID == documentTypeId : document.DocumentTypeID > 0)
														  && (documentStatus == DocumentStatus.Investor ? (investorId > 0 ? (document.InvestorID ?? 0) == investorId : (document.InvestorID ?? 0) > 0) : (fundId > 0 ? (document.FundID ?? 0) == fundId : (document.FundID ?? 0) > 0))
															select new DocumentDetail {
																DocumentDate = document.DocumentDate,
																FileName = document.File.FileName,
																FilePath = document.File.FilePath,
																FileTypeName = document.File.FileType.FileTypeName,
																InvestorName = document.Investor.InvestorName,
																FundName = document.Fund.FundName,
																DocumentType = document.DocumentType.DocumentTypeName,
															});
				entityTypeQuery = entityTypeQuery.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<DocumentDetail> paginatedList = new PaginatedList<DocumentDetail>(entityTypeQuery, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		#endregion
	}
}