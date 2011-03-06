using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Transaction;

namespace DeepBlue.Controllers.Transaction {
	public interface ITransactionRepository {
		List<DeepBlue.Models.Entity.Fund> GetAllFundNames();
		List<FundClosing> GetAllFundClosings(int fundId);
    }
}

