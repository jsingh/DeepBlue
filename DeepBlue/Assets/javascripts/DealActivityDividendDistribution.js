dealActivity.makeNewDD=function () {
	var ufid=dealActivity.getDDDirect();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		jAlert("Direct is required");
	} else {
		dealActivity.loadDD(false);
	}
};
dealActivity.expandMDDTree=function (index,img) {
	var display="";
	if(img.src.indexOf('treeplus')>0) {
		display="";
		img.src=img.src.replace("treeplus.gif","treeminus.gif");
	} else {
		display="none";
		img.src=img.src.replace("treeminus.gif","treeplus.gif");
	}
	$("#ManualDD_Deal_"+index).css("display",display);
};
dealActivity.setDDDirect=function (item) {
	$("#DDDirectTypeId").val(item.otherid);
	$("#DDDirectId").val(item.otherid2);
	$("#SpnDDName").html(item.value);
	$("tbody","#DividendDistributionList").empty();
	$("tbody","#PRDividendDistributionList").empty();
	$("#DDDetail").attr("issearch","true").show();
	dealActivity.loadDD(true);
};
dealActivity.loadDD=function (isRefresh) {
	var tbl=$("#DividendDistributionList");
	var loading=$("#DDLoading");
	$("#DividendDistribution").show();
	$("#PRDividendDistribution").show();
	$("#PRDDListBox").hide();
	var target=$("tbody",tbl);
	var rowsLength=$("tr",target).length;
	if(rowsLength==0) { isRefresh=true; }
	if(isRefresh) {
		target.empty();
		$("#DividendDistribution").hide();
		loading.html(jHelper.loadingHTML());
		$.getJSON("/Deal/DirectDividendDistributionList",{ 
			"_": (new Date).getTime()
			,"securityTypeID": dealActivity.getDDDirectType()
			,"securityID": dealActivity.getDDDirect()
		},function (data) {
			loading.empty();
			$.each(data,function (i,item) {
				item["Index"]=i;
				$("#DividendDistributionAddTemplate").tmpl(item).appendTo(target);
			});
			dealActivity.setUpRow($("tr",target));
			jHelper.jqCheckBox(target);
			rowsLength=$("tr",target).length;
			if(rowsLength>0) { $("#DividendDistribution").show(); }
			$("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
			$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
			$(".mcc",target).removeAttr("class");
			dealActivity.showManualDDCtl();
		});
	}
};
dealActivity.getDDDirect=function (id) { return parseInt($("#DDDirectId").val()); };
dealActivity.getDDDirectType=function (id) { return parseInt($("#DDDirectTypeId").val()); };
dealActivity.submitDD=function (frm) {
	try {
		var param=$(frm).serializeForm();
		var loading=$("#SpnDDSaveLoading");
		loading.html(jHelper.savingHTML());
		var chk=$("#IsManualDividendDistribution").get(0);
		var totalRows=($("tbody tr[type='prow']","#DividendDistributionList").length);
		var isManual=chk.checked;
		var isError=false;
		if(isError==false) {
			param[param.length]={ name: "TotalRows",value: totalRows };
			param[param.length]={ name: "IsManualDividendDistribution",value: isManual };
			$.post("/Deal/CreateDirectDividendDistribution",param,function (data) {
				loading.empty();
				if($.trim(data)!="") {
					dealActivity.processErrMsg(data,frm);
				} else {
					jAlert("Dividend Distributions Saved");
					//dealActivity.resetDividendDistribution();
				}
			});
		}
	} catch(e) { jAlert(e); }
	return false;
};
dealActivity.resetDividendDistribution=function () {
	$("#DDDetail").hide();
	$("#DividendDistribution").hide();
	$("#PRDividendDistribution").hide();
	$("tbody","#DividendDistributionList").empty();
	$("#DD_Direct").val("");
	$("#SpnDDName").empty();
	$("#DDDirectId").val(0);
	$("#DD_Direct").focus();
};
dealActivity.showManualDDCtl=function () {
	var chk=$("#IsManualDividendDistribution").get(0);
	var display="";
	if(chk.checked) { display=""; } else { display="none"; }
	$(".ismanual","#DividendDistributionList").css("display",display);
};