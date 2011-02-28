var editTransaction={
	init: function () {
		var transactionTypeId=$("#TransactionTypeId").val();
		var splitdetail=document.getElementById("splitdetail");
		if(parseInt(transactionTypeId)==5) {
			splitdetail.style.display="block";
		}
		editTransaction.resizeIframe();
	}
	,resizeIframe: function () {
		$("document").ready(function () {
			var theFrame=$("#iframe_modal",parent.document.body);
			if(theFrame) {
				theFrame.height($(".transaction-edit").height()+30);
			}
		});
	}
	,onClickTransactionType: function (rdo) {
		var splitdetail=document.getElementById("splitdetail");
		var CommitmentAmount=document.getElementById("CommitmentAmount");
		splitdetail.style.display="none";
		CommitmentAmount.innerHTML="Commitment Amount:";
		if(rdo.checked) {
			switch(rdo.value) {
				case "5": // Split transction type
					splitdetail.style.display="block";
					CommitmentAmount.innerHTML="New Commitment Amount:";
					break;
			}
		}
		editTransaction.resizeIframe();
	}
	,selectInvestor: function (id) {
		$("#OtherInvestorId").val(id);
	}
	,onBegin: function () {
		$("#UpdateEditCmtLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Save...");
	}
	,closeDialog: function () {
		var reload=false;
		if($("#UpdateTargetId").html()=="True")
			reload=true;
		parent.transactionController.closeEditTransactionDialog(reload);
	}
	,closeEditCommitAmtDialog: function () {
		$("#EditCommitmentAmount").dialog('close');
		if($("#UpdateTargetId").html()=="True") {
			transactionController.loadFundDetails();
		}
		$("#UpdateTargetId").html("");
	}
	,onSubmit: function () {
		var frm=document.getElementById("EditTransaction");
		var Split=document.getElementById("Split");
		if(Split) {
			if(Split.checked) {
				var OtherInvestorId=document.getElementById("OtherInvestorId").value;
				var OtherInvestor=document.getElementById("OtherInvestorName");
				var OICA=document.getElementById("OtherInvestorCommitmentAmount");
				if(parseInt(OtherInvestorId)<=0||isNaN(parseInt(OtherInvestorId))) {
					editTransaction.checkInputValid($(OtherInvestor));
					return false;
				}
				if(parseFloat(OICA.value)<=0||isNaN(parseInt(OICA.value))) {
					editTransaction.checkInputValid($(OICA));
					return false;
				}
			}
		}
		return true;
	},checkInputValid: function (input) {
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