var editInvestor={
	currentForm: null
	,init: function () {
		var id=$("#id").val();
		editInvestor.selectInvestor(id);
	}
	,selectInvestor: function (id) {
		$("#Loading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON("/Investor/FindInvestor/"+id,function (data) {
			$("#Loading").html("");
			var $investorInfo=$("#investor_"+id);
			if(!($investorInfo.get(0))) {
				var $investorInfo=editInvestor.cloneInvestorInfo();
				$("#editinfo").append($investorInfo);
				$investorInfo.css("display","");
				$investorInfo.attr("id","investor_"+id);
				$("#InvestorId",$investorInfo).val(id);
				editInvestor.loadInvestorInfo($investorInfo,data);
				// Read Address Information
				$("#AddressInfoCount",$investorInfo).val(data.AddressInformations.length);
				$.each(
					 data.AddressInformations,
					 function (i,address) {
					 	i=parseInt(i)+1;
					 	editInvestor.loadAddressInfo($investorInfo,address,i);
					 });
				// Read Contact Information
				$("#ContactInfoCount",$investorInfo).val(data.ContactInformations.length);
				$.each(
					 data.ContactInformations,
					 function (i,contact) {
					 	i=parseInt(i)+1;
					 	editInvestor.loadContactInfo($investorInfo,contact,i);
					 });
				// Read Account Information
				$("#AccountInfoCount",$investorInfo).val(data.AccountInformations.length);
				$.each(
					 data.AccountInformations,
					 function (i,account) {
					 	i=parseInt(i)+1;
					 	editInvestor.loadAccountInfo($investorInfo,account,i);
					 });
				editInvestor.applyAccordion($investorInfo);
			}
		});
	}
	,cloneInvestorInfo: function () {
		var $investorInfo=$("#investorInfo").clone();
		$("#addressInfo",$investorInfo).remove();
		$("#contactInfo",$investorInfo).remove();
		$("#accountInfo",$investorInfo).remove();
		editInvestor.assignId($investorInfo);
		return $investorInfo;
	}
	,assignId: function ($target) {
		$("input",$target).each(function () {
			if($(this).attr("name")!="") {
				this.id=$(this).attr("name");
			}
		});
		$("select",$target).each(function () {
			if($(this).attr("name")!="") {
				$(this).attr("id",$(this).attr("name"));
			}
		});
	}
	,cloneAddressInfo: function () {
		var addressInfo=$("#addressInfo").clone();
		editInvestor.assignId(addressInfo);
		return addressInfo;
	}
	,cloneContactInfo: function () {
		var contactInfo=$("#contactInfo").clone();
		editInvestor.assignId(contactInfo);
		return contactInfo;
	}
	,cloneAccountInfo: function () {
		var accountInfo=$("#accountInfo").clone();
		editInvestor.assignId(accountInfo);
		return accountInfo;
	}
	,applyAccordion: function ($investorInfo) {
		var $accordion=$("#accordion",$investorInfo)
		$accordion.accordion({ animated: false,active: -1,collapsible: true,autoHeight: false,navigation: true
		});
	}
	,loadInvestorInfo: function ($investorInfo,investor) {
		$("#TitleInvestorName",$investorInfo).html(investor.InvestorName);
		$("#InvestorName",$investorInfo).html(investor.InvestorName);
		$("#DisplayName",$investorInfo).html(investor.DisplayName);
		$("#SocialSecurityTaxId",$investorInfo).html(investor.SocialSecurityTaxId);
		if(investor.DomesticForeigns)
			$("#DomesticForeigns",$investorInfo).val("true");
		else
			$("#DomesticForeigns",$investorInfo).val("false");
		$("#StateOfResidency",$investorInfo).val(investor.StateOfResidency);
		$("#EntityType",$investorInfo).val(investor.EntityType);
		return $investorInfo;
	}
	,loadAddressInfo: function ($investorInfo,address,index) {
		var $addressInfo=$("#addressInfo_"+address.AddressId,$investorInfo);
		if(!($addressInfo.get(0))) {
			$addressInfo=editInvestor.cloneAddressInfo($investorInfo);
			//Replace the index
			$addressInfo.html($addressInfo.html().replace(/0_/g,(index)+"_"));
			$addressInfo.css("display","");
			$addressInfo.attr("id","addressInfo_"+address.AddressId);
			$("#addressInfoMain",$investorInfo).append($addressInfo);

			$("#addressIndex",$addressInfo).html(index);
			$("#"+index+"_"+"AddressId",$addressInfo).val(address.AddressId);
			$("#"+index+"_"+"Phone",$addressInfo).val(address.Phone);
			$("#"+index+"_"+"Fax",$addressInfo).val(address.Fax);
			$("#"+index+"_"+"Email",$addressInfo).val(address.Email);
			$("#"+index+"_"+"WebAddress",$addressInfo).val(address.WebAddress);
			$("#"+index+"_"+"Address1",$addressInfo).val(address.Address1);
			$("#"+index+"_"+"Address2",$addressInfo).val(address.Address2);
			$("#"+index+"_"+"City",$addressInfo).val(address.City);
			$("#"+index+"_"+"State",$addressInfo).val(address.State);
			$("#"+index+"_"+"PostalCode",$addressInfo).val(address.Zip);
			$("#"+index+"_"+"Country",$addressInfo).val(address.Country);
		}
	}
	,loadContactInfo: function ($investorInfo,contact,index) {
		var $contactInfo=$("#contactInfo_"+contact.ContactId,$investorInfo);
		if(!($contactInfo.get(0))) {
			$contactInfo=editInvestor.cloneContactInfo($investorInfo);
			//Replace the index
			$contactInfo.html($contactInfo.html().replace(/0_/g,(index)+"_"));
			$("#contactInfoMain",$investorInfo).append($contactInfo);
			$contactInfo.css("display","");
			$contactInfo.attr("id","contactInfo_"+contact.ContactId);
			$("#contactIndex",$contactInfo).html(index);

			$("#"+index+"_"+"ContactId",$contactInfo).val(contact.ContactId);
			$("#"+index+"_"+"ContactAddressId",$contactInfo).val(contact.ContactAddressId);
			$("#"+index+"_"+"ContactPerson",$contactInfo).val(contact.ContactPerson);
			$("#"+index+"_"+"Designation",$contactInfo).val(contact.Designation);
			$("#"+index+"_"+"ContactPhoneNumber",$contactInfo).val(contact.ContactPhoneNumber);
			$("#"+index+"_"+"ContactFaxNumber",$contactInfo).val(contact.ContactFaxNumber);
			$("#"+index+"_"+"ContactEmail",$contactInfo).val(contact.ContactEmail);
			$("#"+index+"_"+"ContactWebAddress",$contactInfo).val(contact.ContactWebAddress);
			$("#"+index+"_"+"ContactAddress1",$contactInfo).val(contact.ContactAddress1);
			$("#"+index+"_"+"ContactAddress2",$contactInfo).val(contact.ContactAddress2);
			$("#"+index+"_"+"ContactCity",$contactInfo).val(contact.ContactCity);
			$("#"+index+"_"+"ContactState",$contactInfo).val(contact.ContactState);
			$("#"+index+"_"+"ContactPostalCode",$contactInfo).val(contact.ContactZip);
			$("#"+index+"_"+"ContactCountry",$contactInfo).val(contact.ContactCountry);
			$("#"+index+"_"+"DistributionNotices",$contactInfo).get(0).checked=contact.DistributionNotices;
			$("#"+index+"_"+"Financials",$contactInfo).get(0).checked=contact.Financials;
			$("#"+index+"_"+"K1",$contactInfo).get(0).checked=contact.K1;
			$("#"+index+"_"+"InvestorLetters",$contactInfo).get(0).checked=contact.InvestorLetters;
		}
	}
	,loadAccountInfo: function ($investorInfo,account,index) {
		var $accountInfo=$("#accountInfo_"+account.BankId,$investorInfo);
		if(!($accountInfo.get(0))) {
			$accountInfo=editInvestor.cloneAccountInfo($investorInfo);
			//Replace the index
			$accountInfo.html($accountInfo.html().replace(/0_/g,(index)+"_"));
			$("#accountInfoMain",$investorInfo).append($accountInfo);
			$accountInfo.css("display","");
			$accountInfo.attr("id","accountInfo_"+account.BankId);
			$("#accountIndex",$accountInfo).html(index);

			$("#"+index+"_"+"AccountId",$accountInfo).val(account.AccountId);
			$("#"+index+"_"+"BankName",$accountInfo).val(account.BankName);
			$("#"+index+"_"+"AccountNumber",$accountInfo).val(account.AccountNumber);
			$("#"+index+"_"+"ABANumber",$accountInfo).val(account.ABANumber);
			$("#"+index+"_"+"AccountOf",$accountInfo).val(account.AccountOf);
			$("#"+index+"_"+"FFC",$accountInfo).val(account.FFC);
			$("#"+index+"_"+"FFCNO",$accountInfo).val(account.FFCNO);
			$("#"+index+"_"+"Swift",$accountInfo).val(account.Swift);
			$("#"+index+"_"+"IBAN",$accountInfo).val(account.IBAN);
			$("#"+index+"_"+"ByOrderOf",$accountInfo).val(account.ByOrderOf);
			$("#"+index+"_"+"Reference",$accountInfo).val(account.Reference);
			$("#"+index+"_"+"Attention",$accountInfo).val(account.Attention);
		}
	}
	,submit: function (frm) {
		editInvestor.currentForm=frm;
		return true;
	}
	,onBegin: function () {
		$("#UpdateLoading",editInvestor.currentForm).html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Updating...");
	}
	,onSuccess: function () {
		$("#UpdateLoading",editInvestor.currentForm).html("Update Successfully");
		setTimeout(function () {
			$("#UpdateLoading",editInvestor.currentForm).html("");
		},2000)
	}
};