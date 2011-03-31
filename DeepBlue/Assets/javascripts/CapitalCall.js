var capitalCall={
	init: function () {
		$(document).ready(function () {
			$("#TierDetailMain").dialog({
				title: "Rate Schedule Detail",
				autoOpen: false,
				width: 625,
				modal: true,
				position: 'middle',
				autoResize: false
			});
			/*setTimeout(function () {
				$("#CCDetail").hide();
			},200);*/
		});
	}
	,selectFund: function (id) {
		$("#SpnLoading").show();
		var dt=new Date();
		var url="/CapitalCall/FundDetail?id="+id+"&t="+dt.getTime();
		$("#lnkPCC").attr("href","#");
		$.getJSON(url,function (data) {
			$("#lnkPCC").attr("href","/CapitalCall/List/"+id);
			$("#SpnLoading").hide();
			$("#CCDetail").show();
			$("#FundId").val(data.FundId);
			$("#TitleFundName").html(data.FundName);
			$("#CommittedAmount").html(data.TotalCommitment);
			$("#UnfundedAmount").html(data.UnfundedAmount);
			$("#CapitalCallNumber").val(data.CapitalCallNumber);
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
	,selectMFee: function (chk) {
		if(chk.checked)
			$("#ManFeeMain").show();
		else
			$("#ManFeeMain").hide();
	}
	,selectFundExp: function (chk) {
		if(chk.checked)
			$("#FunExpAmount").show();
		else
			$("#FunExpAmount").hide();
	}
	,changeFromDate: function () {
		var fromDate=$("#FromDate").val();
		var toDate=$("#ToDate").val();
		if(fromDate!=""&&toDate!="") {
			var diff=Date.DateDiff("d",fromDate,toDate);
			if(diff<=0) {
				$("#ToDate").val("");
			} else {
				this.calcManFee();
			}
		}
	}
	,changeToDate: function () {
		var fromDate=$("#FromDate").val();
		var toDate=$("#ToDate").val();
		if(fromDate!=""&&toDate!="") {
			var diff=Date.DateDiff("d",fromDate,toDate);
			if(diff<=0) {
				alert("To Date must be greater than From Date.");
				$("#ToDate").val("");
			} else {
				this.calcManFee();
			}
		}
	}
	,calcManFee: function () {
		var fundId=$("#FundId").val();
		var fromDate=$("#FromDate").val();
		var toDate=$("#ToDate").val();
		var diff=Date.DateDiff("d",fromDate,toDate);
		/* Check 3 months  */
		//var endDate=$.datepicker.formatDate('mm/dd/yy',Date.DateAdd("m",3,fromDate));
		var dt=new Date();
		var url="/CapitalCall/CalculateManagementFee/?fundId="+fundId+"&startDate="+fromDate+"&endDate="+toDate+"&t="+dt.getTime();
		$("#SpnMFA").html("<img src='/Assets/images/ajax.jpg'>&nbsp;Calculating...")
		$("#SpnDetail").hide();
		$.getJSON(url,function (data) {
			$("#SpnMFA").html("$"+data.ManagementFee.toFixed(2));
			$("#SpnDetail").show();
			$("#ManagementFees").val(data.ManagementFee.toFixed(2));
			$("#TierDetail").flexAddData(data.Tiers);
		});
	}
	,showDetail: function (img) {
		$("#TierDetailMain").dialog("open");
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
			location.href="/CapitalCall/New";
		}
	}
	
}