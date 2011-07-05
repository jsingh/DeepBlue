dealActivity.makeNewCD=function () {
	var ufid=dealActivity.getCDUnderlyingFundId();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		alert("Underlying Fund is required");
	} else {
		dealActivity.loadCD(false);
	}
};
dealActivity.expandMCDTree=function (index,img) {
	var display="";
	if(img.src.indexOf('treeplus')>0) {
		display="";
		img.src=img.src.replace("treeplus.gif","treeminus.gif");
	} else {
		display="none";
		img.src=img.src.replace("treeminus.gif","treeplus.gif");
	}
	$("#ManualUFCD_Deal_"+index).css("display",display);
};
dealActivity.setCDUnderlyingFund=function (id,name) {
	$("#CDUnderlyingFundId").val(id);
	$("#SpnCDUFName").html(name);
	$("tbody","#CashDistributionList").empty();
	$("tbody","#PRCashDistributionList").empty();
	$("#CDDetail").attr("issearch","true").show();
	dealActivity.loadCD(true);
};
dealActivity.loadCD=function (isRefresh) {
	var loading=$("#CDLoading");
	var tbl=$("#CashDistributionList");
	$("#CashDistribution").show();
	$("#PRCDListBox").hide();
	var target=$("tbody",tbl);
	var rowsLength=$("tr",target).length;
	if(rowsLength==0) { isRefresh=true; }
	if(isRefresh) {
		target.empty();
		$("#CashDistribution").hide();
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON("/Deal/UnderlyingFundCashDistributionList",{ "_": (new Date).getTime(),"underlyingFundId": dealActivity.getCDUnderlyingFundId() },function (data) {
			$("#PRCashDistribution").show();
			loading.empty();
			$.each(data,function (i,item) { item["Index"]=i;$("#CashDistributionAddTemplate").tmpl(item).appendTo(target); });
			dealActivity.setUpRow($("tr",target));
			rowsLength=$("tr",target).length;
			if(rowsLength>0) { $("#CashDistribution").show(); }
			$("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
			$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
			$(".mcc",target).removeAttr("class");
			dealActivity.showManualCDCtl();
		});
	}
};
dealActivity.getCDUnderlyingFundId=function (id) {
	return parseInt($("#CDUnderlyingFundId").val());
};
dealActivity.submitUFCashDistribution=function (frm) {
	try {
		var param=$(frm).serializeArray();
		var loading=$("#SpnCDSaveLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		var chk=$("#IsManualCashDistribution").get(0);
		var totalRows=($("tbody tr","#CashDistributionList").length);
		var isManual=chk.checked;
		var isError=false;
		/*if(totalRows>0&&isManual) {
		var msg="";var isFocus=false;
		$(".manualcamount").each(function () {
		var amount=parseFloat(this.value);if(isNaN(amount)) { amount=0; }
		if(amount>0) {
		var parentRow=$(this).parents("tr:first");
		var dealname=$(".dealname",parentRow).html();
		var date=$(".manualdate",parentRow).val();
		if($.trim(date)=="") {
		msg+=dealname+" Distribution Date is required\n";
		if(isFocus==false) {
		$(".manualdate",parentRow).focus();
		isFocus=true;
		}
		}
		}
		});
		if(msg!="") {
		alert(msg);
		isError=true;
		loading.empty();
		}
		}*/
		if(isError==false) {
			param[param.length]={ name: "TotalRows",value: ($("tbody tr","#CashDistributionList").length) };
			param[param.length]={ name: "IsManualCashDistribution",value: isManual };
			$.post("/Deal/CreateUnderlyingFundCashDistribution",param,function (data) {
				loading.empty();
				if($.trim(data)!="") { dealActivity.processErrMsg(data,frm); } else {
					alert("Cash Distributions Saved");
					dealActivity.resetCashDistribution();
				}
			});
		}
	} catch(e) { alert(e); }
	return false;
};
dealActivity.resetCashDistribution=function () {
	$("#CashDistribution").hide();
	$("#PRCashDistribution").hide();
	$("tbody","#CashDistributionList").empty();
	$("#CD_UnderlyingFund").val("");
	$("#SpnCDUFName").empty();
	$("#CDUnderlyingFundId").val(0);
	$("#CD_UnderlyingFund").focus();
};
dealActivity.showManualCDCtl=function (parentId) {
	var chk=$("#IsManualCashDistribution").get(0);
	var display="";
	if(chk.checked) { display=""; } else { display="none"; }
	$(".ismanual","#CashDistributionList").css("display",display);
};