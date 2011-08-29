var editInvestor={
	currentForm: null
	,currentType: ''
	,init: function () {
		$(document).ready(function () {
			//			var id=$("#id").val();
			//			if(parseInt(id)>0) {
			//				editInvestor.selectInvestor(id);
			//			}
			jHelper.waterMark($("body"));
		});
	}
	,selectInvestor: function (id) {
		var dt=new Date();
		var target=$("#editinfo");
		target.empty();
		if(id>0) {
			var loading=$("#Loading");
			loading.html(jHelper.loadingHTML());
			$.getJSON("/Investor/FindInvestorDetail/"+id+"?t="+dt.getTime(),function (data) {
				loading.empty();
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

				jHelper.checkValAttr(target);
				jHelper.jqComboBox(target);
				jHelper.jqCheckBox(target);
				editInvestor.formatEditor(target);
			});
		}
	}
	,formatEditor: function (target) {
		$(".notes",target).each(function () { $(this).html($(this).text()); });
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
	,applyAccordion: function ($invInfo) {
		var $accordion=$("#accordion",$invInfo);
		$accordion.accordion({ animated: false,active: -1,collapsible: true,autoHeight: false,navigation: true });
	}
	,scroll: function (target) {
		//$.scrollTo(target,800);
		/* $('html, body').animate({
		scrollTop: ($(document).height()-$(window).height()-800)
		},
		1000
		); */
	}
	,addContactInfo: function (that,investorId) {
		var contactInfoMain=$(that).parents("#contactInfoMain:first");
		$("#ContactInfoTemplate").tmpl(getNewContact(investorId)).appendTo(contactInfoMain);
		var addInfo=$(".contactInfo:last",contactInfoMain);
		$("#ContactInfoAddNew",contactInfoMain).hide();
		editInvestor.setupAddressInfo(addInfo);
		$(".show",addInfo).hide();
		$(".hide",addInfo).show();
		//editInvestor.scroll(addInfo);
	}
	,deleteContact: function (that,contactId) {
		if(confirm("Are you sure you want to delete this investor contact?")) {
			var editinfo=$(that).parents("form:first").get(0);
			var contactInfoMain=$(editinfo).parents("#contactInfoMain:first");
			if(parseInt(contactId)>0) {
				var dt=new Date();
				var url="/Investor/DeleteContactAddress/"+contactId+"?t="+dt.getTime();
				$.get(url,function (data) {
					if(data!="True") {
						jAlert(data);
					} else {
						$(editinfo).remove();
						editInvestor.showContactAddNew(contactInfoMain);
						//editInvestor.scroll(contactInfoMain);;
					}
				});
			} else {
				$(editinfo).remove();
				editInvestor.showContactAddNew(contactInfoMain);
				//editInvestor.scroll(contactInfoMain);;
			}
		}
	}
	,deleteAccount: function (that,accountId) {
		if(confirm("Are you sure you want to delete this investor account?")) {
			var editinfo=$(that).parents("form:first").get(0);
			var accountInfoMain=$(editinfo).parents("#accountInfoMain:first");
			if(parseInt(accountId)>0) {
				var dt=new Date();
				var url="/Investor/DeleteInvestorAccount/"+accountId+"?t="+dt.getTime();
				$.get(url,function (data) {
					if(data!="True") {
						jAlert(data);
					} else {
						$(editinfo).remove();
						editInvestor.showAccountAddNew(accountInfoMain);
						//editInvestor.scroll(accountInfoMain);
					}
				});
			} else {
				$(editinfo).remove();
				editInvestor.showAccountAddNew(accountInfoMain);
				//editInvestor.scroll(accountInfoMain);
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
		$("#AccountInfoAddNew",accountInfoMain).hide();
		editInvestor.setupAddressInfo(addInfo);
		$(".show",addInfo).hide();
		$(".hide",addInfo).show();
		//editInvestor.scroll(addInfo);
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
		$(".show",cntdiv).hide();
		$(".hide",cntdiv).show();
		//editInvestor.scroll(cntdiv);;
	}
	,cancelInvestorInfo: function (that) {
		var cntdiv=$(that).parents(".editinfo:first").get(0);
		$(".show",cntdiv).show();
		$(".hide",cntdiv).hide();
		//editInvestor.scroll(cntdiv);
	}
	,saveInvestorDetail: function (img) {
		try {
			var frm=$(img).parents("form:first");
			var loading=$("#Loading",frm);
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.post("/Investor/UpdateInvestorInformation",$(frm).serializeArray(),function (data) {
				var arr=data.split("||");
				if($.trim(arr[0])!="True") {
					jAlert(data);
					loading.empty();
				} else {
					jAlert("Investor Information Saved.");
					var investorInfo=$("#investorInfoMain");
					var displayInvestorInfoMain=$("#displayInvestorInfoMain");
					investorInfo.empty();
					displayInvestorInfoMain.empty();
					$.get("/Investor/FindInvestorInformation/"+arr[1]+"?_"+(new Date).getTime(),function (newdata) {
						$("#InvestorInfoEditTemplate").tmpl(newdata).appendTo(investorInfo);
						$("#DisplayInvestorInfoTemplate").tmpl(newdata).appendTo(displayInvestorInfoMain);
						jHelper.checkValAttr(investorInfo);
						jHelper.jqComboBox(investorInfo);
						jHelper.jqCheckBox(investorInfo);
						//editInvestor.scroll(investorInfo);
						editInvestor.formatEditor(displayInvestorInfoMain);
					});
				}
			});
		} catch(e) {
		}
		return false;
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
					jAlert("Investor Address Saved.");
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
						jHelper.jqComboBox(addInfo);
						jHelper.jqCheckBox(addInfo);
						frm.remove();
						//editInvestor.scroll(addInfo);
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
					jAlert("Investor Contact Saved.");
					var target=$(".contactInfo:first",frm);
					var contactInfoMain=$(target).parents("#contactInfoMain:first");
					$.get("/Investor/FindInvestorContact/"+arr[1]+"?_"+(new Date).getTime(),function (newdata) {
						newdata.i=0;
						$("#ContactInfoTemplate").tmpl(newdata).insertAfter(target);
						target.remove();
						var addInfo=$("#contactInfo"+arr[1],contactInfoMain);
						editInvestor.setupAddressInfo(addInfo);
						jHelper.checkValAttr(addInfo);
						jHelper.jqComboBox(addInfo);
						jHelper.jqCheckBox(addInfo);
						//editInvestor.scroll(addInfo);
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
					jAlert("Investor Account Saved.");
					var target=$(".accountInfo:first",frm);
					var accountInfoMain=$(target).parents("#accountInfoMain:first");
					$.get("/Investor/FindInvestorAccount/"+arr[1]+"?_"+(new Date).getTime(),function (newdata) {
						newdata.i=0;
						$("#BankInfoTemplate").tmpl(newdata).insertAfter(target);
						target.remove();
						var addInfo=$("#accountInfo"+arr[1],accountInfoMain);
						editInvestor.setupAddressInfo(addInfo);
						jHelper.checkValAttr(addInfo);
						jHelper.jqComboBox(addInfo);
						jHelper.jqCheckBox(addInfo);
						//editInvestor.scroll(addInfo);
					});
				}
			});
		} catch(e) {
		}
		return false;
	}
	,setupCommunication: function (addInfo) {
		$(".comvalue",addInfo).each(function () {
			$("span[id='"+$(this).attr("id")+"']",addInfo).html($(this).val());
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
	,deleteInvestor: function (that,investorId) {
		if(confirm("Are you sure you want to delete this investor?")) {
			var editinfo=$(that).parents(".edit-info:first").get(0); // get edit-info box
			var loading=$("#DeleteLoading");
			loading.html(jHelper.deleteHTML());
			var dt=new Date();
			var url="/Investor/Delete/"+investorId+"?t="+dt.getTime();
			$.get(url,function (data) {
				if($.trim(data)!="") {
					jAlert(data);
				} else {
					$(editinfo).remove();
					$("#Investor").val("").focus();
				}
			});
		}
	}
};
$.extend(window,{
	getItem: function (i,item) { item.i=i;return item; }
	,getNewAddress: function (investorId) {
		return { i: 0
		,InvestorId: investorId
		,Country: 225
		,CountryName: "United States"
		,State: 0
		,StateName: ""
		,Address1: ""
		,Address2: ""
		,AddressId: 0
		,City: ""
		,Zip: ""
		,InvestorCommunications: {}
		};
	}
	,getNewContact: function (investorId) {
		return { i: 0
				,InvestorId: investorId
				,Person: ""
				,DistributionNotices: false
				,Financials: false
				,InvestorLetters: false
				,K1: false
				,Designation: ""
				,ContactId: 0
				,InvestorContactId: 0
				,ContactCommunications: {}
				,AddressInformations: getNewAddress(investorId)
		};
	}
	,getNewBank: function (investorId) {
		return { i: 0
				,InvestorId: investorId
				,ABANumber: ""
				,AccountNumber: ""
				,Attention: ""
				,AccountOf: ""
				,BankName: ""
				,ByOrderOf: ""
				,FFC: ""
				,FFCNO: ""
				,AccountId: 0
				,IBAN: ""
				,Reference: ""
				,Swift: ""
		};
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