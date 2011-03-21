using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Fund;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.Fund {

	public interface IFundRepository {

		#region EntiryType
		List<FundListModel> GetAllFunds(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		Models.Entity.Fund FindFund(int fundId);
		List<Models.Entity.Fund> FindFunds(string fundName);
		bool TaxIdAvailable(string taxId, int fundId);
		bool FundNameAvailable(string fundName, int fundId);
		#endregion

		#region Fund Rate Schedules
		List<FundRateSchedule> GetAllFundRateSchdules(int fundId);
		List<MultiplierType> GetAllMultiplierTypes();
		void DeleteFundRateSchedule(int id);
		void DeleteManagementFeeRateSchedule(int id);
		FundRateSchedule FindRateSchedule(int id);
		ManagementFeeRateSchedule FindManagementFeeRateSchedule(int id);
		ManagementFeeRateScheduleTier FindManagementFeeRateScheduleTier(int id);
		#endregion

		#region Save Fund
		IEnumerable<ErrorInfo> SaveFund(Models.Entity.Fund fund);
		#endregion

		#region Save Fund ManagementFeeRateSchedule
		IEnumerable<ErrorInfo> SaveManagementFeeRateSchedule(ManagementFeeRateSchedule managementFeeRateSchedule);
		#endregion
	}
}
