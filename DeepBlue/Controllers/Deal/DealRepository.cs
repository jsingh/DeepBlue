using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Models.Deal;
using System.Data.Objects;

namespace DeepBlue.Controllers.Deal {
	public class DealRepository : IDealRepository {

		#region Deal

		public int GetMaxDealNumber(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Int32> query = (from deal in context.Deals
										   where deal.FundID == fundId
										   select deal.DealNumber);
				if (query.Count() > 0)
					return query.Max() + 1;
				else
					return 1;
			}
		}

		public bool DealNameAvailable(string dealName, int dealId, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from deal in context.Deals
						 where deal.DealName == dealName && (deal.DealID != dealId && deal.FundID == fundId)
						 select deal.DealID).Count()) > 0 ? true : false;
			}
		}

		public Models.Entity.Deal FindDeal(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.Deals
							  .Include("Contact")
							  .Include("Contact.ContactCommunications")
							  .Include("Contact.ContactCommunications.Communication")
							  .Include("Contact1")
							  .Include("Contact1.ContactCommunications")
							  .Include("Contact1.ContactCommunications.Communication")
							  .Include("Partner")
							  .Include("Fund")
							  .Where(deal => deal.DealID == dealId).SingleOrDefault();
			}
		}

		public DealDetailModel FindDealDetail(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				DealDetailModel dealDetail = (from deal in context.Deals
											  where deal.DealID == dealId
											  select new DealDetailModel {
												  ContactId = deal.ContactID,
												  DealId = deal.DealID,
												  DealName = deal.DealName,
												  DealNumber = deal.DealNumber,
												  FundId = deal.Fund.FundID,
												  FundName = deal.Fund.FundName,
												  IsPartnered = deal.IsPartnered,
												  PartnerName = deal.Partner.PartnerName,
												  PurchaseTypeId = deal.PurchaseTypeID,
												  SellerContactId = deal.SellerContactID,
											  }).SingleOrDefault();

				if ((dealDetail.SellerContactId ?? 0) > 0) {
					dealDetail.SellerInfo = (from sellerContact in context.Contacts
											 where sellerContact.ContactID == dealDetail.SellerContactId
											 select new DealSellerDetailModel {
												 ContactName = sellerContact.ContactName,
												 SellerName = sellerContact.FirstName,
												 SellerContactId = sellerContact.ContactID,
												 DealId = dealId
											 }).SingleOrDefault();
					List<CommunicationDetailModel> communications = GetContactCommunications(context, dealDetail.SellerContactId ?? 0);
					dealDetail.SellerInfo.Email = GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.Email);
					dealDetail.SellerInfo.Phone = GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.HomePhone);
					dealDetail.SellerInfo.Fax = GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.Fax);
					dealDetail.SellerInfo.CompanyName = GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.Company);
				}

				dealDetail.DealExpenses = GetDealClosingCostModel(context, 0, dealDetail.DealId).ToList();

				dealDetail.DealUnderlyingFunds = GetDealUnderlyingFundModel(context, 0, dealDetail.DealId).ToList();

				dealDetail.DealUnderlyingDirects = GetDealUnderlyingDirectModel(context, 0, dealDetail.DealId).ToList();

				return dealDetail;
			}
		}

		public IEnumerable<ErrorInfo> SaveDeal(Models.Entity.Deal deal) {
			return deal.Save();
		}

		public List<AutoCompleteList> FindDeals(string dealName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> dealListQuery = (from deal in context.Deals
															  where deal.DealName.Contains(dealName)
															  orderby deal.DealName
															  select new AutoCompleteList {
																  id = deal.DealID,
																  label = deal.DealName + " (" + deal.Fund.FundName + ")",
																  value = deal.DealName
															  });
				return new PaginatedList<AutoCompleteList>(dealListQuery, 1, 20);
			}
		}

		public List<DealListModel> GetAllDeals(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<DealListModel> query = (from deal in context.Deals
												   select new DealListModel {
													   DealId = deal.DealID,
													   DealName = deal.DealName,
													   FundName = deal.Fund.FundName
												   });
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<DealListModel> paginatedList = new PaginatedList<DealListModel>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		#endregion

		#region DealExpense

		private IQueryable<DealClosingCostModel> GetDealClosingCostModel(DeepBlueEntities context, int dealClosingCostId, int dealId) {
			IQueryable<DealClosingCost> dealClosingCosts = null;
			if (dealClosingCostId > 0) {
				dealClosingCosts = context.DealClosingCosts.Where(dealClosingCost => dealClosingCost.DealClosingCostID == dealClosingCostId);
			}
			if (dealId > 0) {
				dealClosingCosts = context.DealClosingCosts.Where(dealClosingCost => dealClosingCost.DealID == dealId);
			}
			if (dealClosingCosts != null) {
				return (from expense in dealClosingCosts
						select new DealClosingCostModel {
							Amount = expense.Amount,
							Date = expense.Date,
							DealClosingCostId = expense.DealClosingCostID,
							DealClosingCostTypeId = expense.DealClosingCostTypeID,
							DealId = expense.DealID,
							Description = expense.DealClosingCostType.Name
						});
			}
			else {
				return null;
			}
		}

		public DealClosingCost FindDealClosingCost(int dealClosingCostId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealClosingCosts
							  .Include("DealClosingCostType")
							  .Where(dealClosingCost => dealClosingCost.DealClosingCostID == dealClosingCostId).SingleOrDefault();
			}
		}

		public DealClosingCostModel FindDealClosingCostModel(int dealClosingCostId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return GetDealClosingCostModel(context, dealClosingCostId, 0).SingleOrDefault();
			}
		}


		public void DeleteDealClosingCost(int dealClosingCostId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				DealClosingCost dealClosingCost = context.DealClosingCosts.SingleOrDefault(dealClose => dealClose.DealClosingCostID == dealClosingCostId);
				if (dealClosingCost != null) {
					context.DealClosingCosts.DeleteObject(dealClosingCost);
					context.SaveChanges();
				}
			}
		}

		public IEnumerable<ErrorInfo> SaveDealClosingCost(DealClosingCost dealClosingCost) {
			return dealClosingCost.Save();
		}

		#endregion

		#region DealUnderlyingFund

		private IQueryable<DealUnderlyingFundModel> GetDealUnderlyingFundModel(DeepBlueEntities context, int dealUnderlyingFundId, int dealId) {
			IQueryable<DealUnderlyingFund> dealUnderlyingFunds = null;
			if (dealUnderlyingFundId > 0) {
				dealUnderlyingFunds = context.DealUnderlyingFunds.Where(dealUnderlyingFund => dealUnderlyingFund.DealUnderlyingtFundID == dealUnderlyingFundId);
			}
			if (dealId > 0) {
				dealUnderlyingFunds = context.DealUnderlyingFunds.Where(dealUnderlyingFund => dealUnderlyingFund.DealID == dealId);
			}
			if (dealUnderlyingFunds != null) {
				return (from fund in dealUnderlyingFunds
						select new DealUnderlyingFundModel {
							CommittedAmount = fund.CommittedAmount,
							DealId = fund.DealID,
							DealUnderlyingFundId = fund.DealUnderlyingtFundID,
							FundName = fund.UnderlyingFund.FundName,
							FundNav = fund.FundNav,
							Percent = fund.Percent,
							RecordDate = fund.RecordDate,
							UnderlyingFundId = fund.UnderlyingFundID,
							GrossPurchasePrice = fund.GrossPurchasePrice,
							ReassignedGPP = fund.ReassignedGPP,
							UnfundedAmount = fund.UnfundedAmount,
							PostRecordDateCapitalCall = fund.PostRecordDateCapitalCall,
							PostRecordDateDistribution = fund.PostRecordDateDistribution,
							DealClosingId = fund.DealClosingID
						});
			}
			else {
				return null;
			}
		}

		public DealUnderlyingFund FindDealUnderlyingFund(int dealUnderlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealUnderlyingFunds
							  .Include("UnderlyingFund")
							  .Where(dealUnderlyingFund => dealUnderlyingFund.DealUnderlyingtFundID == dealUnderlyingFundId).SingleOrDefault();
			}
		}

		public DealUnderlyingFundModel FindDealUnderlyingFundModel(int dealUnderlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return GetDealUnderlyingFundModel(context, dealUnderlyingFundId, 0).SingleOrDefault();
			}
		}

		public void DeleteDealUnderlyingFund(int dealUnderlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				DealUnderlyingFund dealUnderlyingFund = context.DealUnderlyingFunds.SingleOrDefault(underlyingFund => underlyingFund.DealUnderlyingtFundID == dealUnderlyingFundId);
				if (dealUnderlyingFund != null) {
					context.DealUnderlyingFunds.DeleteObject(dealUnderlyingFund);
					context.SaveChanges();
				}
			}
		}

		public IEnumerable<ErrorInfo> SaveDealUnderlyingFund(DealUnderlyingFund dealUnderlyingFund) {
			return dealUnderlyingFund.Save();
		}

		public List<DealUnderlyingFundDetail> GetAllDealUnderlyingFundDetails(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from dealUnderlyingFund in context.DealUnderlyingFunds
						where dealUnderlyingFund.DealID == dealId
						select new DealUnderlyingFundDetail {
							Commitment = dealUnderlyingFund.CommittedAmount,
							FundName = dealUnderlyingFund.UnderlyingFund.FundName,
							NAV = dealUnderlyingFund.FundNav,
							RecordDate = dealUnderlyingFund.RecordDate
						}).ToList();
			}
		}

		public List<DealUnderlyingFund> GetDealUnderlyingFunds(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealUnderlyingFunds.Where(dealUnderlyingFund => dealUnderlyingFund.DealID == dealId).ToList();
			}
		}

		public List<DealUnderlyingFund> GetAllDealUnderlyingFunds(int underlyingFundId, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from underlyingFund in context.DealUnderlyingFunds
						where underlyingFund.UnderlyingFundID == underlyingFundId
						&& underlyingFund.Deal.FundID == fundId
						select underlyingFund).ToList();
			}
		}

		#endregion

		#region DealUnderlyingDirect

		private IQueryable<DealUnderlyingDirectModel> GetDealUnderlyingDirectModel(DeepBlueEntities context, int dealUnderlyingDirectId, int dealId) {
			IQueryable<DealUnderlyingDirect> dealUnderlyingDirects = null;
			if (dealUnderlyingDirectId > 0) {
				dealUnderlyingDirects = context.DealUnderlyingDirects.Where(dealUnderlyingDirect => dealUnderlyingDirect.DealUnderlyingDirectID == dealUnderlyingDirectId);
			}
			if (dealId > 0) {
				dealUnderlyingDirects = context.DealUnderlyingDirects.Where(dealUnderlyingDirect => dealUnderlyingDirect.DealID == dealId);
			}
			if (dealUnderlyingDirects != null) {
				return (from dealUnderlyingDirect in dealUnderlyingDirects
						join equity in context.Equities on dealUnderlyingDirect.SecurityID equals equity.EquityID into equities
						join fixedIncome in context.FixedIncomes on dealUnderlyingDirect.SecurityID equals fixedIncome.FixedIncomeID into fixedIncomes
						from equity in equities.DefaultIfEmpty()
						from fixedIncome in fixedIncomes.DefaultIfEmpty()
						select new DealUnderlyingDirectModel {
							FMV = dealUnderlyingDirect.FMV,
							DealId = dealUnderlyingDirect.DealID,
							DealUnderlyingDirectId = dealUnderlyingDirect.DealUnderlyingDirectID,
							PurchasePrice = dealUnderlyingDirect.PurchasePrice,
							SecurityId = dealUnderlyingDirect.SecurityID,
							SecurityTypeId = dealUnderlyingDirect.SecurityTypeID,
							TaxCostBase = dealUnderlyingDirect.TaxCostBase,
							TaxCostDate = dealUnderlyingDirect.TaxCostDate,
							NumberOfShares = dealUnderlyingDirect.NumberOfShares ?? 0,
							Percent = dealUnderlyingDirect.Percent ?? 0,
							RecordDate = dealUnderlyingDirect.RecordDate,
							SecurityType = dealUnderlyingDirect.SecurityType.Name,
							DealClosingId = dealUnderlyingDirect.DealClosingID,
							IssuerId = (dealUnderlyingDirect.SecurityTypeID == (int)Models.Deal.Enums.SecurityType.Equity ? equity.Issuer.IssuerID : fixedIncome.Issuer.IssuerID),
							IssuerName = (dealUnderlyingDirect.SecurityTypeID == (int)Models.Deal.Enums.SecurityType.Equity ? equity.Issuer.Name : fixedIncome.Issuer.Name),
							Security = (dealUnderlyingDirect.SecurityTypeID == (int)Models.Deal.Enums.SecurityType.Equity ? equity.Symbol : fixedIncome.Symbol),
						});
			}
			else {
				return null;
			}
		}

		public DealUnderlyingDirect FindDealUnderlyingDirect(int dealUnderlyingDirectId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealUnderlyingDirects
							  .Include("SecurityType")
							  .Where(dealUnderlyingDirect => dealUnderlyingDirect.DealUnderlyingDirectID == dealUnderlyingDirectId).SingleOrDefault();
			}
		}

		public DealUnderlyingDirectModel FindDealUnderlyingDirectModel(int dealUnderlyingDirectId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<DealUnderlyingDirectModel> query = GetDealUnderlyingDirectModel(context, dealUnderlyingDirectId, 0);
				return (query != null ? query.SingleOrDefault() : null);
			}
		}

		public List<DealUnderlyingDirectDetail> GetAllDealUnderlyingDirects(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from dealUnderlyingDirect in context.DealUnderlyingDirects
						join equity in context.Equities on dealUnderlyingDirect.SecurityID equals equity.EquityID into equities
						join fixedIncome in context.FixedIncomes on dealUnderlyingDirect.SecurityID equals fixedIncome.FixedIncomeID into fixedIncomes
						from equity in equities.DefaultIfEmpty()
						from fixedIncome in fixedIncomes.DefaultIfEmpty()
						where dealUnderlyingDirect.DealID == dealId
						select new DealUnderlyingDirectDetail {
							Company = (dealUnderlyingDirect.SecurityTypeID == (int)Models.Deal.Enums.SecurityType.Equity ? equity.Issuer.Name : fixedIncome.Issuer.Name),
							FMV = dealUnderlyingDirect.FMV,
							NoOfShares = dealUnderlyingDirect.NumberOfShares,
							Percentage = dealUnderlyingDirect.Percent,
							RecordDate = dealUnderlyingDirect.RecordDate,
							Security = (dealUnderlyingDirect.SecurityTypeID == (int)Models.Deal.Enums.SecurityType.Equity ? equity.Symbol : fixedIncome.Symbol)
						}).ToList();
			}
		}

		public List<DealUnderlyingDirectListModel> GetAllDealUnderlyingDirects(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<DealUnderlyingDirectListModel> query = (from dealUnderlyingDirect in context.DealUnderlyingDirects
																   join equity in context.Equities on dealUnderlyingDirect.SecurityID equals equity.EquityID into equities
																   join fixedIncome in context.FixedIncomes on dealUnderlyingDirect.SecurityID equals fixedIncome.FixedIncomeID into fixedIncomes
																   from equity in equities.DefaultIfEmpty()
																   from fixedIncome in fixedIncomes.DefaultIfEmpty()
																   select new DealUnderlyingDirectListModel {
																	   CloseDate = dealUnderlyingDirect.DealClosing.CloseDate,
																	   DealUnderlyingDirectId = dealUnderlyingDirect.DealUnderlyingDirectID,
																	   DealName = dealUnderlyingDirect.Deal.DealName,
																	   SecurityType = dealUnderlyingDirect.SecurityType.Name,
																	   IssuerName = (dealUnderlyingDirect.SecurityTypeID == (int)Models.Deal.Enums.SecurityType.Equity ? equity.Issuer.Name : fixedIncome.Issuer.Name)
																   });
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<DealUnderlyingDirectListModel> paginatedList = new PaginatedList<DealUnderlyingDirectListModel>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<DealUnderlyingDirect> GetDealUnderlyingDirects(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealUnderlyingDirects.Where(dealUnderlyingDirect => dealUnderlyingDirect.DealID == dealId).ToList();
			}
		}

		public void DeleteDealUnderlyingDirect(int dealUnderlyingDirectId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				DealUnderlyingDirect dealUnderlyingDirect = context.DealUnderlyingDirects.SingleOrDefault(underlyingFund => underlyingFund.DealUnderlyingDirectID == dealUnderlyingDirectId);
				if (dealUnderlyingDirect != null) {
					context.DealUnderlyingDirects.DeleteObject(dealUnderlyingDirect);
					context.SaveChanges();
				}
			}
		}

		public IEnumerable<ErrorInfo> SaveDealUnderlyingDirect(DealUnderlyingDirect dealUnderlyingDirect) {
			return dealUnderlyingDirect.Save();
		}

		public IEnumerable<ErrorInfo> UpdatePostRecordDateDistribution(int underlyingFundId, int fundId) {
			List<DealUnderlyingFund> dealUnderlyingFunds = GetAllDealUnderlyingFunds(underlyingFundId, fundId);
			IEnumerable<ErrorInfo> errorInfo = null;
			foreach (var dealUnderlyingFund in dealUnderlyingFunds) {
				if (dealUnderlyingFund.DealClosingID == null) {
					dealUnderlyingFund.PostRecordDateDistribution = GetSumOfCashDistribution(underlyingFundId, dealUnderlyingFund.DealID);
					errorInfo = dealUnderlyingFund.Save();
					if (errorInfo != null)
						break;
				}
			}
			return errorInfo;
		}

		public IEnumerable<ErrorInfo> UpdatePostRecordDateCapitalCall(int underlyingFundId, int fundId) {
			List<DealUnderlyingFund> dealUnderlyingFunds = GetAllDealUnderlyingFunds(underlyingFundId, fundId);
			IEnumerable<ErrorInfo> errorInfo = null;
			foreach (var dealUnderlyingFund in dealUnderlyingFunds) {
				if (dealUnderlyingFund.DealClosingID == null) {
					dealUnderlyingFund.PostRecordDateCapitalCall = GetSumOfUnderlyingFundCapitalCallLineItem(underlyingFundId, dealUnderlyingFund.DealID);
					dealUnderlyingFund.UnfundedAmount = dealUnderlyingFund.CommittedAmount - dealUnderlyingFund.PostRecordDateCapitalCall;
					errorInfo = dealUnderlyingFund.Save();
					if (errorInfo != null)
						break;
				}
			}
			return errorInfo;
		}

		#endregion

		#region DealClosing
		public IEnumerable<ErrorInfo> SaveDealClosing(DealClosing dealClosing) {
			return dealClosing.Save();
		}

		public CreateDealCloseModel FindDealClosingModel(int dealClosingId, int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				CreateDealCloseModel model = (from dealClosing in context.DealClosings
											  where dealClosing.DealClosingID == dealClosingId
											  select new CreateDealCloseModel {
												  CloseDate = dealClosing.CloseDate,
												  DealId = dealClosing.DealID,
												  DealNumber = dealClosing.DealNumber,
												  DealClosingId = dealClosing.DealClosingID,
												  IsFinalClose = dealClosing.IsFinalClose ?? false
											  }).SingleOrDefault();
				if (model == null) {
					model = new CreateDealCloseModel();
					model.DealNumber = GetMaxDealClosingNumber(dealId);
					model.DealId = dealId;
				}
				if (dealId > 0) {
					model.DealUnderlyingDirects = GetDealUnderlyingDirectModel(context, 0, dealId).ToList();
					model.DealUnderlyingFunds = GetDealUnderlyingFundModel(context, 0, dealId).ToList();
				}
				return model;
			}
		}

		public DealClosing FindDealClosing(int dealClosingId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealClosings.Where(dealClosing => dealClosing.DealClosingID == dealClosingId).SingleOrDefault();
			}
		}

		public bool DealCloseDateAvailable(DateTime dealCloseDate, int dealId, int dealCloseId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from dealClose in context.DealClosings
						 where dealClose.CloseDate == dealCloseDate && dealClose.DealID == dealId && dealClose.DealClosingID != dealCloseId
						 select dealClose.DealClosingID).Count()) > 0 ? true : false;
			}
		}

		public int GetMaxDealClosingNumber(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Int32> query = (from dealClosing in context.DealClosings
										   where dealClosing.DealID == dealId
										   select dealClosing.DealNumber ?? 0);
				if (query.Count() > 0)
					return query.Max() + 1;
				else
					return 1;
			}
		}

		public List<DealCloseListModel> GetAllDealClosingLists(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<DealCloseListModel> query = (from dealClose in context.DealClosings
														where dealClose.DealID == dealId
														select new DealCloseListModel {
															CloseDate = dealClose.CloseDate,
															DealClosingId = dealClose.DealClosingID,
															DealName = dealClose.Deal.DealName,
															FundName = dealClose.Deal.Fund.FundName
														});
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<DealCloseListModel> paginatedList = new PaginatedList<DealCloseListModel>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}
		#endregion

		#region DealReport
		public List<DealReportModel> GetAllReportDeals(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<DealReportModel> query = (from deal in context.Deals
													 where deal.FundID == fundId
													 select new DealReportModel {
														 DealId = deal.DealID,
														 DealName = deal.DealName,
														 DealNumber = deal.DealNumber,
														 FundName = deal.Fund.FundName,
														 SellerName = (deal.Contact1 != null ? deal.Contact1.FirstName : string.Empty)
													 });
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<DealReportModel> paginatedList = new PaginatedList<DealReportModel>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<DealReportModel> GetAllExportDeals(string sortName, string sortOrder, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<DealReportModel> query = (from deal in context.Deals
													 where deal.FundID == fundId
													 select new DealReportModel {
														 DealId = deal.DealID,
														 DealName = deal.DealName,
														 DealNumber = deal.DealNumber,
														 FundName = deal.Fund.FundName,
														 SellerName = (deal.Contact1 != null ? deal.Contact1.ContactName : "")
													 });
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				List<DealReportModel> deals = query.ToList();
				foreach (var deal in deals) {
					deal.DealUnderlyingFunds = (from fund in context.DealUnderlyingFunds
												where fund.DealID == deal.DealId
												select new DealUnderlyingFundDetail {
													Commitment = fund.CommittedAmount,
													FundName = fund.UnderlyingFund.FundName,
													NAV = fund.FundNav,
													RecordDate = fund.RecordDate
												}).ToList();
					deal.DealUnderlyingDirects = (from dealUnderlyingDirect in context.DealUnderlyingDirects
												  join equity in context.Equities on dealUnderlyingDirect.SecurityID equals equity.EquityID into equities
												  join fixedIncome in context.FixedIncomes on dealUnderlyingDirect.SecurityID equals fixedIncome.FixedIncomeID into fixedIncomes
												  from equity in equities.DefaultIfEmpty()
												  from fixedIncome in fixedIncomes.DefaultIfEmpty()
												  where dealUnderlyingDirect.DealID == deal.DealId
												  select new DealUnderlyingDirectDetail {
													  Company = (dealUnderlyingDirect.SecurityTypeID == (int)Models.Deal.Enums.SecurityType.Equity ? equity.Issuer.Name : fixedIncome.Issuer.Name),
													  FMV = dealUnderlyingDirect.FMV,
													  NoOfShares = dealUnderlyingDirect.NumberOfShares,
													  Percentage = dealUnderlyingDirect.Percent,
													  RecordDate = dealUnderlyingDirect.RecordDate,
													  Security = (dealUnderlyingDirect.SecurityTypeID == (int)Models.Deal.Enums.SecurityType.Equity ? equity.Symbol : fixedIncome.Symbol)
												  }).ToList();
				}
				return deals;
			}
		}
		#endregion

		#region UnderlyingFund
		public List<UnderlyingFundListModel> GetAllUnderlyingFunds(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<UnderlyingFundListModel> query = (from underlyingFund in context.UnderlyingFunds
															 select new UnderlyingFundListModel {
																 FundName = underlyingFund.FundName,
																 FundType = underlyingFund.UnderlyingFundType.Name,
																 IssuerName = underlyingFund.Issuer.Name,
																 UnderlyingFundId = underlyingFund.UnderlyingtFundID
															 });
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<UnderlyingFundListModel> paginatedList = new PaginatedList<UnderlyingFundListModel>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<UnderlyingFund> GetAllUnderlyingFunds() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from underlyingFund in context.UnderlyingFunds
						orderby underlyingFund.FundName
						select underlyingFund).ToList();
			}
		}

		public CreateUnderlyingFundModel FindUnderlyingFundModel(int underlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				CreateUnderlyingFundModel model = (from underlyingFund in context.UnderlyingFunds
												   where underlyingFund.UnderlyingtFundID == underlyingFundId
												   select new CreateUnderlyingFundModel {
													   FundName = underlyingFund.FundName,
													   FundTypeId = underlyingFund.FundTypeID,
													   GeographyId = underlyingFund.GeographyID,
													   IndustryId = underlyingFund.IndustryID,
													   IssuerId = underlyingFund.IssuerID,
													   IsFeesIncluded = underlyingFund.IsFeesIncluded,
													   ReportingTypeId = underlyingFund.ReportingTypeID,
													   ReportingFrequencyId = underlyingFund.ReportingFrequencyID,
													   VintageYear = underlyingFund.VintageYear,
													   UnderlyingFundId = underlyingFund.UnderlyingtFundID,
													   TotalSize = underlyingFund.TotalSize,
													   TerminationYear = underlyingFund.TerminationYear,
													   Routing = underlyingFund.Account.Routing,
													   Account = underlyingFund.Account.AccountNumberCash,
													   AccountOf = underlyingFund.Account.AccountOf,
													   Attention = underlyingFund.Account.Attention,
													   Reference = underlyingFund.Account.Reference,
													   BankName = underlyingFund.Account.BankName,
													   ContactId = (underlyingFund.Contact != null ? underlyingFund.Contact.ContactID : 0),
													   ContactName = (underlyingFund.Contact != null ? underlyingFund.Contact.ContactName : string.Empty),
													   IncentiveFee = underlyingFund.IncentiveFee,
													   LegalFundName = underlyingFund.LegalFundName,
													   Description = underlyingFund.Description,
													   FiscalYearEnd = underlyingFund.FiscalYearEnd,
													   ManagementFee = underlyingFund.ManagementFee,
													   TaxRate = underlyingFund.TaxRate,
													   Taxable = underlyingFund.Taxable,
													   FundStructureId = underlyingFund.FundStructureID,
													   FundRegisteredOfficeId = underlyingFund.FundRegisteredOfficeID,
													   InvestmentTypeId = underlyingFund.InvestmentTypeID,
													   ManagerContactId = underlyingFund.ManagerContactID,
													   AuditorName = underlyingFund.AuditorName,
													   Exempt = underlyingFund.Exempt,
													   IsDomestic = underlyingFund.IsDomestic
												   }).SingleOrDefault();
				if (model != null) {
					List<CommunicationDetailModel> communications = GetContactCommunications(context, model.ContactId);
					model.Address = GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.MailingAddress);
					model.Email = GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.Email);
					model.Phone = GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.HomePhone);
					model.WebAddress = GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.WebAddress);
				}
				return model;
			}
		}

		public UnderlyingFund FindUnderlyingFund(int underlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.UnderlyingFunds
					.Include("Account")
					.Include("Contact")
					.Include("Contact.ContactCommunications")
					.Include("Contact.ContactCommunications.Communication")
					.Where(underlyingFund => underlyingFund.UnderlyingtFundID == underlyingFundId)
					.SingleOrDefault();
			}
		}

		public bool UnderlyingFundNameAvailable(string fundName, int underlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from underlyingFund in context.UnderlyingFunds
						 where underlyingFund.FundName == fundName && underlyingFund.UnderlyingtFundID != underlyingFundId
						 select underlyingFund.UnderlyingtFundID).Count()) > 0 ? true : false;
			}
		}

		public IEnumerable<ErrorInfo> SaveUnderlyingFund(UnderlyingFund underlyingFund) {
			return underlyingFund.Save();
		}

		public List<AutoCompleteList> FindUnderlyingFunds(string fundName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> fundListQuery = (from fund in context.UnderlyingFunds
															  where fund.FundName.Contains(fundName)
															  orderby fund.FundName
															  select new AutoCompleteList {
																  id = fund.UnderlyingtFundID,
																  label = fund.FundName,
																  value = fund.FundName
															  });
				return new PaginatedList<AutoCompleteList>(fundListQuery, 1, 20);
			}
		}
		#endregion

		#region Communication

		private List<CommunicationDetailModel> GetContactCommunications(DeepBlueEntities context, int contactId) {
			return (from contactCommunication in context.ContactCommunications
					join communication in context.Communications on contactCommunication.CommunicationID equals communication.CommunicationID
					where contactCommunication.ContactID == contactId
					select new CommunicationDetailModel {
						CommunicationValue = communication.CommunicationValue,
						CommunicationTypeId = communication.CommunicationTypeID
					}).ToList();
		}

		public string GetCommunicationValue(List<CommunicationDetailModel> communications, Models.Admin.Enums.CommunicationType communicationType) {
			return (from communication in communications
					where communication.CommunicationTypeId == (int)communicationType
					select communication.CommunicationValue).SingleOrDefault();
		}

		#endregion

		#region UnderlyingFundCashDistribution

		public UnderlyingFundCashDistributionModel FindUnderlyingFundCashDistributionModel(int underlyingFundCashDistributionId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from cashDistribution in context.UnderlyingFundCashDistributions
						where cashDistribution.UnderlyingFundCashDistributionID == underlyingFundCashDistributionId
						select new UnderlyingFundCashDistributionModel {
							Amount = cashDistribution.Amount,
							CashDistributionTypeId = cashDistribution.CashDistributionTypeID,
							FundId = cashDistribution.FundID,
							UnderlyingFundCashDistributionId = cashDistribution.UnderlyingFundCashDistributionID,
							IsPostRecordDateTransaction = cashDistribution.IsPostRecordDateTransaction,
							NoticeDate = cashDistribution.NoticeDate,
							PaidDate = cashDistribution.PaidDate,
							ReceivedDate = cashDistribution.ReceivedDate,
							UnderlyingFundId = cashDistribution.UnderlyingFundID,
							FundName = cashDistribution.Fund.FundName,
							UnderlyingFundName = cashDistribution.UnderlyingFund.FundName
						}).SingleOrDefault();
			}
		}

		public UnderlyingFundCashDistribution FindUnderlyingFundCashDistribution(int underlyingFundCashDistributionId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from cashDistribution in context.UnderlyingFundCashDistributions
						where cashDistribution.UnderlyingFundCashDistributionID == underlyingFundCashDistributionId
						select cashDistribution).SingleOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveUnderlyingFundCashDistribution(UnderlyingFundCashDistribution underlyingFundCashDistribution) {
			return underlyingFundCashDistribution.Save();
		}

		public List<UnderlyingFundCashDistributionList> GetAllUnderlyingFundCashDistributions(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<UnderlyingFundCashDistributionList> query = GetUnderlyingFundCashDistributionListQuery(context.UnderlyingFundCashDistributions);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<UnderlyingFundCashDistributionList> paginatedList = new PaginatedList<UnderlyingFundCashDistributionList>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public UnderlyingFundCashDistributionList GetUnderlyingFundCashDistribution(int underlyingFundCashDitributionId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<UnderlyingFundCashDistributionList> query = GetUnderlyingFundCashDistributionListQuery(context.UnderlyingFundCashDistributions.Where(distribution => distribution.UnderlyingFundCashDistributionID == underlyingFundCashDitributionId));
				return query.SingleOrDefault();
			}
		}

		private IQueryable<UnderlyingFundCashDistributionList> GetUnderlyingFundCashDistributionListQuery(IQueryable<UnderlyingFundCashDistribution> underlyingFundCashDistributions) {
			return (from cashDistribution in underlyingFundCashDistributions
					select new UnderlyingFundCashDistributionList {
						Amount = cashDistribution.Amount,
						FundName = cashDistribution.Fund.FundName,
						NoticeDate = cashDistribution.NoticeDate,
						ReceivedDate = cashDistribution.ReceivedDate,
						UnderlyingFundCashDistributionId = cashDistribution.UnderlyingFundCashDistributionID,
						UnderlyingFundName = cashDistribution.UnderlyingFund.FundName
					});
		}

		public bool DeleteUnderlyingFundCashDistribution(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				UnderlyingFundCashDistribution underlyingFundCashDistribution = context.UnderlyingFundCashDistributions.SingleOrDefault(distribution => distribution.UnderlyingFundCashDistributionID == id);
				if (underlyingFundCashDistribution != null) {
					List<CashDistribution> cashDistributions = underlyingFundCashDistribution.CashDistributions.ToList();
					foreach (var cashDistribution in cashDistributions) {
						context.CashDistributions.DeleteObject(cashDistribution);
					}
					context.UnderlyingFundCashDistributions.DeleteObject(underlyingFundCashDistribution);
					context.SaveChanges();
					UpdatePostRecordDateDistribution(underlyingFundCashDistribution.UnderlyingFundID, underlyingFundCashDistribution.FundID);
					return true;
				}
				return false;
			}
		}

		#endregion

		#region CashDistribution

		public List<CashDistributionListModel> GetAllCashDistributions(int underlyingFundCashDistributionId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from cashDistribution in context.CashDistributions
						where cashDistribution.UnderluingFundCashDistributionID == underlyingFundCashDistributionId
						select new CashDistributionListModel {
							Amount = cashDistribution.Amount,
							CashDistributionId = cashDistribution.CashDistributionID,
							DealName = cashDistribution.Deal.DealName
						}).ToList();
			}
		}

		public CashDistribution FindCashDistribution(int underlyingFundCashDistributionId, int underlyingFundId, int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CashDistributions.Where(cashDistribution => cashDistribution.UnderluingFundCashDistributionID == underlyingFundCashDistributionId
														&& cashDistribution.UnderlyingFundID == underlyingFundId
														&& cashDistribution.DealID == dealId).SingleOrDefault();
			}
		}

		public decimal GetSumOfCashDistribution(int underlyingFundId, int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				decimal totalCashDistribution = 0;
				IQueryable<CashDistribution> query = context.CashDistributions
					.Where(cashDistribution => cashDistribution.UnderlyingFundID == underlyingFundId && cashDistribution.DealID == dealId);
				if (query.Count() > 0)
					totalCashDistribution = query.Sum(cashDistribution => cashDistribution.Amount);
				return totalCashDistribution;
			}
		}

		public IEnumerable<ErrorInfo> SaveCashDistribution(CashDistribution cashDistribution) {
			return cashDistribution.Save();
		}
		#endregion

		#region UnderlyingFundCapitalCall

		public UnderlyingFundCapitalCallModel FindUnderlyingFundCapitalCallModel(int underlyingFundCapitalCallId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalCall in context.UnderlyingFundCapitalCalls
						where capitalCall.UnderlyingFundCapitalCallID == underlyingFundCapitalCallId
						select new UnderlyingFundCapitalCallModel {
							Amount = capitalCall.Amount,
							FundId = capitalCall.FundID,
							UnderlyingFundCapitalCallId = capitalCall.UnderlyingFundCapitalCallID,
							IsPostRecordDateTransaction = capitalCall.IsPostRecordDateTransaction,
							NoticeDate = capitalCall.NoticeDate,
							ReceivedDate = capitalCall.ReceivedDate,
							UnderlyingFundId = capitalCall.UnderlyingFundID,
							IsDeemedCapitalCall = capitalCall.IsDeemedCapitalCall,
							FundName = capitalCall.Fund.FundName,
							UnderlyingFundName = capitalCall.UnderlyingFund.FundName
						}).SingleOrDefault();
			}
		}

		public UnderlyingFundCapitalCall FindUnderlyingFundCapitalCall(int underlyingFundCapitalCallId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalCall in context.UnderlyingFundCapitalCalls
						where capitalCall.UnderlyingFundCapitalCallID == underlyingFundCapitalCallId
						select capitalCall).SingleOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveUnderlyingFundCapitalCall(UnderlyingFundCapitalCall underlyingFundCapitalCall) {
			return underlyingFundCapitalCall.Save();
		}

		public List<UnderlyingFundCapitalCallList> GetAllUnderlyingFundCapitalCalls(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<UnderlyingFundCapitalCallList> query = GetUnderlyingFundCapitalCallQuery(context.UnderlyingFundCapitalCalls);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<UnderlyingFundCapitalCallList> paginatedList = new PaginatedList<UnderlyingFundCapitalCallList>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public UnderlyingFundCapitalCallList GetUnderlyingFundCapitalCall(int underlyingFundCapitalCallId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<UnderlyingFundCapitalCallList> query = GetUnderlyingFundCapitalCallQuery(context.UnderlyingFundCapitalCalls.Where(capitalCall => capitalCall.UnderlyingFundCapitalCallID == underlyingFundCapitalCallId));
				return query.SingleOrDefault();
			}
		}

		private IQueryable<UnderlyingFundCapitalCallList> GetUnderlyingFundCapitalCallQuery(IQueryable<UnderlyingFundCapitalCall> underlyingFundCapitalCalls) {
			return (from capitalCall in underlyingFundCapitalCalls
					select new UnderlyingFundCapitalCallList {
						Amount = capitalCall.Amount,
						FundName = capitalCall.Fund.FundName,
						NoticeDate = capitalCall.NoticeDate,
						ReceivedDate = capitalCall.ReceivedDate,
						UnderlyingFundCapitalCallId = capitalCall.UnderlyingFundCapitalCallID,
						UnderlyingFundName = capitalCall.UnderlyingFund.FundName
					});
		}

		public bool DeleteUnderlyingFundCapitalCall(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				UnderlyingFundCapitalCall underlyingFundCapitalCall = context.UnderlyingFundCapitalCalls.SingleOrDefault(capitalCall => capitalCall.UnderlyingFundCapitalCallID == id);
				if (underlyingFundCapitalCall != null) {
					List<UnderlyingFundCapitalCallLineItem> underlyingFundCapitalCallLineItems = underlyingFundCapitalCall.UnderlyingFundCapitalCallLineItems.ToList();
					foreach (var capitalCallLineItem in underlyingFundCapitalCallLineItems) {
						context.UnderlyingFundCapitalCallLineItems.DeleteObject(capitalCallLineItem);
					}
					context.UnderlyingFundCapitalCalls.DeleteObject(underlyingFundCapitalCall);
					context.SaveChanges();
					UpdatePostRecordDateCapitalCall(underlyingFundCapitalCall.UnderlyingFundID, underlyingFundCapitalCall.FundID);
					return true;
				}
				return false;
			}
		}

		#endregion

		#region UnderlyingFundCapitalCallLineItem

		public UnderlyingFundCapitalCallLineItem FindUnderlyingFundCapitalCallLineItem(int underlyingFundCapitalCallId, int underlyingFundId, int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.UnderlyingFundCapitalCallLineItems.Where(lineItem => lineItem.UnderlyingFundCapitalCallID == underlyingFundCapitalCallId
														&& lineItem.UnderlyingFundID == underlyingFundId
														&& lineItem.DealID == dealId).SingleOrDefault();
			}
		}

		public decimal GetSumOfUnderlyingFundCapitalCallLineItem(int underlyingFundId, int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				decimal totalCapitalCall = 0;
				IQueryable<UnderlyingFundCapitalCallLineItem> query = context.UnderlyingFundCapitalCallLineItems.Where(lineItem => lineItem.UnderlyingFundID == underlyingFundId && lineItem.DealID == dealId);
				if (query.Count() > 0)
					totalCapitalCall = query.Sum(lineItem => lineItem.Amount);
				return totalCapitalCall;
			}
		}

		public IEnumerable<ErrorInfo> SaveUnderlyingFundCapitalCallLineItem(UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem) {
			return underlyingFundCapitalCallLineItem.Save();
		}
		#endregion
	}
}