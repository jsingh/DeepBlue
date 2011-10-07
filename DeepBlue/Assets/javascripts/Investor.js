var investor={
	newContactDetail: null
	,init: function () {
		var investorId=$("#InvestorId").val();
		var target=$("#InvestorContainer");
		target.html("<center>"+jHelper.loadingHTML()+"</center>");
		$.getJSON("/Investor/FindInvestorDetail/"+investorId,function (data) {
			target.empty();
			$("#InvestorInformationTemplate").tmpl(data).appendTo(target);
			$("#AddressInformationTemplate").tmpl(data).appendTo(target);
			$("#BankInformationTemplate").tmpl(data).appendTo(target);
			$("#ContactInformationTemplate").tmpl(data).appendTo(target);
			jHelper.jqCheckBox(target);
			jHelper.jqComboBox(target);
			investor.initInvestorEvents();
			$("#addressInfoMain",target).each(function () {
				var addInfo=$(this);
				investor.setupAddressInfo(addInfo);
			});
			$("#StateOfResidencyName",target)
				.autocomplete({ source: "/Admin/FindStates",minLength: 1
				,select: function (event,ui) {
					$("#StateOfResidency",target).val(ui.item.id);
				}
				,appendTo: "body",delay: 300
				});
			investor.createAccount();
			investor.createContact();
			$(".line:first","#AccountInfoBox").remove();
			$(".line:first","#ContactInfoBox").remove();
		});
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
	,addAccountInfo: function (that,investorId) {
		var accountInfoMain=$(that).parents("#accountInfoMain:first");
		var accountLength=$("#AccountLength");
		var total=parseInt(accountLength.val());
		accountLength.val(total+1);
		var data={ i: accountLength.val() };
		$("#BankInfoEditTemplate").tmpl(data).appendTo(accountInfoMain);
		var addInfo=$(".accountInfo:last",accountInfoMain);
		$("#AccountInfoAddNew",accountInfoMain).hide();
		investor.setupAddressInfo(addInfo);
		$(".show",addInfo).hide();
		$(".hide",addInfo).show();
		//investor.scroll(addInfo);
	}
	,cancelInvestorInfo: function (that) {
		var cntdiv=$(that).parents(".editinfo:first").get(0);
		$(".show",cntdiv).show();
		$(".hide",cntdiv).hide();
		//editInvestor.scroll(cntdiv);
	}
	,editInvestorInfo: function (that) {
		var cntdiv=$(that).parents(".editinfo:first").get(0);
		$(".show",cntdiv).hide();
		$(".hide",cntdiv).show();
		//editInvestor.scroll(cntdiv);;
	}
	,setupCommunication: function (addInfo) {
		$(".comvalue",addInfo).each(function () {
			$("span[id='"+$(this).attr("id")+"']",addInfo).html($(this).val());
		});
	}
	,setupAddressInfo: function (addInfo) {
		investor.setupCountryState(addInfo);
		investor.setupCommunication(addInfo);
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
	,scrollTo: function (target) {
		//$.scrollTo(target,{ speed: 1000 });
	}
	,createAccount: function () {
		var accountLength=$("#AccountLength");
		var total=parseInt(accountLength.val());
		accountLength.val(total+1);
		var data={ i: accountLength.val(),InvestorId: 0 };
		var target=$("#AccountInfoBox");
		$("#BankInfoEditTemplate").tmpl(data).appendTo(target);
		investor.scrollTo(".accountinfo-box:last");
	}
	,save: function (frm) {
		try {
			var loading=$("#UpdateLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.post("/Investor/Create",$(frm).serializeForm(),function (data) {
				loading.empty();
				if($.trim(data)!="") {
					jAlert(data);
				} else {
					jAlert("Investor Saved.");
					location.href="/Investor/New";
				}
			});
		} catch(e) {
			jAlert(e);
		}
		return false;
	}
	,deleteAccount: function (that) {
		if(confirm("Are you sure you want to delete this Account?")) {
			var AccountInfo=$(that).parents(".accountinfo-box").get(0);
			$(AccountInfo).remove();
		}
	}
	,changeCountry: function (item) {
		var country=$("#Country","#AddressInfoMain");
		country.val(item.id);
		var state=$("#State","#AddressInfoMain");
		var staterow=$("#StateRow","#AddressInfoMain");
		var stateName=$("#StateName","#AddressInfoMain");
		state.val(52);
		if(item.label!="United States") {
			staterow.hide();
		} else {
			staterow.show();
			stateName.val("");
			state.val(0);
		}
	}
	,createContact: function () {
		var contactLength=$("#ContactLength");
		var total=parseInt(contactLength.val());
		contactLength.val(total+1);
		var data=investor.newContactDetail;
		data.i=contactLength.val();
		data.InvestorId=0;
		$("#ContactInfoEditTemplate").tmpl(data).appendTo($("#ContactInfoBox"));
		var newContact=$("#ContactInfo"+data.i,"#ContactInfoBox");
		jHelper.jqComboBox(newContact);
		jHelper.jqCheckBox(newContact);

		$("#ContactCountryName",newContact)
			.autocomplete({ source: "/Admin/FindCountrys",minLength: 1
			,select: function (event,ui) {
				$("#ContactCountry",newContact).val(ui.item.id);
				var stateRow=$("#StateRow",newContact);
				var state=$("#ContactState",newContact);
				var stateName=$("#ContactStateName",newContact);
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

		$("#ContactStateName",newContact)
			.autocomplete({ source: "/Admin/FindStates",minLength: 1
			,select: function (event,ui) {
				$("#ContactState",newContact).val(ui.item.id);
			}
			,appendTo: "body",delay: 300
			});
		investor.scrollTo(".contactinfo-box:last");
	}
	,deleteContact: function (that) {
		if(confirm("Are you sure you want to delete this Contact?")) {
			var ContactInfo=$(that).parents(".contactinfo-box").get(0);
			$(ContactInfo).remove();
		}
	}
	,validation: function (formId) {
		var frm=document.getElementById(formId);
		var message='';
		var invnameValid=$("#InvestorName_validationMessage",frm);
		var sstaxidValid=$("#SocialSecurityTaxId_validationMessage",frm);
		if(jQuery.trim(invnameValid.html())!="") {
			message+=invnameValid.html()+"\n";
		}
		if(jQuery.trim(sstaxidValid.html())!="") {
			message+=sstaxidValid.html()+"\n";
		}
		if(jQuery.trim(message)!="") {
			jAlert(message);
			return false;
		} else {
			Sys.Mvc.FormContext.getValidationForForm(frm).validate('submit');
			$(".field-validation-error",frm).each(function () {
				if(this.innerHTML!='') {
					message+=this.innerHTML+"\n";
				}
			});
			if(jQuery.trim(message)!="") {
				jAlert(message);
				return false;
			} else {
				return true;
			}
		}
	}
	,checkEmail: function (name) {
		if(this.checkInputValid(name)) {
			var input=$("input[name='"+name+"']");
			var editorfield=input.parent();
			var customvalidation=$(".custom-validation",editorfield).get(0);
			if(customvalidation) {
				var regMail=/^([_a-zA-Z0-9-]+)(\.[_a-zA-Z0-9-]+)*@([a-zA-Z0-9-]+\.)+([a-zA-Z]{2,3})$/;
				if(regMail.test(input.val())==false) {
					customvalidation.innerHTML="Invalid Email";
					return false;
				}
			}
		} else {
			return false;
		}
		return true;
	}
	,checkZip: function (name) {
		if(this.checkInputValid(name)) {
			var input=$("input[name='"+name+"']");
			var editorfield=input.parent();
			var customvalidation=$(".custom-validation",editorfield).get(0);
			if(customvalidation) {
				var regZip=/^(\d{5}-\d{4}|\d{5}|\d{9})$|^([a-zA-Z]\d[a-zA-Z]\d[a-zA-Z]\d)$/;
				if(regZip.test(input.val())==false) {
					customvalidation.innerHTML="Invalid Zip";
					return false;
				}
			}
		} else {
			return false;
		}
		return true;
	}
	,checkPhone: function (name) {
		if(this.checkInputValid(name)) {
			var input=$("input[name='"+name+"']");
			var editorfield=input.parent();
			var customvalidation=$(".custom-validation",editorfield).get(0);
			if(customvalidation) {
				var regPhone=/^[01]?[-.]?(\([2-9]\d{2}\)|[2-9]\d{2})[-.]?\d{3}[-.]?\d{4}$/;
				if(regPhone.test(input.val())==false) {
					customvalidation.innerHTML="Invalid Telephone No";
					return false;
				}
			}
		} else {
			return false;
		}
		return true;
	}
	,checkInputValid: function (name) {
		var input=$("input[name='"+name+"']");
		var editorfield=input.parent();
		var customvalidation=$(".custom-validation",editorfield).get(0);
		if(!customvalidation) {
			customvalidation=document.createElement("span");
			editorfield.append(customvalidation);
		}
		customvalidation.className="custom-validation";
		customvalidation.innerHTML="";
		$(input).focus(function () {
			customvalidation.innerHTML="";
		});
		if(input.val()=="") {
			customvalidation.innerHTML="Required";
			return false;
		}
		return true;
	}
	,checkSelectValid: function (name) {
		var select=$("select[name='"+name+"']");
		var editorfield=select.parent();
		var customvalidation=$(".custom-validation",editorfield).get(0);
		if(!customvalidation) {
			customvalidation=document.createElement("span");
			editorfield.append(customvalidation);
		}
		customvalidation.className="custom-validation";
		customvalidation.innerHTML="";
		$(select).focus(function () {
			customvalidation.innerHTML="";
		});
		if(select.val()=="") {
			customvalidation.innerHTML="Required";
			return false;
		}
		return true;
	}
	,onBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="") {
			jAlert(UpdateTargetId.html())
		} else {
			jAlert("Investor Saved.");
			location.href="/Investor/New";
		}
	}
	,getInvestorId: function () {
		var id=parseInt($("#InvestorId").val());
		if(isNaN(id)) id=0;
		return id;
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