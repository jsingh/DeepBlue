var capitalCall={
	init: function () {
		$(document).ready(function () {
			jHelper.waterMark();
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
			$("#lnkPCC").attr("href","/CapitalCall/Detail?fundId="+id+"&typeId=1");
			$("#SpnLoading").hide();
			$("#CCDetail").show();
			$("#FundId").val(data.FundId);
			$("#Fund").val(data.FundName);
			$("#TitleFundName").html(data.FundName);
			$("#CommittedAmount").html(jHelper.dollarAmount(data.TotalCommitment));
			$("#UnfundedAmount").html(jHelper.dollarAmount(data.UnfundedAmount));
			$("#CapitalCallNumber").val(data.CapitalCallNumber);
			$("#SpnCapitalCallNumber").html(data.CapitalCallNumber);
		});
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
				alert("To Date must be greater than From Date.");
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
		capitalCall.calcExistingInvestmentAmount();
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
			$("#TierDetail").flexAddData(data.Tiers);
			capitalCall.calcExistingInvestmentAmount();
		});
	}
	,showDetail: function (img) {
		$("#TierDetailMain").dialog("open");
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
			var param=$(frm).serializeArray();
			param[param.length]={ name: "FundId",value: $("#FundId").val() };
			param[param.length]={ name: "CapitalCallNumber",value: $("#CapitalCallNumber").val() };
			$.post("/CapitalCall/Create",param,function (data) {
				loading.empty();
				var arr=data.split("||");
				if($.trim(arr[0])!="True") {
					alert(data);
				} else {
					alert("Capital Call Saved.");
					$("#SpnCapitalCallNumber").html(arr[1]);
					$("#CapitalCallNumber").val(arr[1]);
					$("#SpnMFA").html("");
					$("#SpnExistingInvestmentAmount").html("");
					var chkmfee=$("#AddManagementFees").get(0);
					if(chkmfee) {
						chkmfee.checked=false;
						capitalCall.selectMFee(chkmfee);
					}
					var chkae=$("#AddFundExpenses").get(0);
					if(chkae) {
						chkae.checked=false;
						capitalCall.selectFundExp(chkae);
					}
					jHelper.resetFields(frm);
				}
			});
		} catch(e) {
			alert(e);
		}
		return false;
	}
}