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

		public int GetMaxDealNumber() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Int32> query = (from deal in context.Deals
										   select deal.DealNumber);
				if (query.Count() > 0)
					return query.Max() + 1;
				else
					return 1;
			}
		}

		public bool DealNameAvailable(string dealName, int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from deal in context.Deals
						 where deal.DealName == dealName && deal.DealID != dealId
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

					List<CommunicationDetailModel> communications = (from contactCommunication in context.ContactCommunications
																	 join communication in context.Communications on contactCommunication.CommunicationID equals communication.CommunicationID
																	 where contactCommunication.ContactID == dealDetail.SellerContactId
																	 select new CommunicationDetailModel {
																		 CommunicationValue = communication.CommunicationValue,
																		 CommunicationTypeId = communication.CommunicationTypeID
																	 }).ToList();
					dealDetail.SellerInfo.Email = (from communication in communications
												   where communication.CommunicationTypeId == (int)Models.Investor.Enums.CommunicationType.Email
												   select communication.CommunicationValue).SingleOrDefault();
					dealDetail.SellerInfo.Phone = (from communication in communications
												   where communication.CommunicationTypeId == (int)Models.Investor.Enums.CommunicationType.Home_Phone
												   select communication.CommunicationValue).SingleOrDefault();
					dealDetail.SellerInfo.Fax = (from communication in communications
												 where communication.CommunicationTypeId == (int)Models.Investor.Enums.CommunicationType.Fax
												 select communication.CommunicationValue).SingleOrDefault();
				}

				dealDetail.DealExpenses = (from expense in context.DealClosingCosts
										   where expense.DealID == dealId
										   select new DealClosingCostModel {
											   Amount = expense.Amount,
											   Date = expense.Date,
											   DealClosingCostId = expense.DealClosingCostID,
											   DealClosingCostTypeId = expense.DealClosingCostID,
											   DealId = expense.DealID,
											   Description = expense.DealClosingCostType.Name
										   }).ToList();

				dealDetail.DealUnderlyingFunds = (from fund in context.DealUnderlyingFunds
												  where fund.DealID == dealId
												  select new DealUnderlyingFundModel {
													  CommittedAmount = fund.CommittedAmount,
													  DealId = fund.DealID,
													  DealUnderlyingFundId = fund.DealUnderlyingtFundID,
													  FundName = fund.UnderlyingFund.FundName,
													  FundNav = fund.FundNav,
													  Percent = fund.Percent,
													  RecordDate = fund.RecordDate,
													  UnderlyingFundId = fund.UnderlyingFundID
												  }).ToList();

				dealDetail.DealUnderlyingDirects = (from direct in context.DealUnderlyingDirects
													join equity in context.Equities on direct.SecurityID equals equity.EquityID into equities
													join fixedIncome in context.FixedIncomes on direct.SecurityID equals fixedIncome.FixedIncomeID into fixedIncomes
													from equity in equities.DefaultIfEmpty()
													from fixedIncome in fixedIncomes.DefaultIfEmpty()
													where direct.DealID == dealId
													select new DealUnderlyingDirectModel {
														DealId = direct.DealID,
														DealUnderlyingDirectId = direct.DealUnderlyingDirectID,
														FMV = direct.FMV,
														NumberOfShares = direct.NumberOfShares,
														Percent = direct.Percent,
														RecordDate = direct.RecordDate,
														SecurityId = direct.SecurityID,
														SecurityTypeId = direct.SecurityTypeID,
														SecurityType = direct.SecurityType.Name,
														Security = (direct.SecurityTypeID == (int)Models.Deal.Enums.SecurityType.Equity ? equity.Symbol : fixedIncome.Symbol),
														IssuerId = (direct.SecurityTypeID == (int)Models.Deal.Enums.SecurityType.Equity ? equity.Issuer.IssuerID : fixedIncome.Issuer.IssuerID),
														IssuerName = (direct.SecurityTypeID == (int)Models.Deal.Enums.SecurityType.Equity ? equity.Issuer.Name : fixedIncome.Issuer.Name)
													}).ToList();
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
																  label = deal.DealName,
																  value = deal.DealName
															  });
				return new PaginatedList<AutoCompleteList>(dealListQuery, 1, 20);
			}
		}

		public List<Models.Entity.Deal> GetAllDeals(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.Deal> query = (from deal in context.Deals
														select deal);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Entity.Deal> paginatedList = new PaginatedList<Models.Entity.Deal>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		#endregion

		#region DealExpense

		public DealClosingCost FindDealClosingCost(int dealClosingCostId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealClosingCosts
							  .Include("DealClosingCostType")
							  .Where(dealClosingCost => dealClosingCost.DealClosingCostID == dealClosingCostId).SingleOrDefault();
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

		public DealUnderlyingFund FindDealUnderlyingFund(int dealUnderlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealUnderlyingFunds
							  .Include("UnderlyingFund")
							  .Where(dealUnderlyingFund => dealUnderlyingFund.DealUnderlyingtFundID == dealUnderlyingFundId).SingleOrDefault();
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

		public List<UnderlyingFund> GetAllUnderlyingFunds() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from underlyingFund in context.UnderlyingFunds
						orderby underlyingFund.FundName
						select underlyingFund).ToList();
			}
		}

		public List<DealUnderlyingFundDetail> GetAllDealUnderlyingFunds(int dealId) {
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

		#endregion

		#region DealUnderlyingDirect

		public DealUnderlyingDirect FindDealUnderlyingDirect(int dealUnderlyingDirectId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealUnderlyingDirects
							  .Include("SecurityType")
							  .Where(dealUnderlyingDirect => dealUnderlyingDirect.DealUnderlyingDirectID == dealUnderlyingDirectId).SingleOrDefault();
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
		#endregion

		#region DealClosing
		public IEnumerable<ErrorInfo> SaveDealClosing(DealClosing dealClosing) {
			return dealClosing.Save();
		}

		public DealClosing FindDealClosing(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from dealClosing in context.DealClosings
						where dealClosing.DealID == dealId
						select dealClosing).SingleOrDefault();
			}
		}
		#endregion

		#region DealReport
		public List<DealReportModel> GetAllReportDeals(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<DealReportModel> query = (from deal in context.Deals
													 select new DealReportModel {
														 DealId = deal.DealID,
														 DealName = deal.DealName,
														 DealNumber = deal.DealNumber,
														 FundName = deal.Fund.FundName,
														 SellerName = (deal.Contact1 != null ? deal.Contact1.ContactName : "")
													 });
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<DealReportModel> paginatedList = new PaginatedList<DealReportModel>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<DealReportModel> GetAllExportDeals(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<DealReportModel> query = (from deal in context.Deals
													 select new DealReportModel {
														 DealId = deal.DealID,
														 DealName = deal.DealName,
														 DealNumber = deal.DealNumber,
														 FundName = deal.Fund.FundName,
														 SellerName = (deal.Contact1 != null ? deal.Contact1.ContactName : "")
													 });
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<DealReportModel> paginatedList = new PaginatedList<DealReportModel>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				foreach (var deal in paginatedList) {
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
				return paginatedList;
			}
		}
		#endregion


	}
}