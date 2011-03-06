using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Transaction {
	public class ErrorModel {
	  public IEnumerable<ErrorInfo> ErrorInfo { get; set; }
	}
}