using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Investor;

namespace DeepBlue.Controllers.Investor {
	public interface IInvestorRepository {
        List<AddressType> GetAllAddressTypes();
        List<COUNTRY> GetAllCountries();
        List<STATE> GetAllStates();
        List<InvestorEntityType> GetAllInvestorEntityTypes();
		List<InvestorType> GetAllInvestorTypes();
        List<CommunicationType> GetAllCommunicationTypes();
		List<InvestorDetail> FindInvestors(string investorName);
	    DeepBlue.Models.Entity.Investor	FindInvestor(int investorId);
	    List<InvestorFund> FindInvestorFunds(int investorId);
		InvestorFund FindInvestorFund(int investorFundId);
        void Add(DeepBlue.Models.Entity.Investor investor);
        void Delete(DeepBlue.Models.Entity.Investor investor);
        void Save();
    }
}
