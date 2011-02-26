var investor={
	createAccount: function () {
		var AccountLength=parseInt($("#AccountLength").val());
		var AccountInfo=document.createElement("div");
		AccountInfo.id="AccountInfo_"+(AccountLength+1);
		AccountInfo.className="accountinfo";
		AccountInfo.innerHTML=$("#AccountInfo").html().replace(/1_/g,(AccountLength+1)+"_");
		$("input[type!='hidden'][type!='checkbox']",AccountInfo).val("");
		$("select",AccountInfo).val("");
		$("#AccountLength").val(AccountLength+1);
		//$("#index",AccountInfo).html(AccountLength+1);
		$(".custom-validation",AccountInfo).remove();
		$(".delete",AccountInfo).css("display","block");
		$("#AccountInfoBox").append(AccountInfo);
	}
	,deleteAccount: function (that) {
		if(confirm("Are you sure you want to delete this Account?")) {
			var AccountInfo=$(that).parents(".accountinfo").get(0);
			var AccountLength=parseInt($("#AccountLength").val());
			$("#AccountLength").val(AccountLength-1);
			$(AccountInfo).remove();
		}
	}
	,createContact: function () {
		var ContactLength=parseInt($("#ContactLength").val());
		var ContactInfo=document.createElement("div");
		ContactInfo.id="ContactInfo_"+(ContactLength+1);
		ContactInfo.className="contactinfo";
		ContactInfo.innerHTML=$("#ContactInfo").html().replace(/1_/g,(ContactLength+1)+"_");
		$("input[type!='hidden'][type!='checkbox']",ContactInfo).val("");
		$("select",ContactInfo).val("");
		$("#ContactLength").val(ContactLength+1);
		//$("#index",ContactInfo).html(ContactLength+1);
		$(".custom-validation",ContactInfo).remove();
		$(".delete",ContactInfo).css("display","block");
		$(".add",ContactInfo).remove();
		$("#ContactInfoBox").append(ContactInfo);
		$("select[name='"+(ContactLength+1)+"_ContactCountry']",ContactInfo).val(225);
	}
	,deleteContact: function (that) {
		if(confirm("Are you sure you want to delete this Contact?")) {
			var ContactInfo=$(that).parents(".contactinfo").get(0);
			var ContactLength=parseInt($("#ContactLength").val());
			$("#ContactLength").val(ContactLength-1);
			$(ContactInfo).remove();
		}
	}
	,validation: function () {
		var validForm=true;
		var AccountLength=parseInt($("#AccountLength").val());
		var index;
		for(index=1;index<AccountLength+1;index++) {
			validForm=this.checkInputValid(index+"_BankName");
			validForm=this.checkInputValid(index+"_AccountNumber");
		}
		var ContactLength=parseInt($("#ContactLength").val());
		var index;
		for(index=1;index<ContactLength+1;index++) {
			//validForm=this.checkInputValid(index+"_ContactPerson");
			validForm=this.checkSelectValid(index+"_ContactState");
			validForm=this.checkSelectValid(index+"_ContactCountry");
			validForm=this.checkEmail(index+"_ContactEmail");
			validForm=this.checkZip(index+"_ContactZip");
			validForm=this.checkPhone(index+"_ContactPhoneNumber");
		}
		return validForm;
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
				var regZip=/^(\d{5}-\d{4}|\d{5}|\d{9})$|^([a-zA-Z]\d[a-zA-Z] \d[a-zA-Z]\d)$/;
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
};