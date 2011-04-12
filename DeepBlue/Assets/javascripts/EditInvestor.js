var editInvestor={
	currentForm: null
	,currentType: ''
	,init: function () {
		var id=$("#id").val();
		if(parseInt(id)>0) {
			editInvestor.selectInvestor(id);
		}
	}
	,selectInvestor: function (id) {
		$("#Loading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		var dt=new Date();
		$.getJSON("/Investor/FindInvestor/"+id+"?t="+dt.getTime(),function (data) {
			$("#Loading").html("");
			var $invInfo=$("#investor_"+id);
			var alreadyExist=false;
			if($invInfo.get(0)) {
				alreadyExist=true;
			} else {
				$invInfo=editInvestor.cloneInvestorInfo();
				$("#editinfo").append($invInfo);
				$invInfo.css("display","");
				$invInfo.attr("id","investor_"+id);
				$("#InvestorId",$invInfo).val(id);
			}
			$("#addressInfo_0",$invInfo).remove();
			$("#contactInfo_0",$invInfo).remove();
			$("#accountInfo_0",$invInfo).remove();
			$("#addressInfo",$invInfo).remove();
			$("#contactInfo",$invInfo).remove();
			$("#accountInfo",$invInfo).remove();

			editInvestor.loadInvestorInfo($invInfo,data);
			editInvestor.buildFundTable($invInfo,data.FundInformations);
			// Read Address Information
			$("#AddressInfoCount",$invInfo).val(data.AddressInformations.length);
			$.each(
					 data.AddressInformations,
					 function (i,address) {
					 	i=parseInt(i)+1;
					 	editInvestor.loadAddressInfo($invInfo,address,i);
					 });
			// Read Contact Information
			$("#ContactInfoCount",$invInfo).val(data.ContactInformations.length);
			$.each(
					 data.ContactInformations,
					 function (i,contact) {
					 	i=parseInt(i)+1;
					 	editInvestor.loadContactInfo($invInfo,contact,i);
					 });
			// Read Account Information
			$("#AccountInfoCount",$invInfo).val(data.AccountInformations.length);
			$.each(
					 data.AccountInformations,
					 function (i,account) {
					 	i=parseInt(i)+1;
					 	editInvestor.loadAccountInfo($invInfo,account,i);
					 });
			// Read Custom Fields
			$.each(
					 data.CustomField.Values,
					 function (i,value) {
					 	var obj=$("'#CustomField_"+value.CustomFieldId+"'",$invInfo).get(0);
					 	if(obj) {
					 		switch(value.DataTypeId) {
					 			case 1: // Integer 
					 				if(value.IntegerValue>0)
					 					obj.value=value.IntegerValue;
					 				break;
					 			case 2: // Text
					 				obj.value=value.TextValue;
					 				break;
					 			case 3: // Boolean
					 				obj.checked=value.BooleanValue;
					 				break;
					 			case 4: // DateTime
					 				obj.value=value.DateValue;
					 				obj.id="CustomField_"+value.Key+"_"+value.CustomFieldId;
					 				$(obj).datepicker({ changeMonth: true,changeYear: true });
					 				break;
					 			case 5: // Currency
					 				if(value.CurrencyValue>0)
					 					obj.value=value.CurrencyValue;
					 				break;
					 		}
					 	}
					 });
			if(alreadyExist==false) {
				editInvestor.applyAccordion($invInfo);
			}
			editInvestor.loadDisplCtl($invInfo);
			$(".editinfo",$invInfo).each(function () {
				editInvestor.showControls(this,false);
			});
			$(".InvestorUpdateLoading").remove();
			$(".UpdateInvestorInfo").hide();
			$(".EditInvestorInfo").show();
		});
	}
	,createDispField: function (obj) {
		var parent=$(obj).parent("div");
		$("span",parent).remove();
		var span=document.createElement("span");
		span.id="Disp_"+obj.id;
		//span.innerHTML=obj.value;
		$(obj).before(span);
	}
	,buildFundTable: function ($invInfo,data) {
		var fundDetails=$("#funddetails",$invInfo).html("");
		var html="<table cellpadding='0' cellspacing='0' border='0' style='width:100%'><thead><tr>";
		html+="<th style='width:30%'>Fund Name</th>";
		html+="<th style='width:20%' align='right'>Committed Amount</th>";
		html+="<th style='width:20%' align='right'>Unfunded Amount</th>";
		html+="<th style='width:30%'>Investror Type</th>";
		html+="</tr></thead></table>";
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
		var $invInfo=$("#investorInfo").clone();
		$("#addressInfo",$invInfo).remove();
		$("#contactInfo",$invInfo).remove();
		$("#accountInfo",$invInfo).remove();
		editInvestor.assignId($invInfo);
		return $invInfo;
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
	,applyAccordion: function ($invInfo) {
		var $accordion=$("#accordion",$invInfo);
		$accordion.accordion({ animated: false,active: -1,collapsible: true,autoHeight: false,navigation: true });
	}
	,addContactInfo: function (that) {
		var editinfo=$(that).parents(".edit-info:first").get(0); // get edit-info box
		var contactInfo=$("#contactInfo").clone();
		var contactInfoMain=$(that).parents("#contactInfoMain").get(0);
		//$("#ContactInfoAddNew",contactInfoMain).hide();
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
		editInvestor.currentForm=$(that).parents("form:first");
		editInvestor.currentType=type;
		var cntdiv=$(that).parents(".ui-accordion-content").get(0);
		var editbtn=$(that).parents(".editor-editbtn").get(0);
		var EditInvestorInfo=$(".EditInvestorInfo",cntdiv);
		var UpdateInvestorInfo=$(".UpdateInvestorInfo",cntdiv);
		$(".InvestorUpdateLoading",editbtn).remove();
		var loading=document.createElement("div");
		loading.innerHTML="<img src='/Assets/images/ajax.jpg'/>&nbsp;Updating...";
		loading.className="InvestorUpdateLoading";
		$(loading).css("float","left");
		$(EditInvestorInfo).before(loading);
		var editinfo=$(that).parents(".edit-info").get(0);
		$("#Update",editinfo).click();
		//editInvestor.showControls(cntdiv,false);
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
	,loadInvestorInfo: function ($invInfo,investor) {
		$("#TitleInvestorName",$invInfo).html(investor.InvestorName);
		$("#InvestorName",$invInfo).html(investor.InvestorName);
		$("#DisplayName",$invInfo).val(investor.DisplayName);
		$("#Notes",$invInfo).val(investor.Notes);
		$("#Spn_DisplayName",$invInfo).html(investor.DisplayName);
		$("#Spn_Notes",$invInfo).html(investor.Notes);
		$("#SocialSecurityTaxId",$invInfo).html(investor.SocialSecurityTaxId);
		var DomesticForeigns=$("#DomesticForeigns",$invInfo);
		var StateOfResidency=$("#StateOfResidency",$invInfo);
		var EntityType=$("#EntityType",$invInfo);
		if(investor.DomesticForeign)
			DomesticForeigns.val("true");
		else
			DomesticForeigns.val("false");

		StateOfResidency.val(investor.StateOfResidency);
		EntityType.val(investor.EntityType);
		return $invInfo;
	}
	,createDispCtl: function (input,$target) {
		var disp=$("'#Disp_"+input.id+"'",$target).get(0);
		if(!disp) {
			disp=document.createElement("span");disp.id="Disp_"+input.id;
			$(input).before(disp);
		}
		return $(disp);
	}
	,loadDisplCtl: function ($target) {
		$(":input",$target).each(function () {
			if($(this).attr("name")!="") {
				this.style.display="none";
				var disp=editInvestor.createDispCtl(this,$target);
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
				var disp=editInvestor.createDispCtl(this,$target);
				if(disp) {
					if(this.options[this.selectedIndex].text=="--Select One--") disp.html("");else disp.html(this.options[this.selectedIndex].text);
				}
				$(this).change(function () {
					disp.html(this.options[this.selectedIndex].text);
				});
			}
		});
	}
	,loadAddressInfo: function ($invInfo,address,index) {
		var $addInfo=$("#addressInfo_"+address.AddressId,$invInfo);
		if(!($addInfo.get(0))) {
			$addInfo=editInvestor.cloneAddressInfo($invInfo);
			//Replace the index
			$addInfo.html($addInfo.html().replace(/0_/g,(index)+"_"));
			$addInfo.css("display","");
			$addInfo.attr("id","addressInfo_"+address.AddressId);
			$("#addressInfoMain",$invInfo).append($addInfo);

			$("#addressIndex",$addInfo).html(index);
			$("#"+index+"_"+"AddressId",$addInfo).val(address.AddressId);
			$("#"+index+"_"+"Phone",$addInfo).val(address.Phone);
			$("#"+index+"_"+"Fax",$addInfo).val(address.Fax);
			$("#"+index+"_"+"Email",$addInfo).val(address.Email);
			$("#"+index+"_"+"WebAddress",$addInfo).val(address.WebAddress);
			$("#"+index+"_"+"Address1",$addInfo).val(address.Address1);
			$("#"+index+"_"+"Address2",$addInfo).val(address.Address2);
			$("#"+index+"_"+"City",$addInfo).val(address.City);
			$("#"+index+"_"+"State",$addInfo).val(address.State);
			$("#"+index+"_"+"PostalCode",$addInfo).val(address.Zip);
			var country=$("#"+index+"_"+"Country",$addInfo);
			country.val(address.Country);
			if(country.val()!="225") {
				$("#"+index+"_"+"AddressStateRow",$addInfo).hide();
			}
		}
	}
	,loadContactInfo: function ($invInfo,contact,index) {
		var $contInfo=$("#contactInfo_"+contact.ContactId,$invInfo);
		if(!($contInfo.get(0))) {
			$contInfo=editInvestor.cloneContactInfo($invInfo);
			//Replace the index
			$contInfo.html($contInfo.html().replace(/0_/g,(index)+"_"));
			$("#contactInfoMain",$invInfo).append($contInfo);
			$contInfo.css("display","");
			$contInfo.attr("id","contactInfo_"+contact.ContactId);
			$("#contactIndex",$contInfo).html(index);

			$("#"+index+"_"+"ContactId",$contInfo).val(contact.ContactId);
			$("#"+index+"_"+"ContactAddressId",$contInfo).val(contact.ContactAddressId);
			$("#"+index+"_"+"ContactPerson",$contInfo).val(contact.ContactPerson);
			$("#"+index+"_"+"Designation",$contInfo).val(contact.Designation);
			$("#"+index+"_"+"ContactPhoneNumber",$contInfo).val(contact.ContactPhoneNumber);
			$("#"+index+"_"+"ContactFaxNumber",$contInfo).val(contact.ContactFaxNumber);
			$("#"+index+"_"+"ContactEmail",$contInfo).val(contact.ContactEmail);
			$("#"+index+"_"+"ContactWebAddress",$contInfo).val(contact.ContactWebAddress);
			$("#"+index+"_"+"ContactAddress1",$contInfo).val(contact.ContactAddress1);
			$("#"+index+"_"+"ContactAddress2",$contInfo).val(contact.ContactAddress2);
			$("#"+index+"_"+"ContactCity",$contInfo).val(contact.ContactCity);
			$("#"+index+"_"+"ContactState",$contInfo).val(contact.ContactState);
			$("#"+index+"_"+"ContactPostalCode",$contInfo).val(contact.ContactZip);
			$("#"+index+"_"+"DistributionNotices",$contInfo).get(0).checked=contact.DistributionNotices;
			$("#"+index+"_"+"Financials",$contInfo).get(0).checked=contact.Financials;
			$("#"+index+"_"+"K1",$contInfo).get(0).checked=contact.K1;
			$("#"+index+"_"+"InvestorLetters",$contInfo).get(0).checked=contact.InvestorLetters;
			var contactCountry=$("#"+index+"_"+"ContactCountry",$contInfo);
			contactCountry.val(contact.ContactCountry);
			if(contactCountry.val()!="225") {
				$("#"+index+"_"+"ContactSateRow",$contInfo).hide();
			}
		}
	}
	,loadAccountInfo: function ($invInfo,account,index) {
		var $accInfo=$("#accountInfo_"+account.AccountId,$invInfo);
		if(!($accInfo.get(0))) {
			$accInfo=editInvestor.cloneAccountInfo($invInfo);
			//Replace the index
			$accInfo.html($accInfo.html().replace(/0_/g,(index)+"_"));
			$("#accountInfoMain",$invInfo).append($accInfo);
			$accInfo.css("display","");
			$accInfo.attr("id","accountInfo_"+account.AccountId);
			$("#accountIndex",$accInfo).html(index);

			$("#"+index+"_"+"AccountId",$accInfo).val(account.AccountId);
			$("#"+index+"_"+"BankName",$accInfo).val(account.BankName);
			$("#"+index+"_"+"AccountNumber",$accInfo).val(account.AccountNumber);
			$("#"+index+"_"+"ABANumber",$accInfo).val(account.ABANumber);
			$("#"+index+"_"+"AccountOf",$accInfo).val(account.AccountOf);
			$("#"+index+"_"+"FFC",$accInfo).val(account.FFC);
			$("#"+index+"_"+"FFCNO",$accInfo).val(account.FFCNO);
			$("#"+index+"_"+"Swift",$accInfo).val(account.Swift);
			$("#"+index+"_"+"IBAN",$accInfo).val(account.IBAN);
			$("#"+index+"_"+"ByOrderOf",$accInfo).val(account.ByOrderOf);
			$("#"+index+"_"+"Reference",$accInfo).val(account.Reference);
			$("#"+index+"_"+"Attention",$accInfo).val(account.Attention);
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
		$("#UpdateTargetId").html("");
		$("#UpdateLoading",editInvestor.currentForm).html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Updating...");
	}
	,onSuccess: function () {
		$("#UpdateLoading",editInvestor.currentForm).html("Update Successfully");
		var UpdateTargetId=$("#UpdateTargetId");
		var InvestorId=$("#InvestorId",editInvestor.currentForm).val();
		/*$(".InvestorUpdateLoading").remove();*/
		setTimeout(function () {
			$("#UpdateLoading",editInvestor.currentForm).html("");
		},2000)
		if($.trim(UpdateTargetId.html())!="") {
			alert(UpdateTargetId.html());$(".InvestorUpdateLoading").remove();
		} else {
			editInvestor.selectInvestor(InvestorId);
		}
	}
};
