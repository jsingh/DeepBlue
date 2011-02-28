﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Investor;


namespace DeepBlue.Controllers.Investor {
	public class InvestorRepository : IInvestorRepository {

		DeepBlueEntities DeepBlueDb = new DeepBlueEntities();

		#region IInvestorRepository Investors

		public List<AddressType> GetAllAddressTypes() {
			return (from addressType in DeepBlueDb.AddressTypes
					orderby addressType.AddressTypeName
					select addressType).ToList();
		}


		public List<COUNTRY> GetAllCountries() {
			return (from country in DeepBlueDb.COUNTRies
					orderby country.CountryName ascending
					select country).ToList();
		}

		public List<STATE> GetAllStates() {
			return (from state in DeepBlueDb.STATEs
					orderby state.Name ascending
					select state).ToList();
		}

		public List<CommunicationType> GetAllCommunicationTypes() {
			return (from communicationType in DeepBlueDb.CommunicationTypes
					orderby communicationType.CommunicationTypeName
					select communicationType).ToList();
		}

		public List<InvestorEntityType> GetAllInvestorEntityTypes() {
			return (from investorEntityType in DeepBlueDb.InvestorEntityTypes
					orderby investorEntityType.InvestorEntityTypeName
					select investorEntityType).ToList();
		}

		public List<InvestorType> GetAllInvestorTypes() {
			return (from investorType in DeepBlueDb.InvestorTypes
					orderby investorType.InvestorTypeName
					select investorType).ToList();
		}
		
		public List<InvestorDetail> FindOtherInvestors(string investorName, int excludeInvestorId) {
			return (from investor in DeepBlueDb.Investors
					where investor.InvestorName.Contains(investorName) && investor.InvestorID != excludeInvestorId
					select new InvestorDetail {
						InvestorName = investor.InvestorName,
						InvestorId = investor.InvestorID,
						Social = investor.Social
					}).ToList();
		}

		public DeepBlue.Models.Entity.Investor FindInvestor(int investorId) {
			return DeepBlueDb.Investors.SingleOrDefault(investor => investor.InvestorID == investorId);
		}

		public List<InvestorFund> FindInvestorFunds(int investorId) {
			return (from investorFund in DeepBlueDb.InvestorFunds
					where investorFund.InvestorID == investorId
					select investorFund).ToList();
		}

		public InvestorFund FindInvestorFund(int investorFundId) {
			return DeepBlueDb.InvestorFunds.SingleOrDefault(investorFund => investorFund.InvestorFundID == investorFundId);
		}

		public InvestorFundTransaction FindInvestorFundTransaction(int transactionId) {
			return DeepBlueDb.InvestorFundTransactions.SingleOrDefault(investorFundTransaction => investorFundTransaction.InvestorFundTransactionID == transactionId);
		}

		public void Add(DeepBlue.Models.Entity.Investor investor) {
			DeepBlueDb.Investors.AddObject(investor);
		}

		public void Delete(DeepBlue.Models.Entity.Investor investor) {
			DeepBlueDb.Investors.DeleteObject(investor);
		}

		public void Save() {
			DeepBlueDb.SaveChanges();
		}

		public IEnumerable<Helpers.ErrorInfo> SaveInvestor(Models.Entity.Investor investor) {
			return investor.Save();
		}
		
		public InvestorType FindInvestorType(int investorTypeId) {
			return DeepBlueDb.InvestorTypes.SingleOrDefault(investorType => investorType.InvestorTypeID == investorTypeId);
		}

		public List<InvestorDetail> FindInvestors(string investorName) {
			return (from investor in DeepBlueDb.Investors
					where investor.InvestorName.Contains(investorName)
					select new InvestorDetail {
						InvestorName = investor.InvestorName,
						DisplayName = investor.Alias,
						InvestorId = investor.InvestorID,
						Social = investor.Social
					}).ToList();
		}

		public InvestorDetail FindInvestorDetail(int investorId) {
			return (from investor in DeepBlueDb.Investors
					where investor.InvestorID  == investorId
					select new InvestorDetail {
						InvestorName = investor.InvestorName,
						DisplayName = investor.Alias,
						InvestorId = investor.InvestorID,
						Social = investor.Social
					}).SingleOrDefault();
		}


		#endregion
		 
	}
}