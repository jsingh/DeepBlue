using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Models.Deal;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

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
												  IsDealClose = deal.DealClosings.Where(closeDeal => closeDeal.IsFinalClose == true)
																.Select(closeDeal => closeDeal.IsFinalClose).FirstOrDefault()
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

				dealDetail.DealUnderlyingFunds = GetDealUnderlyingFundModel(context, context.DealUnderlyingFunds.Where(fund => fund.DealID == dealId)).ToList();

				dealDetail.DealUnderlyingDirects = GetDealUnderlyingDirectModel(context, context.DealUnderlyingDirects.Where(direct => direct.DealID == dealId)).ToList();

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

		public List<AutoCompleteList> FindDeals(int fundId, string dealName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> dealListQuery = (from deal in context.Deals
															  where deal.DealName.Contains(dealName) && deal.FundID == fundId
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

		public object GetDealDetail(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from deal in context.Deals
						where deal.DealID == dealId
						select new {
							FundName = deal.Fund.FundName,
							DealName = deal.DealName,
							DealNumber = deal.DealNumber,
							FundId = deal.FundID,
							TotalDealClosing = deal.DealClosings.Count(),
							TotalUnderlyingFundClosing = deal.DealUnderlyingFunds.Where(dealUnderlyingFund => dealUnderlyingFund.DealClosingID != null && dealUnderlyingFund.DealID == dealId).Count(),
							TotalUnderlyingDirectClosing = deal.DealUnderlyingDirects.Where(dealUnderlyingDirect => dealUnderlyingDirect.DealClosingID != null && dealUnderlyingDirect.DealID == dealId).Count(),
							TotalUnderlyingFundNotClosing = deal.DealUnderlyingFunds.Where(dealUnderlyingFund => dealUnderlyingFund.DealClosingID == null && dealUnderlyingFund.DealID == dealId).Count(),
							TotalUnderlyingDirectNotClosing = deal.DealUnderlyingDirects.Where(dealUnderlyingDirect => dealUnderlyingDirect.DealClosingID == null && dealUnderlyingDirect.DealID == dealId).Count(),
						}).SingleOrDefault();
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

		private IQueryable<DealUnderlyingFundModel> GetDealUnderlyingFundModel(DeepBlueEntities context, IQueryable<DealUnderlyingFund> dealUnderlyingFunds) {
			return (from fund in dealUnderlyingFunds
					join fundNAV in context.UnderlyingFundNAVs on new { fund.UnderlyingFundID, fund.Deal.FundID } equals new { fundNAV.UnderlyingFundID, fundNAV.FundID } into underlyingFundNAVS
					from fundNAV in underlyingFundNAVS.DefaultIfEmpty()
					select new DealUnderlyingFundModel {
						CommittedAmount = fund.CommittedAmount,
						DealId = fund.DealID,
						DealUnderlyingFundId = fund.DealUnderlyingtFundID,
						FundName = fund.UnderlyingFund.FundName,
						Percent = fund.Percent,
						RecordDate = fund.RecordDate,
						UnderlyingFundId = fund.UnderlyingFundID,
						GrossPurchasePrice = fund.GrossPurchasePrice,
						ReassignedGPP = fund.ReassignedGPP,
						UnfundedAmount = fund.UnfundedAmount,
						PostRecordDateCapitalCall = fund.PostRecordDateCapitalCall,
						PostRecordDateDistribution = fund.PostRecordDateDistribution,
						DealClosingId = fund.DealClosingID,
						FundNAV = underlyingFundNAVS.FirstOrDefault().FundNAV
					});
		}

		public DealUnderlyingFund FindDealUnderlyingFund(int dealUnderlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealUnderlyingFunds
					.Include("Deal")
					.Where(dealUnderlyingFund => dealUnderlyingFund.DealUnderlyingtFundID == dealUnderlyingFundId).SingleOrDefault();
			}
		}

		public DealUnderlyingFundModel FindDealUnderlyingFundModel(int dealUnderlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return GetDealUnderlyingFundModel(context, context.DealUnderlyingFunds.Where(fund => fund.DealUnderlyingtFundID == dealUnderlyingFundId)).SingleOrDefault();
			}
		}

		public bool DeleteDealUnderlyingFund(int dealUnderlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				DealUnderlyingFund dealUnderlyingFund = context.DealUnderlyingFunds.SingleOrDefault(underlyingFund => underlyingFund.DealUnderlyingtFundID == dealUnderlyingFundId);
				if (dealUnderlyingFund != null) {
					List<DealUnderlyingFundAdjustment> dealUnderlyingFundAdjustments = dealUnderlyingFund.DealUnderlyingFundAdjustments.ToList();
					foreach (var adjustment in dealUnderlyingFundAdjustments) {
						context.DealUnderlyingFundAdjustments.DeleteObject(adjustment);
					}
					context.DealUnderlyingFunds.DeleteObject(dealUnderlyingFund);
					context.SaveChanges();
				}
				return true;
			}
		}

		public IEnumerable<ErrorInfo> SaveDealUnderlyingFund(DealUnderlyingFund dealUnderlyingFund) {
			return dealUnderlyingFund.Save();
		}

		public List<DealUnderlyingFundModel> GetAllDealUnderlyingFundDetails(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return GetDealUnderlyingFundModel(context, context.DealUnderlyingFunds.Where(fund => fund.DealID == dealId)).ToList();
			}
		}

		public List<DealUnderlyingFund> GetDealUnderlyingFunds(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealUnderlyingFunds.Where(dealUnderlyingFund => dealUnderlyingFund.DealID == dealId).ToList();
			}
		}

		public List<DealUnderlyingFund> GetDealUnderlyingFunds(int dealId, int dealCloseId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealUnderlyingFunds.Where(dealUnderlyingFund => dealUnderlyingFund.DealID == dealId && dealUnderlyingFund.DealClosingID == dealCloseId).ToList();
			}
		}

		public List<DealUnderlyingFund> GetAllDealClosingUnderlyingFunds(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealUnderlyingFunds.Where(dealUnderlyingFund => dealUnderlyingFund.DealID == dealId && dealUnderlyingFund.DealClosingID != null).ToList();
			}
		}

		public List<DealUnderlyingFund> GetAllClosingDealUnderlyingFunds(int underlyingFundId, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from underlyingFund in context.DealUnderlyingFunds
						where underlyingFund.UnderlyingFundID == underlyingFundId
						&& underlyingFund.Deal.FundID == fundId && underlyingFund.DealClosingID != null
						select underlyingFund).ToList();
			}
		}

		public List<DealUnderlyingFund> GetAllNotClosingDealUnderlyingFunds(int underlyingFundId, int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from underlyingFund in context.DealUnderlyingFunds
						where underlyingFund.UnderlyingFundID == underlyingFundId
						&& underlyingFund.DealID == dealId && underlyingFund.DealClosingID == null
						select underlyingFund).ToList();
			}
		}

		public decimal FindFundNAV(int underlyingFundId, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fundNAV in context.UnderlyingFundNAVs
						where fundNAV.UnderlyingFundID == underlyingFundId && fundNAV.FundID == fundId
						select fundNAV.FundNAV ?? 0).FirstOrDefault();
			}
		}

		#endregion

		#region DealUnderlyingDirect

		private IQueryable<DealUnderlyingDirectModel> GetDealUnderlyingDirectModel(DeepBlueEntities context, IQueryable<DealUnderlyingDirect> dealUnderlyingDirects) {
			return (from dealUnderlyingDirect in dealUnderlyingDirects
					join deal in context.Deals on dealUnderlyingDirect.DealID equals deal.DealID
					join equity in context.Equities on dealUnderlyingDirect.SecurityID equals equity.EquityID into equities
					join fixedIncome in context.FixedIncomes on dealUnderlyingDirect.SecurityID equals fixedIncome.FixedIncomeID into fixedIncomes
					join directLastPrice in context.UnderlyingDirectLastPrices on new { deal.FundID, dealUnderlyingDirect.SecurityID, dealUnderlyingDirect.SecurityTypeID }
					equals new { directLastPrice.FundID, directLastPrice.SecurityID, directLastPrice.SecurityTypeID } into directLastPrices
					from directLastPrice in directLastPrices.DefaultIfEmpty()
					from equity in equities.DefaultIfEmpty()
					from fixedIncome in fixedIncomes.DefaultIfEmpty()
					select new DealUnderlyingDirectModel {
						FMV = dealUnderlyingDirect.FMV,
						DealId = dealUnderlyingDirect.DealID,
						DealUnderlyingDirectId = dealUnderlyingDirect.DealUnderlyingDirectID,
						PurchasePrice = (directLastPrice != null ? (directLastPrice.LastPrice ?? 0) : 0),
						SecurityId = dealUnderlyingDirect.SecurityID,
						SecurityTypeId = dealUnderlyingDirect.SecurityTypeID,
						TaxCostBase = dealUnderlyingDirect.TaxCostBase,
						TaxCostDate = dealUnderlyingDirect.TaxCostDate,
						NumberOfShares = dealUnderlyingDirect.NumberOfShares ?? 0,
						Percent = dealUnderlyingDirect.Percent ?? 0,
						RecordDate = dealUnderlyingDirect.RecordDate,
						SecurityType = (dealUnderlyingDirect.SecurityType != null ? dealUnderlyingDirect.SecurityType.Name : string.Empty),
						DealClosingId = dealUnderlyingDirect.DealClosingID,
						IssuerId = (dealUnderlyingDirect.SecurityTypeID == (int)Models.Deal.Enums.SecurityType.Equity ? (equity != null ? equity.Issuer.IssuerID : 0) : (fixedIncome != null ? fixedIncome.Issuer.IssuerID : 0)),
						IssuerName = (dealUnderlyingDirect.SecurityTypeID == (int)Models.Deal.Enums.SecurityType.Equity ? (equity != null ? equity.Issuer.Name : string.Empty) : (fixedIncome != null ? fixedIncome.Issuer.Name : string.Empty)),
						Security = (dealUnderlyingDirect.SecurityTypeID == (int)Models.Deal.Enums.SecurityType.Equity ? (equity != null ? equity.Symbol : string.Empty) : (fixedIncome != null ? fixedIncome.Symbol : string.Empty)),
					});
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
				IQueryable<DealUnderlyingDirectModel> query = GetDealUnderlyingDirectModel(context, context.DealUnderlyingDirects.Where(direct => direct.DealUnderlyingDirectID == dealUnderlyingDirectId));
				return (query != null ? query.SingleOrDefault() : null);
			}
		}

		public List<DealUnderlyingDirectModel> GetAllDealUnderlyingDirects(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return GetDealUnderlyingDirectModel(context, context.DealUnderlyingDirects.Where(direct => direct.DealID == dealId)).ToList();
			}
		}

		public List<DealUnderlyingDirect> GetAllDealUnderlyingDirects(int oldSecurityTypeId, int oldSecurityId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealUnderlyingDirects.Where(direct => direct.SecurityTypeID == oldSecurityTypeId && direct.SecurityID == oldSecurityId).ToList();
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

		public List<DealUnderlyingDirect> GetDealUnderlyingDirects(int dealId, int dealCloseId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealUnderlyingDirects.Where(dealUnderlyingDirect => dealUnderlyingDirect.DealID == dealId && dealUnderlyingDirect.DealClosingID == dealCloseId).ToList();
			}
		}

		public List<DealUnderlyingDirect> GetAllDealClosingUnderlyingDirects(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealUnderlyingDirects.Where(dealUnderlyingDirect => dealUnderlyingDirect.DealID == dealId && dealUnderlyingDirect.DealClosingID != null).ToList();
			}
		}

		public bool DeleteDealUnderlyingDirect(int dealUnderlyingDirectId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				DealUnderlyingDirect dealUnderlyingDirect = context.DealUnderlyingDirects.SingleOrDefault(underlyingFund => underlyingFund.DealUnderlyingDirectID == dealUnderlyingDirectId);
				if (dealUnderlyingDirect != null) {
					List<SecurityConversionDetail> securityConversionDetails = dealUnderlyingDirect.SecurityConversionDetails.ToList();
					foreach (var conversionDetail in securityConversionDetails) {
						context.SecurityConversionDetails.DeleteObject(conversionDetail);
					}
					context.DealUnderlyingDirects.DeleteObject(dealUnderlyingDirect);
					context.SaveChanges();
				}
				return true;
			}
		}

		public IEnumerable<ErrorInfo> SaveDealUnderlyingDirect(DealUnderlyingDirect dealUnderlyingDirect) {
			return dealUnderlyingDirect.Save();
		}

		public List<AutoCompleteList> FindDealUnderlyingDirects(string directName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> directListQuery = (from direct in context.DealUnderlyingDirects
																join equity in context.Equities on direct.SecurityID equals equity.EquityID into equities
																join fixedIncome in context.FixedIncomes on direct.SecurityID equals fixedIncome.FixedIncomeID into fixedIncomes
																from equity in equities.DefaultIfEmpty()
																from fixedIncome in fixedIncomes.DefaultIfEmpty()
																where direct.Deal.DealName.Contains(directName)
																orderby direct.Deal.DealName
																select new AutoCompleteList {
																	id = direct.DealUnderlyingDirectID,
																	label = direct.Deal.DealName + ">" +
																	(direct.SecurityTypeID == (int)Models.Deal.Enums.SecurityType.Equity ? (equity != null ? (equity.Symbol + ">" + equity.EquityType.Equity) : string.Empty) : (fixedIncome != null ? (fixedIncome.Symbol + ">" + fixedIncome.FixedIncomeType.FixedIncomeType1) : string.Empty))
																	,
																	value = direct.Deal.DealName
																});
				return new PaginatedList<AutoCompleteList>(directListQuery, 1, 20);
			}
		}

		public List<AutoCompleteList> FindEquityFixedIncomeIssuers(string issuerName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				string equitySecurityTypeId = ((int)DeepBlue.Models.Deal.Enums.SecurityType.Equity).ToString();
				string fixedIncomeSecurityTypeId = ((int)DeepBlue.Models.Deal.Enums.SecurityType.FixedIncome).ToString();
				IQueryable<AutoCompleteList> issuerListQuery = (from equity in context.Equities
																where equity.Issuer.Name.Contains(issuerName)
																orderby equity.Issuer.Name
																select new AutoCompleteList {
																	id = equity.IssuerID,
																	label = equity.Issuer.Name + ">>Equity>>" + equity.Symbol,
																	value = equity.Issuer.Name + "||" + equitySecurityTypeId + "||" + SqlFunctions.StringConvert((double)equity.EquityID, 10),
																}).Union(
																(from fixedIncome in context.FixedIncomes
																 where fixedIncome.Issuer.Name.Contains(issuerName)
																 orderby fixedIncome.Issuer.Name
																 select new AutoCompleteList {
																	 id = fixedIncome.IssuerID,
																	 label = fixedIncome.Issuer.Name + ">>FixedIncome>>" + fixedIncome.Symbol,
																	 value = fixedIncome.Issuer.Name + "||" + fixedIncomeSecurityTypeId + "||" + SqlFunctions.StringConvert((double)fixedIncome.FixedIncomeID, 10)
																 }))
																.OrderBy(list => list.label);
				return new PaginatedList<AutoCompleteList>(issuerListQuery, 1, 20);
			}
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
												  IsFinalClose = dealClosing.IsFinalClose,
												  FundId = dealClosing.Deal.FundID
											  }).SingleOrDefault();
				if (model == null) {
					model = new CreateDealCloseModel();
					model.DealNumber = GetMaxDealClosingNumber(dealId);
					model.DealId = dealId;
					model.FundId = context.Deals.Where(deal => deal.DealID == dealId).Select(deal => deal.FundID).SingleOrDefault();
				}
				if (dealId > 0) {
					IQueryable<DealUnderlyingDirect> dealUnderlyingDirects = context.DealUnderlyingDirects.Where(direct => direct.DealID == dealId && (direct.DealClosingID == null || direct.DealClosingID == dealClosingId));
					model.DealUnderlyingDirects = GetDealUnderlyingDirectModel(context, dealUnderlyingDirects).ToList();
					IQueryable<DealUnderlyingFund> dealUnderlyingFunds = context.DealUnderlyingFunds.Where(fund => fund.DealID == dealId && (fund.DealClosingID == null || fund.DealClosingID == dealClosingId));
					model.DealUnderlyingFunds = GetDealUnderlyingFundModel(context, dealUnderlyingFunds).ToList();
				}
				return model;
			}
		}

		public FinalDealCloseModel GetFinalDealClosingModel(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				FinalDealCloseModel model = new FinalDealCloseModel();
				IQueryable<DealUnderlyingDirect> dealUnderlyingDirects = context.DealUnderlyingDirects.Where(direct => direct.DealID == dealId && direct.DealClosingID != null);
				model.DealUnderlyingDirects = GetDealUnderlyingDirectModel(context, dealUnderlyingDirects).ToList();
				IQueryable<DealUnderlyingFund> dealUnderlyingFunds = context.DealUnderlyingFunds.Where(fund => fund.DealID == dealId && fund.DealClosingID != null);
				model.DealUnderlyingFunds = GetDealUnderlyingFundModel(context, dealUnderlyingFunds).ToList();
				return model;
			}
		}

		public DealClosing FindDealClosing(int dealClosingId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealClosings.Where(dealClosing => dealClosing.DealClosingID == dealClosingId).SingleOrDefault();
			}
		}

		public List<DealClosing> GetAllDealClosing(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealClosings.Where(dealClosing => dealClosing.DealID == dealId).ToList();
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
															FundName = dealClose.Deal.Fund.FundName,
															DealNumber = dealClose.DealNumber,
															TotalGrossPurchasePrice = dealClose.Deal.DealUnderlyingFunds
																					  .Where(fund => fund.DealClosingID == dealClose.DealClosingID)
																					  .Sum(fund => fund.GrossPurchasePrice),
															TotalPostRecordCapitalCall = dealClose.Deal.DealUnderlyingFunds
																					  .Where(fund => fund.DealClosingID == dealClose.DealClosingID)
																					  .Sum(fund => fund.PostRecordDateCapitalCall),
															TotalPostRecordDateDistribution = dealClose.Deal.DealUnderlyingFunds
																					  .Where(fund => fund.DealClosingID == dealClose.DealClosingID)
																					  .Sum(fund => fund.PostRecordDateDistribution)
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
														 UnfundedAmount = deal.DealUnderlyingFunds.Sum(df => df.UnfundedAmount),
														 CommittedAmount = deal.DealUnderlyingFunds.Sum(df => df.CommittedAmount)
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
					deal.DealUnderlyingFunds = GetDealUnderlyingFundModel(context, context.DealUnderlyingFunds.Where(fund => fund.DealID == deal.DealId)).ToList();
					deal.DealUnderlyingDirects = GetDealUnderlyingDirectModel(context, context.DealUnderlyingDirects.Where(direct => direct.DealID == deal.DealId)).ToList();
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

		public CreateUnderlyingFundModel FindUnderlyingFundModel(int underlyingFundId, int issuerId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				CreateUnderlyingFundModel model = (from underlyingFund in context.UnderlyingFunds
												   where underlyingFund.UnderlyingtFundID == underlyingFundId
												   select new CreateUnderlyingFundModel {
													   FundName = underlyingFund.FundName,
													   FundTypeId = underlyingFund.FundTypeID,
													   GeographyId = underlyingFund.GeographyID,
													   IndustryId = underlyingFund.IndustryID,
													   IssuerId = underlyingFund.IssuerID,
													   IssuerName = (underlyingFund.Issuer != null ? underlyingFund.Issuer.Name : String.Empty),
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
				else {
					model = new CreateUnderlyingFundModel();
					model.IssuerId = issuerId;
					model.IssuerName = context.Issuers.Where(issuer => issuer.IssuerID == issuerId).Select(issuer => issuer.Name).SingleOrDefault();
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

		public List<UnderlyingFundCashDistributionModel> GetAllUnderlyingFundCashDistributions(int underlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				var dealUnderlyingFundQuery = (from underlyingFund in context.DealUnderlyingFunds
											   join deal in context.Deals on underlyingFund.DealID equals deal.DealID
											   where underlyingFund.UnderlyingFundID == underlyingFundId && underlyingFund.DealClosingID != null
											   group deal by deal.FundID into deals
											   select new {
												   FundID = deals.Key,
												   UnderlyingFundID = underlyingFundId
											   });
				var newCashDistributionQuery = (from dealUnderlyingFund in dealUnderlyingFundQuery
												join fund in context.Funds on dealUnderlyingFund.FundID equals fund.FundID
												join underlyingFund in context.UnderlyingFunds on dealUnderlyingFund.UnderlyingFundID equals underlyingFund.UnderlyingtFundID
												select new UnderlyingFundCashDistributionModel {
													FundId = fund.FundID,
													FundName = fund.FundName,
													UnderlyingFundId = underlyingFund.UnderlyingtFundID,
													UnderlyingFundName = underlyingFund.FundName,
													Deals = (from dealuf in context.DealUnderlyingFunds
															 where dealuf.UnderlyingFundID == underlyingFund.UnderlyingtFundID && dealuf.Deal.FundID == fund.FundID && dealuf.DealClosingID != null
															 group dealuf by dealuf.DealID into deals
															 select new ActivityDealModel {
																 CommitmentAmount = deals.Sum(duf => duf.CommittedAmount),
																 DealId = deals.FirstOrDefault().DealID,
																 FundId = deals.FirstOrDefault().Deal.FundID,
																 DealName = deals.FirstOrDefault().Deal.DealName,
																 DealNumber = deals.FirstOrDefault().Deal.DealNumber
															 })
												});
				return newCashDistributionQuery.OrderBy(cd => cd.FundName).ToList();
			}
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
					return true;
				}
				return false;
			}
		}

		#endregion

		#region UnderlyingFundPostRecordCashDistribution

		public List<UnderlyingFundPostRecordCashDistributionModel> GetAllUnderlyingFundPostRecordCashDistributions(int underlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				var dealUnderlyingFundQuery = (from underlyingFund in context.DealUnderlyingFunds
											   join deal in context.Deals on underlyingFund.DealID equals deal.DealID
											   where underlyingFund.UnderlyingFundID == underlyingFundId && underlyingFund.DealClosingID == null
											   group deal by deal.DealID into deals
											   select new {
												   DealID = deals.Key,
												   UnderlyingFundID = underlyingFundId
											   });
				var postRecordDistributions = (from distribution in context.CashDistributions
											   where distribution.UnderlyingFundID == underlyingFundId && distribution.UnderluingFundCashDistributionID == null
											   select distribution);
				var newPRCashDistributionQuery = (from dealUnderlyingFund in dealUnderlyingFundQuery
												  join deal in context.Deals on dealUnderlyingFund.DealID equals deal.DealID
												  join underlyingFund in context.UnderlyingFunds on dealUnderlyingFund.UnderlyingFundID equals underlyingFund.UnderlyingtFundID
												  select new UnderlyingFundPostRecordCashDistributionModel {
													  DealId = deal.DealID,
													  DealName = deal.DealName,
													  FundName = deal.Fund.FundName,
													  UnderlyingFundId = underlyingFund.UnderlyingtFundID
												  });
				return newPRCashDistributionQuery.OrderBy(cd => cd.FundName).ToList();
			}
		}

		public CashDistribution FindUnderlyingFundPostRecordCashDistribution(int cashDistributionId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CashDistributions.Where(cashDistribution => cashDistribution.CashDistributionID == cashDistributionId).SingleOrDefault();
			}
		}

		public CashDistribution FindUnderlyingFundPostRecordCashDistribution(int underlyingFundCashDistributionId, int underlyingFundId, int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CashDistributions.Where(cashDistribution => cashDistribution.UnderluingFundCashDistributionID == underlyingFundCashDistributionId
														&& cashDistribution.UnderlyingFundID == underlyingFundId
														&& cashDistribution.DealID == dealId).SingleOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveUnderlyingFundPostRecordCashDistribution(CashDistribution cashDistribution) {
			return cashDistribution.Save();
		}

		public bool DeleteUnderlyingFundPostRecordCashDistribution(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				CashDistribution cashDistribution = context.CashDistributions.Where(distribution => distribution.CashDistributionID == id).SingleOrDefault();
				if (cashDistribution != null) {
					context.CashDistributions.DeleteObject(cashDistribution);
					context.SaveChanges();
					List<DealUnderlyingFund> dealUnderlyingFunds = context.DealUnderlyingFunds.Where(fund => fund.UnderlyingFundID == cashDistribution.UnderlyingFundID && fund.DealID == cashDistribution.DealID).ToList();
					foreach (var dealUnderlyingFund in dealUnderlyingFunds) {
						if (dealUnderlyingFund.DealClosingID == null) {
							dealUnderlyingFund.PostRecordDateDistribution = GetSumOfCashDistribution(dealUnderlyingFund.UnderlyingFundID, dealUnderlyingFund.DealID);
							dealUnderlyingFund.Save();
						}
					}
					return true;
				}
				return false;
			}
		}

		public decimal GetSumOfCashDistribution(int underlyingFundId, int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				decimal totalCashDistribution = 0;
				IQueryable<CashDistribution> query = context.CashDistributions
					.Where(cashDistribution => cashDistribution.UnderlyingFundID == underlyingFundId && cashDistribution.DealID == dealId && cashDistribution.UnderluingFundCashDistributionID == null);
				if (query.Count() > 0)
					totalCashDistribution = query.Sum(cashDistribution => cashDistribution.Amount);
				return totalCashDistribution;
			}
		}

		#endregion

		#region UnderlyingFundCapitalCall

		public UnderlyingFundCapitalCall FindUnderlyingFundCapitalCall(int underlyingFundCapitalCallId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalCall in context.UnderlyingFundCapitalCalls
						where capitalCall.UnderlyingFundCapitalCallID == underlyingFundCapitalCallId
						select capitalCall).FirstOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveUnderlyingFundCapitalCall(UnderlyingFundCapitalCall underlyingFundCapitalCall) {
			return underlyingFundCapitalCall.Save();
		}

		public List<UnderlyingFundCapitalCallModel> GetAllUnderlyingFundCapitalCalls(int underlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				var dealUnderlyingFundQuery = (from underlyingFund in context.DealUnderlyingFunds
											   join deal in context.Deals on underlyingFund.DealID equals deal.DealID
											   where underlyingFund.UnderlyingFundID == underlyingFundId && underlyingFund.DealClosingID != null
											   group deal by deal.FundID into deals
											   select new {
												   FundID = deals.Key,
												   UnderlyingFundID = underlyingFundId
											   });
				var newCapitalCallQuery = (from dealUnderlyingFund in dealUnderlyingFundQuery
										   join fund in context.Funds on dealUnderlyingFund.FundID equals fund.FundID
										   join underlyingFund in context.UnderlyingFunds on dealUnderlyingFund.UnderlyingFundID equals underlyingFund.UnderlyingtFundID
										   select new UnderlyingFundCapitalCallModel {
											   FundId = fund.FundID,
											   FundName = fund.FundName,
											   UnderlyingFundId = underlyingFund.UnderlyingtFundID,
											   Deals = (from dealuf in context.DealUnderlyingFunds
														where dealuf.UnderlyingFundID == underlyingFund.UnderlyingtFundID && dealuf.Deal.FundID == fund.FundID && dealuf.DealClosingID != null
														group dealuf by dealuf.DealID into deals
														select new ActivityDealModel {
															CommitmentAmount = deals.Sum(duf => duf.CommittedAmount),
															DealId = deals.FirstOrDefault().DealID,
															FundId = deals.FirstOrDefault().Deal.FundID,
															DealName = deals.FirstOrDefault().Deal.DealName,
															DealNumber = deals.FirstOrDefault().Deal.DealNumber
														})
										   });
				return newCapitalCallQuery.OrderBy(cc => cc.FundName).ToList();
			}
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
					return true;
				}
				return false;
			}
		}

		#endregion
		
		#region UnderlyingFundPostRecordCapitalCall

		public UnderlyingFundCapitalCallLineItem FindUnderlyingFundPostRecordCapitalCall(int underlyingFundCapitalCallLineItemId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalCall in context.UnderlyingFundCapitalCallLineItems
						where capitalCall.UnderlyingFundCapitalCallLineItemID == underlyingFundCapitalCallLineItemId
						select capitalCall).SingleOrDefault();
			}
		}

		public UnderlyingFundCapitalCallLineItem FindUnderlyingFundPostRecordCapitalCall(int underlyingFundCapitalCallId, int underlyingFundId, int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.UnderlyingFundCapitalCallLineItems.Where(lineItem => lineItem.UnderlyingFundCapitalCallID == underlyingFundCapitalCallId
														&& lineItem.UnderlyingFundID == underlyingFundId
														&& lineItem.DealID == dealId).FirstOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveUnderlyingFundPostRecordCapitalCall(UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem) {
			return underlyingFundCapitalCallLineItem.Save();
		}

		public List<UnderlyingFundPostRecordCapitalCallModel> GetAllUnderlyingFundPostRecordCapitalCalls(int underlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				var dealUnderlyingFundQuery = (from underlyingFund in context.DealUnderlyingFunds
											   join deal in context.Deals on underlyingFund.DealID equals deal.DealID
											   where underlyingFund.UnderlyingFundID == underlyingFundId && underlyingFund.DealClosingID == null
											   group deal by deal.DealID into deals
											   select new {
												   DealID = deals.Key,
												   UnderlyingFundID = underlyingFundId
											   });
				var postRecordCapitalCalls = (from capitalCall in context.UnderlyingFundCapitalCallLineItems
											  where capitalCall.UnderlyingFundID == underlyingFundId && capitalCall.UnderlyingFundCapitalCallID == null
											  select capitalCall);
				var newPRCapitalCallQuery = (from dealUnderlyingFund in dealUnderlyingFundQuery
											 join deal in context.Deals on dealUnderlyingFund.DealID equals deal.DealID
											 join underlyingFund in context.UnderlyingFunds on dealUnderlyingFund.UnderlyingFundID equals underlyingFund.UnderlyingtFundID
											 select new UnderlyingFundPostRecordCapitalCallModel {
												 DealId = deal.DealID,
												 DealName = deal.DealName,
												 FundId = deal.FundID,
												 FundName = deal.Fund.FundName,
												 UnderlyingFundId = underlyingFund.UnderlyingtFundID
											 });
				return newPRCapitalCallQuery.OrderBy(cc => cc.FundName).ToList();
			}
		}

		public bool DeleteUnderlyingFundPostRecordCapitalCall(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem = context.UnderlyingFundCapitalCallLineItems.Where(capitalCall => capitalCall.UnderlyingFundCapitalCallLineItemID == id).SingleOrDefault();
				if (underlyingFundCapitalCallLineItem != null) {
					context.UnderlyingFundCapitalCallLineItems.DeleteObject(underlyingFundCapitalCallLineItem);
					context.SaveChanges();
					List<DealUnderlyingFund> dealUnderlyingFunds = context.DealUnderlyingFunds.Where(fund => fund.UnderlyingFundID == underlyingFundCapitalCallLineItem.UnderlyingFundID && fund.DealID == underlyingFundCapitalCallLineItem.DealID).ToList();
					foreach (var dealUnderlyingFund in dealUnderlyingFunds) {
						if (dealUnderlyingFund.DealClosingID == null) {
							dealUnderlyingFund.PostRecordDateCapitalCall = GetSumOfUnderlyingFundCapitalCallLineItem(dealUnderlyingFund.UnderlyingFundID, dealUnderlyingFund.DealID);
							dealUnderlyingFund.UnfundedAmount = dealUnderlyingFund.UnfundedAmount + underlyingFundCapitalCallLineItem.Amount;
							dealUnderlyingFund.Save();
						}
					}
					return true;
				}
				return false;
			}
		}

		public decimal GetSumOfUnderlyingFundCapitalCallLineItem(int underlyingFundId, int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				decimal totalCapitalCall = 0;
				IQueryable<UnderlyingFundCapitalCallLineItem> query = context.UnderlyingFundCapitalCallLineItems
					.Where(lineItem => lineItem.UnderlyingFundID == underlyingFundId && lineItem.DealID == dealId && lineItem.UnderlyingFundCapitalCallID == null);
				if (query.Count() > 0)
					totalCapitalCall = query.Sum(lineItem => lineItem.Amount);
				return totalCapitalCall;
			}
		}

		#endregion

		#region UnderlyingFundValuation

		public List<UnderlyingFundValuationModel> GetAllUnderlyingFundValuations(int underlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return GetUnderlyingFundValuationModel(context, underlyingFundId).ToList();
			}
		}

		public UnderlyingFundValuationModel FindUnderlyingFundValuationModel(int underlyingFundId, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<UnderlyingFundValuationModel> query = GetUnderlyingFundValuationModel(context, underlyingFundId);
				return (from valuation in query
						where valuation.FundId == fundId
						select valuation).SingleOrDefault();
			}
		}

		public bool DeleteUnderlyingFundValuation(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				UnderlyingFundNAV underlyingFundNAV = context.UnderlyingFundNAVs.Where(fundNAV => fundNAV.UnderlyingFundNAVID == id).SingleOrDefault();
				if (underlyingFundNAV != null) {
					List<UnderlyingFundNAVHistory> underlyingFundNAVHistories = underlyingFundNAV.UnderlyingFundNAVHistories.ToList();
					foreach (var navHistory in underlyingFundNAVHistories) {
						context.UnderlyingFundNAVHistories.DeleteObject(navHistory);
					}
					context.UnderlyingFundNAVs.DeleteObject(underlyingFundNAV);
					context.SaveChanges();
					return true;
				}
				return false;
			}
		}

		public UnderlyingFundNAV FindUnderlyingFundNAV(int underlyingFundId, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.UnderlyingFundNAVs.Where(fundNAV => fundNAV.UnderlyingFundID == underlyingFundId && fundNAV.FundID == fundId).SingleOrDefault();
			}
		}

		public decimal SumOfTotalCapitalCalls(int underlyingFundId, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<UnderlyingFundCapitalCall> underlyingFundCapitalCalls = context.UnderlyingFundCapitalCalls.Where(capitalCall => capitalCall.UnderlyingFundID == underlyingFundId && capitalCall.FundID == fundId);
				return (underlyingFundCapitalCalls.Count() > 0 ? underlyingFundCapitalCalls.Sum(capitalCall => capitalCall.Amount) : 0);
			}
		}

		public decimal SumOfTotalDistributions(int underlyingFundId, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<UnderlyingFundCashDistribution> underlyingFundCashDistributions = context.UnderlyingFundCashDistributions.Where(distribution => distribution.UnderlyingFundID == underlyingFundId && distribution.FundID == fundId);
				return (underlyingFundCashDistributions.Count() > 0 ? underlyingFundCashDistributions.Sum(capitalCall => capitalCall.Amount) : 0);
			}
		}

		public IEnumerable<ErrorInfo> SaveUnderlyingFundNAV(UnderlyingFundNAV underlyingFundNAV) {
			return underlyingFundNAV.Save();
		}

		private IQueryable<UnderlyingFundValuationModel> GetUnderlyingFundValuationModel(DeepBlueEntities context, int underlyingFundId) {
			var underlyingFundQuery = (from underlyingFund in context.DealUnderlyingFunds
									   join deal in context.Deals on underlyingFund.DealID equals deal.DealID
									   where underlyingFund.UnderlyingFundID == underlyingFundId
									   group deal by deal.FundID into deals
									   select new {
										   FundID = deals.Key,
										   UnderlyingFundID = underlyingFundId
									   });
			DateTime todayDate;
			DateTime.TryParse(DateTime.Now.ToString("MM/dd/yyyy"), out todayDate);
			var query = (from dealUnderlyingFund in underlyingFundQuery
						 join fund in context.Funds on dealUnderlyingFund.FundID equals fund.FundID
						 join underlyingFund in context.UnderlyingFunds on dealUnderlyingFund.UnderlyingFundID equals underlyingFund.UnderlyingtFundID
						 join underlyingFundNAV in context.UnderlyingFundNAVs on new {
							 dealUnderlyingFund.UnderlyingFundID,
							 dealUnderlyingFund.FundID
						 } equals new {
							 underlyingFundNAV.UnderlyingFundID,
							 underlyingFundNAV.FundID
						 } into underlyingFundNAVs
						 from underlyingFundNAV in underlyingFundNAVs.DefaultIfEmpty()
						 select new UnderlyingFundValuationModel {
							 FundId = fund.FundID,
							 FundName = fund.FundName,
							 UnderlyingFundId = underlyingFund.UnderlyingtFundID,
							 UnderlyingFundName = underlyingFund.FundName,
							 FundNAV = (underlyingFundNAV != null ? underlyingFundNAV.FundNAV : 0),
							 FundNAVDate = (underlyingFundNAV != null ? underlyingFundNAV.FundNAVDate : DateTime.MinValue),
							 TotalCapitalCall = (from cc in context.UnderlyingFundCapitalCalls
												 where cc.UnderlyingFundID == underlyingFund.UnderlyingtFundID
												 && cc.FundID == fund.FundID
												 && cc.NoticeDate >= (underlyingFundNAV != null ? EntityFunctions.TruncateTime(underlyingFundNAV.FundNAVDate) : todayDate)
												 select cc.Amount).Sum(),
							 TotalPostRecordCapitalCall = (from pcc in context.UnderlyingFundCapitalCallLineItems
														   where pcc.UnderlyingFundID == underlyingFund.UnderlyingtFundID
														   && pcc.Deal.FundID == fund.FundID
														   && pcc.CapitalCallDate >= (underlyingFundNAV != null ? EntityFunctions.TruncateTime(underlyingFundNAV.FundNAVDate) : todayDate)
														   && pcc.UnderlyingFundCapitalCallID == null
														   select pcc.Amount).Sum(),
							 TotalDistribution = (from ds in context.UnderlyingFundCashDistributions
												  where ds.UnderlyingFundID == underlyingFund.UnderlyingtFundID
												  && ds.FundID == fund.FundID
												  && ds.NoticeDate >= (underlyingFundNAV != null ? EntityFunctions.TruncateTime(underlyingFundNAV.FundNAVDate) : todayDate)
												  select ds.Amount).Sum(),
							 TotalPostRecordDistribution = (from pds in context.CashDistributions
															where pds.UnderlyingFundID == underlyingFund.UnderlyingtFundID
															&& pds.Deal.FundID == fund.FundID
															&& pds.DistributionDate >= (underlyingFundNAV != null ? EntityFunctions.TruncateTime(underlyingFundNAV.FundNAVDate) : todayDate)
															&& pds.UnderluingFundCashDistributionID == null
															select pds.Amount).Sum(),
							 UnderlyingFundNAVId = (underlyingFundNAV != null ? underlyingFundNAV.UnderlyingFundNAVID : 0),
							 UpdateNAV = underlyingFundNAV.FundNAV
						 });

			return query;
		}

		#endregion

		#region UnderlyingFundNAVHistory

		public IEnumerable<ErrorInfo> SaveUnderlyingFundNAVHistory(UnderlyingFundNAVHistory underlyingFundNAVHistroy) {
			return underlyingFundNAVHistroy.Save();
		}

		#endregion

		#region NewHoldingPattern

		public List<NewHoldingPatternModel> NewHoldingPatternList(int activityTypeId, int activityId, int securityTypeId, int securityId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<NewHoldingPatternModel> newHoldingPatternQuery = null;
				switch ((DeepBlue.Models.Deal.Enums.ActivityType)activityTypeId) {
					case Models.Deal.Enums.ActivityType.Split:
						newHoldingPatternQuery = (from direct in context.DealUnderlyingDirects
												  where direct.SecurityTypeID == securityTypeId && direct.SecurityID == securityId
												  group direct by direct.Deal.FundID into directs
												  join fund in context.Funds on directs.Key equals fund.FundID
												  from equitySplit in context.EquitySplits
												  where equitySplit.EquiteSplitID == activityId
												  select new NewHoldingPatternModel {
													  FundId = fund.FundID,
													  FundName = fund.FundName,
													  OldNoOfShares = directs.Sum(d => d.NumberOfShares),
													  NewNoOfShares = directs.Sum(d => d.NumberOfShares) * equitySplit.SplitFactor,
												  });
						break;
					case Models.Deal.Enums.ActivityType.Conversion:
						newHoldingPatternQuery = (from direct in context.DealUnderlyingDirects
												  where direct.SecurityTypeID == securityTypeId && direct.SecurityID == securityId
												  group direct by direct.Deal.FundID into directs
												  join fund in context.Funds on directs.Key equals fund.FundID
												  from securityConversion in context.SecurityConversions
												  where securityConversion.SecurityConversionID == activityId
												  select new NewHoldingPatternModel {
													  FundId = fund.FundID,
													  FundName = fund.FundName,
													  NewNoOfShares = directs.Sum(d => d.NumberOfShares),
													  OldNoOfShares = securityConversion.SecurityConversionDetails.Sum(cd => cd.OldNumberOfShares)
												  });
						break;
				}
				if (newHoldingPatternQuery != null)
					return newHoldingPatternQuery.ToList();
				else
					return null;
			}
		}

		#endregion

		#region EquitySplit

		public EquitySplit FindEquitySplit(int equityId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.EquitySplits.Where(equity => equity.EquityID == equityId).SingleOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveEquitySplit(EquitySplit equitySplit) {
			return equitySplit.Save();
		}

		#endregion

		#region SecurityConversion

		public SecurityConversion FindSecurityConversion(int newSecurityId, int newSecurityTypeId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.SecurityConversions.Where(securityConversion => securityConversion.NewSecurityID == newSecurityId && securityConversion.NewSecurityTypeID == newSecurityTypeId).SingleOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveSecurityConversion(SecurityConversion securityConversion) {
			return securityConversion.Save();
		}

		public IEnumerable<ErrorInfo> SaveSecurityConversionDetail(SecurityConversionDetail securityConversionDetail) {
			return securityConversionDetail.Save();
		}
		#endregion

		#region FundActivityHistory

		public IEnumerable<ErrorInfo> SaveFundActivityHistory(FundActivityHistory fundActivityHistory) {
			return fundActivityHistory.Save();
		}

		#endregion

		#region FundExpense

		public FundExpense FindFundExpense(int fundExpenseId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.FundExpenses.Where(fundExpense => fundExpense.FundExpenseID == fundExpenseId).SingleOrDefault();
			}
		}

		public List<FundExpenseModel> GetAllFundExpenses(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return GetFundExpenseModel(context.FundExpenses.Where(fundExpense => fundExpense.FundID == fundId)).ToList();
			}
		}

		public FundExpenseModel FindFundExpenseModel(int fundExpenseId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return GetFundExpenseModel(context.FundExpenses.Where(fundExpense => fundExpense.FundExpenseID == fundExpenseId)).SingleOrDefault();
			}
		}

		private IQueryable<FundExpenseModel> GetFundExpenseModel(IQueryable<FundExpense> fundExpenses) {
			return (from fundExpense in fundExpenses
					select new FundExpenseModel {
						Amount = fundExpense.Amount,
						Date = fundExpense.Date,
						FundExpenseId = fundExpense.FundExpenseID,
						FundExpenseTypeId = fundExpense.FundExpenseTypeID,
						FundId = fundExpense.FundID,
						FundExpenseType = fundExpense.FundExpenseType.Name
					}).OrderByDescending(fundExpense => fundExpense.Date);
		}

		public IEnumerable<ErrorInfo> SaveFundExpense(FundExpense fundExpense) {
			return fundExpense.Save();
		}

		#endregion

		#region UnderlyingDirectValuation

		public List<UnderlyingDirectValuationModel> UnderlyingDirectValuationList(int issuerId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				var dealUnderlyingDirectsQuery = (from dealDirect in context.DealUnderlyingDirects
												  group dealDirect by new { dealDirect.Deal.FundID, dealDirect.SecurityID, dealDirect.SecurityTypeID } into dealDirects
												  select dealDirects);
				var equityValuationQuery = (from equityDirect in dealUnderlyingDirectsQuery
											join equity in context.Equities on equityDirect.Key.SecurityID equals equity.EquityID
											join fund in context.Funds on equityDirect.Key.FundID equals fund.FundID
											join directValuation in context.UnderlyingDirectLastPrices on new {
												equityDirect.Key.FundID,
												equityDirect.Key.SecurityID,
												equityDirect.Key.SecurityTypeID
											} equals new {
												directValuation.FundID,
												directValuation.SecurityID,
												directValuation.SecurityTypeID
											} into directValuations
											from directValuation in directValuations.DefaultIfEmpty()
											where equity.IssuerID == issuerId && equityDirect.Key.SecurityTypeID == (int)DeepBlue.Models.Deal.Enums.SecurityType.Equity
											select new UnderlyingDirectValuationModel {
												FundId = fund.FundID,
												FundName = fund.FundName,
												DirectName = equity.Issuer.Name + ">" + (equity.EquityType != null ? equity.EquityType.Equity : string.Empty) + ">" + (equity.ShareClassType != null ? equity.ShareClassType.ShareClass : string.Empty),
												SecurityId = equityDirect.Key.SecurityID,
												SecurityTypeId = equityDirect.Key.SecurityTypeID,
												LastPrice = directValuation.LastPrice,
												LastPriceDate = directValuation.LastPriceDate,
												NewPrice = directValuation.LastPrice,
												NewPriceDate = (directValuation != null ? directValuation.LastPriceDate : DateTime.Now),
												UnderlyingDirectLastPriceId = (directValuation != null ? directValuation.UnderlyingDirectLastPriceID : 0)
											});
				var fixedIncomeValuationQuery = (from fixedIncomeDirect in dealUnderlyingDirectsQuery
												 join fixedIncome in context.FixedIncomes on fixedIncomeDirect.Key.SecurityID equals fixedIncome.FixedIncomeID
												 join fund in context.Funds on fixedIncomeDirect.Key.FundID equals fund.FundID
												 join directValuation in context.UnderlyingDirectLastPrices on new {
													 fixedIncomeDirect.Key.FundID,
													 fixedIncomeDirect.Key.SecurityID,
													 fixedIncomeDirect.Key.SecurityTypeID
												 } equals new {
													 directValuation.FundID,
													 directValuation.SecurityID,
													 directValuation.SecurityTypeID
												 } into directValuations
												 from directValuation in directValuations.DefaultIfEmpty()
												 where fixedIncome.IssuerID == issuerId && fixedIncomeDirect.Key.SecurityTypeID == (int)DeepBlue.Models.Deal.Enums.SecurityType.FixedIncome
												 select new UnderlyingDirectValuationModel {
													 FundId = fund.FundID,
													 FundName = fund.FundName,
													 DirectName = fixedIncome.Issuer.Name + ">" + (fixedIncome.FixedIncomeType != null ? fixedIncome.FixedIncomeType.FixedIncomeType1 : string.Empty),
													 SecurityId = fixedIncomeDirect.Key.SecurityID,
													 SecurityTypeId = fixedIncomeDirect.Key.SecurityTypeID,
													 LastPrice = directValuation.LastPrice,
													 LastPriceDate = directValuation.LastPriceDate,
													 NewPrice = directValuation.LastPrice,
													 NewPriceDate = (directValuation != null ? directValuation.LastPriceDate : DateTime.Now),
													 UnderlyingDirectLastPriceId = (directValuation != null ? directValuation.UnderlyingDirectLastPriceID : 0)
												 });
				return equityValuationQuery.Union(fixedIncomeValuationQuery).ToList();
			}
		}

		public UnderlyingDirectValuationModel FindUnderlyingDirectValuationModel(int underlyingDirectLastPriceId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from directValuation in context.UnderlyingDirectLastPrices
						join equity in context.Equities on directValuation.SecurityID equals equity.EquityID into equities
						from equity in equities.DefaultIfEmpty()
						join fixedIncome in context.FixedIncomes on directValuation.SecurityID equals fixedIncome.FixedIncomeID into fixedIncomes
						from fixedIncome in fixedIncomes.DefaultIfEmpty()
						where directValuation.UnderlyingDirectLastPriceID == underlyingDirectLastPriceId
						select new UnderlyingDirectValuationModel {
							FundId = directValuation.FundID,
							FundName = directValuation.Fund.FundName,
							SecurityId = directValuation.SecurityID,
							SecurityTypeId = directValuation.SecurityTypeID,
							LastPrice = directValuation.LastPrice,
							LastPriceDate = directValuation.LastPriceDate,
							UnderlyingDirectLastPriceId = directValuation.UnderlyingDirectLastPriceID
						}).SingleOrDefault();
			}
		}

		public UnderlyingDirectLastPrice FindUnderlyingDirectLastPrice(int fundId, int securityId, int securityTypeId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.UnderlyingDirectLastPrices.Where(lastPrice => lastPrice.FundID == fundId && lastPrice.SecurityID == securityId && lastPrice.SecurityTypeID == securityTypeId).SingleOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveUnderlyingDirectValuation(UnderlyingDirectLastPrice underlyingDirectLastPrice) {
			return underlyingDirectLastPrice.Save();
		}

		public decimal FindLastPurchasePrice(int fundId, int securityId, int securityTypeId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from lastPrice in context.UnderlyingDirectLastPrices
						where lastPrice.FundID == fundId && lastPrice.SecurityID == securityId && lastPrice.SecurityTypeID == securityTypeId
						select lastPrice.LastPrice ?? 0).SingleOrDefault();
			}
		}

		public bool DeleteUnderlyingDirectValuation(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				UnderlyingDirectLastPrice underlyingDirectLastPrice = context.UnderlyingDirectLastPrices.Where(lastPrice => lastPrice.UnderlyingDirectLastPriceID == id).SingleOrDefault();
				if (underlyingDirectLastPrice != null) {
					List<UnderlyingDirectLastPriceHistory> underlyingDirectLastPriceHistories = underlyingDirectLastPrice.UnderlyingDirectLastPriceHistories.ToList();
					foreach (var lastPriceHistory in underlyingDirectLastPriceHistories) {
						context.UnderlyingDirectLastPriceHistories.DeleteObject(lastPriceHistory);
					}
					context.UnderlyingDirectLastPrices.DeleteObject(underlyingDirectLastPrice);
					context.SaveChanges();
					return true;
				}
				return false;
			}
		}

		#endregion

		#region UnderlyingDirectValuationHistory

		public IEnumerable<ErrorInfo> SaveUnderlyingDirectValuationHistory(UnderlyingDirectLastPriceHistory underlyingDirectLastPriceHistory) {
			return underlyingDirectLastPriceHistory.Save();
		}

		#endregion

		#region Reconcile
		public List<ReconcileListModel> GetAllReconciles(int pageIndex, int pageSize, DateTime fromDate, DateTime toDate, int underlyingFundId, int fundId, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				var reconciles = (from capitalCall in context.UnderlyingFundCapitalCalls
								  where capitalCall.ReceivedDate >= EntityFunctions.TruncateTime(fromDate) && capitalCall.ReceivedDate <= EntityFunctions.TruncateTime(toDate)
								  && (fundId > 0 ? capitalCall.FundID == fundId : capitalCall.FundID > 0)
								  select new ReconcileListModel {
									  EventType = 1,
									  CapitalCallAmount = capitalCall.Amount,
									  FundName = capitalCall.Fund.FundName,
									  PaymentOrReceivedDate = capitalCall.ReceivedDate,
									  UnderlyingFundName = capitalCall.UnderlyingFund.FundName,
									  DistributionAmount = 0,
									  PaidOnOrReceivedOn = false
								  })
								.Union(from capitalCall in context.UnderlyingFundCapitalCallLineItems
									   where capitalCall.CapitalCallDate >= EntityFunctions.TruncateTime(fromDate) && capitalCall.CapitalCallDate <= EntityFunctions.TruncateTime(toDate)
									   && (fundId > 0 ? capitalCall.Deal.FundID == fundId : capitalCall.Deal.FundID > 0)
									   && capitalCall.UnderlyingFundCapitalCallID == null
									   select new ReconcileListModel {
										   EventType = 2,
										   CapitalCallAmount = capitalCall.Amount,
										   FundName = capitalCall.Deal.Fund.FundName,
										   PaymentOrReceivedDate = capitalCall.CapitalCallDate,
										   UnderlyingFundName = capitalCall.UnderlyingFund.FundName,
										   DistributionAmount = 0,
										   PaidOnOrReceivedOn = false,
									   })
								.Union(from cashDistribution in context.UnderlyingFundCashDistributions
									   where cashDistribution.ReceivedDate >= EntityFunctions.TruncateTime(fromDate) && cashDistribution.ReceivedDate <= EntityFunctions.TruncateTime(toDate)
									   && (fundId > 0 ? cashDistribution.FundID == fundId : cashDistribution.FundID > 0)
									   select new ReconcileListModel {
										   EventType = 3,
										   CapitalCallAmount = 0,
										   FundName = cashDistribution.Fund.FundName,
										   PaymentOrReceivedDate = cashDistribution.ReceivedDate,
										   UnderlyingFundName = cashDistribution.UnderlyingFund.FundName,
										   DistributionAmount = cashDistribution.Amount,
										   PaidOnOrReceivedOn = false,
									   })
								.Union(from cashDistribution in context.CashDistributions
									   where cashDistribution.DistributionDate >= EntityFunctions.TruncateTime(fromDate) && cashDistribution.DistributionDate <= EntityFunctions.TruncateTime(toDate)
									   && (fundId > 0 ? cashDistribution.Deal.FundID == fundId : cashDistribution.Deal.FundID > 0)
									   && cashDistribution.UnderluingFundCashDistributionID == null
									   select new ReconcileListModel {
										   EventType = 4,
										   CapitalCallAmount = 0,
										   FundName = cashDistribution.Deal.Fund.FundName,
										   PaymentOrReceivedDate = cashDistribution.DistributionDate,
										   UnderlyingFundName = cashDistribution.UnderlyingFund.FundName,
										   DistributionAmount = cashDistribution.Amount,
										   PaidOnOrReceivedOn = false
									   });
				PaginatedList<ReconcileListModel> paginatedList = new PaginatedList<ReconcileListModel>(reconciles.OrderByDescending(re => re.PaymentOrReceivedDate), pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList.ToList();
			}
		}
		#endregion

		#region UnfundedAdjustment

		public List<UnfundedAdjustmentModel> GetAllUnfundedAdjustments(int underlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return GetUnfundedAdjustmentModel(context.DealUnderlyingFunds.Where(dealUnderlyingFund => dealUnderlyingFund.UnderlyingFundID == underlyingFundId)).ToList();
			}
		}

		public UnfundedAdjustmentModel FindUnfundedAdjustmentModel(int dealUnderlyingFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return GetUnfundedAdjustmentModel(context.DealUnderlyingFunds.Where(dealUnderlyingFund => dealUnderlyingFund.DealUnderlyingtFundID == dealUnderlyingFundId)).SingleOrDefault();
			}
		}

		private IQueryable<UnfundedAdjustmentModel> GetUnfundedAdjustmentModel(IQueryable<DealUnderlyingFund> dealUnderlyingFunds) {
			return (from dealUnderlyingFund in dealUnderlyingFunds
					select new UnfundedAdjustmentModel {
						CommitmentAmount = dealUnderlyingFund.CommittedAmount,
						UnfundedAmount = dealUnderlyingFund.UnfundedAmount,
						DealUnderlyingFundId = dealUnderlyingFund.DealUnderlyingtFundID,
						FundName = dealUnderlyingFund.Deal.Fund.FundName
					});
		}

		public IEnumerable<ErrorInfo> SaveDealUnderlyingFundAdjustment(DealUnderlyingFundAdjustment dealUnderlyingFundAdjustment) {
			return dealUnderlyingFundAdjustment.Save();
		}

		#endregion

		#region Direct

		public List<DeepBlue.Models.Entity.Issuer> GetAllIssuers() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.Issuers.OrderBy(issuer => issuer.Name).ToList();
			}
		}

		public CreateIssuerModel FindIssuerModel(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				CreateIssuerModel createIssuerModel = new CreateIssuerModel();
				createIssuerModel.IssuerDetailModel = (from issuer in context.Issuers
													   join country in context.COUNTRies on issuer.CountryID equals country.CountryID into countries
													   from country in countries.DefaultIfEmpty()
													   where issuer.IssuerID == id
													   select new IssuerDetailModel {
														   CountryId = issuer.CountryID,
														   IssuerId = issuer.IssuerID,
														   Name = issuer.Name,
														   ParentName = issuer.ParentName,
														   Country = (country != null ? country.CountryName : string.Empty)
													   }).SingleOrDefault();
				/* createIssuerModel.EquityDetailModel = (from equity in context.Equities
													   where equity.IssuerID == id
													   select new EquityDetailModel {
														   EquityId = equity.EquityID,
														   CurrencyId = equity.CurrencyID,
														   EquityType = (equity.EquityType != null ? equity.EquityType.Equity : string.Empty),
														   EquityTypeId = equity.EquityTypeID,
														   Industry = (equity.Industry != null ? equity.Industry.Industry1 : string.Empty),
														   IndustryId = equity.IndustryID,
														   IssuerId = equity.IssuerID,
														   Public = equity.Public,
														   ShareClassType = (equity.ShareClassType != null ? equity.ShareClassType.ShareClass : string.Empty),
														   ShareClassTypeId = equity.ShareClassTypeID,
														   Symbol = equity.Symbol,
													   }).FirstOrDefault();
				createIssuerModel.FixedIncomeDetailModel = (from fixedIncome in context.FixedIncomes
															where fixedIncome.IssuerID == id
															select new FixedIncomeDetailModel {
																CouponInformation = fixedIncome.CouponInformation,
																CurrencyId = fixedIncome.CurrencyID,
																FaceValue = fixedIncome.FaceValue,
																FirstAccrualDate = fixedIncome.FirstAccrualDate,
																FirstCouponDate = fixedIncome.FirstCouponDate,
																FixedIncomeId = fixedIncome.FixedIncomeID,
																FixedIncomeType = (fixedIncome.FixedIncomeType != null ? fixedIncome.FixedIncomeType.FixedIncomeType1 : string.Empty),
																Symbol = fixedIncome.Symbol,
																Maturity = fixedIncome.Maturity,
																FixedIncomeTypeId = fixedIncome.FixedIncomeTypeID,
																Frequency = fixedIncome.Frequency,
																IndustryId = fixedIncome.IndustryID,
																IssuedDate = fixedIncome.IssuedDate,
																IssuerId = fixedIncome.IssuerID,
																Industry = (fixedIncome.Industry != null ? fixedIncome.Industry.Industry1 : string.Empty)
															}).FirstOrDefault();
				 */
				return createIssuerModel;
			}
		}

		public Models.Entity.Issuer FindIssuer(int issuerId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.Issuers.Where(issuer => issuer.IssuerID == issuerId).SingleOrDefault();
			}
		}


		public List<AutoCompleteList> FindIssuers(string issuerName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> issuerListQuery = (from issuer in context.Issuers
																where issuer.Name.Contains(issuerName)
																orderby issuer.Name
																select new AutoCompleteList {
																	id = issuer.IssuerID,
																	label = issuer.Name,
																	value = issuer.Name
																});
				return new PaginatedList<AutoCompleteList>(issuerListQuery, 1, 20);
			}
		}

		public bool DeleteIssuer(int issuerId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				Models.Entity.Issuer issuer = context.Issuers.SingleOrDefault(field => field.IssuerID == issuerId);
				if (issuer != null) {
					if (issuer.Equities.Count == 0 && issuer.FixedIncomes.Count == 0 && issuer.UnderlyingFunds.Count == 0) {
						context.Issuers.DeleteObject(issuer);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public bool IssuerNameAvailable(string issuerName, int issuerId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from issuer in context.Issuers
						 where issuer.Name == issuerName && issuer.IssuerID != issuerId
						 select issuer.IssuerID).Count()) > 0 ? true : false;
			}
		}

		public IEnumerable<ErrorInfo> SaveIssuer(Models.Entity.Issuer issuer) {
			return issuer.Save();
		}


		#region Equity

		public List<Equity> GetAllEquity(int issuerId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from equity in context.Equities
						where equity.IssuerID == issuerId
						select equity).ToList();
			}
		}

		public List<EquityListModel> GetAllEquity(int issuerId, int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<EquityListModel> query = (from equity in context.Equities
													 where equity.IssuerID == issuerId
													 select new EquityListModel {
														 EquityId = equity.EquityID,
														 EquityType = equity.EquityType.Equity,
														 Industry = equity.Industry.Industry1,
														 Symbol = equity.Symbol
													 });
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<EquityListModel> paginatedList = new PaginatedList<EquityListModel>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public Equity FindEquity(int equityId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.Equities.Where(equity => equity.EquityID == equityId).SingleOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveEquity(Equity equity) {
			return equity.Save();
		}

		public bool DeleteEquity(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				Equity equity = context.Equities.SingleOrDefault(field => field.EquityID == id);
				if (equity != null) {
					context.Equities.DeleteObject(equity);
					context.SaveChanges();
					return true;
				}
				return false;
			}
		}

		public List<AutoCompleteList> FindEquityDirects(string issuerName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from equity in context.Equities
						where equity.Issuer.Name.Contains(issuerName)
						orderby equity.Issuer.Name
						select new AutoCompleteList {
							id = equity.EquityID,
							label = equity.Issuer.Name + ">" + (equity.EquityType != null ? equity.EquityType.Equity : "") + ">" + (equity.ShareClassType != null ? equity.ShareClassType.ShareClass : ""),
							value = equity.Issuer.Name
						}).Take(20).ToList();
			}
		}

		public string FindEquitySymbol(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from equity in context.Equities where equity.EquityID == id select equity.Symbol).SingleOrDefault();
			}
		}

		#endregion

		#region FixedIncome

		public List<FixedIncome> GetAllFixedIncome(int issuerId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fixedIncome in context.FixedIncomes
						where fixedIncome.IssuerID == issuerId
						select fixedIncome).ToList();
			}
		}

		public FixedIncome FindFixedIncome(int fixedIncomeId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fixedIncome in context.FixedIncomes
						where fixedIncome.FixedIncomeID == fixedIncomeId
						select fixedIncome).SingleOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveFixedIncome(FixedIncome fixedIncome) {
			return fixedIncome.Save();
		}

		public bool DeleteFixedIncome(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				FixedIncome fixedIncome = context.FixedIncomes.SingleOrDefault(field => field.FixedIncomeID == id);
				if (fixedIncome != null) {
					context.FixedIncomes.DeleteObject(fixedIncome);
					context.SaveChanges();
					return true;
				}
				return false;
			}
		}

		public List<AutoCompleteList> FindFixedIncomeDirects(string issuerName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fixedIncome in context.FixedIncomes
						where fixedIncome.Issuer.Name.Contains(issuerName)
						orderby fixedIncome.Issuer.Name
						select new AutoCompleteList {
							id = fixedIncome.FixedIncomeID,
							label = fixedIncome.Issuer.Name + ">" + (fixedIncome.FixedIncomeType != null ? fixedIncome.FixedIncomeType.FixedIncomeType1 : ""),
							value = fixedIncome.Issuer.Name
						}).Take(20).ToList();
			}
		}

		public object FindFixedIncomeSecurityConversionModel(int fixedIncomeId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fixedIncome in context.FixedIncomes
						where fixedIncome.FixedIncomeID == fixedIncomeId
						select new {
							Symbol = fixedIncome.Symbol
						}).FirstOrDefault();
			}
		}

		public object FindEquitySecurityConversionModel(int equityId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from equity in context.Equities
						where equity.EquityID == equityId
						select new {
							Symbol = equity.Symbol
						}).FirstOrDefault();
			}
		}

		#endregion
		#endregion

	}
}