dealActivity.makeManualNewCD=function () {
	var ufid=dealActivity.getManualCDUnderlyingFundId();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		alert("Underlying Fund is required");
	} else {
		dealActivity.loadManualCD(false);
	}
};
dealActivity.setManualCDUnderlyingFund=function (id,name) {
	$("#ManualCDUnderlyingFundId").val(id);
	$("#SpnManualCDUFName").html(name);
	$("tbody","#ManualCashDistributionList").empty();
	$("tbody","#ManualPRCashDistributionList").empty();
	$("#ManualCDDetail").attr("issearch","true").show();
	dealActivity.loadManualCD(true);
};
dealActivity.loadManualCD=function (isRefresh) {
	var loading=$("#ManualCDLoading");
	var tbl=$("#ManualCashDistributionList");
	$("#ManualCashDistribution").show();
	$("#ManualPRCDListBox").hide();
	var target=$("tbody",tbl);
	var rowsLength=$("tr",target).length;
	if(rowsLength==0) { isRefresh=true; }
	if(isRefresh) {
		target.empty();
		$("#ManualCashDistribution").hide();
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON("/Deal/UnderlyingFundManualCashDistributionList",{ "_": (new Date).getTime(),"underlyingFundId": dealActivity.getManualCDUnderlyingFundId() },function (data) {
			$("#ManualPRCashDistribution").show();
			loading.empty();
			$.each(data,function (i,item) {
				item["Index"]=i;
				data["IsManualCapitalCall"]=true;
				$("#ManualCashDistributionAddTemplate").tmpl(item).appendTo(target);
			});
			dealActivity.setUpRow($("tr",target));
			rowsLength=$("tr",target).length;
			if(rowsLength>0) { $("#ManualCashDistribution").show(); }
			$("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
			$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
			$(".mcc",target).removeAttr("class");
		});
	}
};
dealActivity.getManualCDUnderlyingFundId=function (id) {
	return parseInt($("#ManualCDUnderlyingFundId").val());
};
dealActivity.submitManualUFCashDistribution=function (frm) {
	try {
		var param=$(frm).serializeArray();
		var loading=$("#SpnManualCDSaveLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		param[param.length]={ name: "TotalRows",value: ($("tbody tr","#ManualCashDistributionList").length) };
		$.post("/Deal/CreateUnderlyingFundCashDistribution",param,function (data) {
			loading.empty();
			if($.trim(data)!="") { dealActivity.processErrMsg(data,frm); } else {
				alert("Manual Cash Distributions Saved");
				dealActivity.resetManualCashDistribution();
			}
		});
	} catch(e) { alert(e); }
	return false;
};
dealActivity.resetManualCashDistribution=function () {
	$("#ManualCashDistribution").hide();
	$("#ManualPRCashDistribution").hide();
	$("tbody","#ManualCashDistributionList").empty();
	$("#ManualCD_UnderlyingFund").val("");
	$("#SpnManualCDUFName").empty();
	$("#ManualCDUnderlyingFundId").val(0);
	$("#ManualCD_UnderlyingFund").focus();
};
dealActivity.expandMCDTree=function (index,that) {
	if(that.src.indexOf("minus")>0) {
		that.src="/Assets/images/treeplus.gif";
		$("#ManualUFCD_Deal_"+index).hide();
	} else {
		that.src="/Assets/images/treeminus.gif";
		$("#ManualUFCD_Deal_"+index).show();
	}
};