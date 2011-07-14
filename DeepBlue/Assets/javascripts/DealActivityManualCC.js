﻿dealActivity.makeManualNewCC=function () {
	var ufid=dealActivity.getManualCCUnderlyingFund();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		alert("Underlying Fund is required");
	} else {
		dealActivity.loadManualCC(false);
	}
};
dealActivity.setManualCCUnderlyingFund=function (id,name) {
	$("#ManualCCUnderlyingFundId").val(id);
	$("#SpnManualCCUFName").html(name);
	$("tbody","#ManualCapitalCallList").empty();
	$("tbody","#ManualPRCapitalCallList").empty();
	$("#ManualCCDetail").attr("issearch","true").show();
	dealActivity.loadManualCC(true);
};
dealActivity.loadManualCC=function (isRefresh) {
	var tbl=$("#ManualCapitalCallList");
	var loading=$("#CCLoading");
	$("#ManualCapitalCall").show();
	$("#ManualPRCapitalCall").show();
	$("#ManualPRCCListBox").hide();
	var target=$("tbody",tbl);
	var rowsLength=$("tr",target).length;
	if(rowsLength==0) { isRefresh=true; }
	if(isRefresh) {
		target.empty();
		$("#ManualCapitalCall").hide();
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON("/Deal/UnderlyingFundManualCapitalCallList",{ "_": (new Date).getTime()
			,"underlyingFundId": dealActivity.getManualCCUnderlyingFund()
		},function (data) {
			loading.empty();
			$.each(data,function (i,item) {
				item["Index"]=i;
				data["IsManualCapitalCall"]=true;
				$("#ManualCapitalCallAddTemplate").tmpl(item).appendTo(target);
			});
			dealActivity.setUpRow($("tr",target));
			rowsLength=$("tr",target).length;
			if(rowsLength>0) { $("#ManualCapitalCall").show(); }
			$("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
			$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
			$(".mcc",target).removeAttr("class");
		});
	}
};
dealActivity.getManualCCUnderlyingFund=function (id) {
	return parseInt($("#ManualCCUnderlyingFundId").val());
};
dealActivity.submitManualUFCapitalCall=function (frm) {
	try {
		var param=$(frm).serializeArray();
		var loading=$("#SpnManualCCSaveLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		param[param.length]={ name: "TotalRows",value: ($("tbody tr","#ManualCapitalCallList").length) };
		$.post("/Deal/CreateUnderlyingFundCapitalCall",param,function (data) {
			loading.empty();
			if($.trim(data)!="") {
				dealActivity.processErrMsg(data,frm);
			} else {
				alert("Manual Capital Calls Saved");
				dealActivity.resetManualCapitalCall();
			}
		});
	} catch(e) { alert(e); }
	return false;
};
dealActivity.resetManualCapitalCall=function () {
	$("#ManualCapitalCall").hide();
	$("#ManualPRCapitalCall").hide();
	$("tbody","#ManualCapitalCallList").empty();
	$("#ManualCC_UnderlyingFund").val("");
	$("#SpnManualCCUFName").empty();
	$("#ManualCCUnderlyingFundId").val(0);
	$("#ManualCC_UnderlyingFund").focus();
};
dealActivity.expandMCCTree=function (index,that) {
	if(that.src.indexOf("minus")>0) {
		that.src="/Assets/images/treeplus.gif";
		$("#ManualUFCC_Deal_"+index).hide();
	} else {
		that.src="/Assets/images/treeminus.gif";
		$("#ManualUFCC_Deal_"+index).show();
	}
};