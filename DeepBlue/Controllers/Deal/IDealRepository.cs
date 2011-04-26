using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.Deal {
	public interface IDealRepository {
		Models.Entity.Deal FindDeal(int dealId);
		IEnumerable<ErrorInfo> SaveDeal(Models.Entity.Deal deal);
		List<AutoCompleteList> FindDeals(string dealName);
		bool DealNameAvailable(string fundName, int fundId);
		int GetMaxDealNumber(int fundId);

		#region DealExpense
		DealClosingCost FindDealClosingCost(int dealClosingCostId);
		void DeleteDealClosingCost(int dealClosingCostId);
		IEnumerable<ErrorInfo> SaveDealClosingCost(DealClosingCost dealClosingCost);
		#endregion
	}
}
