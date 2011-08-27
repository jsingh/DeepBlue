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
		var target=$("#editinfo");
		target.empty();
		$.getJSON("/Investor/FindInvestorDetail/"+id+"?t="+dt.getTime(),function (data) {
			$("#Loading").html("");
			$("#EditInvestorTemplate").tmpl(data).appendTo(target);
			editInvestor.applyAccordion(target);

			$("#addressInfoMain",target).each(function () {
				var addInfo=$(this);
				editInvestor.setupAddressInfo(addInfo);
			});

			$("#contactInfoMain .contactInfo",target).each(function () {
				var addInfo=$(this);
				editInvestor.setupAddressInfo(addInfo);
			});

			$(".stateac",target).each(function () {
				var statetxt=$(this);
				$(statetxt)
				.autocomplete({ source: "/Admin/FindStates",minLength: 1
				,select: function (event,ui) {
					$("#"+statetxt.attr("hiddenid"),target).val(ui.item.id);
				}
				,appendTo: "body",delay: 300
				});
			});

			$(".countryac",target).each(function () {
				var countrytxt=$(this);
				countrytxt
				.autocomplete({ source: "/Admin/FindCountrys",minLength: 1
				,select: function (event,ui) {
					$("#"+countrytxt.attr("hiddenid"),target).val(ui.item.id);
					var stateRow=$("#"+countrytxt.attr("staterowid"),target);
					var stateName=$(".stateac:first",stateRow);
					var state=$("#"+stateName.attr("hiddenid"),stateRow);
					state.val(52);
					if(ui.item.label!="United States") {
						stateRow.hide();
					} else {
						stateRow.show();
						stateName.val("");
						state.val(0);
					}
				}
				,appendTo: "body",delay: 300
				});
			});

			jHelper.checkValAttr(target);


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
			showTableToggleBtn: false,useBoxStyle: false
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
	,scroll: function () {
		$('html, body').animate({
			scrollTop: $(document).height()-$(window).height()-10
		},
   500
);
	}
	,addContactInfo: function (that,investorId) {
		var contactInfoMain=$(that).parents("#contactInfoMain:first");
		$("#ContactInfoTemplate").tmpl(getNewContact(investorId)).appendTo(contactInfoMain);
		var addInfo=$(".contactInfo:last",contactInfoMain);
		editInvestor.setupAddressInfo(addInfo);
		editInvestor.scroll();
	}
	,deleteContact: function (that,contactId) {
		if(confirm("Are you sure you want to delete this investor contact?")) {
			var editinfo=$(that).parents("form:first").get(0);
			var contactInfoMain=$(editinfo).parents(".contactInfoMain:first");
			if(parseInt(contactId)>0) {
				var dt=new Date();
				var url="/Investor/DeleteContactAddress/"+contactId+"?t="+dt.getTime();
				$.get(url,function (data) {
					if(data!="True") {
						alert(data);
					} else {
						$(editinfo).remove();
						editInvestor.showContactAddNew(contactInfoMain);
						editInvestor.scroll();
					}
				});
			} else {
				$(editinfo).remove();
				editInvestor.showContactAddNew(contactInfoMain);
				editInvestor.scroll();
			}
		}
	}
	,deleteAccount: function (that,accountId) {
		if(confirm("Are you sure you want to delete this investor account?")) {
			var editinfo=$(that).parents("form:first").get(0);
			var accountInfoMain=$(editinfo).parents(".accountInfoMain:first");
			if(parseInt(accountId)>0) {
				var dt=new Date();
				var url="/Investor/DeleteInvestorAccount/"+accountId+"?t="+dt.getTime();
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
	,addAccountInfo: function (that,investorId) {
		var accountInfoMain=$(that).parents("#accountInfoMain:first");
		$("#BankInfoTemplate").tmpl(getNewBank(investorId)).appendTo(accountInfoMain);
		var addInfo=$(".accountInfo:last",accountInfoMain);
		editInvestor.setupAddressInfo(addInfo);
		editInvestor.scroll();
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
		$(".show",cntdiv).hide()
		$(".hide",cntdiv).show();
		editInvestor.scroll();
	}
	,cancelInvestorInfo: function (that) {
		var cntdiv=$(that).parents(".editinfo:first").get(0);
		$(".show",cntdiv).show()
		$(".hide",cntdiv).hide();
		editInvestor.scroll();
	}
	,saveInvestorAddressDetail: function (img) {
		try {
			var frm=$(img).parents("form:first");
			var loading=$("#Loading",frm);
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.post("/Investor/UpdateInvestorAddress",$(frm).serializeArray(),function (data) {
				var arr=data.split("||");
				if($.trim(arr[0])!="True") {
					jAlert(data);
					loading.empty();
				} else {
					alert("Investor Address Saved.");
					var target=$("#addressInfoMain");
					$.get("/Investor/FindInvestorAddress/"+arr[1]+"?_"+(new Date).getTime(),function (newdata) {
						newdata.i=0;
						$("#AddressInfoTemplate").tmpl(newdata).appendTo(target);
						var addressInfoMain=frm.parents("#addressInfoMain:first");
						$(addressInfoMain).each(function () {
							var addInfo=$(this);
							editInvestor.setupAddressInfo(addInfo);
							jHelper.checkValAttr(addInfo);
						});
						frm.remove();
						editInvestor.scroll();
					});
				}
			});
		} catch(e) {
		}
		return false;
	}
	,saveContactAddressDetail: function (img) {
		try {
			var frm=$(img).parents("form:first");
			var loading=$("#Loading",frm);
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.post("/Investor/UpdateInvestorContact",$(frm).serializeArray(),function (data) {
				var arr=data.split("||");
				if($.trim(arr[0])!="True") {
					jAlert(data);
					loading.empty();
				} else {
					alert("Investor Contact Saved.");
					var target=$(".contactInfo:first",frm);
					var contactInfoMain=$(target).parents("#contactInfoMain:first");
					$.get("/Investor/FindInvestorContact/"+arr[1]+"?_"+(new Date).getTime(),function (newdata) {
						newdata.i=0;
						$("#ContactInfoTemplate").tmpl(newdata).insertAfter(target);
						target.remove();
						var addInfo=$("#contactInfo"+arr[1],contactInfoMain);
						editInvestor.setupAddressInfo(addInfo);
						jHelper.checkValAttr(addInfo);
						editInvestor.scroll();
					});
				}
			});
		} catch(e) {
		}
		return false;
	}
	,saveBankDetail: function (img) {
		try {
			var frm=$(img).parents("form:first");
			var loading=$("#Loading",frm);
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.post("/Investor/UpdateInvestorBankDetail",$(frm).serializeArray(),function (data) {
				var arr=data.split("||");
				if($.trim(arr[0])!="True") {
					jAlert(data);
					loading.empty();
				} else {
					alert("Investor Account Saved.");
					var target=$(".accountInfo:first",frm);
					var accountInfoMain=$(target).parents("#accountInfoMain:first");
					$.get("/Investor/FindInvestorAccount/"+arr[1]+"?_"+(new Date).getTime(),function (newdata) {
						newdata.i=0;
						$("#BankInfoTemplate").tmpl(newdata).insertAfter(target);
						target.remove();
						var addInfo=$("#accountInfo"+arr[1],accountInfoMain);
						editInvestor.setupAddressInfo(addInfo);
						jHelper.checkValAttr(addInfo);
						editInvestor.scroll();
					});
				}
			});
		} catch(e) {
		}
		return false;
	}
	,setupCommunication: function (addInfo) {
		$(".comvalue",addInfo).each(function () {
			$("#"+$(this).attr("id"),addInfo).html(this.value);
		});
	}
	,setupAddressInfo: function (addInfo) {

		editInvestor.setupCountryState(addInfo);
		editInvestor.setupCommunication(addInfo);

	}
	,setupCountryState: function (addInfo) {
		$("#StateName",addInfo)
				.autocomplete({ source: "/Admin/FindStates",minLength: 1
				,select: function (event,ui) {
					$("#State",addInfo).val(ui.item.id);
				}
				,appendTo: "body",delay: 300
				});

		$("#CountryName",addInfo)
				.autocomplete({ source: "/Admin/FindCountrys",minLength: 1
				,select: function (event,ui) {
					$("#Country",addInfo).val(ui.item.id);
					var stateRow=$("#AddressStateRow",addInfo);
					var stateName=$("#StateName",stateRow);
					var state=$("#State",stateRow);
					state.val(52);
					if(ui.item.label!="United States") {
						stateRow.hide();
					} else {
						stateRow.show();
						stateName.val("");
						state.val(0);
					}
				}
				,appendTo: "body",delay: 300
				});
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
						disp.html("<img src='/Assets/images/tick.png' />");
					} else {
						disp.html("");
					}
					$(this).change(function () {
						if(this.checked) {
							disp.html("<img src='/Assets/images/tick.png' />");
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
				if($.trim(data)!="") {
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
			alert("Investor Saved.");
			editInvestor.selectInvestor(InvestorId);
		}
	}
};
$.extend(window,{
	getItem: function (i,item) { item.i=i;return item; }
	,getNewAddress: function (investorId) {
		return { i: 0,InvestorCommunications: {},InvestorId: investorId,Country: 225,CountryName: "United States",State: 0,StateName: "" };
	}
	,getNewContact: function (investorId) {
		return { i: 0,InvestorContactId: 0,InvestorId: investorId,ContactCommunications: {},AddressInformations: getNewAddress(investorId) };
	}
	,getNewBank: function (investorId) {
		return { i: 0,AccountId: 0,InvestorId: investorId };
	}
	,getCommunicationValue: function (communications,typeId) {
		var i;
		var value="";
		if(communications!=null) {
			for(i=0;i<communications.length;i++) {
				if(communications[i].CommunicationTypeId==typeId) {
					value=communications[i].CommunicationValue;
					break;
				}
			}
		}
		return value;
	}
	,getAddress: function (addressInfo,name) {
		var info=null;
		if(addressInfo!=null) {
			$(addressInfo).each(function () {
				info=this;
				return false;
			});
		}
		if(info==null) info={};
		return info;
	}
});