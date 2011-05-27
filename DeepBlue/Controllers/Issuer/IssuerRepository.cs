﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Models.Issuer;

namespace DeepBlue.Controllers.Issuer {
	public class IssuerRepository : IIssuerRepository {

		#region Issuer
		public List<IssuerDetailModel> GetAllIssuers() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from issuer in context.Issuers
						select new IssuerDetailModel { IssuerId = issuer.IssuerID, Name = issuer.Name }).ToList();
			}
		}

		public List<IssuerListModel> GetAllIssuers(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<IssuerListModel> query = (from issuer in context.Issuers
													 join country in context.COUNTRies on issuer.CountryID equals country.CountryID into countries
													 from country in countries.DefaultIfEmpty()
													 select new IssuerListModel {
														 IssuerId = issuer.IssuerID,
														 Name = issuer.Name,
														 ParentName = issuer.ParentName,
														 Country = country.CountryName
													 });
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<IssuerListModel> paginatedList = new PaginatedList<IssuerListModel>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public Models.Entity.Issuer FindIssuer(int issuerId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.Issuers.Where(issuer => issuer.IssuerID == issuerId).SingleOrDefault();
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

		public EditIssuerModel FindIssuerModel(int issuerId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				EditIssuerModel model = null;
				model = (from issuer in context.Issuers
						 where issuer.IssuerID == issuerId
						 select new EditIssuerModel {
							 CountryId = issuer.CountryID,
							 ParentName = issuer.ParentName,
							 Name = issuer.Name,
							 IssuerId = issuer.IssuerID
						 }).SingleOrDefault();
				if (model != null) {
					IQueryable<EquityDetailModel> queryEquities = GetEquityModel(context, model.IssuerId, 0);
					model.Equities = (queryEquities != null ? queryEquities.ToList() : null);
					IQueryable<FixedIncomeDetailModel> queryFixedIncomes = GetFixedIncomeModel(context, model.IssuerId, 0);
					model.FixedIncomes = (queryFixedIncomes != null ? queryFixedIncomes.ToList() : null);
				}
				return model;
			}
		}
		#endregion

		#region Equity

		private IQueryable<EquityDetailModel> GetEquityModel(DeepBlueEntities context, int issuerId, int equityId) {
			IQueryable<Equity> equities = null;
			if (issuerId > 0) {
				equities = context.Equities.Where(deepBlueEquity => deepBlueEquity.IssuerID == issuerId);
			}
			if (equityId > 0) {
				equities = context.Equities.Where(deepBlueEquity => deepBlueEquity.EquityID == equityId);
			}
			if (equities != null) {
				return (from equity in equities
						select new EquityDetailModel {
							EquityId = equity.EquityID,
							Symbol = equity.Symbol,
							IssuerId = equity.IssuerID,
							CurrencyId = equity.CurrencyID,
							EquityTypeId = equity.EquityTypeID,
							IndustryId = equity.IndustryID,
							ShareClassTypeId = equity.ShareClassTypeID,
							Public = equity.Public,
							Currency = equity.Currency.Currency1,
							EquityType = equity.EquityType.Equity,
							Industry = equity.Industry.Industry1,
							ShareClassType = equity.ShareClassType.ShareClass
						});
			}
			else {
				return null;
			}

		}

		public EquityDetailModel FindEquityModel(int equityId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return GetEquityModel(context, 0, equityId).SingleOrDefault();
			}
		}

		public List<Equity> GetAllEquity(int issuerId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from equity in context.Equities
						where equity.IssuerID == issuerId
						select equity).ToList();
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
		#endregion

		#region FixedIncome

		private IQueryable<FixedIncomeDetailModel> GetFixedIncomeModel(DeepBlueEntities context, int issuerId, int fixedIncomeId) {
			IQueryable<FixedIncome> fixedIncomes = null;
			if (issuerId > 0) {
				fixedIncomes = context.FixedIncomes.Where(deepBlueFixedIncome => deepBlueFixedIncome.IssuerID == issuerId);
			}
			if (fixedIncomeId > 0) {
				fixedIncomes = context.FixedIncomes.Where(deepBlueFixedIncome => deepBlueFixedIncome.FixedIncomeID == fixedIncomeId);
			}
			if (fixedIncomes != null) {
				return (from fixedIncome in fixedIncomes
						select new FixedIncomeDetailModel {
							CouponInformation = fixedIncome.CouponInformation,
							CurrencyId = fixedIncome.CurrencyID,
							FaceValue = fixedIncome.FaceValue,
							FirstAccrualDate = fixedIncome.FirstAccrualDate,
							FirstCouponDate = fixedIncome.FirstCouponDate,
							FixedIncomeTypeId = fixedIncome.FixedIncomeTypeID,
							Frequency = fixedIncome.Frequency,
							IndustryId = fixedIncome.IndustryID,
							IssuedDate = fixedIncome.IssuedDate,
							Maturity = fixedIncome.Maturity,
							Symbol = fixedIncome.Symbol,
							IssuerId = fixedIncome.IssuerID,
							FixedIncomeId = fixedIncome.FixedIncomeID,
							FixedIncomeType = fixedIncome.FixedIncomeType.FixedIncomeType1
						});
			}
			else {
				return null;
			}

		}

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

		public FixedIncomeDetailModel FindFixedIncomeModel(int fixedIncomeId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<FixedIncomeDetailModel> queryFixedIncomes = GetFixedIncomeModel(context, 0, fixedIncomeId);
				return queryFixedIncomes.SingleOrDefault();
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
		#endregion



	
	}
}