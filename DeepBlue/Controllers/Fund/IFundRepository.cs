using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Fund;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.Fund {

	public interface IFundRepository {

		#region Fund
		List<FundListModel> GetAllFunds(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int? fundId);
		List<InvestorListModel> GetAllInvestorFunds(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int fundId);
		Helpers.FundLists GetAllFunds(int pageIndex, int pageSize);
		Models.Entity.Fund FindFund(int fundId);
		Models.Entity.Fund FindFund(string fundName);
		CreateModel FindFundDetail(int fundId);
		FundDetail FindLastFundDetail();
		//List<AutoCompleteList> FindFunds(string fundName,ref int totalCount);
		List<AutoCompleteList> FindFunds(string fundName);
		List<AutoCompleteList> FindFundClosings(string fundClosingName, int? fundId);
		List<AutoCompleteList> FindDealFunds(int underlyingFundId, string fundName);
		bool TaxIdAvailable(string taxId, int fundId);
		bool FundNameAvailable(string fundName, int fundId);
		decimal FindTotalCommittedAmount(int fundId, int investorTypeId);
		IEnumerable<ErrorInfo> SaveFund(Models.Entity.Fund fund);
		string FindFundName(int fundId);
		#endregion

		#region Fund Rate Schedules
		List<FundRateSchedule> GetAllFundRateSchdules(int fundId);
		List<MultiplierType> GetAllMultiplierTypes();
		void DeleteFundRateSchedule(int id);
		void DeleteManagementFeeRateSchedule(int id);
		FundRateSchedule FindRateSchedule(int id);
		ManagementFeeRateSchedule FindManagementFeeRateSchedule(int id);
		ManagementFeeRateScheduleTier FindManagementFeeRateScheduleTier(int id);
		IEnumerable<ErrorInfo> SaveManagementFeeRateSchedule(ManagementFeeRateSchedule managementFeeRateSchedule);
		#endregion
	}
}
