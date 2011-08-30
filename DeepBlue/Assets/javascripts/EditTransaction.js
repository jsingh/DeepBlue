var editTransaction={
	init: function () {
		/*var transactionTypeId=$("#TransactionTypeId").val();
		var splitdetail=document.getElementById("splitdetail");
		if(parseInt(transactionTypeId)==5) {
		splitdetail.style.display="block";
		}*/
		editTransaction.resizeIframe();
	}
	,editCA: function (frm) {
		try {
			var loading=$("#UpdateEditCmtLoading","#EditCommitmentAmount");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.post("/Transaction/UpdateCommitmentAmount",$(frm).serializeArray(),function (data) {
				loading.empty();
				if($.trim(data)!="") {
					jAlert(data);
				} else {
					jAlert("Committed Amount Saved");
					jHelper.resetFields(frm);
					parent.transactionController.closeEditTransactionDialog(true);
				}
			});
		} catch(e) {
			jAlert(e);
		}
		return false;
	}
	,save: function (img) {
		try {
			var frm=$(img).parents("form:first");
			var loading=$("#UpdateLoading","#EditTransaction");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.post("/Transaction/CreateFundTransaction",$(frm).serializeArray(),function (data) {
				loading.empty();
				if($.trim(data)!="") {
					jAlert(data);
				} else {
					jAlert("Transaction Saved");
					transactionController.loadFundDetails();
					$("#EditTransaction").dialog("close");
				}
			});
		} catch(e) {
			jAlert(e);
		}
		return false;
	}
	,resizeIframe: function () {
		$("document").ready(function () {
			var theFrame=$("#iframe_modal",parent.document.body);
			if(theFrame) {
				theFrame.height($("body").height()+100);
			}
		});
	}
	,onClickTransactionType: function (rdo) {
		var splitdetail=document.getElementById("splitdetail");
		var CommitmentAmount=document.getElementById("CommitmentAmount");
		splitdetail.style.display="none";
		CommitmentAmount.innerHTML="Commitment Amount";
		if(rdo.checked) {
			switch(rdo.value) {
				case "5": // Split transction type
					splitdetail.style.display="block";
					CommitmentAmount.innerHTML="New Commitment Amount";
					break;
			}
		}
		editTransaction.resizeIframe();
	}
	,selectInvestor: function (id) {
		$("#CounterPartyInvestorId").val(id);
		editTransaction.loadInvestorType(id);
	}
	,onInvestorBlur: function (txt) {
		if(txt.value=="") {
			$("#CounterPartyInvestorId").val(0);
			editTransaction.loadInvestorType(0);
		}
	}
	,onBegin: function () {
		$("#UpdateEditCmtLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Save...");
	}
	,closeDialog: function (reload) {
		parent.transactionController.closeEditTransactionDialog(reload);
	}
	,closeEditCommitAmtDialog: function () {
		$("#EditCommitmentAmount").dialog('close');
		if($("#UpdateTargetId").html()=="True") {
			transactionController.loadFundDetails();
		}
		$("#UpdateTargetId").html("");
	}
	,onEditCommitAmgSubmit: function (formId) {
		var frm=document.getElementById(formId);
		Sys.Mvc.FormContext.getValidationForForm(frm).validate('submit');
		var message='';
		$(".field-validation-error",frm).each(function () {
			if(this.innerHTML!='') {
				message+=this.innerHTML+"\n";
			}
		});
		if(message!="") {
			jAlert(message);
			return false;
		} else {
			return true;
		}
		return true;
	}
	,onTransactionBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Save...");
	}
	,onTransactionSuccess: function () {
		$("#UpdateLoading").html("");
		if($("#UpdateTargetId").html()=="True") {
			editTransaction.closeDialog(true);
		} else {
			jAlert($("#UpdateTargetId").html());
		}
		$("#UpdateTargetId").html("");
	}
	,onSubmit: function (formId) {
		var frm=document.getElementById(formId);
		Sys.Mvc.FormContext.getValidationForForm(frm).validate('submit');
		var message='';
		$(".field-validation-error",frm).each(function () {
			if(this.innerHTML!='') {
				message+=this.innerHTML+"\n";
			}
		});
		var UnfundedAmount=parseFloat($("#UnfundedAmount",frm).val());
		var CommitmentAmount=parseFloat($("#CommitmentAmount",frm).val());
		if(isNaN(UnfundedAmount)) {
			UnfundedAmount=0;
		}
		if(isNaN(CommitmentAmount)) {
			CommitmentAmount=0;
		}
		if(CommitmentAmount>UnfundedAmount) {
			message+="Transaction Amount should be less than Unfunded Commitment Amount\n";
		}
		if(message!="") {
			jAlert(message);
			return false;
		} else {
			return true;
		}
		return true;
	}
	,checkInputValid: function (input) {
		var editorfield=input.parent();
		var customvalidation=$(".custom-validation",editorfield).get(0);
		if(!customvalidation) {
			customvalidation=document.createElement("span");
			editorfield.append(customvalidation);
		}
		customvalidation.className="custom-validation";
		customvalidation.innerHTML="Required";
		$(input).focus(function () {
			customvalidation.innerHTML="";
		});
		return true;
	}
}