var manualCapitalCall={
	init: function () {
		$(document).ready(function () {
		/*	setTimeout(function () {
				$("#CCDetail").hide();
				$("#InvestorDetail").hide();
			},200); */
			$("#Investor").focus(function () {
				this.value="";
			});
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
			$("#SpnCapitalCallNumber").html(data.CapitalCallNumber)
		});
	}
	,selectInvestor: function (id,name) {
		if(this.checkInvestor(id,name)) {
			$("#InvestorDetail").show();
			var InvestorList=document.getElementById("InvestorList");
			var tbody=$("tbody",InvestorList);
			var rows=$("tr",tbody).length;
			var tr;
			var trFirst=$("tr:first",tbody);
			var investorCount=parseInt($("#InvestorCount").val());
			if(isNaN(investorCount)) {
				investorCount=1;
			} else {
				investorCount++;
			}
			if(parseInt($("#InvestorId",tr).val())=="0") {
				tr=trFirst;
			} else {
				tr=document.createElement("tr");
				$(tbody).append(tr);
				$("td",trFirst).each(function () {
					var td=document.createElement("td");
					td.innerHTML=this.innerHTML.replace(/1_/g,(investorCount)+"_");
					$(tr).append(td);
				});
			}
			$("#InvestorCount").val(investorCount);
			$(":input",tr).val("");
			$("#InvestorId",tr).val(id);
			$("#SpnInvestorName",tr).html(name);
			if((rows+1)%2==0)
				tr.className="erow";
			var lastRow=$("tr:last",InvestorList);
			$("#txtCapitalAmountCalled",lastRow).get(0).focus();
			$("#Investor").blur();
		}
	}
	,calcCCA: function () {
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
		$("#"+hdnid).val(total);
		$("#"+lblid).html("$"+total.toFixed(2));
	}
	,checkInvestor: function (id,name) {
		var InvestorList=document.getElementById("InvestorList");
		var tbody=$("tbody",InvestorList);
		var result=true;
		$("tr",tbody).each(function () {
			if($("#InvestorId",this).val()==id) {
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
			location.href="/CapitalCall/NewManualCapitalCall";
		}
	}
}