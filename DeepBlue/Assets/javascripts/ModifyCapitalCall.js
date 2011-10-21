var modifyCapitalCall={
	init: function () {
		$(document).ready(function () {
			jHelper.waterMark();
			$("#TierDetailMain").dialog({
				title: "Rate Schedule Detail",
				autoOpen: false,
				width: 700,
				modal: true,
				position: 'top',
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
			$("#lnkPCC").attr("href","/CapitalCall/Detail?fundId="+id+"&typeId=1");
			$("#SpnLoading").hide();
			$("#CCDetail").show();
			$("#FundId").val(data.FundId);
			$("#Fund").val(data.FundName);
			$("#TitleFundName").html(data.FundName);
			$("#CommittedAmount").html(jHelper.dollarAmount(data.TotalCommitment));
			$("#UnfundedAmount").html(jHelper.dollarAmount(data.UnfundedAmount));
			$("#CapitalCallNumber").val(data.CapitalCallNumber);
			$(".ccnumber").html(data.CapitalCallNumber);
		});
	}
	,selectMFee: function (chk) {
		var spnAddMF=$("#SpnAddManagementFee");
		if(chk.checked) {
			$("#ManFeeMain").show();
			spnAddMF.html("Management Fees");
		} else {
			$("#ManFeeMain").hide();
			spnAddMF.html("Add Management Fees");
		}
	}
	,selectFundExp: function (chk) {
		var spnAddFE=$("#SpnAddFundExpenses");
		if(chk.checked) {
			$("#FunExpAmount").show();
			spnAddFE.html("Fund Expenses");
		} else {
			$("#FunExpAmount").hide();
			spnAddFE.html("Add Fund Expenses");
		}
	}
	,changeFromDate: function () {
		var fromDate=$("#FromDate").val();
		var toDate=$("#ToDate").val();
		this.resetManFee();
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
		this.resetManFee();
		if(fromDate!=""&&toDate!="") {
			var diff=Date.DateDiff("d",fromDate,toDate);
			if(diff<=0) {
				jAlert("To Date must be greater than From Date.");
				$("#ToDate").val("");
			} else {
				this.calcManFee();
			}
		}
	}
	,resetManFee: function () {
		$("#SpnMFA","#CapitalCall").html("");
		$("#SpnDetail","#CapitalCall").show();
		$("#ManagementFees","#CapitalCall").val(0);
		modifyCapitalCall.calcExistingInvestmentAmount();
	}
	,calcManFee: function () {
		this.resetManFee();
		var fundId=$("#FundId").val();
		var fromDate=$("#FromDate").val();
		var toDate=$("#ToDate").val();
		var diff=Date.DateDiff("d",fromDate,toDate);
		/* Check 3 months  */
		//var endDate=$.datepicker.formatDate('mm/dd/yy',Date.DateAdd("m",3,fromDate));
		var dt=new Date();
		var url="/CapitalCall/CalculateManagementFee/?fundId="+fundId+"&startDate="+fromDate+"&endDate="+toDate+"&t="+dt.getTime();
		$("#SpnMFA","#CapitalCall").html("<img src='/Assets/images/ajax.jpg'>&nbsp;Calculating...")
		$("#SpnDetail","#CapitalCall").hide();
		$.getJSON(url,function (data) {
			$("#SpnMFA","#CapitalCall").html(jHelper.dollarAmount(data.ManagementFee));
			$("#SpnDetail","#CapitalCall").show();
			$("#ManagementFees","#CapitalCall").val(data.ManagementFee.toFixed(2));
			var target=$("tbody","#TierDetail");
			target.empty();
			$("#TierDetailTemplate").tmpl(data).appendTo(target);
			modifyCapitalCall.calcExistingInvestmentAmount();
		});
	}
	,showDetail: function (img) {
		$("#TierDetailMain").dialog("open");
	}
	,edit: function (img) {
		var tr=$(img).parents("tr:first");
		this.editRow(tr);
		$("#Save",tr).show();
	}
	,editRow: function (tr) {
		$(".show",tr).hide();
		$(".hide",tr).show();
		$(":input:first",tr).focus();
	}
	,calcExistingInvestmentAmount: function () {
		var newInvestmentAmount=parseFloat($("#NewInvestmentAmount","#CapitalCall").val());
		var capitalAmountCalled=parseFloat($("#CapitalAmountCalled","#CapitalCall").val());
		if(isNaN(newInvestmentAmount)) { newInvestmentAmount=0; }
		if(isNaN(capitalAmountCalled)) { capitalAmountCalled=0; }
		var fundExpenseAmount=parseFloat($("#FundExpenseAmount","#CapitalCall").val());
		if(isNaN(fundExpenseAmount)) { fundExpenseAmount=0; }
		var fundExpenseAmount=parseFloat($("#FundExpenseAmount","#CapitalCall").val());
		if(isNaN(fundExpenseAmount)) { fundExpenseAmount=0; }
		var managementFees=parseFloat($("#ManagementFees","#CapitalCall").val());
		if(isNaN(managementFees)) { managementFees=0; }
		var existingInvAmount=((capitalAmountCalled-fundExpenseAmount-managementFees)-newInvestmentAmount);
		if(existingInvAmount<=0) { existingInvAmount=0; }
		$("#ExistingInvestmentAmount","#CapitalCall").val(existingInvAmount);
		$("#SpnExistingInvestmentAmount","#CapitalCall").html(jHelper.dollarAmount(existingInvAmount.toString()));
	}
	,selectTab: function (type,lnk) {
		var CC=$("#NewCapitalCall");
		var MCC=$("#NewManualCapitalCall");
		$("#NewCCTab").removeClass("section-tab-sel");
		$("#ManCCTab").removeClass("section-tab-sel");
		CC.hide();MCC.hide();
		$(lnk).addClass("section-tab-sel");
		switch(type) {
			case "C": CC.show();break;
			case "M": MCC.show();break;
		}
	}
	,save: function (frmid) {
		try {
			var frm=$("#"+frmid);
			var loading=$("#UpdateLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			var param=$(frm).serializeForm();
			param[param.length]={ name: "FundId",value: $("#FundId").val() };
			param[param.length]={ name: "CapitalCallNumber",value: $("#CapitalCallNumber").val() };
			$.post("/CapitalCall/UpdateCapitalCall",param,function (data) {
				loading.empty();
				if($.trim(data)!="True") {
					jAlert(data);
				} else {
					jAlert("Capital Call Saved.");
				}
			});
		} catch(e) {
			jAlert(e);
		}
		return false;
	}
}