using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Admin.Enums {
	public enum ControllerType {
		EntityType = 1,
		FOIA = 2,
		ERISA = 3
	}

	public enum Module {
		Investor = 1,
		Fund = 2
	}

	public enum AddressType {
		Home = 1,
		Work = 2
	}

	public enum DefaultCountry {
		USA = 225
	}


	public enum CustomFieldDataType {
		Integer = 1,
		Text = 2,
		Boolean = 3,
		DateTime = 4,
		Currency = 5,
		MultiSelectOpiton = 6,
		SingleSelectOption = 7
	}
	 
	public enum CommunicationGrouping {
		Phone = 1,
		Email = 2,
		IM = 3,
		Social_Networking = 4,
		Other = 5
	}

	public enum CommunicationType {
		HomePhone = 1,
		WorkPhone = 2,
		Mobile = 3,
		Pager = 4,
		Fax = 5,
		Email = 6,
		WebAddress = 7,
		Company = 8,
		MailingAddress = 9,
		WebUsername = 10,
		WebPassword = 11
	}

	public enum ExportType {
		Word = 1,
		Pdf = 2,
		Print = 3,
		Excel = 4
	}

	public enum LogType {
		/// <summary>
		/// Default.
		/// </summary>
		Unspecified = 1,

		/// <summary>
		/// Informational.
		/// </summary>
		Information = 2,

		/// <summary>
		/// Non-critical error/warning.
		/// </summary>
		Warning = 3,

		/// <summary>
		/// Critical error.
		/// </summary>
		Error = 4,

		/// <summary>
		/// Performance Stats
		/// </summary>
		Stats = 5
	}
	
}