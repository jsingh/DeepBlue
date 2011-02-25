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
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Save...");
	}
	,closeDialog: function () {
		parent.transactionController.closeEditTransactionDialog();
	}
	,onSubmit: function (frm) {
		var Split=document.getElementById("Split");
		if(Split.checked) {
			var OtherInvestorId=document.getElementById("OtherInvestorId").value;
			var OtherInvestor=document.getElementById("OtherInvestorName");
			var OICA=document.getElementById("OtherInvestorCommitmentAmount");
			if(parseFloat(OICA.value)<=0) {
				editTransaction.checkInputValid($(OICA));
				return false;
			}
			if(parseInt(OtherInvestorId)<=0) {
				editTransaction.checkInputValid($(OtherInvestor));
				return false;
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