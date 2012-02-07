dealActivity.makeNewCC=function () {
	var ufid=dealActivity.getCCUnderlyingFund();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		jAlert("Underlying Fund is required");
	} else {
		dealActivity.loadCC(false);
	}
};
dealActivity.expandMCCTree=function (index,img) {
	var display="";
	if(img.src.indexOf('treeplus')>0) {
		display="";
		img.src=img.src.replace("treeplus.gif","treeminus.gif");
	} else {
		display="none";
		img.src=img.src.replace("treeminus.gif","treeplus.gif");
	}
	$("#ManualUFCC_Deal_"+index).css("display",display);
};
dealActivity.setCCUnderlyingFund=function (id,name) {
	$("#CCUnderlyingFundId").val(id);
	$("#SpnCCUFName").html(name);
	$("tbody","#CapitalCallList").empty();
	$("tbody","#PRCapitalCallList").empty();
	$("#CCDetail").attr("issearch","true").show();
	dealActivity.loadCC(true);
};
dealActivity.loadCC=function (isRefresh) {
	var tbl=$("#CapitalCallList");
	var loading=$("#CCLoading");
	$("#CapitalCall").show();
	$("#PRCapitalCall").show();
	$("#PRCCListBox").hide();
	var target=$("tbody",tbl);
	var rowsLength=$("tr",target).length;
	if(rowsLength==0) { isRefresh=true; }
	if(isRefresh) {
		target.empty();
		$("#CapitalCall").hide();
		loading.html(jHelper.loadingHTML());
		$.getJSON("/Deal/UnderlyingFundCapitalCallList",{ "_": (new Date).getTime()
			,"underlyingFundId": dealActivity.getCCUnderlyingFund()
		},function (data) {
			loading.empty();
			$.each(data,function (i,item) {
				item["Index"]=i;
				$("#CapitalCallAddTemplate").tmpl(item).appendTo(target);
			});
			dealActivity.setUpRow($("tr",target));
			jHelper.jqCheckBox(target);
			rowsLength=$("tr",target).length;
			if(rowsLength>0) { $("#CapitalCall").show(); }
			$("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
			$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
			$(".mcc",target).removeAttr("class");
			dealActivity.showManualCCCtl();
		});
	}
};
dealActivity.getCCUnderlyingFund=function (id) {
	return parseInt($("#CCUnderlyingFundId").val());
};
dealActivity.submitUFCapitalCall=function (frm) {
	try {
		var param=$(frm).serializeForm();
		var loading=$("#SpnCCSaveLoading");
		loading.html(jHelper.savingHTML());
		var chk=$("#IsManualCapitalCall").get(0);
		var totalRows=($("tbody tr","#CapitalCallList").length);
		var isManual=chk.checked;
		var isError=false;
		if(isError==false) {
			param[param.length]={ name: "TotalRows",value: totalRows };
			param[param.length]={ name: "IsManualCapitalCall",value: isManual };
			$.post("/Deal/CreateUnderlyingFundCapitalCall",param,function (data) {
				loading.empty();
				if($.trim(data)!="") {
					dealActivity.processErrMsg(data,frm);
				} else {
					jAlert("Capital Calls Saved");
					//dealActivity.resetCapitalCall();
				}
			});
		}
	} catch(e) { jAlert(e); }
	return false;
};
dealActivity.resetCapitalCall=function () {
	$("#CCDetail").hide();
	$("#CapitalCall").hide();
	$("#PRCapitalCall").hide();
	$("tbody","#CapitalCallList").empty();
	$("#CC_UnderlyingFund").val("");
	$("#SpnCCUFName").empty();
	$("#CCUnderlyingFundId").val(0);
	$("#CC_UnderlyingFund").focus();
};
dealActivity.showManualCCCtl=function () {
	var chk=$("#IsManualCapitalCall").get(0);
	var display="";
	if(chk.checked) { display=""; } else { display="none"; }
	$(".ismanual","#CapitalCallList").css("display",display);
};