var editInvestor={
	currentForm: null
	,currentType: ''
	,init: function () {
		$(document).ready(function () {
			jHelper.waterMark($("body"));
			editInvestor.selectInvestor(editInvestor.getInvestorId());
		});
	}
	,setInvestorId: function (id) {
		$("#InvestorId").val(id);
	}
	,selectInvestor: function (id) {
		var dt=new Date();
		var target=$("#InvestorContainer");
		target.empty();
		editInvestor.setInvestorId(id);
		if(id>0) {
			var loading=$("#Loading");
			loading.html(jHelper.loadingHTML());
			target.html("<center>"+jHelper.loadingHTML()+"</center>");
			$.getJSON("/Investor/FindInvestorDetail/"+id+"?t="+dt.getTime(),function (data) {
				loading.empty();
				target.empty();
				$("#Investor").val(data.InvestorName);
				$("#InvestorInformationTemplate").tmpl(data).appendTo(target);
				$("#AddressInformationTemplate").tmpl(data).appendTo(target);
				$("#BankInformationTemplate").tmpl(data).appendTo(target);
				$("#ContactInformationTemplate").tmpl(data).appendTo(target);

				editInvestor.initInvestorEvents();

				if(data.AddressInformations.length==0) {
					var addInfo=$("#addressInfoMain .addressinfo-box");
					$("#AddressInfoEditTemplate").tmpl(getNewAddress(id)).appendTo(addInfo);
				}

				$("#addressInfoMain",target).each(function () {
					var addInfo=$(this);
					editInvestor.setupAddressInfo(addInfo);
				});

				$("#contactInfoMain .contactInfo",target).each(function () {
					var addInfo=$(this);
					editInvestor.setupAddressInfo(addInfo);
				});

				$("#StateOfResidencyName",target)
				.autocomplete({ source: "/Admin/FindStates",minLength: 1
				,select: function (event,ui) {
					$("#StateOfResidency",target).val(ui.item.id);
				}
				,appendTo: "body",delay: 300
				});

				jHelper.checkValAttr(target);
				jHelper.jqComboBox(target);
				jHelper.jqCheckBox(target);
				editInvestor.formatEditor(target);
				editInvestor.removeFirstLine();
				var invloading=$("#InvListLoading");
				var p=new Array();
				p[p.length]={ "name": "investorId","value": id };
				$("#InvestmentList",target).flexigrid({
					usepager: true
					,url: "/Investor/InvestmentDetailList"
					,params: p
					,resizeWidth: true
					,method: "GET"
					,sortname: "FundName"
					,sortorder: "asc"
					,autoload: true
					,useBoxStyle: false
					,onSubmit: function () { invloading.html(jHelper.loadingHTML());return true; }
					,onSuccess: function () { invloading.empty(); }
				});
			});
		}
	}
	,removeFirstLine: function () {
		$(".line:first","#AccountInfoBox").remove();
		$(".line:first","#ContactInfoBox").remove();
	}
	,scrollableElement: function (els) {
		for(var i=0,argLength=arguments.length;i<argLength;i++) {
			var el=arguments[i],
			$scrollElement=$(el);
			if($scrollElement.scrollTop()>0) {
				return el;
			} else {
				$scrollElement.scrollTop(1);
				var isScrollable=$scrollElement.scrollTop()>0;
				$scrollElement.scrollTop(0);
				if(isScrollable) {
					return el;
				}
			}
		}
		return [];
	}
	,scrollTo: function (target) {
		//setTimeout(function () {
		var scrollElem=editInvestor.scrollableElement('html','body');
		var targetOffset=$(target).offset().top;
		// Animate to target
		$(scrollElem).animate({ scrollTop: targetOffset-300 },400,function () {
			// Set hash in URL after animation successful
			//location.hash=target;
		});
		//},0);
		/*try {
		setTimeout(function () {
		$.scrollTo(target,1000);
		},1000);
		//} catch(e) { }
		*/
	}
	,initInvestorEvents: function () {
		$(".expandheader").click(function () {
			//$(".expandtitle").hide();
			//$(".expandimg").show();
			var bolexpandsel=$(this).hasClass("expandsel");
			//$(".expandsel").removeClass("expandsel");
			//$(".fieldbox").hide();
			//$(".expandaddbtn").hide();
			if(!bolexpandsel) {
				$(this).addClass("expandsel");
				$(".rightuarrow").remove();
				$("#img",this).hide();
				$("#title",this).show();
				$("#title .expandtitle",this).show();
				$(".expandaddbtn",this).show();
				$(".makenew-header",this).show();
				$(".fieldbox",$(this).parent()).show();
				$(".expandaddbtn",$(this).parent()).show().addClass("addbtn-extend");
			} else {
				$(this).removeClass("expandsel");
				$(".expandtitle",this).hide();
				$(".expandimg",this).show();
				$(this).next(".fieldbox").hide();
				$(this).prev(".expandaddbtn").hide();
				switch($(this).parent().get(0).id) {
					case "DealSellerInfo":
						$(".fieldbox",$("#SellerInfo")).hide();
						break;
					case "DealUnderlyingDirects":
						$(this).next().next(".fieldbox").hide();
						break;
				}
			}
		});
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
	,addContactInfo: function (that,investorId) {
		var contactInfoMain=$(that).parents("#contactInfoMain:first");
		$("#ContactInfoEditTemplate").tmpl(getNewContact(investorId)).appendTo(contactInfoMain);
		var addInfo=$(".contactInfo:last",contactInfoMain);
		$("#ContactInfoAddNew",contactInfoMain).hide();
		editInvestor.setupAddressInfo(addInfo);
		jHelper.jqCheckBox(addInfo);
		jHelper.jqComboBox(addInfo);
		$(".show",addInfo).hide();
		$(".hide",addInfo).show();
		$(".contactinfo-box:first .line").remove();
		editInvestor.removeFirstLine();
		editInvestor.scrollTo(addInfo);
	}
	,deleteContact: function (that,contactId) {
		if(confirm("Are you sure you want to delete this investor contact?")) {
			var editinfo=$(that).parents(".contactinfo-box:first").get(0);
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
						editInvestor.scrollTo(contactInfoMain);;
					}
				});
			} else {
				$(editinfo).remove();
				editInvestor.showContactAddNew(contactInfoMain);
				editInvestor.scrollTo(contactInfoMain);;
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
						editInvestor.scrollTo(accountInfoMain);
					}
				});
			} else {
				$(editinfo).remove();
				editInvestor.showAccountAddNew(accountInfoMain);
				editInvestor.scrollTo(accountInfoMain);
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
		$("#BankInfoEditTemplate").tmpl(getNewBank(investorId)).appendTo(accountInfoMain);
		var addInfo=$(".accountInfo:last",accountInfoMain);
		$("#AccountInfoAddNew",accountInfoMain).hide();
		editInvestor.setupAddressInfo(addInfo);
		$(".show",addInfo).hide();
		$(".hide",addInfo).show();
		editInvestor.scrollTo(addInfo);
		editInvestor.removeFirstLine();
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
		editInvestor.scrollTo(cntdiv);
	}
	,cancelInvestorInfo: function (that) {
		var cntdiv=$(that).parents(".editinfo:first").get(0);
		$(".show",cntdiv).show();
		$(".hide",cntdiv).hide();
		editInvestor.scrollTo(cntdiv);
	}
	,saveInvestorDetail: function (img) {
		try {
			var frm=$("#EditInvestor");
			var loading=$("#UpdateLoading",frm);
			loading.html(jHelper.savingHTML());
			$.post("/Investor/UpdateInvestorInformation",$(frm).serializeForm(),function (data) {
				loading.empty();
				var arr=data.split("||");
				if($.trim(arr[0])!="True") {
					jAlert(data);
					loading.empty();
				} else {
					jAlert("Investor Information Saved.");
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
			loading.html(jHelper.savingHTML());
			var param=$(frm).serializeForm();
			param[param.length]={ name: "InvestorId",value: editInvestor.getInvestorId() };
			$.post("/Investor/UpdateInvestorAddress",param,function (data) {
				var arr=data.split("||");
				if($.trim(arr[0])!="True") {
					jAlert(data);
					loading.empty();
				} else {
					jAlert("Investor Address Saved.");
					var target=$("#addressInfoMain .addressinfo-box");
					$.get("/Investor/FindInvestorAddress/"+arr[1]+"?_"+(new Date).getTime(),function (newdata) {
						newdata.i=0;
						target.empty();
						$("#AddressInfoEditTemplate").tmpl(newdata).appendTo(target);
						var addressInfoMain=frm.parents("#addressInfoMain:first");
						editInvestor.setupAddressInfo(target);
						jHelper.checkValAttr(target);
						jHelper.jqComboBox(target);
						jHelper.jqCheckBox(target);
						editInvestor.removeFirstLine();
						editInvestor.scrollTo(addInfo);
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
			loading.html(jHelper.savingHTML());
			$.post("/Investor/UpdateInvestorContact",$(frm).serializeForm(),function (data) {
				var arr=data.split("||");
				if($.trim(arr[0])!="True") {
					jAlert(data);
					loading.empty();
				} else {
					jAlert("Investor Contact Saved.");
					var target=$(frm).parents(".contactinfo-box:first");
					var contactInfoMain=$(target).parents("#contactInfoMain:first");
					$.get("/Investor/FindInvestorContact/"+arr[1]+"?_"+(new Date).getTime(),function (newdata) {
						newdata.i=0;
						$("#ContactInfoEditTemplate").tmpl(newdata).insertAfter(target);
						target.remove();
						var addInfo=$("#contactInfo"+arr[1],contactInfoMain);
						editInvestor.setupAddressInfo(addInfo);
						jHelper.checkValAttr(addInfo);
						jHelper.jqComboBox(addInfo);
						jHelper.jqCheckBox(addInfo);
						editInvestor.removeFirstLine();
						editInvestor.scrollTo(addInfo);
					});
				}
			});
		} catch(e) {
		}
		return false;
	}
	,getInvestorId: function () {
		var id=parseInt($("#InvestorId").val());
		if(isNaN(id)) id=0;
		return id;
	}
	,saveBankDetail: function (img) {
		try {
			var frm=$(img).parents("form:first");
			var loading=$("#Loading",frm);
			loading.html(jHelper.savingHTML());
			var param=$(frm).serializeForm();
			param[param.length]={ name: "InvestorId",value: editInvestor.getInvestorId() };
			$.post("/Investor/UpdateInvestorBankDetail",param,function (data) {
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
						$("#BankInfoEditTemplate").tmpl(newdata).insertAfter(target);
						target.remove();
						var addInfo=$("#accountInfo"+arr[1],accountInfoMain);
						editInvestor.setupAddressInfo(addInfo);
						jHelper.checkValAttr(addInfo);
						jHelper.jqComboBox(addInfo);
						jHelper.jqCheckBox(addInfo);
						editInvestor.removeFirstLine();
						editInvestor.scrollTo(addInfo);
					});
				}
			});
		} catch(e) {
			alert(e);
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
		var investorId=editInvestor.getInvestorId();
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
					jAlert("Investor Deleted.");
					editInvestor.setInvestorId(0);
					$("#InvestorContainer").empty();
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