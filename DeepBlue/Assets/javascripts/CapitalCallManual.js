var manualCapitalCall={
	calcCCA: function () {
		this.calc("txtCapitalAmountCalled","CapitalAmountCalled","SpnCapitalAmountCalled");
	}
	,calcMFIAmt: function () {
		this.calc("txtManagementFeeInterest","ManagementFeeInterest","SpnManagementFeeInterest");
	}
	,calcIAI: function () {
		this.calc("txtInvestedAmountInterest","InvestedAmountInterest","SpnInvestedAmountInterest");
	}
	,calcMF: function () {
		this.calc("txtManagementFees","ManagementFees","SpnManagementFees");
	}
	,calcFE: function () {
		this.calc("txtFundExpenses","FundExpenses","SpnFundExpenses");
	}
	,calc: function (txtid,hdnid,lblid) {
		var InvestorList=document.getElementById("InvestorList");
		var tbody=$("tbody",InvestorList);
		var total=0;
		$("tr",tbody).each(function () {
			var amt=parseFloat($("#"+txtid,this).val());
			if(isNaN(amt)) {
				amt=0;
			}
			total+=amt;
		});
		$("#"+hdnid,"#ManualCapitalCall").val(total);
		$("#"+lblid,"#ManualCapitalCall").html(jHelper.dollarAmount(total));
	}
	,checkInvestor: function (id,name,index) {
		var InvestorList=document.getElementById("InvestorList");
		var tbody=$("tbody",InvestorList);
		var result=true;
		$("tr",tbody).each(function () {
			var i=$("tr",tbody).index(this);
			if($("#InvestorId",this).val()==id&&i!=index) {
				result=false;
				alert(name+" is already chosen");
				return;
			}
		});
		return result;
	}
	,deleteInvestor: function (img) {
		if(confirm("Are you sure you want to delete this investor?")) {
			$(img).parents("tr:first").remove();
			this.calcCCA();this.calcMFIAmt();this.calcIAI();this.calcMF();this.calcFE();
		}
	}
	,save: function (frmid) {
		try {
			var frm=$("#"+frmid);
			var loading=$("#ManualUpdateLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			var param=$(frm).serializeForm();
			param[param.length]={ name: "FundId",value: $("#FundId").val() };
			param[param.length]={ name: "CapitalCallNumber",value: $("#CapitalCallNumber").val() };
			$.post("/CapitalCall/CreateManualCapitalCall",param,function (data) {
				loading.empty();
				var arr=data.split("||");
				if($.trim(arr[0])!="True") {
					alert(data);
				} else {
					alert("Manual Capital Call Saved.");
					$("#SpnCapitalCallNumber").html(arr[1]);
					$("#CapitalCallNumber").val(arr[1]);
					$("#SpnCapitalAmountCalled").html("");
					$("#SpnManagementFeeInterest").html("");
					$("#SpnInvestedAmountInterest").html("");
					$("#SpnFundExpenses").html("");
					$("#SpnManagementFees").html("");
					$("tbody","#InvestorList").empty();
					jHelper.resetFields(frm);
				}
			});
		} catch(e) {
			alert(e);
		}
		return false;
	}
	,addInvestor: function () {
		var target=$("tbody","#InvestorList");
		var investorCount=parseInt($("#InvestorCount").val());
		if(isNaN(investorCount)) {
			investorCount=1;
		} else {
			investorCount++;
		}
		$("#InvestorCount").val(investorCount);
		var data={ "Index": investorCount };
		$("#CapitalCallInvestorTemplate").tmpl(data).prependTo(target);
		var tr=$("tr:first",target);
		$("#Investor",tr).autocomplete({ source: function (request,response) {
			$.getJSON("/Investor/FindInvestors"+"?term="+request.term+"&fundId="+$("#FundId").val(),function (data) {
				response(data);
			});
		}
		 ,minLength: 1,select: function (event,ui) {
		 	var index=$("tr",target).index(tr);
		 	if(manualCapitalCall.checkInvestor(ui.item.id,ui.item.value,index)) {
		 		$("#InvestorId",tr).val(ui.item.id);
		 	} else {
		 		$("#InvestorId",tr).val(0);
		 		setTimeout(function () {
		 			$("#Investor",tr).val("");
		 		},100);
		 	}
		 },appendTo: "body",delay: 300
		});
		jHelper.applyGridClass(target);
	}
}