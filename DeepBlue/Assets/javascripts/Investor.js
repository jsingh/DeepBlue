var investor={
	newContactDetail: null
	,createAccount: function () {
		var accountLength=$("#AccountLength");
		var total=parseInt(accountLength.val());
		accountLength.val(total+1);
		var data={ i: accountLength.val() };
		$("#BankInformationTemplate").tmpl(data).appendTo($("#AccountInfoBox"));
	}
	,save: function (frm) {
		try {
			var loading=$("#UpdateLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.post("/Investor/Create",$(frm).serializeArray(),function (data) {
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
			var AccountInfo=$(that).parents(".accountinfo").get(0);
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
		$("#ContactInformationTemplate").tmpl(data).appendTo($("#ContactInfoBox"));
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
	}
	,deleteContact: function (that) {
		if(confirm("Are you sure you want to delete this Contact?")) {
			var ContactInfo=$(that).parents(".contactinfo").get(0);
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
};