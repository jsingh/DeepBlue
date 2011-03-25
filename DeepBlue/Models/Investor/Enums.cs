using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Investor.Enums {
    public enum AddressType {
        Home = 1,
        Work = 2
    }

    public enum CommunicationGrouping {
        Phone = 1,
        Email = 2,
        IM = 3,
        Social_Networking = 4,
        Other = 5
    }

    public enum CommunicationType {
        Home_Phone = 1,
        Work_Phone = 2,
        Mobile = 3,
        Pager = 4,
        Fax = 5,
        Email = 6
    }

	public enum Source {
		FEG = 1,
		Cambridge = 2,
		NEPC = 3
	}

	public enum DefaultCountry {
	   USA = 225
	}

	public enum InvestorType {
		ManagingMember = 1,
		NonManagingMember = 2
	}
}