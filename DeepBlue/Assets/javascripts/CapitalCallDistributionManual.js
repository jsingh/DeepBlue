﻿var manualDistribution={
	calcCDA: function () {
		this.calc("txtDistributionAmount","DistributionAmount","SpnDistributionAmount");
	}
	,calcRMFAmt: function () {
		this.calc("txtReturnManagementFees","ReturnManagementFees","SpnReturnManagementFees");
	}
	,calcRFE: function () {
		this.calc("txtReturnFundExpenses","ReturnFundExpenses","SpnReturnFundExpenses");
	}
	,calcGP: function (txt) {
		var p=jHelper.cFloat(txt.value);
		if(isNaN(p)) { p=0; }
		if(p>100) {
			jAlert("Profit must be under 100%.");
			txt.value="";
		}
		this.calc("txtGPProfits","GPProfits","SpnGPProfits");
	}
	,calcPR: function () {
		this.calc("txtPreferredReturn","PreferredReturn","SpnPreferredReturn");
	}
	,calcCR: function () {
		this.calc("txtCapitalReturn","CapitalReturn","SpnCapitalReturn");
	}
	,calc: function (txtid,hdnid,lblid) {
		var InvestorList=document.getElementById("InvestorList");
		var tbody=$("tbody",InvestorList);
		var total=0;
		$("tr",tbody).each(function () {
			var amt=jHelper.cFloat($("#"+txtid,this).val());
			if(isNaN(amt)) {
				amt=0;
			}
			total+=amt;
		});
		$("#"+hdnid,"#ManualDistribution").val(total);
		$("#"+lblid,"#ManualDistribution").html(jHelper.dollarAmount(total));
	}
	,checkInvestor: function (id,name,index) {
		var InvestorList=document.getElementById("InvestorList");
		var tbody=$("tbody",InvestorList);
		var result=true;
		$("tr",tbody).each(function () {
			var i=$("tr",tbody).index(this);
			if($("#InvestorId",this).val()==id&&i!=index) {
				result=false;
				jAlert(name+" is already chosen");
				return;
			}
		});
		return result;
	}
	,deleteInvestor: function (img) {
		if(confirm("Are you sure you want to delete this investor?")) {
			$(img).parents("tr:first").remove();
			this.calcCDA();this.calcRMFAmt();this.calcIAI();this.calcGP();this.calcPR();
		}
	}
	,save: function (frmid) {
		try {
			var frm=$("#"+frmid);
			var loading=$("#ManualUpdateLoading");
			loading.html(jHelper.savingHTML());
			var param=$(frm).serializeForm();
			param[param.length]={ name: "FundId",value: $("#FundId").val() };
			param[param.length]={ name: "DistributionNumber",value: $("#DistributionNumber").val() };
			$.post("/CapitalCall/CreateManualDistribution",param,function (data) {
				loading.empty();
				var arr=data.split("||");
				if($.trim(arr[0])!="True") {
					jAlert(data);
				} else {
					jAlert("Manal Capital Distribution Saved.");
					$("#SpnDistributionNumber").html(arr[1]);
					$("#DistributionNumber").val(arr[1]);
					$("#SpnManualDistributionNumber").html(arr[1]);
					$("#SpnDistributionAmount").html("");
					$("#SpnReturnManagementFees").html("");
					$("#SpnReturnFundExpenses").html("");
					$("#SpnPreferredReturn").html("");
					jHelper.resetFields(frm);
				}
			});
		} catch(e) {
			jAlert(e);
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
		$("tr",target).each(function () { $(this).removeAttr("id"); });
		$("#CapitalDistributionInvestorTemplate").tmpl(data).prependTo(target);
		var tr=$("tr:first",target);
		tr.attr("id","Row0");
		$("#Investor",tr).autocomplete({ source: function (request,response) {
			$.getJSON("/Investor/FindInvestors"+"?term="+request.term+"&fundId="+$("#FundId").val(),function (data) {
				response(data);
			});
		},minLength: 1,select: function (event,ui) {
			var index=$("tr",target).index(tr);
			if(manualDistribution.checkInvestor(ui.item.id,ui.item.value,index)) {
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