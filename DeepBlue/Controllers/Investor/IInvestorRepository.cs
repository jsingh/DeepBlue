using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Investor;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.Investor {
    public interface IInvestorRepository {
		
		#region Find
		List<AutoCompleteList> FindInvestors(string investorName, int? fundId);
		List<AutoCompleteList> FindOtherInvestors(string investorName, int excludeInvestorId);
        DeepBlue.Models.Entity.Investor FindInvestor(int investorId);
		InvestorDetail GetInvestorDetail(int investorId);
		InvestorType FindInvestorType(int investorTypeId);
        List<InvestorFund> FindInvestorFunds(int investorId);
		InvestorFund FindInvestorFund(int investorId,int fundId);
        InvestorFund FindInvestorFund(int investorFundId);
		InvestorFundTransaction FindInvestorFundTransaction(int transactionId);
		decimal FindSumOfSellAmount(int investorFundId);
		bool InvestorNameAvailable(string invesorName, int investorId);
		bool SocialSecurityTaxIdAvailable(string socialSecurityId, int investorId);
		#endregion
		 
		#region Delete
		bool Delete(int investorId);
		bool DeleteInvestorContact(int investorContactId);
		bool DeleteInvestorAccount(int investorAccountId);
		#endregion

		#region Save
        IEnumerable<ErrorInfo> SaveInvestor(DeepBlue.Models.Entity.Investor investor);
		IEnumerable<ErrorInfo> SaveInvestorFund(DeepBlue.Models.Entity.InvestorFund investorFund);
		IEnumerable<ErrorInfo> SaveInvestorFundTransaction(InvestorFundTransaction investorFundTransaction);
		#endregion

		#region Investor Address
		InvestorAddress FindInvestorAddress(int investorAddressId);
		object FindInvestorAddressModel(int investorAddressId);
		IEnumerable<ErrorInfo> SaveInvestorAddress(InvestorAddress investorAddress);
		#endregion

		#region Investor Contact
		InvestorContact FindInvestorContact(int investorContactId);
		object FindInvestorContactModel(int investorContactId);
		IEnumerable<ErrorInfo> SaveInvestorContact(InvestorContact investorContact);
		#endregion
		
		#region Investor Communication
		List<InvestorCommunication> FindInvestorCommunications(int investorId);
		IEnumerable<ErrorInfo> SaveInvestorCommunication(InvestorCommunication investorCommunication);
		#endregion

		#region Investor Bank Account
		InvestorAccount FindInvestorAccount(int investorAccountId);
		object FindInvestorAccountModel(int investorAccountId);
		IEnumerable<ErrorInfo> SaveInvestorAccount(InvestorAccount investorAccount);
		#endregion

		#region Investor Information
		EditModel FindInvestorDetail(int investorId);
		InvestorInformation FindInvestorInformation(int investorId);
		#endregion
	}
}
