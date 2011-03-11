using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Document {
	public enum DocumentStatus {
		Investor = 1,
		Fund = 2
	}

	public enum FileType {
		PDF = 1,
		Word = 2,
		Excel = 3
	}
}