var documentUpload={
	changeType: function (select) {
		var InvestorRow=document.getElementById("InvestorRow");
		var FundRow=document.getElementById("FundRow");
		InvestorRow.style.display="none";
		FundRow.style.display="none";
		$("#FundId").val(0);
		$("#InvestorId").val(0);
		if(select.value=="1")
			InvestorRow.style.display="";
		else if(select.value=="2")
			FundRow.style.display="";
	}
	,selectInvestor: function (id) {
		$("#InvestorId").val(id);
	}
	,InvestorBlur: function (txt) {
		if(txt.value=="") {
			$("#InvestorId").val(0);
		}
	}
	,selectFund: function (id) {
		$("#FundId").val(id);
	}
	,FundBlur: function (txt) {
		if(txt.value=="") {
			$("#FundId").val(0);
		}
	}
	,showErrorMessage: function (frm) {
		var message='';
		$(".field-validation-error",frm).each(function () {
			if(this.innerHTML!='') {
				message+=this.innerHTML+"\n";
			}
		});
		if(message!="") {
			alert(message);
			return false;
		} else {
			return true;
		}
	}
	,onSubmit: function (formId) {
		var frm=document.getElementById(formId);
		Sys.Mvc.FormContext.getValidationForForm(frm).validate('submit');
		this.showErrorMessage(frm);
	}
}