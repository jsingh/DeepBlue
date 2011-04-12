var distribution={
	init: function () {
		/*$(document).ready(function () {
			setTimeout(function () {
				$("#CCDetail").hide();
			},200);
		});*/
	}
	,selectFund: function (id) {
		$("#SpnLoading").show();
		var dt=new Date();
		var url="/CapitalCall/FundDetail?id="+id+"&t="+dt.getTime();
		$("#lnkPCC").attr("href","#");
		$.getJSON(url,function (data) {
			$("#lnkPCC").attr("href","/CapitalCall/List/"+id);
			$("#lnkPCD").attr("href","/CapitalCall/CapitalDistributionList/"+id);
			$("#SpnLoading").hide();
			$("#CCDetail").show();
			$("#FundId").val(data.FundId);
			$("#TitleFundName").html(data.FundName);
			$("#SpnCommittedAmount").html(data.TotalCommitment);
			$("#CommittedAmount").val(jHelper.cfloat(data.TotalCommitment.replace("$","").replace(",","")));
			$("#UnfundedAmount").html(data.UnfundedAmount);
			$("#SpnDistributionNumber").html(data.DistributionNumber);
			$("#DistributionNumber").val(data.DistributionNumber);
		});
	}
	,onSubmit: function (formId) {
		var frm=document.getElementById(formId);
		var message='';
		Sys.Mvc.FormContext.getValidationForForm(frm).validate('submit');
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
		return true;
	}
	,onCreateCapitalCallBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateCapitalCallSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="") {
			alert(UpdateTargetId.html())
		} else {
			location.href="/CapitalCall/CapitalDistributionList";
		}
	}
	,showControl: function (chk,boxId) {
		var box=document.getElementById(boxId);
		if(box) {
			if(chk.checked)
				box.style.display="";
			else
				box.style.display="none";
		}
	}
	,calcProfit: function () {
		var DistributionAmount=jHelper.cfloat($("#DistributionAmount").val());
		var PreferredReturn=jHelper.cfloat($("#PreferredReturn").val());
		var ReturnManagementFees=jHelper.cfloat($("#ReturnManagementFees").val());
		var ReturnFundExpenses=jHelper.cfloat($("#ReturnFundExpenses").val());
		var PreferredCatchUp=jHelper.cfloat($("#PreferredCatchUp").val());
		var profit = (DistributionAmount-PreferredReturn-ReturnManagementFees-ReturnFundExpenses-PreferredCatchUp);
	}
}