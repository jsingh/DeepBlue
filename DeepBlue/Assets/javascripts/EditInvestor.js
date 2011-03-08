var editInvestor={
	currentForm: null
	,currentType: ''
	,init: function () {
		var id=$("#id").val();
		if(parseInt(id)>0)
			editInvestor.selectInvestor(id);
	}
	,selectInvestor: function (id) {
		$("#Loading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		var dt=new Date();

		$.getJSON("/Investor/FindInvestor/"+id+"?t="+dt.getTime(),function (data) {
			$("#Loading").html("");
			var $investorInfo=$("#investor_"+id);
			var alreadyExist=false;
			if($investorInfo.get(0)) {
				alreadyExist=true;
			} else {
				$investorInfo=editInvestor.cloneInvestorInfo();
				$("#editinfo").append($investorInfo);
				$investorInfo.css("display","");
				$investorInfo.attr("id","investor_"+id);
				$("#InvestorId",$investorInfo).val(id);
			}
			$("#addressInfo_0",$investorInfo).remove();
			$("#contactInfo_0",$investorInfo).remove();
			$("#accountInfo_0",$investorInfo).remove();
			$("#addressInfo",$investorInfo).remove();
			$("#contactInfo",$investorInfo).remove();
			$("#accountInfo",$investorInfo).remove();

			editInvestor.loadInvestorInfo($investorInfo,data);
			editInvestor.buildFundTable($investorInfo,data.FundInformations);
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
			if(alreadyExist==false) {
				editInvestor.applyAccordion($investorInfo);
			}
			editInvestor.loadDisplCtl($investorInfo);
			$(".InvestorUpdateLoading").remove();
		});
	}
	,buildFundTable: function ($investorInfo,data) {
		var fundDetails=$("#funddetails",$investorInfo).html("");
		var html="<table cellpadding='0' cellspacing='0' border='0' style='width:100%'><thead><tr><th>Fund Name</th><th style='width:20%'>Committed Amount</th><th style='width:20%'>Unfunded Amount</th><th>Investror Type</th></tr></thead></table>";
		$(fundDetails).html(html);
		var table=$("table",fundDetails).flexigrid({
			url: '/Json/FlexigridList',
			dataType: 'json',
			title: 'FundDetails',
			resizable: false,
			autoload: false,
			useRp: true,
			rp: 15,
			showTableToggleBtn: false
		});
		$(table).flexAddData(data);
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
		$(":input",$target).each(function () {
			if($(this).attr("name")!="") {
				this.id=$(this).attr("name");
				this.style.display="none";
			}
		});
		$("select",$target).each(function () {
			if($(this).attr("name")!="") {
				$(this).attr("id",$(this).attr("name"));
				this.style.display="none";
			}
		});
	}
	,changeCountry: function (CountryId,StateId,StateRow) {
		var country=document.getElementById(CountryId);
		var state=document.getElementById(StateId);
		var staterow=document.getElementById(StateRow);
		state.value=52;
		if(country.value!="225") {
			staterow.style.display="none";
		} else {
			staterow.style.display="";
			state.value=0;
		}
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
		var $accordion=$("#accordion",$investorInfo);
		$accordion.accordion({ animated: false,active: -1,collapsible: true,autoHeight: false,navigation: true
		});
	}
	,addContactInfo: function (that) {
		var editinfo=$(that).parents(".edit-info:first").get(0); // get edit-info box
		var contactInfo=$("#contactInfo").clone();
		var contactInfoMain=$(that).parents("#contactInfoMain").get(0);
		$("#ContactInfoAddNew",contactInfoMain).hide();
		var contactInfoCount=parseInt($("#ContactInfoCount",editinfo).val());
		contactInfo.html(contactInfo.html().replace(/0_/g,(contactInfoCount+1)+"_"));
		$("#ContactInfoCount",editinfo).val(contactInfoCount+1);
		editInvestor.assignId(contactInfo);
		editInvestor.showControls(contactInfo,true);
		$(":input",contactInfo).each(function () {
			if($(this).attr("type")=="hidden") {
				this.value="0";
			} else if($(this).attr("type")=="checkbox") {
				this.checked=false;
			} else {
				this.value="";
			}
		});
		$("select",contactInfo).each(function () {
			if(this.id.indexOf("Country")>0) {
				this.value="225";
			} else {
				this.value="0";
			}
		});
		$(contactInfoMain).append(contactInfo);
		$(".UpdateInvestorInfo",contactInfo).show();
		$(".EditInvestorInfo",contactInfo).hide();
	}
	,deleteContact: function (that,contactId) {
		if(confirm("Are you sure you want to delete this investor contact?")) {
			var editinfo=$(that).parents(".editinfo:first").get(0);
			var contactInfoMain=$(editinfo).parent();
			var contact=document.getElementById(contactId);
			if(parseInt(contact.value)>0) {
				var dt=new Date();
				var url="/Investor/DeleteContactAddress/"+contact.value+"?t="+dt.getTime();
				$.get(url,function (data) {
					if(data!="True") {
						alert(data);
					} else {
						$(editinfo).remove();
						editInvestor.showContactAddNew(contactInfoMain);
					}
				});
			} else {
				$(editinfo).remove();
				editInvestor.showContactAddNew(contactInfoMain);
			}
		}
	}
	,deleteAccount: function (that,accountId) {
		if(confirm("Are you sure you want to delete this investor account?")) {
			var editinfo=$(that).parents(".editinfo:first").get(0);
			var accountInfoMain=$(editinfo).parent();
			var account=document.getElementById(accountId);
			if(parseInt(account.value)>0) {
				var dt=new Date();
				var url="/Investor/DeleteInvestorAccount/"+account.value+"?t="+dt.getTime();
				$.get(url,function (data) {
					if(data!="True") {
						alert(data);
					} else {
						$(editinfo).remove();
						editInvestor.showAccountAddNew(accountInfoMain);
					}
				});
			} else {
				$(editinfo).remove();
				editInvestor.showAccountAddNew(accountInfoMain);
			}
		}
	}
	,showAccountAddNew: function (accountInfoMain) {
		var editinfo=$(".editinfo",accountInfoMain).length;
		if(editinfo==0) {
			$("#AccountInfoAddNew",accountInfoMain).show();
		} else {
			$("#AccountInfoAddNew",accountInfoMain).hide();
		}
	}
	,addAccountInfo: function (that) {
		var editinfo=$(that).parents(".edit-info:first").get(0); // get edit-info box
		var accountInfo=$("#accountInfo").clone();
		var accountInfoMain=$(that).parents("#accountInfoMain").get(0);
		$("#AccountInfoAddNew",accountInfoMain).hide();
		var accountInfoCount=parseInt($("#AccountInfoCount",editinfo).val());
		accountInfo.html(accountInfo.html().replace(/0_/g,(accountInfoCount+1)+"_"));
		$("#AccountInfoCount",editinfo).val(accountInfoCount+1);
		editInvestor.assignId(accountInfo);
		editInvestor.showControls(accountInfo,true);
		$(":input",accountInfo).each(function () {
			if($(this).attr("type")=="hidden") {
				this.value="0";
			} else if($(this).attr("type")=="checkbox") {
				this.checked=false;
			} else {
				this.value="";
			}
		});
		$("select",accountInfo).each(function () {
			if(this.id.indexOf("Country")>0) {
				this.value="225";
			} else {
				this.value="0";
			}
		});
		$(accountInfoMain).append(accountInfo);
		$(".UpdateInvestorInfo",accountInfo).show();
		$(".EditInvestorInfo",accountInfo).hide();
	}
	,showContactAddNew: function (contactInfoMain) {
		var editinfo=$(".editinfo",contactInfoMain).length;
		if(editinfo==0) {
			$("#ContactInfoAddNew",contactInfoMain).show();
		} else {
			$("#ContactInfoAddNew",contactInfoMain).hide();
		}
	}
	,editInvestorInfo: function (that) {
		var cntdiv=$(that).parents(".editinfo:first").get(0);
		$(".UpdateInvestorInfo",cntdiv).show();
		$(".EditInvestorInfo",cntdiv).hide();
		editInvestor.showControls(cntdiv,true);
	}
	,updateInvestorInfo: function (that,type) {
		editInvestor.currentType=type;
		var cntdiv=$(that).parents(".ui-accordion-content").get(0);
		var editbtn=$(that).parents(".editor-editbtn").get(0);
		var EditInvestorInfo=$(".EditInvestorInfo",cntdiv);
		var UpdateInvestorInfo=$(".UpdateInvestorInfo",cntdiv);
		UpdateInvestorInfo.hide();
		EditInvestorInfo.show();
		$(".InvestorUpdateLoading",editbtn).remove();
		var loading=document.createElement("div");
		loading.innerHTML="<img src='/Assets/images/ajax.jpg'/>&nbsp;Updating...";
		loading.className="InvestorUpdateLoading";
		$(loading).css("float","left");
		$(EditInvestorInfo).before(loading);
		var editinfo=$(that).parents(".edit-info").get(0);
		$("#Update",editinfo).click();
		editInvestor.showControls(cntdiv,false);
	}
	,cancelInvestorInfo: function (that) {
		var cntdiv=$(that).parents(".editinfo:first").get(0);
		$(".UpdateInvestorInfo",cntdiv).hide();
		$(".EditInvestorInfo",cntdiv).show();
		editInvestor.showControls(cntdiv,false);
	}
	,showControls: function (cntdiv,isShow) {
		$(":input",cntdiv).each(function () {
			var disp=$("'#Disp_"+this.id+"'",cntdiv);
			if(isShow) {
				disp.css("display","none");
				$(this).css("display","");
			} else {
				disp.css("display","");
				$(this).css("display","none");
			}
		});
		$("select",cntdiv).each(function () {
			var disp=$("'#Disp_"+this.id+"'",cntdiv);
			if(isShow) {
				disp.css("display","none");
				$(this).css("display","");
			} else {
				disp.css("display","");
				$(this).css("display","none");
			}
		});
	}
	,loadInvestorInfo: function ($investorInfo,investor) {
		$("#TitleInvestorName",$investorInfo).html(investor.InvestorName);
		$("#InvestorName",$investorInfo).html(investor.InvestorName);
		$("#DisplayName",$investorInfo).val(investor.DisplayName);
		$("#Notes",$investorInfo).val(investor.Notes);
		$("#Spn_DisplayName",$investorInfo).html(investor.DisplayName);
		$("#Spn_Notes",$investorInfo).html(investor.Notes);
		$("#SocialSecurityTaxId",$investorInfo).html(investor.SocialSecurityTaxId);
		var DomesticForeigns=$("#DomesticForeigns",$investorInfo);
		var StateOfResidency=$("#StateOfResidency",$investorInfo);
		var EntityType=$("#EntityType",$investorInfo);
		if(investor.DomesticForeign)
			DomesticForeigns.val("true");
		else
			DomesticForeigns.val("false");

		StateOfResidency.val(investor.StateOfResidency);
		EntityType.val(investor.EntityType);
		return $investorInfo;
	}
	,loadDisplCtl: function ($target) {
		$(":input",$target).each(function () {
			if($(this).attr("name")!="") {
				this.style.display="none";
				var disp=$("'#Disp_"+this.id+"'",$target);
				var type=$(this).attr("type");
				if(type!="hidden"&&type!="checkbox") {
					disp.html(this.value);
					$(this).change(function () {
						disp.html(this.value);
					});
				} else if(type=="checkbox") {
					if(this.checked) {
						disp.html("<img src='/Assets/images/Tick.gif' />");
					} else {
						disp.html("");
					}
					$(this).change(function () {
						if(this.checked) {
							disp.html("<img src='/Assets/images/Tick.gif' />");
						} else {
							disp.html("");
						}
					});
				}
			}
		});
		$("select",$target).each(function () {
			if($(this).attr("name")!="") {
				this.style.display="none";
				var disp=$("'#Disp_"+this.id+"'",$target);
				if(disp)
					disp.html(this.options[this.selectedIndex].text);
				$(this).change(function () {
					disp.html(this.options[this.selectedIndex].text);
				});
			}
		});
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
			var country=$("#"+index+"_"+"Country",$addressInfo);
			country.val(address.Country);
			if(country.val()!="225") {
				$("#"+index+"_"+"AddressStateRow",$addressInfo).hide();
			}
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
			$("#"+index+"_"+"DistributionNotices",$contactInfo).get(0).checked=contact.DistributionNotices;
			$("#"+index+"_"+"Financials",$contactInfo).get(0).checked=contact.Financials;
			$("#"+index+"_"+"K1",$contactInfo).get(0).checked=contact.K1;
			$("#"+index+"_"+"InvestorLetters",$contactInfo).get(0).checked=contact.InvestorLetters;
			var contactCountry=$("#"+index+"_"+"ContactCountry",$contactInfo);
			contactCountry.val(contact.ContactCountry);
			if(contactCountry.val()!="225") {
				$("#"+index+"_"+"ContactSateRow",$contactInfo).hide();
			}
		}
	}
	,loadAccountInfo: function ($investorInfo,account,index) {
		var $accountInfo=$("#accountInfo_"+account.AccountId,$investorInfo);
		if(!($accountInfo.get(0))) {
			$accountInfo=editInvestor.cloneAccountInfo($investorInfo);
			//Replace the index
			$accountInfo.html($accountInfo.html().replace(/0_/g,(index)+"_"));
			$("#accountInfoMain",$investorInfo).append($accountInfo);
			$accountInfo.css("display","");
			$accountInfo.attr("id","accountInfo_"+account.AccountId);
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
	,deleteInvestor: function (that) {
		if(confirm("Are you sure you want to delete this investor?")) {
			var editinfo=$(that).parents(".edit-info:first").get(0); // get edit-info box
			var InvestorId=$("#InvestorId",editinfo).val();
			var dt=new Date();
			var url="/Investor/Delete/"+InvestorId+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="True") {
					alert(data);
				} else {
					$(editinfo).remove();
				}
			});
		}
	}
	,onBegin: function () {
		$("#UpdateLoading",editInvestor.currentForm).html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Updating...");
	}
	,onSuccess: function () {
		$("#UpdateLoading",editInvestor.currentForm).html("Update Successfully");
		var InvestorId=$("#InvestorId",editInvestor.currentForm).val();
		$(".InvestorUpdateLoading").remove();
		setTimeout(function () {
			$("#UpdateLoading",editInvestor.currentForm).html("");
		},2000)
		editInvestor.selectInvestor(InvestorId);
	}
};
