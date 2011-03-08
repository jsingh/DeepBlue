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
		IEnumerable<Helpers.ErrorInfo> SaveFund(Models.Entity.Fund fund);
		#endregion
		
	}
}
